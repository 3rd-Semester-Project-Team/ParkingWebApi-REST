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
    [Route("api/[controller]")]
    [ApiController]
    public class SensorsController : ControllerBase
    {
        private readonly SensorsManager _manager;

        public SensorsController(SensorsManager manager)
        {
            _manager = manager;
        }

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

        // POST api/<SensorsController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Sensor> Post([FromBody] Sensor sensor)
        {
            var result = _manager.Add(sensor);
            if (result != null)
            {
                return Ok(result);
            }

            return Conflict($"The Sensor with the id {sensor.SensorId} already exists.");
        }

        // DELETE api/<SensorsController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public void Delete(int id)
        {
            _manager.Delete(id);
        }

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
