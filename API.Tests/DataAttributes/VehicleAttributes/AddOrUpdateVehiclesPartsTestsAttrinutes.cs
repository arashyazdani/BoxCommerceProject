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
using Domain.Specifications;

namespace API.Tests.DataAttributes.VehicleAttributes
{
    public class AddOrUpdateVehiclesPartsTestsAttrinutes : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[]
            {
                FakeVehicles<Vehicle>.FakeVehiclesPartsParams(),
                FakeCommonData<GetObjectFromServicesSpecification>.FakeServiceObject(404, "The Vehicle Id is not found.", FakeVehicles<Vehicle>.FakeVehicleData(null, new Vehicle())),
                typeof(NotFoundObjectResult)
            };
        }
    }
}
