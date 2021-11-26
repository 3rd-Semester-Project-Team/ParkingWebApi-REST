using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
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
        private Mock<IParkingsManager> _mockParkingManager;
        private Mock<ISensorsManager> _mockSensorManager;
        private ParkingsController _controller;

        private List<ParkingSlot> _parkingSlot = new List<ParkingSlot>()
        {
            new ParkingSlot() { ParkingId = 1, Occupied = false, SensorDateTime = DateTime.Now },
            new ParkingSlot() { ParkingId = 2, Occupied = false, SensorDateTime = DateTime.Now },
            new ParkingSlot() { ParkingId = 3, Occupied = false, SensorDateTime = DateTime.Now },
            new ParkingSlot() { ParkingId = 4, Occupied = false, SensorDateTime = DateTime.Now }
        };

        public ParkingsControllerTests()
        {
            _mockParkingManager = new Mock<IParkingsManager>();
            _mockSensorManager = new Mock<ISensorsManager>();
        }

        [TestMethod]
        public void GetAllParkings_Test()
        {
            // Arrange
            _mockParkingManager.Setup(mock => 
                mock.GetAll())
                .Returns(_parkingSlot);
            _controller = new ParkingsController(_mockParkingManager.Object, _mockSensorManager.Object);
            
            // Act
            var result = _controller.GetAll().Result as ObjectResult;

            // Assert
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsInstanceOfType(result.Value, typeof(IEnumerable<ParkingSlot>));
            Assert.AreEqual(4, (result.Value as IEnumerable<ParkingSlot>).Count());
        }

        [TestMethod]
        public void GetById_ValidId_Test()
        {
            _mockParkingManager.Setup(mock => 
                mock.GetById(1))
                .Returns(_parkingSlot[0]);
            _controller = new ParkingsController(_mockParkingManager.Object, _mockSensorManager.Object);

            var result = _controller.GetById(1).Result as ObjectResult;

            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(1, (result.Value as ParkingSlot).ParkingId);
        }

        [TestMethod]
        public void GetById_InvalidId_Test()
        {
            _mockParkingManager.Setup(mock => 
                mock.GetById(100))
                .Returns(() => null);
            _controller = new ParkingsController(_mockParkingManager.Object, _mockSensorManager.Object);

            var result = _controller.GetById(100);
            
            Assert.AreEqual(404, (result.Result as StatusCodeResult).StatusCode);
        }

        [TestMethod]
        public void Add_Successful_Test()
        {
            // Arrange
            var slot = new ParkingSlot()
                { ParkingId = 2, Occupied = true, SensorDateTime = DateTime.Now };

            _mockParkingManager.Setup(mock => 
                mock.AddParkSlot(It.IsAny<ParkingSlot>()))
                .Returns(slot);
            _mockSensorManager.Setup(m => 
                m.GetById(1))
                .Returns(new Sensor() { SensorId = 1, ParkingId = 2 });
            _controller = new ParkingsController(_mockParkingManager.Object, _mockSensorManager.Object);

            RawData data = new RawData() { Occupied = true, SensorId = 1 };

            // Act
            var createdResult = _controller.Post(data).Result;

            // Assert
            Assert.IsNotNull(createdResult);
            Assert.IsInstanceOfType(createdResult, typeof(CreatedResult));
        }

        [TestMethod]
        public void Add_WithConflict_Test()
        {
            // Arrange
            _mockParkingManager.Setup(mock => 
                mock.AddParkSlot(It.IsAny<ParkingSlot>()))
                .Returns(() => null);
            _mockSensorManager.Setup(m => 
                m.GetById(1))
                .Returns(new Sensor() { SensorId = 1, ParkingId = 2 });
            _controller = new ParkingsController(_mockParkingManager.Object, _mockSensorManager.Object);
            RawData data = new RawData() { Occupied = true, SensorId = 1 };

            // Act
            var createdResult = _controller.Post(data).Result;

            // Assert
            Assert.IsNotNull(createdResult);
            Assert.IsInstanceOfType(createdResult, typeof(ConflictResult));
        }
    }
}
