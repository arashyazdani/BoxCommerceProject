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
            yield return new object[] { FakeData<CreateCategoryParams>.CategoryData(null, new CreateCategoryParams()), FakeData<Category>.CategoryData(null, new Category()), FakeData<CategoryToReturnDto>.CategoryData(null, new CategoryToReturnDto()), typeof(CreatedAtRouteResult) };
            yield return new object[] { FakeData<CreateCategoryParams>.CategoryData(200, new CreateCategoryParams()), FakeData<Category>.CategoryData(null, new Category()), FakeData<CategoryToReturnDto>.CategoryData(null, new CategoryToReturnDto()), typeof(NotFoundObjectResult) };
            yield return new object[] { FakeData<CreateCategoryParams>.CategoryData(null, new CreateCategoryParams()), FakeData<Category>.CategoryData(null, new Category()), FakeData<CategoryToReturnDto>.CategoryData(null, new CategoryToReturnDto()), typeof(BadRequestObjectResult) };
        }
    }
}
