using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebApi_REST.Models;

namespace WebApi_REST.Managers
{
    public class EFParkingsManager : IParkingsManager
    {
        private ParkingDbContext _context;

        public EFParkingsManager(ParkingDbContext context)
        {
            _context = context;
        }
        public ParkingSlot AddParkSlot(ParkingSlot parkingSlot)
        {
          var newParkingData = _context.ParkingSlots.Add(parkingSlot);
            _context.SaveChanges();
            return newParkingData.Entity;
        }

        public IEnumerable<ParkingSlot> GetAll()
        {
            return _context.ParkingSlots;
        }

        public ParkingSlot GetById(int id)
        {
            return _context.ParkingSlots.Find(id);
        }
    }
}
