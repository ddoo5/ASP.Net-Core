 using ContractControlCentre.DB.Entities;
using ContractControlCentre.Requests;
using ContractControlCentre.Service;
using Microsoft.AspNetCore.Mvc;

namespace ContractControlCentre.Controllers
{
    [ApiController]
    [Route("api/persons")]
    public class Person_Controller : ControllerBase    //todo add documentation on swagger
    {
        private readonly ILogger<Person_Controller> _logger;
        private readonly PersonService _service;



        public Person_Controller(ILogger<Person_Controller> logger, PersonService service)
        {
            this._logger = logger;
            this._service = service;
        }



        [HttpGet("getById")]
        public IActionResult GetById([FromQuery] int id)
        {
            var person = _service.GetById(id);

            if(person != null)
            {
                return Ok(person);
            }
            else
            {
                return BadRequest($"Person with Id:{id} not found");
            }
        }


        [HttpGet("searchByName")]
        public IActionResult GetByName([FromQuery] string searchTerm)
        {
            var table = _service.GetByName(searchTerm);

            if(table != null)
            {
                return Ok(table);
            }
            else
            {
                return BadRequest($"Person(s) with name: {searchTerm} not found");
            }
        }


        [HttpGet("searchByPagination")]
        public IActionResult GetByPagination([FromQuery] int skip, [FromQuery] int take)
        {
            var table = _service.GetByPagination(skip, take);

            if(table != null)
            {
                return Ok(table);
            }
            else
            {
                return BadRequest($"Norhing found on page {take}");
            }
        }


        [HttpPost("register")]
        public IActionResult Register([FromBody] PersonRegisterRequest request)
        {
            var message = _service.Create(request);

            if (message[0] == 'P')   //bad check, i know
            {
                return Ok(message);
            }
            else
            {
                return BadRequest(message);
            }
        }


        [HttpPut("update")]
        public IActionResult Update([FromBody] PersonModelEntity request)
        {
            var message = _service.Update(request);
            if (message[0] == 'U')   //same bad check
            {
                return Ok($"Person with Id:{request.Id} updated");
            }
            else
            {
                return BadRequest(message);
            }
        }


        [HttpDelete("deleteById")]
        public IActionResult DeleteById([FromQuery] int id)
        {
            var message = _service.DeleteById(id);
            if (message[0] == 'D')
            {
                return Ok($"Person with Id:{id} deleted");
            }
            else
            {
                return BadRequest(message);
            }
        }
    }
}

