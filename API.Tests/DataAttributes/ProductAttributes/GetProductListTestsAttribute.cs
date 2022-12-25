using API.Tests.FakeData;
using Microsoft.AspNetCore.Mvc;
using Moq;
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
    public class GetProductListTestsAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] { FakeProducts<IReadOnlyList<Product>>.FakeProductList.Generate(10), typeof(OkObjectResult) };
            yield return new object[] { It.IsAny<List<Product>>(), typeof(NotFoundObjectResult) };
        }
    }
}
