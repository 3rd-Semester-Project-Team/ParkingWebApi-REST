using System;
using System.ComponentModel.DataAnnotations;


namespace WebApi_REST.Models
{
    /// <summary>
    /// Defines the info of a parking slot
    /// </summary>
    public class ParkingSlot
    {
        /// <summary>
        /// Id of the Parking slot
        /// </summary>
        [Required]
        [Key]
        public int ParkingId { get; set; }
        /// <summary>
        /// Time when the state of Occupied (boolean) is changed
        /// </summary>
        public DateTime SensorDateTime { get; set; }
        /// <summary>
        /// States if a parking slot is taken or not
        /// </summary>
        public bool Occupied { get; set; }
    }
}