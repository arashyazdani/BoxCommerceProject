using API.DTOs;
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
using Xunit.Sdk;

namespace API.Tests.DataAttributes.WarehouseAttributes
{
    public class CreateWarehouseTestsAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[]
            {
                FakeWarehouses<CreateWarehouseParams>.FakeWarehouseData(null, new CreateWarehouseParams()),
                FakeWarehouses<Warehouse>.FakeWarehouseData(null, new Warehouse()),
                FakeWarehouses<WarehouseToReturnDto>.FakeWarehouseData(null, new WarehouseToReturnDto()),
                typeof(CreatedAtRouteResult),
                FakeCommonData<GetObjectFromServicesSpecification>.FakeServiceObject(201, "Warehouse has been created successfully.", FakeWarehouses<Warehouse>.FakeWarehouseData(null, new Warehouse()))
            };

            yield return new object[]
            {
                FakeWarehouses<CreateWarehouseParams>.FakeWarehouseData(200, new CreateWarehouseParams()),
                FakeWarehouses<Warehouse>.FakeWarehouseData(null, new Warehouse()),
                FakeWarehouses<WarehouseToReturnDto>.FakeWarehouseData(null, new WarehouseToReturnDto()),
                typeof(FormatException),
                FakeCommonData<GetObjectFromServicesSpecification>.FakeServiceObject(400, "Exception.", FakeWarehouses<Warehouse>.FakeWarehouseData(null, new Warehouse()))
            };

            yield return new object[]
            {
                FakeWarehouses<CreateWarehouseParams>.FakeWarehouseData(null, new CreateWarehouseParams()),
                FakeWarehouses<Warehouse>.FakeWarehouseData(null, new Warehouse()),
                FakeWarehouses<WarehouseToReturnDto>.FakeWarehouseData(null, new WarehouseToReturnDto()),
                typeof(BadRequestObjectResult),
                FakeCommonData<GetObjectFromServicesSpecification>.FakeServiceObject(400, "Bad request.", FakeWarehouses<Warehouse>.FakeWarehouseData(null, new Warehouse()))
            };

            yield return new object[]
            {
                FakeWarehouses<CreateWarehouseParams>.FakeWarehouseData(null, new CreateWarehouseParams()),
                FakeWarehouses<Warehouse>.FakeWarehouseData(null, new Warehouse()),
                FakeWarehouses<WarehouseToReturnDto>.FakeWarehouseData(null, new WarehouseToReturnDto()),
                typeof(ConflictObjectResult),
                FakeCommonData<GetObjectFromServicesSpecification>.FakeServiceObject(409, "The warehouse name is already exist.", FakeWarehouses<Warehouse>.FakeWarehouseData(null, new Warehouse()))
            };
        }
    }
}
