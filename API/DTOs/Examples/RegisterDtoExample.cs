using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.DTOs.Examples
{
    public class RegisterDtoExample : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(RegisterDTO))
            {
                schema.Example = new OpenApiObject()
                {
                    ["DisplayName"] = new OpenApiString("Arash"),
                    ["Email"] = new OpenApiString("example@email.com"),
                    ["Password"] = new OpenApiString("Pa$$w0rd"),
                };
            }
        }
    }
}
