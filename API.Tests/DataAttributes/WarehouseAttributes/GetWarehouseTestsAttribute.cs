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

namespace API.Tests.DataAttributes.WarehouseAttributes
{
    public class GetWarehouseTestsAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[]
            {
                1,
                FakeCategories<Warehouse>.FakeCategoryData(null,new Warehouse()),
                typeof(OkObjectResult)
            };

            yield return new object[]
            {
                -1,
                FakeCategories<Warehouse>.FakeCategoryData(null,new Warehouse()),
                typeof(FormatException)
            };

            yield return new object[]
            {
                100,
                It.IsAny<Warehouse>(),
                typeof(NotFoundObjectResult)
            };
        }
    }
}
