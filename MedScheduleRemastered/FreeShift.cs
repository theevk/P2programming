using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedScheduleRemastered
{
    /* Klassens formål er at definere en frivagt, eller fridag */
    public class FreeShift : Shift
    {
        public FreeShift(DateTime d) : base(d) //definerer hvor mange sygeplejersker der kan være på vagten
        {
            _nursesPerShift = 1;
        }
        public override string ToString()
        {
            return "Freeshift";
        }
    }
}
