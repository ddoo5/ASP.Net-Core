using System.Reflection;
using ContractControlCentre.DB.Connections;
using ContractControlCentre.DB.Entities;
using ContractControlCentre.DB.Interfaces;
using ContractControlCentre.DB.Repositories;
using ContractControlCentre.Service;
using Microsoft.OpenApi.Models;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//swagger options
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CCC", Version = "v1", Description = "This Api helps to control different contracts" });
//});

builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "CCC (Contract Control Centre)",
        Description = "This Api helps to control different contracts",
        Contact = new OpenApiContact
        {
            Name = "DD",
            Email = "kritantablake@gmail.com",
            Url = new Uri("https://github.com/ddoo5"),
        }
    });
});



//registration stand
builder.Services.AddSingleton<IPersonRepository, PersonRepoForDB>();
builder.Services.AddSingleton<PersonService>();

builder.Services.AddSingleton<IEmployeeRepository, EmployeeRepoForDB>();
builder.Services.AddSingleton<EmployeeService>();

builder.Services.AddSingleton<DbConnection>();
builder.Services.AddCors();


//build app
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

