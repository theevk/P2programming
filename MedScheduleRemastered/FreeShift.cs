using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedScheduleRemastered
{
    public class FreeShift : Shift
    {
        public FreeShift(DateTime d) : base(d)
        {
            _nursesPerShift = 1;
        }
        public override string ToString()
        {
            return "Freeshift";
        }
    }
}
