using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CarApi.Business;
using CarApi.Data.VO;
using CarApi.Hypermedia.Filters;
using System.Collections.Generic;

namespace CarApi.Controllers
{

    [ApiVersion("1")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/[controller]/v{version:apiVersion}")]
    public class CarController : ControllerBase
    {

        private readonly ILogger<CarController> _logger;

        // Declaration of the service used
        private ICarBusiness _carBusiness;

        // Injection of an instance of ICarService
        // when creating an instance of CarController
        public CarController(ILogger<CarController> logger, ICarBusiness carBusiness)
        {
            _logger = logger;
            _carBusiness = carBusiness;
        }

        // Maps GET requests to https://localhost:{port}/api/car
        // Get no parameters for FindAll -> Search All
        [HttpGet("{sortDirection}/{pageSize}/{page}")]
        [ProducesResponseType((200), Type = typeof(List<CarVO>))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get(
            [FromQuery] string make,
            string sortDirection,
            int pageSize,
            int page)
        {
            return Ok(_carBusiness.FindWithPagedSearch(make, sortDirection, pageSize, page));
        }

        // Maps GET requests to https://localhost:{port}/api/car/{id}
        // receiving an ID as in the Request Path
        // Get with parameters for FindById -> Search by ID
        [HttpGet("{id}")]
        [ProducesResponseType((200), Type = typeof(CarVO))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get(long id)
        {
            var car = _carBusiness.FindByID(id);
            if (car == null) return NotFound();
            return Ok(car);
        }

        // Maps POST requests to https://localhost:{port}/api/car/
        // [FromBody] consumes the JSON object sent in the request body
        [HttpPost]
        [ProducesResponseType((200), Type = typeof(CarVO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Post([FromBody] CarVO car)
        {
            if (car == null) return BadRequest();
            return Ok(_carBusiness.Create(car));
        }

        // Maps PUT requests to https://localhost:{port}/api/car/
        // [FromBody] consumes the JSON object sent in the request body
        [HttpPut]
        [ProducesResponseType((200), Type = typeof(CarVO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Put([FromBody] CarVO car)
        {
            if (car == null) return BadRequest();
            return Ok(_carBusiness.Update(car));
        }

        // Maps DELETE requests to https://localhost:{port}/api/car/{id}
        // receiving an ID as in the Request Path
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Delete(long id)
        {
            _carBusiness.Delete(id);
            return NoContent();
        }
    }
}
