using Bogus;
using Domain.Entities;
using Domain.Specifications.VehicleSpecifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Tests.FakeData
{
    public static class FakeVehicles<T> where T : class
    {
        private static int _id = 1;

        public static T FakeVehicleData(int? parentId, T obj)
        {
            dynamic returnData = obj;
            var faker = new Faker();

            //if (typeof(T) != typeof(CreateVehicleParams)) returnData.Id = 1;
            returnData.Priority = 1;
            returnData.Name = faker.Company.CompanyName();
            returnData.Enabled = true;
            returnData.Details = faker.Commerce.ProductDescription();
            if (parentId == -1) returnData.UpdatedDate = DateTimeOffset.Now;

            return returnData;
        }

        public static Faker<Vehicle> FakeVehicleList { get; } =
            new Faker<Vehicle>()
                .RuleFor(p => p.Id, f => _id++)
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Details, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Priority, f => f.Commerce.Random.Int(1, _id))
                .RuleFor(p => p.Enabled, f => true);
    }
}
