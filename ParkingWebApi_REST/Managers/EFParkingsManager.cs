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

        public IEnumerable<ParkingSlot> GetById(int id)
        {
            return _context.ParkingSlots.Where(p=>p.ParkingId==id);
        }

        public IEnumerable<ParkingSlot> GetLatestParkingStatus()
        {
            List<ParkingSlot> resultList = new List<ParkingSlot>();

            var allParkings = _context.ParkingSlots;
            var allIds = new List<int>();
            foreach (var slot in allParkings)
            {
                if (!allIds.Contains(slot.ParkingId))
                {
                    allIds.Add(slot.ParkingId);
                }
            }
            allIds.Sort();
            foreach(int i in allIds)
            {
                var list = GetById(i);
                list.OrderBy(p => p.SensorDateTime).ToList();
                var latestPost = list.Last();
                resultList.Add(latestPost);
            }

            return resultList;
        }
    }
}
