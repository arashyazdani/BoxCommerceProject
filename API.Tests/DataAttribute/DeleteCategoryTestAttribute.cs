using Domain.Specifications;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace API.Tests.DataAttribute
{
    public class DeleteCategoryTestAttribute : Xunit.Sdk.DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] { 1, FakeData<Category>.CategoryData(null), typeof(NoContentResult) };
            yield return new object[] { 200, FakeData<Category>.CategoryData(null), typeof(NotFoundObjectResult) };
            yield return new object[] { 1, FakeData<Category>.CategoryData(null), typeof(BadRequestObjectResult) };
        }
    }
}
