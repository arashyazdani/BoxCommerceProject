using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using API.DTOs;
using Bogus;
using Domain.Entities;
using Domain.Specifications;
using Domain.Specifications.CategorySpecifications;

namespace API.Tests.FakeData
{
    public static class FakeCategories<T> where T : class
    {
        private static int _id = 1;

        public static T FakeCategoryData(int? parentId, T obj)
        {
            dynamic returnData = obj;
            var faker = new Faker();

            if (typeof(T) != typeof(CreateCategoryParams)) returnData.Id = 1;
            returnData.Priority = 1;
            returnData.Name = faker.Company.CompanyName();
            returnData.Enabled = true;
            returnData.Details = faker.Commerce.ProductDescription();
            if (parentId != null) returnData.ParentCategoryId = parentId;
            if (parentId==-1) returnData.UpdatedDate = DateTimeOffset.Now;

                return returnData;
        }

        public static Faker<Category> FakeCategoryList { get; } =
            new Faker<Category>()
                .RuleFor(p => p.Id, f => _id++)
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Details, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Priority, f => f.Commerce.Random.Int(1, _id))
                .RuleFor(p => p.ParentCategoryId, f => _id > 1 ? f.Commerce.Random.Int(1, 3) : null)
                .RuleFor(p => p.Enabled, f => true);

        

    }
}