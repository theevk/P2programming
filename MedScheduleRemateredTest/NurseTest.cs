using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedScheduleRemastered;
using NUnit.Framework;

namespace MedScheduleRemateredTest
{
    [TestFixture]
    public class NurseTest
    {
        [Test]
        public void IncrementsNurseWorkCounter_Nurse_Adds1ToWorkCounter()
        {
            //arrange
            Nurse n1 = new Nurse(69, "Mia Khalifa");
            //act
            n1.IncrementShiftCounter();
            n1.IncrementShiftCounter();
            int expected = 2;
            int actual = n1.WorkCounter;
            //assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ResetsWorkCounter_Nurse_SetsWorkCounterToZero()
        {
            //arrange
            Nurse n1 = new Nurse(5, "Al Capone");
            n1.IncrementShiftCounter();
            n1.IncrementShiftCounter();
            n1.IncrementShiftCounter();
            n1.IncrementShiftCounter();
            n1.IncrementShiftCounter();
            n1.IncrementShiftCounter();
            //act
            n1.ResetShiftCounter();
            //Assert
            int actual = n1.WorkCounter;
            int expected = 0;
            Assert.AreEqual(expected, actual);
            
        }
    }
}
