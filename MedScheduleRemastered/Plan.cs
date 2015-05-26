using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MedScheduleRemastered
{
    /* Klassens formål er at lave selve vagtplanen. Det er denne klasses ansvar at udføre algoritmerne for at får lavet planen */
    public class Plan
    {
        private List<Day> _days = new List<Day>(); //en vagt har en liste af dage
        private List<Nurse> _nurseDataBase = new List<Nurse>(); // en vagt har liste af alle sygeplejersker
        private int _nightViolations; //hvor mange gange en sygeplejerske har mere end to nattevagter i træk
        private int _freeShiftViolations; //hvor mange gange en sygeplejerske har for mange fridage i træk
        private double _averageShiftDifference; //forskellen ml. sygeplejerskerne med flest og færrest antal vagter
        private double _fitnessScore; //værdien der bruges til at vurdere hvor god/dårlig den oprettede plan er
        private int minNurseShiftDifference = 1000; //arbritært stort tal der bruges til sammenligning af sygeplejersker, når den med flest vagter skal findes
        private int maxNurseShiftDifference = 0;  //arbritært lille tal der bruges til sammenligning af sygeplejersker, når den med færrest vagter skal findes

        public Plan(int numberOfDays) //når planen laves tilføjes der et antal dage afhængigt af hvad der er indtastet i main, de passende metoder kaldes
        {
            for (int i = 0; i < numberOfDays; i++)
                _days.Add(new Day(DateTime.Now.AddDays(i)));
            LoadNurseCatalogue();
            Initialize();
            EvaluatePlan();
        }

        private void LoadNurseCatalogue() //metoden der henter sygeplejerskernes data fra en tekstfil og ligger den over i listen _nurseDataBase
        {
            StreamReader sr = new StreamReader("NurseCatalogue.txt");
            string line = "";
            while((line = sr.ReadLine()) != null)
            {
                string[] lineReader = line.Split(' '); //dataen i tekstfilen er separeret med et mellemrum
                _nurseDataBase.Add(new Nurse(int.Parse(lineReader[0]), lineReader[1]));
            }
        }

        public void Initialize() //metoden der samler de to metoder, hvori algoritmerne kører
        {
            AddNursesToFirstDay();
            AddNursesToRemainingDays();
        }

        private void AddNursesToFirstDay() //metoden der lægger sygeplejersker over i vagter på den første dag
        {
            List<Nurse> possibleCandidates = new List<Nurse>(_nurseDataBase);
            Random r = new Random();
            
            foreach(Shift shift in _days[0].FullDay)
            {
                while(shift.FullShift != true)
                {
                    Nurse n = possibleCandidates[r.Next(0, possibleCandidates.Count())];
                    
                    shift.AddNurse(n);
                    possibleCandidates.Remove(n);

                }

                if(possibleCandidates.Count() != 0) //lægger de resterende sygeplejersker over i FreeShift
                {
                    if(shift is FreeShift)
                    {
                        foreach (Nurse nurse in possibleCandidates)
                            shift.AddNurse(nurse);
                    }
                }
            }
        }

        private void AddNursesToRemainingDays() //metoden der lægger sygeplejersker over i vagter på de resterende dage
        {
            Random r = new Random();
            for (int i = 1; i < _days.Count(); i++) //tæller igennem for hver dag
            {
                List<Nurse> possibleCandidates = new List<Nurse>(_nurseDataBase);

                for(int k = 0; k < 4; k++) //tæller i gennem for hver shift
                {
                    if (possibleCandidates.Count() == 0)
                        break;
                    while(_days[i].FullDay[0].FullShift != true || _days[i].FullDay[1].FullShift != true || _days[i].FullDay[2].FullShift != true) //så længe dag-, aften- og nattevagterne ikke er fyldt ud
                    { 
                        Nurse n = possibleCandidates[r.Next(0, possibleCandidates.Count())];

                        if((_days[i - 1].FullDay[2].AssignedNurses.Contains(n) || _days[i-1].FullDay[1].AssignedNurses.Contains(n)) != true && _days[i].FullDay[0].FullShift != true) //hard constraints for en dagvagt
                        {
                            _days[i].FullDay[0].AddNurse(n);
                            possibleCandidates.Remove(n);
                        }

                        else if (_days[i - 1].FullDay[2].AssignedNurses.Contains(n) != true && _days[i].FullDay[1].FullShift != true) //hard constraints for en aftenvagt
                        {
                            _days[i].FullDay[1].AddNurse(n);
                            possibleCandidates.Remove(n);
                        }

                        else if(_days[i].FullDay[2].FullShift != true) //sygeplejersken kan lægges i denne vagt, såfremt den ikke kan lægges i de to foregående vagter og vagten ikke er fuld
                        {
                            _days[i].FullDay[2].AddNurse(n);
                            possibleCandidates.Remove(n);
                        }

                        else if(possibleCandidates.Count() != 0) //lægger de resterende sygeplejersker over i en FreeShift, hvis de ikke kan få de andre vagter eller vagterne er fyldt ud
                        {
                            if(_days[i].FullDay[k] is FreeShift)
                            {
                                foreach (Nurse nurse in possibleCandidates)
                                    _days[i].FullDay[k].AddNurse(nurse);
                            }
                        }
                    }
                }
            }
        }

        public void EvaluatePlan() //metoden der evaluerer planen ved at lave en fitness-score for hver plan
        {
            _nightViolations = 0;
            _freeShiftViolations = 0;
            _averageShiftDifference = 0;
            double tempFitness;

            for(int i = 0; i < (_days.Count() - 1); i++) //kører gennem alle dage op til den andensidste
            {
                List<Nurse> todayFreeShiftNurses = _days[i].FullDay[3].AssignedNurses; //listen sygeplejersker på den nuværende dags frivagt
                List<Nurse> nextDayNightShiftNurses = _days[i + 1].FullDay[2].AssignedNurses; //listen af sygeplejersker på den næste dags nattevagt 
                List<Nurse> nextDayFreeShiftNurses = _days[i + 1].FullDay[3].AssignedNurses; //listen af sygeplejersker på den næste dags frivagt
                List<Nurse> tempNurses = new List<Nurse>(); //en midlertidig liste af sygeplejersker
                
                _nightViolations += _days[i].FullDay[2].AssignedNurses.Count(nextDayNightShiftNurses.Contains); //antal gange en sygeplejersker har to nattevagter i træk
                
                foreach(Nurse nurse in todayFreeShiftNurses) //tempNurses indeholder de sygeplejersker, der har to frivagter i træk
                {
                    if (nextDayFreeShiftNurses.Contains(nurse))
                        tempNurses.Add(nurse);
                }

                if (i < (_days.Count - 2))
                    _freeShiftViolations += tempNurses.Count(_days[i + 2].FullDay[3].AssignedNurses.Contains); //antal gange en sygeplejersker har tre frivagter i træk. Til denne bruges tempNurses
            }

            foreach(Nurse nurse in _nurseDataBase) //sygeplejersker med flest og færrest vagter findes i denne løkke
            {
                if (nurse.WorkCounter > maxNurseShiftDifference)
                    maxNurseShiftDifference = nurse.WorkCounter;

                if (nurse.WorkCounter < minNurseShiftDifference)
                    minNurseShiftDifference = nurse.WorkCounter;
            }

            tempFitness = Convert.ToDouble(maxNurseShiftDifference) - Convert.ToDouble(minNurseShiftDifference);
            _averageShiftDifference = tempFitness / Convert.ToDouble(_days.Count());
            _fitnessScore = ((Convert.ToDouble(_freeShiftViolations) + Convert.ToDouble(_nightViolations)) * _averageShiftDifference) / Convert.ToDouble(_days.Count()); //fitness-scoren udregnes her
        }

        public void Printplan()
        {
            foreach(Day day in _days)
            {
                Console.WriteLine(day.FullDay[0].Date.ToShortDateString());
                string dayshift = day.FullDay[0].PrintShift();
                string eveningshift = day.FullDay[1].PrintShift();
                string nightshift = day.FullDay[2].PrintShift();
                string wholeshift = String.Format("{0,-10} | {1,-25} | {2,-20}",dayshift,eveningshift,nightshift);
                Console.WriteLine(wholeshift);
            }
            
            Console.WriteLine(_fitnessScore.ToString());
            Console.WriteLine("{0}", _nightViolations);
        }

        public void PrintNurseWorkLoad()
        {
            foreach(Nurse nurse in _nurseDataBase)
            {
                Console.WriteLine(nurse.Name + " " + nurse.WorkCounter);
            }
        }
        public Plan SuperPlan(int cycles, int days) //SuperPlanen er den plan, der har den bedste fitness-score, og som i sidste udprintes til brugeren
        {
            ResetNurseWorkCounters();
            Plan bestPlan = new Plan(days);
            for(int i = 0; i < cycles; i++) //antal gange en ny plan skal laves, afhængigt af hver der er indtastet i main
            {
                Plan p = new Plan(days);
                if (p._fitnessScore < bestPlan._fitnessScore)
                    bestPlan = p;
                ResetNurseWorkCounters();
            }

            return bestPlan;
        }

        public void ResetNurseWorkCounters() //metoden der kaldes, når sygeplejerskens antal vagter skal sættes til nul
        {
            foreach(Nurse nurse in _nurseDataBase)
                nurse.ResetShiftCounter();
        }
    }
}