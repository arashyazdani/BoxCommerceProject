using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Specifications;

namespace Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        private CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<Category> CreateCategory(CreateCategoryParams createCategoryParams)
        {
            throw new NotImplementedException();
        }

        public Task<Category> UpdateCategory(UpdateCategoryParams updateCategoryParams)
        {
            throw new NotImplementedException();
        }
    }
}
