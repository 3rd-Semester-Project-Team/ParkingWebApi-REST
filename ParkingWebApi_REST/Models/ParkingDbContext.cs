using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_REST.Models
{
    public class ParkingDbContext: DbContext
    {
        public virtual DbSet<ParkingSlot> ParkingSlots { get; set; }
        public virtual DbSet<Sensor> Sensors { get; set; }

        public ParkingDbContext(DbContextOptions<ParkingDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // setting a composite key for ParkingSlot consisting of ParkingId and Date
            modelBuilder.Entity<ParkingSlot>().HasKey(
                nameof(ParkingSlot.ParkingId), nameof(ParkingSlot.SensorDateTime));
        }
    }
}
