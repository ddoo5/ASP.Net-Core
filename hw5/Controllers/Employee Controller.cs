using ContractControlCentre.DB.Entities;
using ContractControlCentre.Requests;
using ContractControlCentre.Security.Models;
using ContractControlCentre.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContractControlCentre.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/employees")]
    public class Employee_Controller : ControllerBase     //todo add documentation on swagger and write logger
    {
        private readonly ILogger<Employee_Controller> _logger;
        private readonly EmployeeService _service;



        public Employee_Controller(ILogger<Employee_Controller> logger, EmployeeService service)
        {
            this._logger = logger;
            this._service = service;
        }



        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromQuery] string username, string password)
        {

            TokenResponse token = _service.Authenticate(username, password);

            if (token is null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            SetTokenCookie(token.RefreshToken);

            return Ok(token);
        }


        [HttpGet("getById")]
        public IActionResult GetById([FromQuery] int id)
        {
            var employee = _service.GetById(id);

            if (employee is null)
            {
                return BadRequest($"Employee with Id:{id} not found");
            }
            else
            {
                return Ok(employee);
            }
        }


        [HttpPost("register")]
        public IActionResult Register([FromBody] EmployeeRegisterRequest request)
        {
            var message = _service.Create(request);

            if (message is null)
            {
                return BadRequest("Error, try again or change options");
            }
            else
            {
                return Ok(message);
            }
        }


        [HttpPut("update")]
        public IActionResult Update([FromBody] EmployeeModelEntity request)
        {
            var message = _service.Update(request);

            if (message is null)
            {
                return BadRequest("Error, try again or change options");
            }
            else
            {
                return Ok($"Person with Id:{request.Id} updated");
            }
        }


        [HttpDelete("deleteById")]
        public IActionResult DeleteById([FromQuery] int id)
        {
            var message = _service.DeleteById(id);

            if (message is null)
            {
                return BadRequest("Employee not found");
            }
            else
            {
                return Ok($"Person with Id:{id} deleted");
            }
        }


        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

    }
}

