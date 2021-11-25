using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        [JsonPropertyName("sensorId")]
        public int SensorId { get; set; }
        /// <summary>
        /// Id of the parking slot, Foreign Key
        /// </summary>
        [Required]
        [JsonPropertyName("parkingId")]
        public int ParkingId { get; set; }
    }
}
