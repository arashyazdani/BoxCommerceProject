using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using API.Tests.FakeData;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit.Sdk;

namespace API.Tests.DataAttributes.VehicleAttributes
{
    public class GetVehicleWithVehiclePartsTestsAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[]
            {
                1,
                FakeVehicles<Vehicle>.FakeVehicleWithPartsData(),
                typeof(OkObjectResult)
            };

            yield return new object[]
            {
                -1,
                FakeVehicles<Vehicle>.FakeVehicleWithPartsData(),
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
