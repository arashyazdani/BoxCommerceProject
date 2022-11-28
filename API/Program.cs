using API.Extensions;
using API.Helpers;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultSqlConnection");
var identityConnectionString = builder.Configuration.GetConnectionString("IdentitySqlConnection");
var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<FactoryContext>(x => x.UseSqlServer(connectionString));
builder.Services.AddSingleton<IConnectionMultiplexer>(c =>
{
    var configuration = ConfigurationOptions.Parse(redisConnectionString, true);
    return ConnectionMultiplexer.Connect(configuration);
});
builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddApplicationServices();

builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerDocumentation();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles();

app.UseSwaggerDocumentation();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var context = services.GetRequiredService<FactoryContext>();
        //var userManager = services.GetRequiredService<UserManager<AppUser>>();
        //var identityContext = services.GetRequiredService<ArashAppIdentityDbContext>();

        await context.Database.MigrateAsync();
        //await FactoryContextSeed.SeedAsync(context, loggerFactory);
        var seedData = new FactoryContextSeed(context, loggerFactory);
        var logger = loggerFactory.CreateLogger<Program>();
        if (await seedData.SeedAsync()) logger.LogInformation("Seeding data has been done successfully");

        //await identityContext.Database.MigrateAsync();
        //await ArashAppIdentityDbContextSeed.SeedUsersAsync(userManager);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occurred during migration");
    }
}

await app.RunAsync();
