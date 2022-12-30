using API.Tests.FakeData;
using Domain.Entities;
using Domain.Specifications.VehicleSpecifications;
using Domain.Specifications;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace API.Tests.DataAttributes.VehicleAttributes
{
    public class UpdateVehicleTestsAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[]
            {
                FakeVehicles<UpdateVehicleParams>.FakeVehicleData(null, new UpdateVehicleParams()),
                FakeVehicles<Vehicle>.FakeVehicleData(null, new Vehicle()),
                typeof(NoContentResult),
                FakeCommonData<GetObjectFromServicesSpecification>.FakeServiceObject(204, "Vehicle has been updated successfully.", FakeVehicles<Vehicle>.FakeVehicleData(null, new Vehicle()))
            };

            yield return new object[]
            {
                FakeVehicles<UpdateVehicleParams>.FakeVehicleData(null, new UpdateVehicleParams()),
                FakeVehicles<Vehicle>.FakeVehicleData(null, new Vehicle()),
                typeof(NotFoundObjectResult),
                FakeCommonData<GetObjectFromServicesSpecification>.FakeServiceObject(404, "The Vehicle Id is not found.", FakeVehicles<Vehicle>.FakeVehicleData(null, new Vehicle()))
            };

            yield return new object[]
            {
                FakeVehicles<UpdateVehicleParams>.FakeVehicleData(null, new UpdateVehicleParams()),
                FakeVehicles<Vehicle>.FakeVehicleData(-1, new Vehicle()),
                typeof(StatusCodeResult),
                FakeCommonData<GetObjectFromServicesSpecification>.FakeServiceObject(304, "Bad request.", FakeVehicles<Vehicle>.FakeVehicleData(-1, new Vehicle()))
            };

            yield return new object[]
            {
                FakeVehicles<UpdateVehicleParams>.FakeVehicleData(null, new UpdateVehicleParams()),
                FakeVehicles<Vehicle>.FakeVehicleData(null, new Vehicle()),
                typeof(BadRequestObjectResult),
                FakeCommonData<GetObjectFromServicesSpecification>.FakeServiceObject(400, "Not modified.", FakeVehicles<Vehicle>.FakeVehicleData(null, new Vehicle()))
            };
        }
    }
}
