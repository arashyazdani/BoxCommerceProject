using System.Reflection;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace API.Tests.DataAttribute
{
    public class CategoryTestFieldsAttribute : Xunit.Sdk.DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[]
            {1,
                FakeData<Category>.CategoryData(null,new Category()),
                typeof(OkObjectResult)
            };
            yield return new object[] {100, It.IsAny<Category>(), typeof(NotFoundObjectResult) };
        }

    }
}
