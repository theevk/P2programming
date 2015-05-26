using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedScheduleRemastered
{
    /* Formålet med klassen at definere hvilken data en sygeplejerske består af, og udfører korrekte udregninger ift. dens variable */
    public class Nurse
    {
        private int _workCounter; //sygeplejersken har en vagttæller, der bruges til at finde ud af hvor mange vagter sygeplejersken har i en plan
        private string _name; //sygeplejersken har et navn
        private int _ID; //sygeplejersken har et ID

        public int WorkCounter { get { return _workCounter; } set { _workCounter = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public int ID { get { return _ID; } set { _ID = value; } }

        public Nurse(int ID, string name) //en sygeplejerske skal altid have et ID og navn, når der laves en instans af den
        {
            _ID = ID;
            _name = name;
        }
        
        public void IncrementShiftCounter() //tæller antallet af sygeplejerskens vagter op
        {
            _workCounter++;
        }

        public void ResetShiftCounter() //sætter antallet af vagter, som sygeplejersken har, op
        {
            _workCounter = 0;
        }

        public override string ToString()
        {
            return "Nurse: " + _name + " " + WorkCounter;
        }
    }
}
