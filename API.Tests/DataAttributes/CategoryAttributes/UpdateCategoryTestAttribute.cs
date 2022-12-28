using API.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using API.Tests.FakeData;
using Domain.Specifications;
using Xunit.Sdk;
using Domain.Specifications.CategorySpecifications;

namespace API.Tests.DataAttributes.CategoryAttributes
{
    public class UpdateCategoryTestAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[]
            {
                FakeCategories<UpdateCategoryParams>.FakeCategoryData(null, new UpdateCategoryParams()), 
                FakeCategories<Category>.FakeCategoryData(null, new Category()),  
                typeof(NoContentResult),
                FakeCommonData<GetObjectFromServicesSpecification>.FakeServiceObject(204, "Category has been updated successfully.", FakeCategories<Category>.FakeCategoryData(null, new Category()))
            };

            yield return new object[]
            {
                FakeCategories<UpdateCategoryParams>.FakeCategoryData(null, new UpdateCategoryParams()), 
                FakeCategories<Category>.FakeCategoryData(null, new Category()), 
                typeof(NotFoundObjectResult),
                FakeCommonData<GetObjectFromServicesSpecification>.FakeServiceObject(404, "The CategoryId is not found.", FakeCategories<Category>.FakeCategoryData(null, new Category()))
            };

            yield return new object[]
            {
                FakeCategories<UpdateCategoryParams>.FakeCategoryData(null, new UpdateCategoryParams()), 
                FakeCategories<Category>.FakeCategoryData(-1, new Category()),  
                typeof(StatusCodeResult),
                FakeCommonData<GetObjectFromServicesSpecification>.FakeServiceObject(304, "Bad request.", FakeCategories<Category>.FakeCategoryData(-1, new Category()))
            };

            yield return new object[]
            {
                FakeCategories<UpdateCategoryParams>.FakeCategoryData(null, new UpdateCategoryParams()),
                FakeCategories<Category>.FakeCategoryData(null, new Category()),
                typeof(BadRequestObjectResult),
                FakeCommonData<GetObjectFromServicesSpecification>.FakeServiceObject(400, "Not modified.", FakeCategories<Category>.FakeCategoryData(null, new Category()))
            };
        }
    }
}
