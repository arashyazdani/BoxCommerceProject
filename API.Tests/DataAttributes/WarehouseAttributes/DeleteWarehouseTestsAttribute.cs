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

namespace API.Tests.DataAttributes.WarehouseAttributes
{
    public class DeleteWarehouseTestsAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[]
            {
                1,
                FakeWarehouses<Warehouse>.FakeWarehouseData(1, new Warehouse()),
                typeof(NoContentResult)
            };

            yield return new object[]
            {
                200,
                FakeWarehouses<Warehouse>.FakeWarehouseData(1, new Warehouse()),
                typeof(NotFoundObjectResult)
            };

            yield return new object[]
            {
                1,
                FakeWarehouses<Warehouse>.FakeWarehouseData(1, new Warehouse()),
                typeof(BadRequestObjectResult)
            };
        }
    }
}
