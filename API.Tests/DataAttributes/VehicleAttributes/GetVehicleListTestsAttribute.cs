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
    public class GetVehicleListTestsAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] {
                FakeVehicles<IReadOnlyList<Vehicle>>.FakeVehicleList.Generate(10),
                typeof(OkObjectResult)
            };

            yield return new object[]
            {
                It.IsAny<List<Vehicle>>(),
                typeof(NotFoundObjectResult)
            };

            yield return new object[]
            {
                FakeVehicles<IReadOnlyList<Vehicle>>.FakeVehicleList.Generate(10),
                typeof(FormatException)
            };
        }
    }
}
