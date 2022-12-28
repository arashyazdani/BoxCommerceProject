using API.Tests.FakeData;
using Domain.Specifications.ProductSpecifications;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Specifications;
using Xunit.Sdk;

namespace API.Tests.DataAttributes.ProductAttributes
{
    public class UpdateProductTestAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[]
            {
                FakeProducts<UpdateProductParams>.FakeProductData(1, new UpdateProductParams()),
                FakeProducts<Product>.FakeProductData(1, new Product()),
                typeof(NoContentResult),
                FakeCommonData<GetObjectFromServicesSpecification>.FakeServiceObject(204, "Product has been updated successfully.", FakeProducts<Product>.FakeProductData(1, new Product()))
            };

            yield return new object[]
            {
                FakeProducts<UpdateProductParams>.FakeProductData(1, new UpdateProductParams()),
                FakeProducts<Product>.FakeProductData(1, new Product()),
                typeof(NotFoundObjectResult),
                FakeCommonData<GetObjectFromServicesSpecification>.FakeServiceObject(404, "The ProductId is not found.", FakeProducts<Product>.FakeProductData(1, new Product()))
            };

            yield return new object[]
            {
                FakeProducts<UpdateProductParams>.FakeProductData(1, new UpdateProductParams()),
                FakeProducts<Product>.FakeProductData(-1, new Product()),
                typeof(StatusCodeResult),
                FakeCommonData<GetObjectFromServicesSpecification>.FakeServiceObject(304, "Bad request.", FakeProducts<Product>.FakeProductData(-1, new Product()))
            };

            yield return new object[]
            {
                FakeProducts<UpdateProductParams>.FakeProductData(1, new UpdateProductParams()),
                FakeProducts<Product>.FakeProductData(1, new Product()),
                typeof(BadRequestObjectResult),
                FakeCommonData<GetObjectFromServicesSpecification>.FakeServiceObject(400, "Not modified.", FakeProducts<Product>.FakeProductData(1, new Product()))
            };
        }
    }
}
