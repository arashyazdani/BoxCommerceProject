using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Specifications;
using Domain.Specifications.ProductSpecifications;
using Domain.Specifications.CategorySpecifications;

namespace API.Tests.FakeData
{
    public static class FakeProducts<T> where T : class
    {
        private static int _id = 1;

        public static T FakeProductData(int parentId, T obj)
        {
            dynamic returnData = obj;
            var faker = new Faker();

            if (typeof(T) != typeof(CreateProductParams)) returnData.Id = 1;
            returnData.Priority = 1;
            returnData.Name = faker.Commerce.ProductName();
            returnData.Enabled = true;
            returnData.Details = faker.Commerce.ProductDescription();
            returnData.CategoryId = parentId;
            returnData.Quantity = faker.Commerce.Random.Int(0, 200);
            returnData.Price = decimal.Parse(faker.Commerce.Price(1000, 170000));
            returnData.IsDiscontinued = false;

            return returnData;
        }

        public static Faker<Product> FakeProductList { get; } =
            new Faker<Product>()
                .RuleFor(p => p.Id, f => _id++)
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Details, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Priority, f => f.Commerce.Random.Int(1, _id))
                .RuleFor(p => p.CategoryId, f => f.Commerce.Random.Int(1, 3))
                .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price(1000, 170000)))
                .RuleFor(p => p.Quantity, f => f.Commerce.Random.Int(0, 200))
                .RuleFor(p => p.Enabled, f => true)
                .RuleFor(p => p.IsDiscontinued, f => f.Commerce.Random.Bool());

        
    }
}
