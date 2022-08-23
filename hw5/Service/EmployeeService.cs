using System;
using ContractControlCentre.DB.Entities;
using ContractControlCentre.DB.Interfaces;
using ContractControlCentre.Requests;
using ContractControlCentre.Security.Authentication.Service;
using ContractControlCentre.Security.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContractControlCentre.Service
{
    public class EmployeeService
    {

        private readonly ILogger<EmployeeService> _logger;
        private readonly IEmployeeRepository _employeeRepo;
        private readonly IAuthenticationService _authenticationService;



        public EmployeeService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepo, IAuthenticationService authenticationService)
        {
            this._logger = logger;
            this._employeeRepo = employeeRepo;
            this._authenticationService = authenticationService;
        }



        /// <summary>
        /// <para>Searching entity by Id</para>
        /// </summary>
        /// <param name="id">Id of entity wich need to search</param>
        /// <returns>found entity or message with exception</returns>
        public EmployeeModelEntity GetById(int id)
        {
            _logger.LogInformation(1, "Method 'GetById' in 'EmployeeService' launched");

            try
            {
                var employee = _employeeRepo.GetById(id);

                if (employee != null)
                {
                    _logger.LogInformation(1, "Method 'GetById' in 'EmployeeService' ended successfully");
                    return employee;
                }
                else
                {
                    _logger.LogInformation(2, "Method 'GetById' in 'EmployeeService' ended successfully with nothing found");

                    return null;
                }
            }
            catch (Exception a)
            {
                _logger.LogError(3, $"Method 'GetById' in 'EmployeeService' ended with excpetion: {a}");

                return null;
            }
        }


        /// <summary>
        /// <para>Creating entity</para>
        /// </summary>
        /// <param name="request">request from API, which include information about new entity</param>
        /// <returns>Create entity or returns message with exception</returns>
        public string Create(EmployeeRegisterRequest request)
        {
            _logger.LogInformation(1, "Method 'Create' in 'EmployeeService' launched");

            try
            {
                _employeeRepo.Create(new EmployeeModelEntity
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Company = request.Company,
                    Age = request.Age,
                    IsDeleted = false
                });
                _logger.LogInformation(1, "Method 'Create' in 'EmployeeService' ended successfully");

                return $"Employee with name {request.FirstName} {request.LastName} registered";
            }
            catch (Exception a)
            {
                _logger.LogError(3, $"Method 'Create' in 'EmployeeService' ended with excpetion: {a}");

                return null;
            }
        }


        /// <summary>
        /// <para>Updating entity</para>
        /// </summary>
        /// <param name="request">request from API, which include information about entity</param>
        /// <returns>Update entity or returns message with exception</returns>
        public string Update(EmployeeModelEntity request)
        {
            _logger.LogInformation(1, "Method 'Update' in 'EmployeeService' launched");

            try
            {
                _employeeRepo.Update(request);

                _logger.LogInformation(1, "Method 'Update' in 'EmployeeService' ended successfully");

                return "Updated";
            }
            catch (Exception a)
            {
                _logger.LogError(3, $"Method 'Update' in 'EmployeeService' ended with excpetion: {a}");

                return null;
            }
        }


        /// <summary>
        /// <para>Deleting entity by id</para>
        /// </summary>
        /// <param name="id">id of entity which should be deleted</param>
        /// <returns>Delete entity or returns message with exception</returns>
        public string DeleteById(int id)
        {
            _logger.LogInformation(1, "Method 'DeleteById' in 'EmployeeService' launched");

            try
            {
                var employee = _employeeRepo.GetById(id);

                if (employee != null)
                {
                    _employeeRepo.Delete(id);

                    _logger.LogInformation(1, "Method 'DeleteById' in 'EmployeeService' ended successfully");

                    return $"Deleted employee with id: {employee.Id}";
                }
                else
                {
                    _logger.LogInformation(2, "Method 'DeleteById' in 'EmployeeService' ended successfully with nothing found");

                    return null;
                }
            }
            catch (Exception a)
            {
                _logger.LogError(3, $"Method 'DeleteById' in 'EmployeeService' ended with excpetion: {a}");

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
            _logger.LogInformation(1, "Method 'Authenticate' in 'EmployeeService' launched");

            try
            {
                if (username != "employee")
                {
                    _logger.LogInformation(2, "Method 'Authenticate' in 'EmployeeService' ended successfully with nothing found");
                    return null;
                }
                else if (username == "employee")
                {
                    TokenResponse token = _authenticationService.Authentication(username, password);

                    _logger.LogInformation(1, "Method 'Authenticate' in 'EmployeeService' ended successfully");

                    return token;
                }

                _logger.LogInformation(2, "Method 'Authenticate' in 'EmployeeService' ended successfully with nothing found");

                return null;
            }
            catch (Exception a)
            {
                _logger.LogError(3, $"Method 'Authenticate' in 'EmployeeService' ended with excpetion: {a}");

                return null;
            }
        }

    }
}

