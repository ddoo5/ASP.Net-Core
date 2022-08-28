using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContractControlCentre.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/invoices")]
    public class Invoice_Controller : ControllerBase
    {

        private readonly ILogger<Invoice_Controller> _logger;


        public Invoice_Controller(ILogger<Invoice_Controller> logger)
        {
            this._logger = logger;
        }


        [HttpGet("get")]
        public IActionResult GetAll()
        {
            return Ok("ok");
        }


        [HttpPut("update/{id}")]
        public IActionResult UpdateById([FromQuery] int id)
        {
            return Ok();
        }
    }
}

