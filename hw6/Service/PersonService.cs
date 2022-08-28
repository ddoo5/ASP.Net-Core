using System;
using System.Collections;
using System.Linq;
using System.Xml.Linq;
using ContractControlCentre.DB.Core;
using ContractControlCentre.DB.Entities;
using ContractControlCentre.DB.Interfaces;
using ContractControlCentre.Requests;
using ContractControlCentre.Security.Authentication.Service;
using ContractControlCentre.Security.Models;
using ContractControlCentre.Validation.Models;
using ContractControlCentre.Validation.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ContractControlCentre.Service
{
    public class PersonService
    {

        private readonly IVValidationService _validation;
        private readonly ILogger<PersonService> _logger;
        private readonly IPersonRepository _personRepo;
        private readonly IAuthenticationService _authenticationService;



        public PersonService(ILogger<PersonService> logger, IPersonRepository personRepo, IAuthenticationService authenticationService, IVValidationService validation)
        {
            this._logger = logger;
            this._personRepo = personRepo;
            this._authenticationService = authenticationService;
            this._validation = validation;
        }



        /// <summary>
        /// <para>Searching entity by Id</para>
        /// </summary>
        /// <param name="id">Id of entity wich need to search</param>
        /// <returns>found entity or message with exception</returns>
        public PersonModelEntity GetById(int id)
        {
            _logger.LogInformation(1, "Method 'GetById' in 'PersonService' launched");

            try
            {
                var person = _personRepo.GetById(id);

                if (person != null)
                {
                    _logger.LogInformation(1, "Method 'GetById' in 'PersonService' ended successfully");
                    return person;
                }
                else
                {
                    _logger.LogInformation(2, "Method 'GetById' in 'PersonService' ended successfully with nothing found");

                    return null;
                }
            }
            catch (Exception a)
            {
                _logger.LogError(3, $"Method 'GetById' in 'PersonService' ended with excpetion: {a}");

                return null ;
            }
        }


        /// <summary>
        /// <para>Creating entity</para>
        /// </summary>
        /// <param name="request">request from API, which include information about new entity</param>
        /// <returns>Create entity or message with exception</returns>
        public string Create(PersonRegisterRequest request)
        {
            _logger.LogInformation(1, "Method 'Create' in 'PersonService' launched");

            IReadOnlyList<IOperationFailure> failures = _validation.ValidateEntity(new ValidationModel(){
                Age = request.Age,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email
            });

            if(failures.Count == 0)
            {
                try
                {
                    _personRepo.Create(new PersonModelEntity
                    {
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Email = request.Email,
                        Company = request.Company,
                        Age = request.Age,
                        IsDeleted = false
                    });
                    _logger.LogInformation(1, "Method 'Create' in 'PersonService' ended successfully");

                    return $"Person with name {request.FirstName} {request.LastName} registered";
                }
                catch (Exception a)
                {
                    _logger.LogError(3, $"Method 'Create' in 'PersonService' ended with excpetion: {a}");

                    return null;
                }
            }
            else
            {
                var s = string.Join("\n", failures.Select(e => $"{e.Code}   {e.Description}\n"));
                return s;
            }
        }


        /// <summary>
        /// <para>Updating entity</para>
        /// </summary>
        /// <param name="request">request from API, which include information about entity</param>
        /// <returns>Update entity or message with exception</returns>
        public string Update(PersonModelEntity request)
        {
            _logger.LogInformation(1, "Method 'Update' in 'PersonService' launched");

            IReadOnlyList<IOperationFailure> failures = _validation.ValidateEntity(new ValidationModel()
            {
                Age = request.Age,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email
            });

            if (failures.Count == 0)
            {
                try
                {
                    _personRepo.Update(request);

                    _logger.LogInformation(1, "Method 'Update' in 'PersonService' ended successfully");

                    return "Updated";
                }
                catch (Exception a)
                {
                    _logger.LogError(3, $"Method 'Update' in 'PersonService' ended with excpetion: {a}");

                    return null;
                }
            }
            else
            {
                var s = string.Join("\n", failures.Select(e => $"{e.Code}   {e.Description}\n"));
                return s;
            }
        }


        /// <summary>
        /// <para>Deleting entity by id</para>
        /// </summary>
        /// <param name="id">id of entity which should be deleted</param>
        /// <returns>Delete entity or message with exception</returns>
        public string DeleteById(int id)
        {
            _logger.LogInformation(1, "Method 'DeleteById' in 'PersonService' launched");

            try
            {
                var person = _personRepo.GetById(id);

                if(person != null)
                {
                    _personRepo.Delete(id);

                    _logger.LogInformation(1, "Method 'DeleteById' in 'PersonService' ended successfully");

                    return $"Deleted person with id: {person.Id}";
                }
                else
                {
                    _logger.LogInformation(2, "Method 'DeleteById' in 'PersonService' ended successfully with nothing found");

                    return null;
                }
            }
            catch (Exception a)
            {
                _logger.LogError(3, $"Method 'DeleteById' in 'PersonService' ended with excpetion: {a}");

                return null;
            }
        }


        /// <summary>
        /// <para>Searching entit(y)/(ies) by name</para>
        /// </summary>
        /// <param name="name">name of entity which will be found</param>
        /// <returns>Entities or message with exception</returns>
        public IList<PersonModelEntity> GetByName(string name)
        {
            _logger.LogInformation(1, "Method 'GetByName' in 'PersonService' launched");

            IReadOnlyList<IOperationFailure> failures = _validation.ValidateEntity(new ValidationModel()
            {
                FirstName = name
            });

            if (failures.Count == 0)
            {
                try
                {
                    var table = _personRepo.GetByName(name);

                    if (table != null)
                    {
                        _logger.LogInformation(1, "Method 'GetByName' in 'PersonService' ended successfully");

                        return table;
                    }
                    else
                    {
                        _logger.LogInformation(2, "Method 'GetByName' in 'PersonService' ended successfully with nothing found");

                        return null;
                    }
                }
                catch (Exception a)
                {
                    _logger.LogError(3, $"Method 'GetByName' in 'PersonService' ended with excpetion: {a}");

                    return null;
                }
            }
            else        //не смог найти решение
            {
                return null;
            }
        }


        /// <summary>
        /// Searching entit(y)/(ies) by pages
        /// </summary>
        /// <param name="skip">id which should be skipped</param>
        /// <param name="take">id from will be searching</param>
        /// <returns>Entities or message with exception</returns>
        public IList<PersonModelEntity> GetByPagination(int skip, int take)
        {
            _logger.LogInformation(1, "Method 'GetByPagination' in 'PersonService' launched");

            try
            {
                var table = _personRepo.GetByPagination(skip, take);

                if(table != null)
                {
                    _logger.LogInformation(1, "Method 'GetByPagination' in 'PersonService' ended successfully");

                    return table;
                }
                else
                {
                    _logger.LogInformation(2, "Method 'GetByPagination' in 'PersonService' ended successfully with nothing found");
                    return null;
                }
            }
            catch(Exception a)
            {
                _logger.LogError(3, $"Method 'GetByPagination' in 'PersonService' ended with excpetion: {a}");

                return null;
            }
        }


        /// <summary>
        /// Creating token for authenticate 
        /// </summary>
        /// <param name="username">User name(like id)
        /// <remarks>,for each controller saved each sudo-user</remarks>
        /// </param>
        /// <param name="password">Password for authenticate</param>
        /// <returns>Token or null</returns>
        public TokenResponse Authenticate([FromQuery] string username, string password)
        {
            _logger.LogInformation(1, "Method 'Authenticate' in 'PersonService' launched");

            try
            {
                if (username != "person")
                {
                    _logger.LogInformation(2, "Method 'Authenticate' in 'PersonService' ended successfully with nothing found");
                    return null;
                }
                else if (username == "person")
                {
                    TokenResponse token = _authenticationService.Authentication(username, password);

                    _logger.LogInformation(1, "Method 'Authenticate' in 'PersonService' ended successfully");

                    return token;
                }

                _logger.LogInformation(2, "Method 'Authenticate' in 'PersonService' ended successfully with nothing found");
                return null;
            }
            catch (Exception a)
            {
                _logger.LogError(3, $"Method 'Authenticate' in 'PersonService' ended with excpetion: {a}");

                return null;
            }
        }

    }
}

