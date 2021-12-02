﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi_REST.Models;

namespace WebApi_REST.Managers
{
    public class ParkingsManager : IParkingsManager
    {
        private static  int _nextId = 0;
        private static List<ParkingSlot> _parkingSlot = new List<ParkingSlot>()
        {
            new ParkingSlot(){ParkingId = ++_nextId, Occupied = false, SensorDateTime = DateTime.Now},
            new ParkingSlot() { ParkingId = ++_nextId, Occupied = false, SensorDateTime = DateTime.Now },
            new ParkingSlot() { ParkingId = ++_nextId, Occupied = false, SensorDateTime = DateTime.Now },
            new ParkingSlot() { ParkingId = ++_nextId, Occupied = false, SensorDateTime = DateTime.Now },
        };

        public IEnumerable<ParkingSlot> GetAll()
        {
            return _parkingSlot;
        }

        public IEnumerable<ParkingSlot> GetById(int id)
        {
            throw new NotImplementedException();
        }

        //public ParkingSlot GetById(int id)
        //{
        //    //Check in the controller if the object is null

        //    //if (_parkingSlot[id] != null) return _parkingSlot[id];
        //    var result = _parkingSlot.Find(f => f.ParkingId == id);
        //    return result;
        //   // else return new ParkingSlot();
        //}

        public ParkingSlot AddParkSlot(ParkingSlot parkingSlot)
        {
           //Check in the controller if the object is null

            _parkingSlot.Add(parkingSlot);
            return parkingSlot;
        }

        public IEnumerable<ParkingSlot> GetLatestParkingStatus()
        {
            throw new NotImplementedException();
        }
    }
}
