using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedScheduleRemastered;

namespace MedScheduleRemateredTest
{
    using NUnit.Framework;
    [TestFixture]
    class PlanTest
    {
        [Test]
        public void ResetAllNurseWorkCounters_NurseDatabase_WorkCounterToZero()
        {
            //arrange
            Plan p = new Plan(5);
            //act
            p.ResetNurseWorkCounters();
        }
    }
}
