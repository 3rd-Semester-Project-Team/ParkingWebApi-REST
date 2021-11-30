using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApi_REST.Managers;
using WebApi_REST.Models;

namespace ParkingWebApi_RESTTests.Managers
{
    [TestClass]
    public class EfParkingsManagerTests : ManagerTestBase
    {
        private IQueryable<ParkingSlot> _parkingSlots;
        private List<ParkingSlot> _parkingSlotsList;
        private Mock<DbSet<ParkingSlot>> _mockParkingSlotsSet;
        private Mock<ParkingDbContext> _mockContext;
        private EFParkingsManager _manager;

        public EfParkingsManagerTests()
        {
            _parkingSlots = ParkingSlots;
            _parkingSlotsList = ParkingSlotList;
            _mockParkingSlotsSet = MockParkingSlots;
            _mockContext = MockContext;

            _mockContext.Setup(c => c.ParkingSlots).Returns(_mockParkingSlotsSet.Object);
            _manager = new EFParkingsManager(_mockContext.Object);
        }

        [TestMethod]
        public void GetAllParkingSlots_Test()
        {
            var result = _manager.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
        }

        //[TestMethod]
        //public void GetById_ExistingId_Test()
        //{
        //    _mockContext.Setup(c => c.ParkingSlots).Returns(_mockParkingSlotsSet.Object);
        //    EFParkingsManager manager = new EFParkingsManager(_mockContext.Object);
        //    int requestId = 1;

        //    var result = manager.GetById(requestId);

        //    Assert.IsNotNull(result);
        //}

        [TestMethod]
        public void AddParkingSlot_Test()
        {
            var result = _manager.AddParkSlot(new ParkingSlot()
                { ParkingId = 10, Occupied = false, SensorDateTime = DateTime.Now });

            _mockParkingSlotsSet.Verify(set=>set.Add(It.IsAny<ParkingSlot>()), Times.Once);
            _mockContext.Verify(c=>c.SaveChanges(), Times.Once);
        }




    }
}
