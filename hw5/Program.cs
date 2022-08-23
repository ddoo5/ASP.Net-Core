using System.Reflection;
using System.Text;
using ContractControlCentre.DB.Connections;
using ContractControlCentre.DB.Entities;
using ContractControlCentre.DB.Interfaces;
using ContractControlCentre.DB.Repositories;
using ContractControlCentre.Security.Authentication.Service;
using ContractControlCentre.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();



//configure swagger
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1.0.5", new OpenApiInfo
    {
        Version = "v1.0.5",
        Title = "CCC (Contract Control Centre)",
        Description = "This Api helps to control different contracts",
        Contact = new OpenApiContact
        {
            Name = "DD",
            Email = "calistoromano@tutamail.com",
            Url = new Uri("https://github.com/ddoo5"),
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
            Array.Empty<string>()
        }
    });
});



//registration stand
builder.Services.AddSingleton<IPersonRepository, PersonRepoForDB>();
builder.Services.AddSingleton<PersonService>();

builder.Services.AddSingleton<IEmployeeRepository, EmployeeRepoForDB>();
builder.Services.AddSingleton<EmployeeService>();

builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();

builder.Services.AddSingleton<DbConnection>();
builder.Services.AddCors();



//add authentication
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x => {
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthenticationService._secretWord)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});



//build app
var app = builder.Build();

//add swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1.0.5/swagger.json", "CCC (Contract Control Centre)");
});

app.UseRouting();

app.UseCors(x => x
.SetIsOriginAllowed(origin => true)
.AllowAnyMethod()
.AllowAnyHeader()
.AllowCredentials());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

