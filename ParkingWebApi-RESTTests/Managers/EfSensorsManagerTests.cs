using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using WebApi_REST.Managers;
using WebApi_REST.Models;

namespace ParkingWebApi_RESTTests.Managers
{
    [TestClass]
    public class EfSensorsManagerTests : ManagerTestBase
    {
        private IQueryable<Sensor> _sensors;
        private List<Sensor> _sensorsList;
        private Mock<DbSet<Sensor>> _mockSensorsSet;
        private Mock<ParkingDbContext> _mockContext;
        private EFSensorsManager _manager;

        public EfSensorsManagerTests()
        {
            _sensors = base.Sensors;
            _sensorsList = base.SensorsList;
            _mockSensorsSet = base.MockSensors;
            _mockContext = base.MockContext;

            _mockContext.Setup(m => m.Sensors).Returns(_mockSensorsSet.Object);
            _manager = new EFSensorsManager(_mockContext.Object);
        }
        
        [TestMethod]
        public void GetAllSensors_Test()
        {
            var sensors = _manager.GetAll();

            Assert.AreEqual(3, sensors.Count);
            Assert.AreEqual(1, sensors[0].ParkingId);
        }

        [DataTestMethod]
        [DataRow(1,1)]
        [DataRow(2,2)]
        public void GetById_Existing_Test(int sensorId, int parkingId )
        {
            var sensor = _manager.GetById(sensorId);
            
            Assert.AreEqual(parkingId, sensor.ParkingId);
        }

        [TestMethod]
        public void GetById_NotExisting_Test()
        {
            int sensorId = 10;

            var sensor = _manager.GetById(sensorId);

            _mockSensorsSet.Verify(set=>set.Find(It.IsAny<object[]>()), Times.Once);
            Assert.IsNull(sensor);
        }

        [TestMethod]
        public void Add_ValidSensorId_Test()
        {
            _manager.Add(new Sensor() { SensorId = 4, ParkingId = 4 });
            
            _mockSensorsSet.Verify(s=>s.Add(It.IsAny<Sensor>()), Times.Once());
            _mockContext.Verify((c=>c.SaveChanges()), Times.Once);
            Assert.AreEqual(4, _sensors.Count());
        }

        [TestMethod]
        public void Add_InvalidSensorId_Test() // Invalid means it already exists
        {
            var result = _manager.Add(new Sensor() { SensorId = 1, ParkingId = 4 });
            
            _mockSensorsSet.Verify(s => s.Add(It.IsAny<Sensor>()), Times.Never);
            _mockContext.Verify((c => c.SaveChanges()), Times.Never);
            Assert.AreEqual(3, _sensors.Count());
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Remove_ExistingId_Test()
        {
            int deleteId = 1;

            _manager.Delete(deleteId);
            
            _mockSensorsSet.Verify(s => s.Remove(It.IsAny<Sensor>()), Times.Once());
            _mockContext.Verify((c => c.SaveChanges()), Times.Once);
            Assert.AreEqual(2, _sensors.Count());
        }

        [TestMethod]
        public void Remove_NotExistingId_Test()
        {
            int deleteId = 10;

            _manager.Delete(deleteId);
            
            _mockSensorsSet.Verify(s => s.Remove(It.IsAny<Sensor>()), Times.Never);
            _mockContext.Verify((c => c.SaveChanges()), Times.Never);
            Assert.AreEqual(3, _sensors.Count());
        }

        [TestMethod]
        public void Update_ExistingId_Test()
        {
            var sensor = new Sensor() { SensorId = 1, ParkingId = 10 };
            _manager.Edit(sensor.SensorId, sensor);

            _mockSensorsSet.Verify(s => s.Update(It.IsAny<Sensor>()), Times.Once);
            _mockContext.Verify((c => c.SaveChanges()), Times.Once);
            Assert.AreEqual(10, _sensorsList[0].ParkingId);
        }

        [TestMethod]
        public void Update_NotExistingId_Test()
        {
            var sensor = new Sensor() { SensorId = 10, ParkingId = 10 };

            _manager.Edit(sensor.SensorId, sensor);

            _mockSensorsSet.Verify(s => s.Update(It.IsAny<Sensor>()), Times.Never);
            _mockContext.Verify((c => c.SaveChanges()), Times.Never);
        }

    }

}
