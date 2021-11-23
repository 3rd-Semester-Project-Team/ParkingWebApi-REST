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
    [TestClass()]
    public class SensorManagerTests
    {
        [TestMethod()]
        public void GetAllTest_ListCountPassing()
        {
            //Arrange
            int expected = 4;
            int actualCount=-1;
            ParkingManager manager = new ParkingManager();
            //Act
            actualCount = manager.GetAll().Count();
            ////Assert
            Assert.AreEqual(expected,actualCount);
        }

        [TestMethod()]
        public void GetByIdTest_()
        {
            //Arrange
            ParkingManager manager = new ParkingManager();
            var expected = false;

            //Assert
            Assert.AreEqual(expected,manager.GetById(2).Occupied);
        }

        [TestMethod()]
        public void AddParkSlotTest()
        {
            //ACt
            ParkingManager manager = new ParkingManager();
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
            ParkingManager manager = new ParkingManager();

            //Act
             var actual = manager.GetById(503769375);

             Assert.IsNull(actual);

        }

        [TestMethod()]
        public void AddNullObjectTest()
        {
            ParkingManager manager = new ParkingManager();
            var actual=manager.AddParkSlot(null);
            Assert.IsNull(actual);
        }

    }
}