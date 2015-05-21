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
    public class ShiftTest
    {
        [Test]
        public void AddsNurseToShift_TakesNurse_ShiftContainsNurse()
        {
            //arrange
            EveningShift es1 = new EveningShift(DateTime.Now);
            Nurse n1 = new Nurse(12, "Jutte");
            //act
            es1.AddNurse(n1);
            //assert
            CollectionAssert.Contains(es1.AssignedNurses, n1);
        }
        [Test]
        public void RemoveNurseFromShift_TakesNurse_DoesNotContainNurse()
        {
            //arrange
            NightShift ns1 = new NightShift(DateTime.Now);
            Nurse n2 = new Nurse(666, "Lucifer");
            Nurse n3 = new Nurse(314, "Phi");
            ns1.AddNurse(n2);
            ns1.AddNurse(n3);
            //act
            ns1.RemoveNurse(n2);
            //assert
            CollectionAssert.DoesNotContain(ns1.AssignedNurses, n2);
        }
        //I denne test vil vi teste for full shift
        [Test]
        public void ReturnsIfShiftIsFull_TakesNoParameters_True()
        {
            //arrange
            DayShift ds1 = new DayShift(DateTime.Now);
            Nurse n11 = new Nurse(3, "Siddhartha");
            Nurse n22 = new Nurse(5, "Dalai Lama");
            ds1.AddNurse(n11);
            ds1.AddNurse(n22);
            //act 
            //assert
            Assert.IsTrue(ds1.FullShift);
        }
        [Test]
        public void ReturnsIfShiftIsFull_TakesNoParameters_False()
        {
            //arrange
            DayShift ds2 = new DayShift(DateTime.Now);
            Nurse hitler = new Nurse(1945, "Hitler");
            ds2.AddNurse(hitler);
            //act 
            //assert
            Assert.IsFalse(ds2.FullShift);
        }
    }
}
