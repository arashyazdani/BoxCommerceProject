using API.Errors;
using API.Extensions;
using API.Helpers;
using API.Middleware;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

var connectionString = configuration.GetConnectionString("DefaultSqlConnection");

var identityConnectionString = configuration.GetConnectionString("IdentitySqlConnection");

var redisConnectionString = configuration.GetConnectionString("Redis");

IWebHostEnvironment environment = builder.Environment;

// Add services to the container.

builder.Services.AddControllers(setupAction =>
    {
        setupAction.ReturnHttpNotAcceptable = true;
        setupAction.Filters.Add(new ProducesAttribute("application/json", "text/json"));
    }).AddNewtonsoftJson(setupAction =>
    {
        setupAction.SerializerSettings.ContractResolver =
            new CamelCasePropertyNamesContractResolver();
    })
    .AddXmlDataContractSerializerFormatters();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<FactoryContext>(x => x.UseSqlServer(connectionString));

builder.Services.AddDbContext<FactoryIdentityContext>(x => x.UseSqlServer(identityConnectionString));

builder.Services.AddSingleton<IConnectionMultiplexer>(c =>
{
    var config = ConfigurationOptions.Parse(redisConnectionString, true);
    return ConnectionMultiplexer.Connect(config);
});

builder.Services.AddAutoMapper(typeof(MappingProfiles));

builder.Services.AddApplicationServices();

builder.Services.AddIdentityServices(configuration);

builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerDocumentation();


// Use services

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseMiddleware<ExceptionMiddleWare>();

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles();

app.UseSwaggerDocumentation();

await app.UseApplicationBuilder();

await app.RunAsync();
