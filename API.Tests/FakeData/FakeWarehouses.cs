using Bogus;
using Domain.Specifications.CategorySpecifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Tests.FakeData
{
    public class FakeWarehouses<T> where T : class
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
            returnData.Address = faker.Address;
            if (parentId == -1) returnData.UpdatedDate = DateTimeOffset.Now;

            return returnData;
        }
    }
}
