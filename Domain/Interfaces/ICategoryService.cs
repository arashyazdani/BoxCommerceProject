using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Specifications.CategorySpecifications;

namespace Domain.Interfaces
{
    public interface ICategoryService
    {
        Task<GetObjectFromCategoryService> CreateCategory(Category createCategoryParams);
        Task<GetObjectFromCategoryService> UpdateCategory(Category updateCategoryParams);

    }
}
