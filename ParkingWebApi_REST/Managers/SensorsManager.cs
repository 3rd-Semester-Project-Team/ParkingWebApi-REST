using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using WebApi_REST.Models;

namespace WebApi_REST.Managers
{
    public class SensorsManager
    {
        private static List<Sensor> _sensors = new List<Sensor>()
        {
            new Sensor() { SensorId = 1, ParkingId = 10 },
            new Sensor() { SensorId = 2, ParkingId = 22 }
        };

        public List<Sensor> GetAll()
        {
            return _sensors;
        }

        public Sensor GetById(int id)
        {
            return _sensors.FirstOrDefault(s => s.SensorId == id);
        }

        public Sensor Add(Sensor sensor)
        {
            if (GetById(sensor.SensorId) == null)
            {
                _sensors.Add(sensor);
                return sensor;
            }

            return null;
        }

        public void Delete(int id)
        {
            var sensor = GetById(id);
            if (sensor != null)
            {
                _sensors.Remove(sensor);
            }
        }

        public Sensor Edit(int id, Sensor newSensor)
        {
            var oldSensor = _sensors.FirstOrDefault(s => s.SensorId == id);
            if (oldSensor != null)
            {
                oldSensor.ParkingId = newSensor.ParkingId;
                return oldSensor;
            }

            return null;
        }

    }
}
