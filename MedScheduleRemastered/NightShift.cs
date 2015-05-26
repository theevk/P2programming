using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedScheduleRemastered
{
    /* Klassens formål er at definere en dagvagt */
    public class NightShift : Shift
    {
        public NightShift(DateTime d) : base(d) //antallet af sygeplejersker der kan være på vagten defineres til at være 4
        {
            _nursesPerShift = 4;
        }
        public override string ToString()
        {
            return "Nightshift";
        }
    }
}
