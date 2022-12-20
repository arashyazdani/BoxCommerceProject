using System.Reflection;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;


namespace API.Tests.DataAttribute
{
    public class CategoryTestFieldsAttribute : Xunit.Sdk.DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[]
            {1,
                new Category()
                {
                    Id = 1,
                    Priority = 1,
                    Name = "Test Category",
                    Enabled = true,
                    Details = "Testing category data"
                },
                typeof(OkObjectResult)
            };
            yield return new object[] {100, new Category(), typeof(OkObjectResult) };
        }
    }
}
