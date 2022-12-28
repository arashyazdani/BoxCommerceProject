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
using Domain.Specifications.WarehouseSpecifications;
using Xunit.Sdk;

namespace API.Tests.DataAttributes.WarehouseAttributes
{
    public class GetWarehouseListTests : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] { 
                FakeWarehouses<IReadOnlyList<Warehouse>>.FakeWarehouseList.Generate(10), 
                typeof(OkObjectResult)
            };

            yield return new object[]
            {
                It.IsAny<List<Warehouse>>(), 
                typeof(NotFoundObjectResult)
            };

            yield return new object[]
            {
                FakeWarehouses<IReadOnlyList<Warehouse>>.FakeWarehouseList.Generate(10),
                typeof(FormatException)
            };
        }
    }
}
