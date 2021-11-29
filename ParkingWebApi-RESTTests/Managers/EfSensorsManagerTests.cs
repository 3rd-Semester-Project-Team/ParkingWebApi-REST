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
    public class EfSensorsManagerTests
    {
        [TestMethod]
        public void AddNewSensor_Test()
        {
            var mockSet = new Mock<DbSet<Sensor>>();

            var mockContext = new Mock<ParkingDbContext>();

            mockContext.Setup(m => m.Sensors).Returns(mockSet.Object);

            var manager = new EFSensorsManager(mockContext.Object);
            var result = manager.Add(new Sensor() { SensorId = 1, ParkingId = 1 });

            mockSet.Verify(m=>m.Add(It.IsAny<Sensor>()), Times.Once);
            mockContext.Verify(m=>m.SaveChanges(), Times.Once);
            Assert.IsNotNull(result);

        }
    }

}
