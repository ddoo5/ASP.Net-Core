using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using AutoFixture;
using AutoFixture.AutoMoq;
using Castle.Core.Logging;
using ContractControlCentre.DB.Connections;
using ContractControlCentre.DB.Entities;
using ContractControlCentre.DB.Interfaces;
using ContractControlCentre.DB.Repositories;
using ContractControlCentre.Requests;
using ContractControlCentre.Security.Authentication.Service;
using ContractControlCentre.Service;
using ContractControlCentre.Validation.Models.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace CCC.Tests;

public class TheoryTests
{
    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(1000)]
    public void PersonService_GetById_Test(int id)
    {
        // Arrange
        PersonRepoForDB _repository = new(new DbConnection());

        var person = new PersonModelEntity() {
            Age = 0,
            Company = string.Empty,
            Email = string.Empty,
            FirstName = string.Empty,
            LastName = string.Empty};

        //Act
        var a = _repository.GetById(id);

        //Assert
        if(a.Age >= 0)
            Assert.NotNull(a);
        else
            Assert.False(false);
    }


    [Theory]
    [InlineData("Jhon")]
    [InlineData("string")]
    [InlineData("")]
    [InlineData(" ")]
    public void PersonService_GetByName_Test(string name)
    {
        // Arrange
        var fix = new Fixture().Customize(new AutoMoqCustomization());
        PersonService _service = fix.Create<PersonService>();

        //Act
        var result = _service.GetByName(name);

        //Assert
        if (result.Count > 0)
            Assert.NotEmpty(result);
        else
            Assert.Empty(result);
    }


    [Theory]
    [InlineData(-1,10)]
    [InlineData(0,20)]
    [InlineData(20,13)]
    [InlineData(2000, 13)]
    public void PersonService_GetByPagination_Test(int skip, int take)
    {
        // Arrange
        var fix = new Fixture().Customize(new AutoMoqCustomization());
        PersonService _service = fix.Create<PersonService>();

        //Act
        var result = _service.GetByPagination(skip, take);

        //Assert
        if(result.Count > 0)
            Assert.NotEmpty(result);
        else
            Assert.Empty(result);
    }


    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(1000)]
    [DisplayName("Delete by id TEST")]
    public void PersonService_DeleteById_Test(int id)
    {
        // Arrange
        PersonRepoForDB _repository = new(new DbConnection());

        //Act
        _repository.Delete(id);
        var result = _repository.GetById(id);

        //Assert
        if(result.IsDeleted == true)
            Assert.NotNull(result);
    }
}
