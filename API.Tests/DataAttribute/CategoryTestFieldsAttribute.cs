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
                FakeData.CreateTestCategory(),
                typeof(OkObjectResult)
            };
            yield return new object[] {100, It.IsAny<Category>(), typeof(NotFoundObjectResult) };
        }

        private Category CreateTestCategory()
        {
            return new Category()
            {
                Id = 1,
                Priority = 1,
                Name = "Test Category",
                Enabled = true,
                Details = "Testing category data"
            };
        }
    }
}
