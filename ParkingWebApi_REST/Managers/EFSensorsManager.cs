using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebApi_REST.Models;

namespace WebApi_REST.Managers
{
    public class EFSensorsManager :ISensorsManager
    {
        private ParkingDbContext _context;

        public EFSensorsManager(ParkingDbContext context)
        {
            _context = context;
        }

        public Sensor Add(Sensor sensor)
        {
            var newSensor = _context.Sensors.Add(sensor);
            _context.SaveChanges();
            return newSensor.Entity;
        }

        public void Delete(int id)
        {
          var sensorToDelete = _context.Sensors.Find(id);
          _context.Sensors.Remove(sensorToDelete);
          _context.SaveChanges();

        }

        public Sensor Edit(int id, Sensor newSensor)
        {
            var updatedSensor = _context.Sensors.Update(newSensor);
            _context.SaveChanges();
            return updatedSensor.Entity;
        }

        public List<Sensor> GetAll()
        {
            return _context.Sensors.ToList();
        }

        public Sensor GetById(int id)
        {
            return _context.Sensors.Find(id);
        }
    }
}
