using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_REST.Models
{
    public class ParkingDbContext: DbContext
    {
        public DbSet<ParkingSlot> ParkingSlots { get; set; }
        public DbSet<Sensor> Sensors { get; set; }

        public ParkingDbContext(DbContextOptions<ParkingDbContext> options) 
            : base(options)
        {

        }


     

    }
}
