using ContractControlCentre.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ContractControlCentre.Controllers
{
    [ApiController]
    [Route("api/contracts")]
    public class Contract_Controller : ControllerBase
    {
        private readonly ILogger<Contract_Controller> _logger;



        public Contract_Controller(ILogger<Contract_Controller> logger)
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
        public IActionResult Register([FromBody] ContractRegisterRequest request)
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

