using System;
using ContractControlCentre.DB.Interfaces;
using ContractControlCentre.Models;
using ContractControlCentre.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ContractControlCentre.Controllers
{
    [ApiController]
    [Route("api/persons")]
    public class Person_Controller : ControllerBase
    {
        private readonly ILogger<Person_Controller> _logger;
        private readonly IPersonRepository _repository;


        public Person_Controller(ILogger<Person_Controller> logger,IPersonRepository repository)
        {
            this._logger = logger;
            this._repository = repository;
        }


        [HttpGet("{id}")]
        public IActionResult GetById([FromQuery] int id)   //gets by id
        {
            try
            {
                var person = _repository.GetById(id);

                return Ok(person);
            }
            catch (Exception a)
            {
                return BadRequest(error: a.Message);
            }
        }


        [HttpGet("search")]
        public IActionResult GetByName([FromQuery] string searchTerm)   //not realized
        {
            return Ok();
        }


        [HttpGet("searchByPagination")]
        public IActionResult GetByPagination([FromQuery] int skip, [FromQuery] int take)   //not realized
        {
            return Ok();
        }


        [HttpPost("register")]
        public IActionResult Register([FromBody] PersonRegisterRequest request)   //register new person
        {
            try
            {
                _repository.Create(new PersonModel
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Company = request.Company,
                    Age = request.Age
                });

                return Ok("Registered");
            }
            catch(Exception a)
            {
                return BadRequest(error: a.Message);
            }
        }


        [HttpPut("update")]
        public IActionResult Update([FromBody] PersonModel request)   //update created person
        {
            try
            {
                _repository.Update(request);

                return Ok("Updated \n" + request.Id);
            }
            catch (Exception a)
            {
                return BadRequest(error: a.Message);
            }
        }


        [HttpDelete("delete{id}")]
        public IActionResult DeleteById([FromQuery] int id)   //delete created person
        {
            try
            {
                var person = _repository.GetById(id);
                _repository.Delete(id);

                return Ok("Deleted \n" + person.Id);
            }
            catch (Exception a)
            {
                return BadRequest(error: a.Message);
            }
        }
    }
}

