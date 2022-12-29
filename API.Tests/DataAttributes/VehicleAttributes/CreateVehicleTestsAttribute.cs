using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using API.DTOs;
using API.Tests.FakeData;
using Domain.Entities;
using Domain.Specifications;
using Domain.Specifications.VehicleSpecifications;
using Microsoft.AspNetCore.Mvc;
using Xunit.Sdk;

namespace API.Tests.DataAttributes.VehicleAttributes
{
    public class CreateVehicleTestsAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[]
            {
                FakeVehicles<CreateVehicleParams>.FakeVehicleData(null, new CreateVehicleParams()),
                FakeVehicles<Vehicle>.FakeVehicleData(null, new Vehicle()),
                FakeVehicles<VehicleToReturnDto>.FakeVehicleData(null, new VehicleToReturnDto()),
                typeof(CreatedAtRouteResult),
                FakeCommonData<GetObjectFromServicesSpecification>.FakeServiceObject(201, "Vehicle has been created successfully.", FakeVehicles<Vehicle>.FakeVehicleData(null, new Vehicle()))
            };

            yield return new object[]
            {
                FakeVehicles<CreateVehicleParams>.FakeVehicleData(200, new CreateVehicleParams()),
                FakeVehicles<Vehicle>.FakeVehicleData(null, new Vehicle()),
                FakeVehicles<VehicleToReturnDto>.FakeVehicleData(null, new VehicleToReturnDto()),
                typeof(FormatException),
                FakeCommonData<GetObjectFromServicesSpecification>.FakeServiceObject(400, "Exception.", FakeVehicles<Vehicle>.FakeVehicleData(null, new Vehicle()))
            };

            yield return new object[]
            {
                FakeVehicles<CreateVehicleParams>.FakeVehicleData(null, new CreateVehicleParams()),
                FakeVehicles<Vehicle>.FakeVehicleData(null, new Vehicle()),
                FakeVehicles<VehicleToReturnDto>.FakeVehicleData(null, new VehicleToReturnDto()),
                typeof(BadRequestObjectResult),
                FakeCommonData<GetObjectFromServicesSpecification>.FakeServiceObject(400, "Bad request.", FakeVehicles<Vehicle>.FakeVehicleData(null, new Vehicle()))
            };

            yield return new object[]
            {
                FakeVehicles<CreateVehicleParams>.FakeVehicleData(null, new CreateVehicleParams()),
                FakeVehicles<Vehicle>.FakeVehicleData(null, new Vehicle()),
                FakeVehicles<VehicleToReturnDto>.FakeVehicleData(null, new VehicleToReturnDto()),
                typeof(ConflictObjectResult),
                FakeCommonData<GetObjectFromServicesSpecification>.FakeServiceObject(409, "The Vehicle name is already exist.", FakeVehicles<Vehicle>.FakeVehicleData(null, new Vehicle()))
            };
        }
    }
}
