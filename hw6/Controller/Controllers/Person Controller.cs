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



        /// <summary>
        /// Метод аунтификации (!?) персоны
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        /// [POST]
        /// 
        ///  <code> /api/persons/authenticate?username=yourlogin   and(replace by symbol)   password=yourpassword </code>
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
        ///       api/persons/getById?id=1
        ///
        /// </remarks>
        /// <param name="id">Id персоны</param>
        /// <returns>Сообщение об успехе или ошибку</returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code = "400" > Запрос не выполнен. Вероятно из-за неправильно переданных данных</response>
        /// <response code = "405" > Запрос не выполнен. Недостаточно прав</response>
        /// <response code = "408" > Соединение разорвано</response>
        /// <response code = "415" > Unsupported Media Type </response>
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


        /// <summary>
        /// Метод поиска по имени
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        /// [GET]
        /// 
        ///  /api/persons/searchByName?searchTerm=aas
        ///
        /// </remarks>
        /// <param name = "searchTerm" >Имя пользователя, которого нужно найти</param>
        /// <returns>персону либо сообщение с ошибкой</returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code = "400" > Запрос не выполнен. Вероятно из-за неправильно переданных данных</response>
        /// <response code = "405" > Запрос не выполнен. Недостаточно прав</response>
        /// <response code = "408" > Соединение разорвано</response>
        /// <response code = "415" > Unsupported Media Type </response>
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


        /// <summary>
        /// Метод поиска по ID
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        /// [GET]
        /// 
        ///  api/persons/searchByPagination?skip=2   and(replace by symbol)   take=2' 
        ///
        /// </remarks>
        /// <param name = "skip" >С какого ID начать поиск</param>
        /// <param name = "take" >Сколько данных нужно собрать</param>
        /// <returns>Данные либо сообщение с ошибкой</returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code = "400" > Запрос не выполнен. Вероятно из-за неправильно переданных данных</response>
        /// <response code = "405" > Запрос не выполнен. Недостаточно прав</response>
        /// <response code = "408" > Соединение разорвано</response>
        /// <response code = "415" > Unsupported Media Type </response>
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


        /// <summary>
        /// Регистрация персоны
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        /// [POST]
        /// 
        ///       api/persons/register
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
        /// <param name="request">Профиль персоны</param>
        /// <returns>Сообщение об успешности или ошибку</returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code = "400" > Запрос не выполнен. Вероятно из-за неправильно переданных данных</response>
        /// <response code = "405" > Запрос не выполнен. Недостаточно прав</response>
        /// <response code = "408" > Соединение разорвано</response>
        /// <response code = "415" > Unsupported Media Type </response>
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


        /// <summary>
        /// Обновление профиля
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        /// [PUT]
        /// 
        ///       api/persons/update
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
        /// <param name="request">Профиль персоны</param>
        /// <returns>Сообщение об успешности или ошибку</returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code = "400" > Запрос не выполнен. Вероятно из-за неправильно переданных данных</response>
        /// <response code = "405" > Запрос не выполнен. Недостаточно прав</response>
        /// <response code = "408" > Соединение разорвано</response>
        /// <response code = "415" > Unsupported Media Type </response>
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


        /// <summary>
        /// Удаление по ID
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        /// [DELETE]
        /// 
        ///       api/persons/deleteById?id=2
        ///
        /// </remarks>
        /// <param name="id">Id персоны</param>
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
                Expires = DateTime.UtcNow.AddDays(6)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

    }
}

