using System;
using System.Text.RegularExpressions;
using AutoFixture;
using AutoFixture.AutoMoq;
using ContractControlCentre.Controllers;
using ContractControlCentre.DB.Entities;
using ContractControlCentre.DB.Interfaces;
using ContractControlCentre.Requests;
using ContractControlCentre.Security.Authentication.Service;
using ContractControlCentre.Service;
using ContractControlCentre.Validation.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CCC.Tests
{
    public class FactTests
    {
        [Fact]
        public void PersonConnectionWithDB_Create_Test()
        {
            var fix = new Fixture().Customize(new AutoMoqCustomization());
            PersonService _service = fix.Create<PersonService>();

            PersonRegisterRequest person1 = new PersonRegisterRequest()
            {
                Age = 0,
                Company = "",
                Email = "some",
                FirstName = "i don't know",
                LastName = "what is that?"
            };

            PersonRegisterRequest person2 = new PersonRegisterRequest()
            {
                Age = 23,
                Company = "LLC",
                Email = "testwedcs@gmail.com",
                FirstName = "Jhohn",
                LastName = "Robert"
            };

            Regex expectedMessage = new($"Person with name {person1.FirstName} {person1.LastName} registered");
            Regex expectedMessage2 = new($"Person with name {person2.FirstName} {person2.LastName} registered");

            //Act
            string result = _service.Create(person1);
            string result2 = _service.Create(person2);

            //Assert
            Assert.DoesNotMatch(expectedMessage, result);
            Assert.Matches(expectedMessage2, result2);
        }


        [Fact]
        public void PersonConnectionWithDB_Update_Test()
        {
            // Arrange
            var fix = new Fixture().Customize(new AutoMoqCustomization());
            PersonService _service = fix.Create<PersonService>();

            PersonModelEntity person1 = new PersonModelEntity()
            {
                Age = 0,
                Company = "",
                Email = "some",
                FirstName = "i don't know",
                LastName = "what is that?",
                Id = -4,
                IsDeleted = true
            };

            PersonModelEntity person2 = new PersonModelEntity()
            {
                Age = 23,
                Company = "LLc",
                Email = "test@gmail.com",
                FirstName = "Jhon",
                LastName = "Roberto",
                Id=3,
                IsDeleted=true
            };

            Regex expectedMessage = new("Updated");
            Regex expectedMessage2 = new("Updated");

            //Act
            string result = _service.Update(person1);
            string result2 = _service.Update(person2);

            //Assert
            Assert.Matches(expectedMessage, result);
            Assert.Matches(expectedMessage2, result2);
        }
    }
}

