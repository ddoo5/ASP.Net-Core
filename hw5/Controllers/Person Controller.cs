 using ContractControlCentre.DB.Entities;
using ContractControlCentre.Requests;
using ContractControlCentre.Security.Authentication.Service;
using ContractControlCentre.Security.Models;
using ContractControlCentre.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ContractControlCentre.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/persons")]
    public class Person_Controller : ControllerBase    //todo add documentation on swagger and write logger
    {
        private readonly ILogger<Person_Controller> _logger;
        private readonly PersonService _service;



        public Person_Controller(ILogger<Person_Controller> logger, PersonService service)
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

            PersonModelEntity person = _service.GetById(id);

            if(person is null)
            {
                return BadRequest($"Person with Id:{id} not found");
            }
            else
            {
                return Ok(person);
            }
        }


        [HttpGet("searchByName")]
        public IActionResult GetByName([FromQuery] string searchTerm)
        {

            var table = _service.GetByName(searchTerm);

            if(table.IsNullOrEmpty<PersonModelEntity>())
            {
                return BadRequest($"Person(s) with name: {searchTerm} not found");
            }
            else
            {
                return Ok(table);
            }
        }


        [HttpGet("searchByPagination")]
        public IActionResult GetByPagination([FromQuery] int skip, [FromQuery] int take)
        {

            var table = _service.GetByPagination(skip, take);

            if(table.IsNullOrEmpty<PersonModelEntity>())
            {
                return BadRequest($"Norhing found on page {take}");
            }
            else
            {
                return Ok(table);
            }
        }


        [HttpPost("register")]
        public IActionResult Register([FromBody] PersonRegisterRequest request)
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
        public IActionResult Update([FromBody] PersonModelEntity request)
        {

            var message = _service.Update(request);

            if (message is null)
            {
                return BadRequest(message);
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
                return BadRequest("Person not found");
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

