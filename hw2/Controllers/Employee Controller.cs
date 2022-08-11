using ContractControlCentre.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ContractControlCentre.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class Employee_Controller : ControllerBase
    {
        private readonly ILogger<Employee_Controller> _logger;


        public Employee_Controller(ILogger<Employee_Controller> logger)
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
        public IActionResult Register([FromBody] EmployeeRegisterRequest request)
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

