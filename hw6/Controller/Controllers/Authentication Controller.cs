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
    public class AuthenticationAdminController : ControllerBase    //todo write logger
    {
        private readonly ILogger<AuthenticationAdminController> _logger;
        private readonly IAuthenticationService _authenticationService;



        public AuthenticationAdminController(ILogger<AuthenticationAdminController> logger, IAuthenticationService authenticationService)
        {
            this._logger = logger;
            this._authenticationService = authenticationService;
        }



        /// <summary>
        /// Метод аунтификации для администратора
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        /// [POST]
        /// 
        ///  <code> /api/authentication/authenticate?username=yourlogin   and(replace by symbol)   password=yourpassword </code>
        ///
        /// </remarks>
        /// <param name = "username" >Имя пользователя</param>
        /// <param name = "password" >Пароль</param>
        /// <returns>Токен авторизации и запасной токен</returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code = "400" > Запрос не выполнен. Вероятно из-за неправильно переданных данных</response>
        /// <response code = "408" > Соединение разорвано</response>
        /// <response code = "415" > Unsupported Media Type </response>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromQuery] string username, string password)
        {
            TokenResponse token = _authenticationService.Authentication(username, password);

            if (token is null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            SetTokenCookie(token.RefreshToken);

            return Ok(token);
        }

        /// <summary>
        /// Получение нового токена
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        /// [POST]
        /// 
        ///       /api/authentication/refreshToken
        ///
        /// </remarks>
        /// <returns>Обновленный токен</returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code = "405" > Запрос не выполнен. Недостаточно прав</response>
        /// <response code = "408" > Соединение разорвано</response>
        [Authorize]
        [HttpPost("refreshToken")]
        public IActionResult Refresh()
        {
            string oldToken = Request.Cookies["refreshToken"];
            string newToken = _authenticationService.RefreshToken(oldToken);

            if (string.IsNullOrWhiteSpace(newToken))
            {
                return Unauthorized(new { message = "♿ token" });
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
