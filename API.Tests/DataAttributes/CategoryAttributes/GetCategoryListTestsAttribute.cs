using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using API.Tests.FakeData;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit.Sdk;

namespace API.Tests.DataAttributes.CategoryAttributes
{
    public class GetCategoryListTestsAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] { FakeCategories<IReadOnlyList<Category>>.FakeCategoryList.Generate(10), typeof(OkObjectResult) };
            yield return new object[] { It.IsAny<List<Category>>(), typeof(NotFoundObjectResult) };

        }
    }
}
