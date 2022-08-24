using ContractControlCentre.Requests;
using ContractControlCentre.Security.Authentication.Service;
using ContractControlCentre.Security.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContractControlCentre.Controller.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationAdminController : ControllerBase    //todo add documentation on swagger and write logger
    {
        private readonly ILogger<AuthenticationAdminController> _logger;
        private readonly IAuthenticationService _authenticationService;



        public AuthenticationAdminController(ILogger<AuthenticationAdminController> logger, IAuthenticationService authenticationService)
        {
            this._logger = logger;
            this._authenticationService = authenticationService;
        }



        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromQuery] string username, string password)
        {
            TokenResponse token = _authenticationService.Authentication(username, password);

            if (token is null)    //todo write in all controllers 'IsNullOrWhiteSpace'
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            SetTokenCookie(token.RefreshToken);

            return Ok(token);
        }


        [Authorize]
        [HttpPost("refreshToken")]
        public IActionResult Refresh()
        {
            string oldToken = Request.Cookies["refreshToken"];
            string newToken = _authenticationService.RefreshToken(oldToken);

            if (string.IsNullOrWhiteSpace(newToken))
            {
                return Unauthorized(new { message = "♿ token" });   //if u don't like, i'll delete it to version 1.0.6
            }

            SetTokenCookie(newToken);
            return Ok(newToken);
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
