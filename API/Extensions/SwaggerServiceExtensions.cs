using Microsoft.OpenApi.Models;
using System.ComponentModel;
using API.DTOs.Examples;
using Microsoft.Extensions.Options;

namespace API.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BoxCommerce API", Version = "v1" });

                // I used DisplayName because I want to display a correct and meaningful name in Swagger Schemas
                c.CustomSchemaIds(x => x.GetCustomAttributes(false).OfType<DisplayNameAttribute>().FirstOrDefault()?.DisplayName ?? SwashbuckleSchemaHelper.GetSchemaId(x));
                
                c.SchemaFilter<RegisterDtoExample>();
                c.SchemaFilter<LoginDtoExample>();

                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Auth Bearer Scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.AddSecurityDefinition("Bearer", securitySchema);
                var securityRequirement = new OpenApiSecurityRequirement { { securitySchema, new[] { "Bearer" } } };
                c.AddSecurityRequirement(securityRequirement);
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "BoxCommerce API v1"); c.DefaultModelExpandDepth(-1); });

            return app;
        }

        internal static class SwashbuckleSchemaHelper
        {
            private static readonly Dictionary<string, int> _schemaNameRepetition = new Dictionary<string, int>();

            public static string GetSchemaId(Type type)
            {
                string id = type.Name;

                if (!_schemaNameRepetition.ContainsKey(id))
                    _schemaNameRepetition.Add(id, 0);

                int count = (_schemaNameRepetition[id] + 1);
                _schemaNameRepetition[id] = count;

                return type.Name + (count > 1 ? count.ToString() : "");
            }
        }
    }
}
