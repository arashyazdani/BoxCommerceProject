using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using API.DTOs;
using Domain.Entities;
using Domain.Specifications;

namespace API.Tests.DataAttribute
{
    public static class FakeData
    {
        public static Category CreateTestCategory()
        {
            return new Category()
            {
                Id = 1,
                Priority = 1,
                Name = "Test Category",
                Enabled = true,
                Details = "Testing category data"
            };
        }

        public static CategoryToReturnDto CreateTestCategoryToReturnDto()
        {
            return new CategoryToReturnDto()
            {
                Id = 1,
                Priority = 1,
                Name = "Test Category",
                Enabled = true,
                Details = "Testing category data"
            };
        }

        public static CreateCategoryParams InsertTestCategory(int? categoryId)
        {
            return new CreateCategoryParams()
            {
                Priority = 1,
                Name = "Test Category",
                Enabled = true,
                Details = "Testing category data",
                ParentCategoryId = categoryId
            };
        }

        public static IReadOnlyList<Category> CreateListOfTestCategory()
        {
            var categoryList = new List<Category>();
            for (int i = 0; i < 10; i++)
            {
                var newCategory = new Category()
                {
                    Id = i,
                    Priority = i,
                    Name = $"Test Category {i}",
                    Enabled = true,
                    Details = $"Testing category data {i}",
                    
                };
                if(i>7) newCategory.ParentCategoryId = 1;
                categoryList.Add(newCategory);
            }

            return categoryList;
        }
    }
}
