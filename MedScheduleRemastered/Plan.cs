using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MedScheduleRemastered
{
    public class Plan
    {
        private List<Day> _days = new List<Day>();
        private List<Nurse> _nurseDataBase = new List<Nurse>();
        private int _nightViolations; //hvor mange gange en sygeplejerske har mere end to nattevagter i træk
        private int _freeShiftViolations; //hvor mange gange en sygeplejerske har for mange fridage i træk
        private double _averageShiftDifference;
        private double _fitnessScore;

        private int minNurseShiftDifference = 1000;
        private int maxNurseShiftDifference = 0;

        public Plan(int numberOfDays)
        {
            for (int i = 0; i < numberOfDays; i++)
                _days.Add(new Day(DateTime.Now.AddDays(i)));
            LoadNurseCatalogue();
            Initialize();
            EvaluatePlan();
        }

        private void LoadNurseCatalogue()
        {
            StreamReader sr = new StreamReader("NurseCatalogue.txt");
            string line = "";
            while((line = sr.ReadLine()) != null)
            {
                string[] lineReader = line.Split(' ');
                _nurseDataBase.Add(new Nurse(int.Parse(lineReader[0]), lineReader[1]));
            }
        }

        public void Initialize()
        {
            AddNursesToFirstDay();
            AddNursesToRemainingDays();
        }

        private void AddNursesToFirstDay()
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

                if(possibleCandidates.Count() != 0)
                {
                    if(shift is FreeShift)
                    {
                        foreach (Nurse nurse in possibleCandidates)
                            shift.AddNurse(nurse);
                    }
                }
            }
        }

        private void AddNursesToRemainingDays()
        {
            Random r = new Random();
            for (int i = 1; i < _days.Count(); i++)
            {
                List<Nurse> possibleCandidates = new List<Nurse>(_nurseDataBase);

                for(int k = 0; k < 4; k++)
                {
                    if (possibleCandidates.Count() == 0)
                        break;
                    while(_days[i].FullDay[0].FullShift != true || _days[i].FullDay[1].FullShift != true || _days[i].FullDay[2].FullShift != true)
                    { 
                        Nurse n = possibleCandidates[r.Next(0, possibleCandidates.Count())];

                        if((_days[i - 1].FullDay[2].AssignedNurses.Contains(n) || _days[i-1].FullDay[1].AssignedNurses.Contains(n)) != true && _days[i].FullDay[0].FullShift != true)
                        {
                            _days[i].FullDay[0].AddNurse(n);
                            possibleCandidates.Remove(n);
                        }

                        else if (_days[i - 1].FullDay[2].AssignedNurses.Contains(n) != true && _days[i].FullDay[1].FullShift != true)
                        {
                            _days[i].FullDay[1].AddNurse(n);
                            possibleCandidates.Remove(n);
                        }

                        else if(_days[i].FullDay[2].FullShift != true)
                        {
                            _days[i].FullDay[2].AddNurse(n);
                            possibleCandidates.Remove(n);
                        }

                        else if(possibleCandidates.Count() != 0)
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

        public void EvaluatePlan()
        {
            _nightViolations = 0;
            _freeShiftViolations = 0;
            _averageShiftDifference = 0;
            double tempFitness;

            for(int i = 0; i < (_days.Count() - 1); i++)
            {
                List<Nurse> todayFreeShiftNurses = _days[i].FullDay[3].AssignedNurses;
                List<Nurse> nextDayNightShiftNurses = _days[i + 1].FullDay[2].AssignedNurses;
                List<Nurse> nextDayFreeShiftNurses = _days[i + 1].FullDay[3].AssignedNurses;
                List<Nurse> tempNurses = new List<Nurse>();
                
                _nightViolations += _days[i].FullDay[2].AssignedNurses.Count(nextDayNightShiftNurses.Contains);
                
                foreach(Nurse nurse in todayFreeShiftNurses)
                {
                    if (nextDayFreeShiftNurses.Contains(nurse))
                        tempNurses.Add(nurse);
                }

                if (i < (_days.Count - 2))
                    //List<Nurse> dayAfterTommorowFreeShiftNurses =new List<Nurse>(_days[i + 2].FullDay[3].AssignedNurses);
                    _freeShiftViolations += tempNurses.Count(_days[i + 2].FullDay[3].AssignedNurses.Contains);
            }

            foreach(Nurse nurse in _nurseDataBase)
            {
                if (nurse.WorkCounter > maxNurseShiftDifference)
                    maxNurseShiftDifference = nurse.WorkCounter;

                if (nurse.WorkCounter < minNurseShiftDifference)
                    minNurseShiftDifference = nurse.WorkCounter;
            }

            tempFitness = Convert.ToDouble(maxNurseShiftDifference) - Convert.ToDouble(minNurseShiftDifference);
            _averageShiftDifference = tempFitness / Convert.ToDouble(_days.Count());
            _fitnessScore = ((Convert.ToDouble(_freeShiftViolations) + Convert.ToDouble(_nightViolations)) * _averageShiftDifference) / Convert.ToDouble(_days.Count());
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
        public Plan SuperPlan(int cycles, int days)
        {
            ResetNurseWorkCounters();
            Plan bestPlan = new Plan(days);
            for(int i = 0; i < cycles; i++)
            {
                Plan p = new Plan(days);
                if (p._fitnessScore < bestPlan._fitnessScore)
                    bestPlan = p;
                ResetNurseWorkCounters();
            }

            return bestPlan;
        }

        public void ResetNurseWorkCounters()
        {
            foreach(Nurse nurse in _nurseDataBase)
                nurse.ResetShiftCounter();
        }
    }
}