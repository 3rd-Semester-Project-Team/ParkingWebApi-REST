using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApi_REST.Controllers;
using WebApi_REST.Managers;
using WebApi_REST.Models;

namespace ParkingWebApi_RESTTests.Controllers
{
    [TestClass]
    public class ParkingsControllerTests
    {
        private Mock<ParkingsManager> _mockManager;
        private ParkingsController _controller;

        private static int _nextId = 0;
        private static List<ParkingSlot> _parkingSlot = new List<ParkingSlot>()
        {
            new ParkingSlot(){ParkingId = ++_nextId, Occupied = false, SensorDateTime = DateTime.Now},
            new ParkingSlot() { ParkingId = ++_nextId, Occupied = false, SensorDateTime = DateTime.Now },
            new ParkingSlot() { ParkingId = ++_nextId, Occupied = false, SensorDateTime = DateTime.Now },
            new ParkingSlot() { ParkingId = ++_nextId, Occupied = false, SensorDateTime = DateTime.Now },
        };

        public ParkingsControllerTests()
        {
            _mockManager = new Mock<ParkingsManager>();
        }

        [TestMethod]
        public void GetAllParkings_Test()
        {
            // Arrange
            _mockManager.Setup(mock => mock.GetAll()).Returns(_parkingSlot);
            _controller = new ParkingsController(_mockManager.Object);
            
            // Act
            var result = _controller.GetAll().Result as ObjectResult;

            // Assert
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsInstanceOfType(result.Value, typeof(IEnumerable<ParkingSlot>));
            Assert.AreEqual(4, (result.Value as IEnumerable<ParkingSlot>).Count());
        }

    }
}
