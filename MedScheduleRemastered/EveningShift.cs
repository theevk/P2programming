using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedScheduleRemastered
{
    /* Klassens formål er at definere en aftenvagt */
    public class EveningShift : Shift
    {
        public EveningShift(DateTime d) : base(d) //antallet af sygeplejersker der kan være på denne vagt defineres til at være 2
        {
            _nursesPerShift = 2;
        }
        public override string ToString()
        {
            return "Eveningshift";
        }
    }
}
