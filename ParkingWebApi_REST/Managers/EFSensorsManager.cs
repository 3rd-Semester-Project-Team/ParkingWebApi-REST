using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi_REST.Models;

namespace WebApi_REST.Managers
{
    /// <summary>
    /// Manager class that inherits and implements ISensorsManager.
    /// </summary>
    public class EFSensorsManager :ISensorsManager
    {
        /// <summary>
        /// An instance of the ParkingsDBContext. This is needed to allow the manager access to the database.
        /// </summary>
        private ParkingDbContext _context;

        /// <summary>
        /// The instance of the ParkingsDBContext is initialized in the constructor.
        /// </summary>
        /// <param name="context"></param>
        public EFSensorsManager(ParkingDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new sensor the the database, if the sensor Id does not already exist.
        /// </summary>
        /// <param name="sensor"></param>
        /// <returns> the newly added sensor</returns>
        public Sensor Add(Sensor sensor)
        {
            if (_context.Sensors.Find(sensor.SensorId) != null)
            {
                return null;
            }
            var newSensor = _context.Sensors.Add(sensor);
            _context.SaveChanges();
            return newSensor.Entity;
        }

        /// <summary>
        /// Removes from the database the sensor whose Id matches the Id being searched.
        /// </summary>
        /// <param name="id">Does not return anything</param>
        public void Delete(int id)
        {
            var sensorToDelete = _context.Sensors.Find(id);
            if (sensorToDelete != null)
            {
                _context.Sensors.Remove(sensorToDelete);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Retrieves from the database the sensor whose Id matched the search criteria 
        /// then updates the sensor, and saves the updates.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newSensor"></param>
        /// <returns>The newly edited sensor</returns>
        public Sensor Edit(int id, Sensor newSensor)
        {
            if (_context.Sensors.Find(newSensor.SensorId) == null)
            {
                return null;
            }
            var updatedSensor = _context.Sensors.Update(newSensor);
            _context.SaveChanges();
            return updatedSensor.Entity;
        }

        /// <summary>
        /// Retrieves all the sensors from the database.
        /// </summary>
        /// <returns>A collection of all the sensors</returns>
        public List<Sensor> GetAll()
        {
            return _context.Sensors.ToList();
        }

        /// <summary>
        /// Retrieves from the database the sensor that has an Id that matches the Id that is the search criteria.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The sensor that whose Id matches and null when not found</returns>
        public Sensor GetById(int id)
        {
            return _context.Sensors.Find(id);
        }
    }
}
