﻿using Domain.Interfaces;
using Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //services.AddSingleton<IResponseCacheService, ResponseCacheService>();
            //services.AddScoped<ITokenService, TokenService>();
            //services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddScoped<IUnitOfWorkLong, UnitOfWorkLong>();
            //services.AddScoped<IUnitOfWorkShort, UnitOfWorkShort>();
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
    }
}
