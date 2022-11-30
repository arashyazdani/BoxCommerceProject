﻿using Domain.Interfaces;
using Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Helpers;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //services.AddSingleton<IResponseCacheService, ResponseCacheService>();
            //services.AddScoped<ITokenService, TokenService>();
            //services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddScoped<ISmsService, SmsService>();
            //services.AddScoped<IApplicationUserService, ApplicationUserService>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            
            //services.Configure<ApiBehaviorOptions>(options =>
            //{
            //    options.InvalidModelStateResponseFactory = actionContext =>
            //    {
            //        var errors = actionContext.ModelState
            //            .Where(e => e.Value.Errors.Count > 0)
            //            .SelectMany(x => x.Value.Errors)
            //            .Select(x => x.ErrorMessage).ToArray();
            //        var errorResponse = new ApiValidationErrorResponse
            //        {
            //            Errors = errors
            //        };
            //        return new BadRequestObjectResult(errorResponse);
            //    };
            //});

            return services;
        }

        public static async Task<WebApplication> UseApplicationBuilder(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                var context = services.GetRequiredService<FactoryContext>();
                //var userManager = services.GetRequiredService<UserManager<AppUser>>();
                //var identityContext = services.GetRequiredService<FactoryApiIdentityDbContext>();

                await context.Database.MigrateAsync();
                //await FactoryContextSeed.SeedAsync(context, loggerFactory);
                var seedData = new FactoryContextSeed(context, loggerFactory);
                var logger = loggerFactory.CreateLogger<Program>();
                if (await seedData.SeedAsync()) logger.LogInformation("Seeding data has been done successfully");

                //await identityContext.Database.MigrateAsync();
                //await ArashAppIdentityDbContextSeed.SeedUsersAsync(userManager);
                return app;
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occurred during migration");
            }
            return null;
        }
    }
}
