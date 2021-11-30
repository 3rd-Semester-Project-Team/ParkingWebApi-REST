using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApi_REST.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi_REST.Models;

namespace WebApi_REST.Managers.Tests
{
    //[TestClass()]
    public class ParkingManagerTests
    {
        [TestMethod()]
        public void GetAllTest_ListCountPassing()
        {
            //Arrange
            int expected = 4;
            int actualCount=-1;
            ParkingsManager manager = new ParkingsManager();
            //Act
            actualCount = manager.GetAll().Count();
            ////Assert
            Assert.AreEqual(expected,actualCount);
        }

        [TestMethod()]
        public void GetByIdTest_()
        {
            //Arrange
            ParkingsManager manager = new ParkingsManager();
            var expected = false;

            //Assert
            Assert.AreEqual(expected,manager.GetById(2).Occupied);
        }

        [TestMethod()]
        public void AddParkSlotTest()
        {
            //ACt
            ParkingsManager manager = new ParkingsManager();
            var newCount = 5;
            var actual = -1;

            //Act
            manager.AddParkSlot(new ParkingSlot());
            actual = manager.GetAll().Count();

            Assert.AreEqual(newCount,actual);
        }

        [TestMethod()]
        //[ExpectedException(typeof(ArgumentException))]
        public void GetParkSlotNonExistentID_Fail()
        {
            //Arrange
            ParkingsManager manager = new ParkingsManager();

            //Act
             var actual = manager.GetById(503769375);

             Assert.IsNull(actual);

        }

        [TestMethod()]
        public void AddNullObjectTest()
        {
            ParkingsManager manager = new ParkingsManager();
            var actual=manager.AddParkSlot(null);
            Assert.IsNull(actual);
        }

    }
}