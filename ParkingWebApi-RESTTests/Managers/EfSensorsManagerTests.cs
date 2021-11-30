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

        #region Test Initialize
        //[TestInitialize]
        //public void Setup()
        //{
        //    // Source of mock data
        //    _sensorsList = new List<Sensor>
        //    {
        //        new Sensor() {SensorId = 1, ParkingId = 1},
        //        new Sensor() {SensorId = 2, ParkingId = 2},
        //        new Sensor() {SensorId = 3, ParkingId = 3}
        //    };

        //    #region DbSet Queryable behaviour
        //    // mock data as queryable, required for mocking
        //    _sensors = _sensorsList.AsQueryable();

        //    // setting up mock behaviour for the db set
        //    _mockSensorsSet = new Mock<DbSet<Sensor>>();
        //    _mockSensorsSet.As<IQueryable<Sensor>>().Setup(m => m.Provider).Returns(_sensors.Provider);
        //    _mockSensorsSet.As<IQueryable<Sensor>>().Setup(m => m.Expression).Returns(_sensors.Expression);
        //    _mockSensorsSet.As<IQueryable<Sensor>>().Setup(m => m.ElementType).Returns(_sensors.ElementType);
        //    _mockSensorsSet.As<IQueryable<Sensor>>().Setup(m => m.GetEnumerator()).Returns(_sensors.GetEnumerator());
        //    #endregion

        //    #region Mocking EntityEntry<Sensor>
        //    var iStateManager = new Mock<IStateManager>();
        //    var model = new Mock<Model>();
        //    var sensorEntityEntry = new Mock<EntityEntry<Sensor>>(
        //        new InternalShadowEntityEntry(iStateManager.Object,new EntityType("Sensor", model.Object, ConfigurationSource.Convention)));
        //    sensorEntityEntry.SetupGet(m => m.Entity).Returns(new Sensor());
        //    #endregion

        //    #region Mocking method behaviour
        //    // Setup: DbSet Find method
        //    _mockSensorsSet.Setup(set => set.Find(It.IsAny<object[]>()))
        //        .Returns((object[] input) => _sensors.FirstOrDefault(s => s.SensorId == (Int32)input[0]));

        //    // Setup: DbSet Add method
        //    _mockSensorsSet.Setup(set => set.Add(It.IsAny<Sensor>()))
        //        .Returns(sensorEntityEntry.Object)
        //        .Callback((Sensor s) => _sensorsList.Add(s));

        //    // Setup: DbSet Remove method
        //    _mockSensorsSet.Setup(set => set.Remove(It.IsAny<Sensor>()))
        //        .Returns(sensorEntityEntry.Object)
        //        .Callback((Sensor s) => _sensorsList.Remove(s));

        //    // Setup: DbSet Update method
        //    _mockSensorsSet.Setup(set => set.Update(It.IsAny<Sensor>())).Returns(sensorEntityEntry.Object)
        //        .Callback((Sensor s) =>
        //        {
        //            var old = _sensorsList.FirstOrDefault(e => e.SensorId == s.SensorId);
        //            if (old != null)
        //            {
        //                old.ParkingId = s.ParkingId;
        //            }
        //        });

        //    #endregion

        //    // Creating a Mock of the DbContext
        //    _mockContext = new Mock<ParkingDbContext>(new DbContextOptions<ParkingDbContext>());
        //}
        #endregion

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
