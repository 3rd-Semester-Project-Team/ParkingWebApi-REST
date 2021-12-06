using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
    public class SensorsController : ControllerBase
    {
        /// <summary>
        /// Instance field referencing the ISensorsManager
        /// </summary>
        private readonly ISensorsManager _manager;

        /// <summary>
        /// API Controller constructor, using dependency injection
        /// </summary>
        /// <param name="manager">This is the SensorsManager that we inject, must implement ISensorsManager</param>

        public SensorsController(ISensorsManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Http Get Method, get all Sensors
        /// </summary>
        /// <returns>Status Code and all the Sensors (if Status is OK)</returns>
        // GET: api/<SensorsController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<IEnumerable<Sensor>> Get()
        {
            var result = _manager.GetAll();
            if (result != null)
            {
                return Ok(result);
            }
            return NoContent();
        }


        /// <summary>
        /// Http Get method, get by id
        /// </summary>
        /// <param name="id">id use to search for a sensor</param>
        /// <returns>Status Code + Sensor that matches the Id from the Search Criteria (if status is OK)</returns>
        // GET api/<SensorsController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Sensor> GetById(int id)
        {
            var result = _manager.GetById(id);
            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }


        /// <summary>
        /// Http Post Method 
        /// </summary>
        /// <param name="sensor">JSON format data, SensorId and ParkingId</param>
        /// <returns>Status Code + Sensor object (if 201) and adding new data to the Database</returns>
        // POST api/<SensorsController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<Sensor> Post([FromBody] Sensor sensor)
        {
            var result = _manager.Add(sensor);
            if (result != null)
            {
                return Created(sensor.SensorId.ToString(), sensor);
            }

            return Conflict($"The Sensor with the id {sensor.SensorId} already exists.");
        }

        /// <summary>
        /// Http Delete Method, Delete Sensor with specified id
        /// </summary>
        /// <param name="id">Id of the Sensor to be deleted</param>
        // DELETE api/<SensorsController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public void Delete(int id)
        {
            _manager.Delete(id);
        }

        /// <summary>
        /// Http Put Method, Updating the ParkingID from Sensor
        /// </summary>
        /// <param name="id"> Id for Sensor to be updated</param>
        /// <param name="sensor">JSON format data, the new data for Sensor</param>
        /// <returns>Status Code + Sensor object updated (if Status is OK)</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Sensor> Put(int id, [FromBody] Sensor sensor)
        {
            var result = _manager.Edit(id, sensor);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }
    }
}
