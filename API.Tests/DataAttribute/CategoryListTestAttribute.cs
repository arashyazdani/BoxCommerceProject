using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace API.Tests.DataAttribute
{
    public class CategoryListTestAttribute : Xunit.Sdk.DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] { FakeData<IReadOnlyList<Category>>.CreateListOfTestCategory(), typeof(OkObjectResult) };
            yield return new object[] { It.IsAny<List<Category>>(), typeof(NotFoundObjectResult) };

        }
    }
}
