using API.DTOs;
using System;
using System.Collections.Generic;
using System.Dynamic;
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

            yield return new object[]
            {
                FakeCategories<CreateCategoryParams>.FakeCategoryData(null, new CreateCategoryParams()), 
                FakeCategories<Category>.FakeCategoryData(null, new Category()), 
                FakeCategories<CategoryToReturnDto>.FakeCategoryData(null, new CategoryToReturnDto()), 
                typeof(CreatedAtRouteResult), 
                FakeCategories<GetObjectFromCategoryService>.FakeCategoryServiceObject(201, "Category has been created successfully.", FakeCategories<Category>.FakeCategoryData(null, new Category()))
            };

            yield return new object[]
            {
                FakeCategories<CreateCategoryParams>.FakeCategoryData(200, new CreateCategoryParams()), 
                FakeCategories<Category>.FakeCategoryData(null, new Category()), 
                FakeCategories<CategoryToReturnDto>.FakeCategoryData(null, new CategoryToReturnDto()), 
                typeof(NotFoundObjectResult), 
                FakeCategories<GetObjectFromCategoryService>.FakeCategoryServiceObject(404, "The ParentCategoryId is not found.", FakeCategories<Category>.FakeCategoryData(null, new Category()))
            };

            yield return new object[]
            {
                FakeCategories<CreateCategoryParams>.FakeCategoryData(null, new CreateCategoryParams()), 
                FakeCategories<Category>.FakeCategoryData(null, new Category()), 
                FakeCategories<CategoryToReturnDto>.FakeCategoryData(null, new CategoryToReturnDto()), 
                typeof(BadRequestObjectResult), 
                FakeCategories<GetObjectFromCategoryService>.FakeCategoryServiceObject(400, "Bad request.", FakeCategories<Category>.FakeCategoryData(null, new Category()))
            };

            yield return new object[]
            {
                FakeCategories<CreateCategoryParams>.FakeCategoryData(null, new CreateCategoryParams()), 
                FakeCategories<Category>.FakeCategoryData(null, new Category()), 
                FakeCategories<CategoryToReturnDto>.FakeCategoryData(null, new CategoryToReturnDto()), 
                typeof(ConflictObjectResult), 
                FakeCategories<GetObjectFromCategoryService>.FakeCategoryServiceObject(409, "The category name is already exist.", FakeCategories<Category>.FakeCategoryData(null, new Category()))
            };

        }
    }
}
