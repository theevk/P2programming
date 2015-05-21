using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedScheduleRemastered
{
    public class Nurse //indeholder data for sygeplejerskerne
    {
        private int _workCounter;
        private string _name;
        private int _ID;

        public int WorkCounter { get { return _workCounter; } set { _workCounter = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public int ID { get { return _ID; } set { _ID = value; } }

        public Nurse(int ID, string name)
        {
            _ID = ID;
            _name = name;
        }
        
        public void IncrementShiftCounter()
        {
            _workCounter++;
        }

        public void ResetShiftCounter()
        {
            _workCounter = 0;
        }

        public override string ToString()
        {
            return "Nurse: " + _name + " " + WorkCounter;
        }
    }
}
