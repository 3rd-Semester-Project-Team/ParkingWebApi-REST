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
    public class SensorsControllerTests
    {
        private readonly Mock<ISensorsManager> _sensorsManager;
        private SensorsController _controller;

        public SensorsControllerTests()
        {
            _sensorsManager = new Mock<ISensorsManager>();
        }

        [TestMethod]
        public void GetAllSensors_Test()
        {
            // Arrange
            _sensorsManager.Setup(m => 
                m.GetAll())
                .Returns(_sensors);
            _controller = new SensorsController(_sensorsManager.Object);

            // Act
            var result = _controller.Get();

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreEqual(2, ((result.Result as ObjectResult).Value as IEnumerable<Sensor>).Count());
        }

        [TestMethod]
        public void GetById_ValidId_Test()
        {
            _sensorsManager.Setup(m =>
                    m.GetById(1))
                .Returns(_sensors[0]);
            _controller = new SensorsController(_sensorsManager.Object);

            var result = _controller.GetById(1).Result as ObjectResult;

            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(10, (result.Value as Sensor).ParkingId);
        }

        [TestMethod]
        public void GetById_InvalidId_Test()
        {
            _sensorsManager.Setup(m =>
                    m.GetById(10))
                .Returns(()=>null);
            _controller = new SensorsController(_sensorsManager.Object);

            var result = _controller.GetById(1).Result as StatusCodeResult;

            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        public void Post_Successful_Test()
        {
            Sensor sensor = new Sensor() { SensorId = 11, ParkingId = 50 };

            _sensorsManager.Setup(m =>
                    m.Add(sensor))
                .Returns(sensor);
            _controller = new SensorsController(_sensorsManager.Object);

            var result = _controller.Post(sensor).Result as ObjectResult;

            Assert.AreEqual(201, result.StatusCode);
            Assert.IsNotNull(result.Value);
        }

        [TestMethod]
        public void Post_WithConflict_Test()
        {
            _sensorsManager.Setup(m =>
                    m.Add(It.IsAny<Sensor>()))
                .Returns(() => null);
            _controller = new SensorsController(_sensorsManager.Object);

            var result = _controller.Post(new Sensor() {SensorId = 11, ParkingId = 2}).Result as ObjectResult;

            Assert.AreEqual(409, result.StatusCode);
        }
        
        private List<Sensor> _sensors = new List<Sensor>()
        {
            new Sensor() { SensorId = 1, ParkingId = 10 },
            new Sensor() { SensorId = 2, ParkingId = 22 }
        };

    }
}
