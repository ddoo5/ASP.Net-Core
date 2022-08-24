using ContractControlCentre.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContractControlCentre.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/customers")]
    public class Customer_Controller : ControllerBase
    {

        private readonly ILogger<Customer_Controller> _logger;


        public Customer_Controller(ILogger<Customer_Controller> logger)
        {
            this._logger = logger;
        }


        [HttpGet("get")]
        public IActionResult GetAll()
        {
            return Ok("ok");
        }


        [HttpGet("get/{id}")]
        public IActionResult GetById([FromQuery] int id)
        {
            return Ok();
        }


        [HttpPost("register")]
        public IActionResult Register([FromBody] CustomerRegisterRequest request)
        {
            return Ok();
        }


        [HttpPut("update/{id}")]
        public IActionResult UpdateById([FromQuery] int id)
        {
            return Ok();
        }


        [HttpDelete("delete/{id}")]
        public IActionResult DeleteById([FromQuery] int id)
        {
            return Ok();
        }
    }


}

