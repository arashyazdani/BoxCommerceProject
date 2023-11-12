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
using Moq;

namespace API.Tests.DataAttributes.VehicleAttributes
{
    public class AddOrUpdateVehiclesPartsTestsAttributes : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[]
            {
                "Test vehicle not found",
                FakeVehicles<Vehicle>.FakeVehiclesPartsParams(),
                FakeCommonData<GetObjectFromServicesSpecification>.FakeServiceObject(404, "The Vehicle Id is not found.", FakeVehicles<Vehicle>.FakeVehicleData(null, new Vehicle())),
                typeof(NotFoundObjectResult),
                FakeProducts<Product>.FakeProductData(1,new Product()),
                It.IsAny<Vehicle>(),
                FakeVehicles<VehiclesPart>.FakeVehiclePartList.Generate(3)
            };

            yield return new object[]
            {
                "Test product not found",
                FakeVehicles<Vehicle>.FakeVehiclesPartsParams(),
                FakeCommonData<GetObjectFromServicesSpecification>.FakeServiceObject(404, "The Vehicle Id is not found.", FakeVehicles<Product>.FakeVehicleData(null, new Product())),
                typeof(NotFoundObjectResult),
                FakeProducts<Product>.FakeProductData(1,new Product()),
                FakeVehicles<Vehicle>.FakeVehicleWithPartsData(),
                FakeVehicles<VehiclesPart>.FakeVehiclePartList.Generate(3)
            };

            yield return new object[]
            {
                "Not Modified.",
                FakeVehicles<Vehicle>.FakeVehiclesPartsParams(),
                FakeCommonData<GetObjectFromServicesSpecification>.FakeServiceObject(304, "Not Modified.", FakeVehicles<Product>.FakeVehicleData(null, new Product())),
                typeof(StatusCodeResult),
                FakeProducts<Product>.FakeProductData(1,new Product()),
                FakeVehicles<Vehicle>.FakeVehicleWithPartsData(),
                FakeVehicles<VehiclesPart>.FakeVehiclePartList.Generate(3)
            };

            yield return new object[]
            {
                "Not Modified.",
                FakeVehicles<Vehicle>.FakeVehiclesPartsParams(),
                FakeCommonData<GetObjectFromServicesSpecification>.FakeServiceObject(204, "VehiclesParts has been updated successfully.", FakeVehicles<Product>.FakeVehicleData(null, new Product())),
                typeof(NoContentResult),
                FakeProducts<Product>.FakeProductData(1,new Product()),
                FakeVehicles<Vehicle>.FakeVehicleWithPartsData(),
                FakeVehicles<VehiclesPart>.FakeVehiclePartList.Generate(3)
            };
        }
    }
}
