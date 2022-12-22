using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using API.DTOs;
using Domain.Entities;
using Domain.Specifications;

namespace API.Tests.DataAttribute
{
    public class FakeData<T> where T : class
    {
        public static T CategoryData(int? categoryId)
        {
            dynamic returnData = new Object();

            if (typeof(T) == typeof(Category))
            {
                returnData = new Category();
            }
            else if (typeof(T) == typeof(CategoryToReturnDto))
            {
                returnData = new CategoryToReturnDto();
            }
            else if (typeof(T) == typeof(CreateCategoryParams))
            {
                returnData = new CreateCategoryParams();
            }
            else if (typeof(T) == typeof(UpdateCategoryParams))
            {
                returnData = new UpdateCategoryParams();
            }

            if (typeof(T) != typeof(CreateCategoryParams)) returnData.Id = 1;
            returnData.Priority = 1;
            returnData.Name = "Test Category";
            returnData.Enabled = true;
            returnData.Details = "Testing category data";
            if (categoryId != null) returnData.ParentCategoryId = categoryId;

            return (T)(Object)returnData;

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
