using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedScheduleRemastered
{
    public abstract class Shift
    {
        protected int _nursesPerShift;
        protected List<Nurse> _assignedNurses;
        protected DateTime _date;
        protected bool _fullShift;

        public Shift(DateTime startTime)
        {
            _date = startTime;
            _assignedNurses = new List<Nurse>();
        }
        public DateTime Date { get { return _date; }}
        public List<Nurse> AssignedNurses { get { return _assignedNurses; } }

        public bool FullShift
        {
            get
            {
                if (_nursesPerShift == _assignedNurses.Count())
                    return true;
                else
                    return false;
            }
        }

        public void AddNurse(Nurse n)
        {
            _assignedNurses.Add(n);
            if(!(this is FreeShift))
                n.IncrementShiftCounter();
        }

        public void RemoveNurse(Nurse n)
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
