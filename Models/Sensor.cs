using System;

namespace WebApi_REST.Models
{
    /// <summary>
    /// Assigning a sensor to a parking slot
    /// </summary>
    public class Sensor
    {
        /// <summary>
        /// Id of the sensor
        /// </summary>
        public int SensorId { get; set; }
        /// <summary>
        /// Id of the parking slot
        /// </summary>
        public int ParkingId { get; set; }
    }
}
