using API.DTOs;
using Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Tests.DataAttribute
{
    public class CreateCategoryTestAttribute : Xunit.Sdk.DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] { FakeData.InsertTestCategory(null), FakeData.CreateTestCategory(), FakeData.CreateTestCategoryToReturnDto(), typeof(CreatedAtRouteResult) };
            yield return new object[] { FakeData.InsertTestCategory(200), FakeData.CreateTestCategory(), FakeData.CreateTestCategoryToReturnDto(), typeof(NotFoundObjectResult) };
            yield return new object[] { FakeData.InsertTestCategory(null), FakeData.CreateTestCategory(), FakeData.CreateTestCategoryToReturnDto(), typeof(BadRequestObjectResult) };
        }
    }
}
