using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebApi_REST.Models
{
    /// <summary>
    /// Raw data incoming from the sensor of each parking slot.
    /// </summary>
    public class RawData
    {
        /// <summary>
        /// Id of the sensor assigned to a parking slot.
        /// </summary>
        [JsonPropertyName("sensorId")]
        public int SensorId { get; set; }

        /// <summary>
        /// Status of the state of the parking slot. Occupied = False when the slot is empty, Occupied = True when the parking slot is taken.
        /// </summary>
        [JsonPropertyName("occupied")]
        public bool Occupied { get; set; }

    }
}
