using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedScheduleRemastered
{
    public class NightShift : Shift
    {
        public NightShift(DateTime d) : base(d)
        {
            _nursesPerShift = 4;
        }
        public override string ToString()
        {
            return "Nightshift";
        }
    }
}
