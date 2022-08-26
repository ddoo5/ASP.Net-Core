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
    public class Employee_Controller : ControllerBase     //todo write logger
    {
        private readonly ILogger<Employee_Controller> _logger;
        private readonly EmployeeService _service;



        public Employee_Controller(ILogger<Employee_Controller> logger, EmployeeService service)
        {
            this._logger = logger;
            this._service = service;
        }



        /// <summary>
        /// Метод аунтификации для работника
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        /// [POST]
        /// 
        ///  <code> /api/employees/authenticate?username=yourlogin   and(replace by symbol)   password=yourpassword </code>
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

            TokenResponse token = _service.Authenticate(username, password);

            if (token is null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            SetTokenCookie(token.RefreshToken);

            return Ok(token);
        }


        /// <summary>
        /// Поиск по ID
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        /// [GET]
        /// 
        ///       api/employees/getById?id=2
        ///
        /// </remarks>
        /// <param name="id">Id работника</param>
        /// <returns>Сообщение о успехе или ошибку</returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code = "400" > Запрос не выполнен. Вероятно из-за неправильно переданных данных</response>
        /// <response code = "405" > Запрос не выполнен. Недостаточно прав</response>
        /// <response code = "408" > Соединение разорвано</response>
        /// <response code = "415" > Unsupported Media Type </response>
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


        /// <summary>
        /// Регистрация работника
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        /// [POST]
        /// 
        ///       api/employees/register
        ///
        /// [BODY]
        ///
        ///        {
        ///              "firstName": "Jhon",
        ///              "lastName": "Jhonson",
        ///              "email": "jj@tutamail.com",
        ///              "company": "LLC",
        ///              "age": 24
        ///          } 
        ///
        /// </remarks>
        /// <param name="request">Профиль работника</param>
        /// <returns>Сообщение об успешности или ошибку</returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code = "400" > Запрос не выполнен. Вероятно из-за неправильно переданных данных</response>
        /// <response code = "405" > Запрос не выполнен. Недостаточно прав</response>
        /// <response code = "408" > Соединение разорвано</response>
        /// <response code = "415" > Unsupported Media Type </response>
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


        /// <summary>
        /// Обновление профиля
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        /// [PUT]
        /// 
        ///       api/employees/update
        ///
        /// [BODY]
        ///
        ///        {
        ///              "id": 1,
        ///              "isDeleted": false,
        ///              "firstName": "Stiv",
        ///              "lastName": "Jhonson",
        ///              "email": "sj@tutamail.com",
        ///              "company": "AAB",
        ///              "age": 29
        ///          } 
        ///
        /// </remarks>
        /// <param name="request">Профиль работника</param>
        /// <returns>Сообщение об успешности или ошибку</returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code = "400" > Запрос не выполнен. Вероятно из-за неправильно переданных данных</response>
        /// <response code = "405" > Запрос не выполнен. Недостаточно прав</response>
        /// <response code = "408" > Соединение разорвано</response>
        /// <response code = "415" > Unsupported Media Type </response>
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


        /// <summary>
        /// Удаление по ID
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        /// [DELETE]
        /// 
        ///       api/employees/deleteById?id=2
        ///
        /// </remarks>
        /// <param name="id">Id работника</param>
        /// <returns>Сообщение о успехе или ошибку</returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code = "400" > Запрос не выполнен. Вероятно из-за неправильно переданных данных</response>
        /// <response code = "405" > Запрос не выполнен. Недостаточно прав</response>
        /// <response code = "408" > Соединение разорвано</response>
        /// <response code = "415" > Unsupported Media Type </response>
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

