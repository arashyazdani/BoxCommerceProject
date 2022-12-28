using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Bogus.DataSets;
using Domain.Specifications.WarehouseSpecifications;

namespace API.Tests.FakeData
{
    public class FakeWarehouses<T> where T : class
    {
        private static int _id = 1;

        public static T FakeWarehouseData(int? parentId, T obj)
        {
            dynamic returnData = obj;
            var faker = new Faker();

            if (typeof(T) != typeof(CreateWarehouseParams)) returnData.Id = 1;
            returnData.Priority = 1;
            returnData.Name = faker.Company.CompanyName();
            returnData.Enabled = true;
            returnData.Details = faker.Commerce.ProductDescription();
            returnData.Address = faker.Address.FullAddress();
            if (parentId == -1) returnData.UpdatedDate = DateTimeOffset.Now;

            return returnData;
        }

        public static Faker<Warehouse> FakeWarehouseList { get; } =
            new Faker<Warehouse>()
                .RuleFor(p => p.Id, f => _id++)
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Details, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Priority, f => f.Commerce.Random.Int(1, _id))
                .RuleFor(p => p.Address, f => f.Address.FullAddress())
                .RuleFor(p => p.Enabled, f => true);


    }
}
