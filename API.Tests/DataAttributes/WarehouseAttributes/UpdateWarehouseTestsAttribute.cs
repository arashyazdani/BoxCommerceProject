using API.Tests.FakeData;
using Domain.Specifications.WarehouseSpecifications;
using Domain.Specifications;
using Microsoft.AspNetCore.Mvc;
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
    public class UpdateWarehouseTestsAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[]
            {
                FakeWarehouses<UpdateWarehouseParams>.FakeWarehouseData(null, new UpdateWarehouseParams()),
                FakeWarehouses<Warehouse>.FakeWarehouseData(null, new Warehouse()),
                typeof(NoContentResult),
                FakeCommonData<GetObjectFromServicesSpecification>.FakeServiceObject(204, "Warehouse has been updated successfully.", FakeWarehouses<Warehouse>.FakeWarehouseData(null, new Warehouse()))
            };

            yield return new object[]
            {
                FakeWarehouses<UpdateWarehouseParams>.FakeWarehouseData(null, new UpdateWarehouseParams()),
                FakeWarehouses<Warehouse>.FakeWarehouseData(null, new Warehouse()),
                typeof(NotFoundObjectResult),
                FakeCommonData<GetObjectFromServicesSpecification>.FakeServiceObject(404, "The Warehouse Id is not found.", FakeWarehouses<Warehouse>.FakeWarehouseData(null, new Warehouse()))
            };

            yield return new object[]
            {
                FakeWarehouses<UpdateWarehouseParams>.FakeWarehouseData(null, new UpdateWarehouseParams()),
                FakeWarehouses<Warehouse>.FakeWarehouseData(-1, new Warehouse()),
                typeof(StatusCodeResult),
                FakeCommonData<GetObjectFromServicesSpecification>.FakeServiceObject(304, "Bad request.", FakeWarehouses<Warehouse>.FakeWarehouseData(-1, new Warehouse()))
            };

            yield return new object[]
            {
                FakeWarehouses<UpdateWarehouseParams>.FakeWarehouseData(null, new UpdateWarehouseParams()),
                FakeWarehouses<Warehouse>.FakeWarehouseData(null, new Warehouse()),
                typeof(BadRequestObjectResult),
                FakeCommonData<GetObjectFromServicesSpecification>.FakeServiceObject(400, "Not modified.", FakeWarehouses<Warehouse>.FakeWarehouseData(null, new Warehouse()))
            };
        }
    }
}
