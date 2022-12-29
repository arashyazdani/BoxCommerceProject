using API.Tests.FakeData;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace API.Tests.DataAttributes.VehicleAttributes
{
    public class GetVehicleTestsAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[]
            {
                1,
                FakeVehicles<Vehicle>.FakeVehicleData(null,new Vehicle()),
                typeof(OkObjectResult)
            };

            yield return new object[]
            {
                -1,
                FakeVehicles<Vehicle>.FakeVehicleData(null,new Vehicle()),
                typeof(FormatException)
            };

            yield return new object[]
            {
                100,
                It.IsAny<Vehicle>(),
                typeof(NotFoundObjectResult)
            };
        }
    }
}
