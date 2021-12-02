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
using Moq;
using WebApi_REST.Models;

namespace ParkingWebApi_RESTTests.Managers
{
    public class ManagerTestBase
    {
        public IQueryable<Sensor> Sensors { get; set; }
        public IQueryable<ParkingSlot> ParkingSlots { get; set; }
        public List<Sensor> SensorsList { get; set; }
        public List<ParkingSlot> ParkingSlotList { get; set; }

        public Mock<DbSet<Sensor>> MockSensors { get; set; }
        public Mock<DbSet<ParkingSlot>> MockParkingSlots { get; set; }
        public Mock<ParkingDbContext> MockContext { get; set; }

        public ManagerTestBase()
        {
            // Source of mock data
            SensorsList = new List<Sensor>
            {
                new Sensor() {SensorId = 1, ParkingId = 1},
                new Sensor() {SensorId = 2, ParkingId = 2},
                new Sensor() {SensorId = 3, ParkingId = 3}
            };

            ParkingSlotList = new List<ParkingSlot>()
            {
                new ParkingSlot() {ParkingId = 1, SensorDateTime = new DateTime(2021,11,30,14,01,00), Occupied = true},
                new ParkingSlot() {ParkingId = 1, SensorDateTime = new DateTime(2021,11,30,14,02,00), Occupied = false},
                new ParkingSlot() {ParkingId = 1, SensorDateTime = new DateTime(2021,11,30,14,03,00), Occupied = true}
            };

            #region DbSet Queryable behaviour
            // mock data as queryable, required for mocking
            Sensors = SensorsList.AsQueryable();
            ParkingSlots = ParkingSlotList.AsQueryable();

            // setting up mock queryable behaviour for the db set
            MockSensors = new Mock<DbSet<Sensor>>();
            MockSensors.As<IQueryable<Sensor>>()
                .Setup(m => m.Provider)
                .Returns(Sensors.Provider);
            MockSensors.As<IQueryable<Sensor>>()
                .Setup(m => m.Expression)
                .Returns(Sensors.Expression);
            MockSensors.As<IQueryable<Sensor>>()
                .Setup(m => m.ElementType)
                .Returns(Sensors.ElementType);
            MockSensors.As<IQueryable<Sensor>>()
                .Setup(m => m.GetEnumerator())
                .Returns(Sensors.GetEnumerator());

            MockParkingSlots = new Mock<DbSet<ParkingSlot>>();
            MockParkingSlots.As<IQueryable<ParkingSlot>>()
                .Setup(m => m.Provider)
                .Returns(ParkingSlots.Provider);
            MockParkingSlots.As<IQueryable<ParkingSlot>>()
                .Setup(m => m.Expression)
                .Returns(ParkingSlots.Expression);
            MockParkingSlots.As<IQueryable<ParkingSlot>>()
                .Setup(m => m.ElementType)
                .Returns(ParkingSlots.ElementType);
            MockParkingSlots.As<IQueryable<ParkingSlot>>()
                .Setup(m => m.GetEnumerator())
                .Returns(ParkingSlots.GetEnumerator);

            #endregion

            #region Mocking EntityEntry<Sensor>
            var iStateManager = new Mock<IStateManager>();
            var model = new Mock<Model>();
            var sensorEntityEntry = new Mock<EntityEntry<Sensor>>(
                new InternalShadowEntityEntry(iStateManager.Object, new EntityType("Sensor", model.Object, ConfigurationSource.Convention)));
            sensorEntityEntry.SetupGet(m => m.Entity).Returns(new Sensor());

            var parkingEntityEntry = new Mock<EntityEntry<ParkingSlot>>(
                new InternalShadowEntityEntry(iStateManager.Object, new EntityType("ParkingSlot", model.Object,
                    ConfigurationSource.Convention)));
            parkingEntityEntry.SetupGet(m => m.Entity).Returns(new ParkingSlot());
            #endregion

            #region Mocking DbSet<Sensor> methods behaviour
            // Setup: DbSet Find method
            MockSensors.Setup(set => set.Find(It.IsAny<object[]>()))
                .Returns((object[] input) => Sensors.FirstOrDefault(s => s.SensorId == (Int32)input[0]));

            // Setup: DbSet Add method
            MockSensors.Setup(set => set.Add(It.IsAny<Sensor>()))
                .Returns(sensorEntityEntry.Object)
                .Callback((Sensor s) => SensorsList.Add(s));

            // Setup: DbSet Remove method
            MockSensors.Setup(set => set.Remove(It.IsAny<Sensor>()))
                .Returns(sensorEntityEntry.Object)
                .Callback((Sensor s) => SensorsList.Remove(s));

            // Setup: DbSet Update method
            MockSensors.Setup(set => set.Update(It.IsAny<Sensor>())).Returns(sensorEntityEntry.Object)
                .Callback((Sensor s) =>
                {
                    var old = SensorsList.FirstOrDefault(e => e.SensorId == s.SensorId);
                    if (old != null)
                    {
                        old.ParkingId = s.ParkingId;
                    }
                });

            #endregion

            #region Mocking DbSet<ParkingSlot> behaviour
            //MockParkingSlots.Setup(set => set.Find(It.IsAny<object[]>()))
            //    .Returns((object[] input) => ParkingSlots.FirstOrDefault(p => p.ParkingId == (Int32)input[0]));

            MockParkingSlots.Setup(set => set.Add(It.IsAny<ParkingSlot>()))
                .Returns(parkingEntityEntry.Object)
                .Callback((ParkingSlot slot) => ParkingSlotList.Add(slot));
            #endregion

            // Creating a Mock of the DbContext
            MockContext = new Mock<ParkingDbContext>(new DbContextOptions<ParkingDbContext>());
        }
    }
}
