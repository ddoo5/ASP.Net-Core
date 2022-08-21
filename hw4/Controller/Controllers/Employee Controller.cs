using ContractControlCentre.DB.Entities;
using ContractControlCentre.Requests;
using ContractControlCentre.Service;
using Microsoft.AspNetCore.Mvc;

namespace ContractControlCentre.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class Employee_Controller : ControllerBase     //todo add documentation on swagger
    {
        private readonly ILogger<Employee_Controller> _logger;
        private readonly EmployeeService _service;



        public Employee_Controller(ILogger<Employee_Controller> logger, EmployeeService service)
        {
            this._logger = logger;
            this._service = service;
        }


        [HttpGet("getById")]
        public IActionResult GetById([FromQuery] int id)
        {
            var employee = _service.GetById(id);

            if (employee != null)
            {
                return Ok(employee);
            }
            else
            {
                return BadRequest($"Employee with Id:{id} not found");
            }
        }


        [HttpPost("register")]
        public IActionResult Register([FromBody] EmployeeRegisterRequest request)
        {
            var message = _service.Create(request);

            if (message[0] == 'E')   //bad check, i know
            {
                return Ok(message);
            }
            else
            {
                return BadRequest(message);
            }
        }


        [HttpPut("update")]
        public IActionResult Update([FromBody] EmployeeModelEntity request)
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

