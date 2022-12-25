using Domain.Specifications;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using API.Tests.FakeData;
using Xunit.Sdk;

namespace API.Tests.DataAttributes.CategoryAttributes
{
    public class DeleteCategoryTestAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] { 1, FakeCategories<Category>.FakeCategoryData(null, new Category()), typeof(NoContentResult) };
            yield return new object[] { 200, FakeCategories<Category>.FakeCategoryData(null, new Category()), typeof(NotFoundObjectResult) };
            yield return new object[] { 1, FakeCategories<Category>.FakeCategoryData(null, new Category()), typeof(BadRequestObjectResult) };
        }
    }
}
