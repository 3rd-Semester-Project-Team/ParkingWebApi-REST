using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApi_REST.Managers;
using WebApi_REST.Models;

namespace ParkingWebApi_RESTTests.Managers
{
    [TestClass]
    public class SensorsManagerTests
    {
        private SensorsManager _manager;

        [TestInitialize]
        public void Setup()
        {
            _manager = new SensorsManager();
        }

        [TestMethod]
        public void GetAllSensors_Test()
        {
            var result = _manager.GetAll();

            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void GetById_ExistingId_Test()
        {
            var result = _manager.GetById(1);

            Assert.AreEqual(1, result.SensorId);
        }

        [TestMethod]
        public void GetById_InvalidId_Test()
        {
            var result = _manager.GetById(100);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Edit_ValidId_Test()
        {
            Sensor newSensor = new Sensor() { SensorId = 1, ParkingId = 100 };

            _manager.Edit(1, newSensor);
            var edited = _manager.GetById(1);

            Assert.AreEqual(100, edited.ParkingId);
        }

        [TestMethod]
        public void Edit_InvalidId_Test()
        {
            Sensor sensor = new Sensor() { SensorId = 30, ParkingId = 1 };
            _manager.Edit(30, sensor);
            Assert.AreEqual(2, _manager.GetAll().Count);
            Assert.IsNull(_manager.GetById(30));
        }

        [TestMethod]
        public void Delete_ValidId_Test()
        {
            _manager.Delete(1);

            Assert.AreEqual(1, _manager.GetAll().Count);
        }

        [TestMethod]
        public void Delete_InvalidId_Test()
        {
            _manager.Delete(13);
            var expected = _manager.GetAll().Count;
            Assert.AreEqual(expected, _manager.GetAll().Count);
        }

        [TestMethod]
        public void Add_WithValidId_Test()
        {
            Sensor sensor = new Sensor() { SensorId = 3, ParkingId = 3 };
            var oldCount = _manager.GetAll().Count;

            var result = _manager.Add(sensor);

            Assert.AreEqual(oldCount +1, _manager.GetAll().Count);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Add_WithInvalidId_Test()
        {
            Sensor sensor = new Sensor() { SensorId = 2, ParkingId = 3 };
            var oldCount = _manager.GetAll().Count;

            var result = _manager.Add(sensor);

            Assert.AreEqual(oldCount, _manager.GetAll().Count);
            Assert.IsNull(result);
        }






    }
}
