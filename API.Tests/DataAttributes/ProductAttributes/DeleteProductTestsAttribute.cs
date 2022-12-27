using API.Tests.FakeData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Xunit.Sdk;

namespace API.Tests.DataAttributes.ProductAttributes
{
    public class DeleteProductTestsAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[]
            {
                1,
                FakeProducts<Product>.FakeProductData(1, new Product()),
                typeof(NoContentResult)
            };

            yield return new object[]
            {
                200,
                FakeProducts<Product>.FakeProductData(1, new Product()),
                typeof(NotFoundObjectResult)
            };

            yield return new object[]
            {
                1,
                FakeProducts<Product>.FakeProductData(1, new Product()),
                typeof(BadRequestObjectResult)
            };
        }
    }
}
