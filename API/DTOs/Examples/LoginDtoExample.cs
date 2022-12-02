using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.DTOs.Examples
{
    public class LoginDtoExample : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(LoginDTO))
            {
                schema.Example = new OpenApiObject()
                {
                    ["Email"] = new OpenApiString("example@email.com"),
                    ["Password"] = new OpenApiString("Pa$$w0rd"),
                };
            }
        }
    }
}
