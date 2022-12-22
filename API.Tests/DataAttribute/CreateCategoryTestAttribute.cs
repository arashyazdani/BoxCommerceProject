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
            yield return new object[] { FakeData<CreateCategoryParams>.CategoryData(null), FakeData<Category>.CategoryData(null), FakeData<CategoryToReturnDto>.CategoryData(null), typeof(CreatedAtRouteResult) };
            yield return new object[] { FakeData<CreateCategoryParams>.CategoryData(200), FakeData<Category>.CategoryData(null), FakeData<CategoryToReturnDto>.CategoryData(null), typeof(NotFoundObjectResult) };
            yield return new object[] { FakeData<CreateCategoryParams>.CategoryData(null), FakeData<Category>.CategoryData(null), FakeData<CategoryToReturnDto>.CategoryData(null), typeof(BadRequestObjectResult) };
        }
    }
}
