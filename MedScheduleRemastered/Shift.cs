using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedScheduleRemastered
{
    /* Klassens formål at være en abstrakt superklasse, som andre klasser nedarves fra. Heri defierens variablerbe og metoder nødvendige for underklasserne */
    public abstract class Shift
    {
        protected int _nursesPerShift;  //en vagt har et specifikt antal sygeplejersker, som skal være på vagten
        protected List<Nurse> _assignedNurses; //en vagt har en liste af sygeplejersker, som er der sygeplejersker der har fået vagten
        protected DateTime _date; //en vagt har en dato
        protected bool _fullShift; //vagt er enten fuld, dvs. der kan ikke være flere sygeplejersker på vagten, eller ej, dvs. der stadig kan lægges sygeplejersker over i vagten.

        public Shift(DateTime startTime) //en instans af en vagt skal have en dato og en liste af sygeplejersker
        {
            _date = startTime;
            _assignedNurses = new List<Nurse>();
        }
        public DateTime Date { get { return _date; }}
        public List<Nurse> AssignedNurses { get { return _assignedNurses; } }

        public bool FullShift //hvis vagten er fuld returnes true, ellers false
        {
            get
            {
                if (_nursesPerShift == _assignedNurses.Count())
                    return true;
                else
                    return false;
            }
        }

        public void AddNurse(Nurse n) //metoden der tilføjer en sygeplejerske til vagten og tæller sygeplejerskens vagttæller op
        {
            _assignedNurses.Add(n);
            if(!(this is FreeShift))
                n.IncrementShiftCounter();
        }

        public void RemoveNurse(Nurse n) //metoden der fjerner en sygeplejerske fra vagten
        {
            _assignedNurses.Remove(n);
        }
        public string PrintShift()
        {
            string ShiftString = ToString();
            foreach (Nurse nurse in _assignedNurses)
                ShiftString += " " + nurse.Name;
            return ShiftString;
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
