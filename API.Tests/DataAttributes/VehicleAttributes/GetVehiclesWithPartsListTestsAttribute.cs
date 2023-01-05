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
using Xunit.Sdk;

namespace API.Tests.DataAttributes.VehicleAttributes
{
    public class GetVehiclesWithPartsListTestsAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] {
                FakeVehicles<IReadOnlyList<Vehicle>>.FakeVehicleWithPartsDataList(),
                typeof(OkObjectResult)
            };

            yield return new object[]
            {
                //It.IsAny<List<Vehicle>>(),
                FakeVehicles<IReadOnlyList<Vehicle>>.FakeVehicleWithPartsDataList(),
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
