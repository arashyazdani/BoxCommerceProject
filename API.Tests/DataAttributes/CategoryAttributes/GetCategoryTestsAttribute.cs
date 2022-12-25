using System.Reflection;
using API.Tests.FakeData;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit.Sdk;


namespace API.Tests.DataAttributes.CategoryAttributes
{
    public class GetCategoryTestsAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[]{ 1,FakeCategories<Category>.FakeCategoryData(null,new Category()), typeof(OkObjectResult) };
            yield return new object[] {100, It.IsAny<Category>(), typeof(NotFoundObjectResult) };
        }

    }
}
