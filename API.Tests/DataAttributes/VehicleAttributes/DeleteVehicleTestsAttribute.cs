using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using API.Tests.FakeData;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Xunit.Sdk;

namespace API.Tests.DataAttributes.VehicleAttributes
{
    public class DeleteVehicleTestsAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[]
            {
                1,
                FakeVehicles<Vehicle>.FakeVehicleData(1, new Vehicle()),
                typeof(NoContentResult)
            };

            yield return new object[]
            {
                200,
                FakeVehicles<Vehicle>.FakeVehicleData(1, new Vehicle()),
                typeof(NotFoundObjectResult)
            };

            yield return new object[]
            {
                1,
                FakeVehicles<Vehicle>.FakeVehicleData(1, new Vehicle()),
                typeof(BadRequestObjectResult)
            };
        }
    }
}
