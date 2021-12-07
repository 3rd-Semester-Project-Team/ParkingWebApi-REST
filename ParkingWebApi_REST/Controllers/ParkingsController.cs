using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using WebApi_REST.Managers;
using WebApi_REST.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi_REST.Controllers
{
    /// <summary>
    /// API Controller for Parking slots. 
    /// </summary>
    
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingsController : ControllerBase
    {
        /// <summary>
        /// Instance field referencing the IParkingsManager
        /// </summary>
        private readonly IParkingsManager _parkingManager;
        /// <summary>
        /// Instance field referencing the ISensorsManager
        /// </summary>
        private readonly ISensorsManager _sensorsManager;

        /// <summary>
        /// API Controller constructor, using dependency injection
        /// </summary>
        /// <param name="manager">This is the ParkingsManager that we inject, must implement IParkinsgManager</param>
        /// <param name="sensorsManager">This is the SensorsManager that we inject, must implement ISensorsManager</param>

        public ParkingsController(IParkingsManager manager, ISensorsManager sensorsManager)  
        {
            _parkingManager = manager;
            _sensorsManager = sensorsManager;
        }
        /// <summary>
        /// Http Get Method, get all ParkingSlots
        /// </summary>
        /// <returns>Status Code and all the ParkingSlots (if Status is OK)</returns>
        // GET: api/<ParkingsController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<IEnumerable<ParkingSlot>>  GetAll()
        {
            var result= _parkingManager.GetAll();
            if (result == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(result);
            }
        }
        /// <summary>
        /// Http Get method, get by id
        /// </summary>
        /// <param name="id">the Parking Slot id</param>
        /// <returns>Status Code + all ParkingSlots that match the Id from the Search Criteria (if status is OK)</returns>
        // GET api/<ParkingsController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<ParkingSlot>> GetById(int id)
        {
            var result = _parkingManager.GetById(id);
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
            
        }

        /// <summary>
        /// Http Get Method, with the last status for each ParkingSlot
        /// </summary>
        /// <returns>Status Code + a ParkingSlot collection with the last register of each in the Db (if Status is OK)</returns>

        [HttpGet("latest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<ParkingSlot>> GetLatest()
        {
            return Ok(_parkingManager.GetLatestParkingStatus());
        }

        /// <summary>
        /// Http Post Method 
        /// </summary>
        /// <param name="data">JSON format data, SensorId and Occupied(bool) </param>
        /// <returns>Status Code + ParkingSlot object (if 201) and adding new data to the Database every time the status(occupied) of ParkingSlot Changes</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<ParkingSlot> Post([FromBody] RawData data)
        {
            ParkingSlot newParkingSlot = new ParkingSlot();

            var sensor = _sensorsManager.GetById(data.SensorId);
            if (sensor == null) return Conflict($"Sensor id {data.SensorId} does not exist.");
            newParkingSlot.ParkingId = sensor.ParkingId;
            newParkingSlot.Occupied = data.Occupied;
            newParkingSlot.SensorDateTime = DateTime.Now;

            var result = _parkingManager.AddParkSlot(newParkingSlot);

            if (result == null)
            {
                return Conflict();
            }
            else
            {
                return Created(result.ParkingId.ToString(), result);
            }
        }
    }
}
