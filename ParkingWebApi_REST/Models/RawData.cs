using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebApi_REST.Models
{
    public class RawData
    {
        [JsonPropertyName("sensorId")]
        public int SensorId { get; set; }

        [JsonPropertyName("occupied")]
        public bool Occupied { get; set; }

    }
}
