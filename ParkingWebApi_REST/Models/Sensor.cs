using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi_REST.Models
{
    /// <summary>
    /// Assigning a sensor to a parking slot
    /// </summary>
    public class Sensor
    {
        /// <summary>
        /// Id of the sensor, Primary Key
        /// </summary>
        [Required]
        public int SensorId { get; set; }
        /// <summary>
        /// Id of the parking slot, Foreign Key
        /// </summary>
        [Required]
        public int ParkingId { get; set; }
    }
}
