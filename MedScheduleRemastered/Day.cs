﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedScheduleRemastered
{
    public class Day
    {
        private List<Shift> _fullDay = new List<Shift>();

        public List<Shift> FullDay { get { return _fullDay; } }

        public Day(DateTime d)
        {
            _fullDay.Add(new DayShift(d));
            _fullDay.Add(new EveningShift(d));
            _fullDay.Add(new NightShift(d));
            _fullDay.Add(new FreeShift(d));
        }
    }
}
