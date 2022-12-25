using API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using API.Tests.FakeData;
using Xunit.Sdk;
using Domain.Specifications.CategorySpecifications;

namespace API.Tests.DataAttributes.CategoryAttributes
{
    public class CreateCategoryTestAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] { FakeCategories<CreateCategoryParams>.FakeCategoryData(null, new CreateCategoryParams()), FakeCategories<Category>.FakeCategoryData(null, new Category()), FakeCategories<CategoryToReturnDto>.FakeCategoryData(null, new CategoryToReturnDto()), typeof(CreatedAtRouteResult) };
            yield return new object[] { FakeCategories<CreateCategoryParams>.FakeCategoryData(200, new CreateCategoryParams()), FakeCategories<Category>.FakeCategoryData(null, new Category()), FakeCategories<CategoryToReturnDto>.FakeCategoryData(null, new CategoryToReturnDto()), typeof(NotFoundObjectResult) };
            yield return new object[] { FakeCategories<CreateCategoryParams>.FakeCategoryData(null, new CreateCategoryParams()), FakeCategories<Category>.FakeCategoryData(null, new Category()), FakeCategories<CategoryToReturnDto>.FakeCategoryData(null, new CategoryToReturnDto()), typeof(BadRequestObjectResult) };
        }
    }
}
