using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebApi_REST.Models;

namespace WebApi_REST.Managers
{
    /// <summary>
    /// Manager class that inherits and implements IParkingsManager.
    /// </summary>
    public class EFParkingsManager : IParkingsManager
    {
        /// <summary>
        /// An instance of the ParkingsDBContext. This is needed to allow the manager access to the database.
        /// </summary>
        private ParkingDbContext _context;

        /// <summary>
        /// The instance of the ParkingsDBContext is initialized in the constructor.
        /// </summary>
        /// <param name="context"></param>
        public EFParkingsManager(ParkingDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds new data to be database every time the status of a parkingSlot changes.
        /// </summary>
        /// <param name="parkingSlot"></param>
        /// <returns> The newly added data from the parkingSlot whose status recently changed</returns>
        public ParkingSlot AddParkSlot(ParkingSlot parkingSlot)
        {
            var newParkingData = _context.ParkingSlots.Add(parkingSlot);
            _context.SaveChanges();
            return newParkingData.Entity;
        }

        /// <summary>
        /// Retrieves all the parkingSlots from the database.
        /// </summary>
        /// <returns>A collection of all the parkingSlots</returns>
        public IEnumerable<ParkingSlot> GetAll()
        {
            return _context.ParkingSlots;
        }

        /// <summary>
        /// Retrieves from the database the parkingSlot that has an Id that matches the Id that is the search criteria.
        /// </summary>
        /// <param name="id"></param>
        /// <returns> The parkingSlot whose Id matches the Id being searched</returns>
        public IEnumerable<ParkingSlot> GetById(int id)
        {
            return _context.ParkingSlots.Where(p=>p.ParkingId==id);
        }

        /// <summary>
        /// Retrieves from the database the most recent data from each parkingSlot.
        /// </summary>
        /// <returns>A collection of the most recent data of each parkingSLot</returns>
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
