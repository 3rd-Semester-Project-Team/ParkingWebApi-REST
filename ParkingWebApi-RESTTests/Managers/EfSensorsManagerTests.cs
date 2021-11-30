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
    public class EfSensorsManagerTests
    {
        private IQueryable<Sensor> _data;
        private List<Sensor> _sourceList;
        private Mock<DbSet<Sensor>> _mockSet;
        private Mock<ParkingDbContext> _mockContext;

        #region Test Initialize
        [TestInitialize]
        public void Setup()
        {
            // Source of mock data
            _sourceList = new List<Sensor>
            {
                new Sensor() {SensorId = 1, ParkingId = 1},
                new Sensor() {SensorId = 2, ParkingId = 2},
                new Sensor() {SensorId = 3, ParkingId = 3}
            };

            #region DbSet Queryable behaviour
            // mock data as queryable, required for mocking
            _data = _sourceList.AsQueryable();

            // setting up mock behaviour for the db set
            _mockSet = new Mock<DbSet<Sensor>>();
            _mockSet.As<IQueryable<Sensor>>().Setup(m => m.Provider).Returns(_data.Provider);
            _mockSet.As<IQueryable<Sensor>>().Setup(m => m.Expression).Returns(_data.Expression);
            _mockSet.As<IQueryable<Sensor>>().Setup(m => m.ElementType).Returns(_data.ElementType);
            _mockSet.As<IQueryable<Sensor>>().Setup(m => m.GetEnumerator()).Returns(_data.GetEnumerator());
            #endregion

            #region Mocking EntityEntry<Sensor>
            var iStateManager = new Mock<IStateManager>();
            var model = new Mock<Model>();
            var sensorEntityEntry = new Mock<EntityEntry<Sensor>>(
                new InternalShadowEntityEntry(iStateManager.Object,new EntityType("Sensor", model.Object, ConfigurationSource.Convention)));
            sensorEntityEntry.SetupGet(m => m.Entity).Returns(new Sensor());
            #endregion

            #region Mocking method behaviour
            // Setup: DbSet Find method
            _mockSet.Setup(set => set.Find(It.IsAny<object[]>()))
                .Returns((object[] input) => _data.FirstOrDefault(s => s.SensorId == (Int32)input[0]));

            // Setup: DbSet Add method
            _mockSet.Setup(set => set.Add(It.IsAny<Sensor>()))
                .Returns(sensorEntityEntry.Object)
                .Callback((Sensor s) => _sourceList.Add(s));

            // Setup: DbSet Remove method
            _mockSet.Setup(set => set.Remove(It.IsAny<Sensor>()))
                .Returns(sensorEntityEntry.Object)
                .Callback((Sensor s) => _sourceList.Remove(s));

            // Setup: DbSet Update method
            _mockSet.Setup(set => set.Update(It.IsAny<Sensor>())).Returns(sensorEntityEntry.Object)
                .Callback((Sensor s) =>
                {
                    var old = _sourceList.FirstOrDefault(e => e.SensorId == s.SensorId);
                    if (old != null)
                    {
                        old.ParkingId = s.ParkingId;
                    }
                });

            #endregion

            // Creating a Mock of the DbContext
            _mockContext = new Mock<ParkingDbContext>(new DbContextOptions<ParkingDbContext>());
        }
        #endregion

        [TestMethod]
        public void GetAllSensors_Test()
        {
            _mockContext.Setup(m => m.Sensors).Returns(_mockSet.Object);
            EFSensorsManager manager = new EFSensorsManager(_mockContext.Object);

            var sensors = manager.GetAll();

            Assert.AreEqual(3, sensors.Count);
            Assert.AreEqual(1, sensors[0].ParkingId);
        }

        [DataTestMethod]
        [DataRow(1,1)]
        [DataRow(2,2)]
        public void GetById_Existing_Test(int sensorId, int parkingId )
        {
            _mockContext.Setup(c => c.Sensors).Returns(_mockSet.Object);
            EFSensorsManager manager = new EFSensorsManager(_mockContext.Object);
            
            var sensor = manager.GetById(sensorId);
            
            Assert.AreEqual(parkingId, sensor.ParkingId);
        }

        [TestMethod]
        public void GetById_NotExisting_Test()
        {
            _mockContext.Setup(c => c.Sensors).Returns(_mockSet.Object);
            EFSensorsManager manager = new EFSensorsManager(_mockContext.Object);
            int sensorId = 10;

            var sensor = manager.GetById(sensorId);

            _mockSet.Verify(set=>set.Find(It.IsAny<object[]>()), Times.Once);
            Assert.IsNull(sensor);
        }

        [TestMethod]
        public void Add_ValidSensorId_Test()
        {
            _mockContext.Setup(c => c.Sensors).Returns(_mockSet.Object);
            EFSensorsManager manager = new EFSensorsManager(_mockContext.Object);

            manager.Add(new Sensor() { SensorId = 4, ParkingId = 4 });
            
            _mockSet.Verify(s=>s.Add(It.IsAny<Sensor>()), Times.Once());
            _mockContext.Verify((c=>c.SaveChanges()), Times.Once);
            Assert.AreEqual(4, _data.Count());
        }

        [TestMethod]
        public void Add_InvalidSensorId_Test() // Invalid means it already exists
        {
            _mockContext.Setup(c => c.Sensors).Returns(_mockSet.Object);
            EFSensorsManager manager = new EFSensorsManager(_mockContext.Object);

            var result = manager.Add(new Sensor() { SensorId = 1, ParkingId = 4 });
            
            _mockSet.Verify(s => s.Add(It.IsAny<Sensor>()), Times.Never);
            _mockContext.Verify((c => c.SaveChanges()), Times.Never);
            Assert.AreEqual(3, _data.Count());
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Remove_ExistingId_Test()
        {
            _mockContext.Setup(c => c.Sensors).Returns(_mockSet.Object);
            EFSensorsManager manager = new EFSensorsManager(_mockContext.Object);
            int deleteId = 1;

            manager.Delete(deleteId);
            
            _mockSet.Verify(s => s.Remove(It.IsAny<Sensor>()), Times.Once());
            _mockContext.Verify((c => c.SaveChanges()), Times.Once);
            Assert.AreEqual(2, _data.Count());
        }

        [TestMethod]
        public void Remove_NotExistingId_Test()
        {
            _mockContext.Setup(c => c.Sensors).Returns(_mockSet.Object);
            EFSensorsManager manager = new EFSensorsManager(_mockContext.Object);
            int deleteId = 10;

            manager.Delete(deleteId);
            
            _mockSet.Verify(s => s.Remove(It.IsAny<Sensor>()), Times.Never);
            _mockContext.Verify((c => c.SaveChanges()), Times.Never);
            Assert.AreEqual(3, _data.Count());
        }

        [TestMethod]
        public void Update_ExistingId_Test()
        {
            _mockContext.Setup(c => c.Sensors).Returns(_mockSet.Object);
            EFSensorsManager manager = new EFSensorsManager(_mockContext.Object);
            var sensor = new Sensor() { SensorId = 1, ParkingId = 10 };

            manager.Edit(sensor.SensorId, sensor);

            _mockSet.Verify(s => s.Update(It.IsAny<Sensor>()), Times.Once);
            _mockContext.Verify((c => c.SaveChanges()), Times.Once);
            Assert.AreEqual(10, _sourceList[0].ParkingId);
        }

        [TestMethod]
        public void Update_NotExistingId_Test()
        {
            _mockContext.Setup(c => c.Sensors).Returns(_mockSet.Object);
            EFSensorsManager manager = new EFSensorsManager(_mockContext.Object);
            var sensor = new Sensor() { SensorId = 10, ParkingId = 10 };

            manager.Edit(sensor.SensorId, sensor);

            _mockSet.Verify(s => s.Update(It.IsAny<Sensor>()), Times.Never);
            _mockContext.Verify((c => c.SaveChanges()), Times.Never);
        }

    }

}
