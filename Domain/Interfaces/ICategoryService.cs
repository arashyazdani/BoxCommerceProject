using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Specifications.CategorySpecifications;

namespace Domain.Interfaces
{
    public interface ICategoryService
    {
        Task<Category> CreateCategory(CreateCategoryParams createCategoryParams);
        Task<Category> UpdateCategory(UpdateCategoryParams updateCategoryParams);

    }
}
