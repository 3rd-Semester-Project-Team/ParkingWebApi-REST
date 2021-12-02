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
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingsController : ControllerBase
    {
        private readonly IParkingsManager _parkingManager;
        private readonly ISensorsManager _sensorsManager;

        public ParkingsController(IParkingsManager manager, ISensorsManager sensorsManager)  
        {
            _parkingManager = manager;
            _sensorsManager = sensorsManager;
        }
        
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

        [HttpGet("latest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<ParkingSlot>> GetLatest()
        {
            return Ok(_parkingManager.GetLatestParkingStatus());
        }

        //To be reviewed again
        // POST api/<ParkingsController>
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

        // PUT api/<ParkingsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ParkingsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
