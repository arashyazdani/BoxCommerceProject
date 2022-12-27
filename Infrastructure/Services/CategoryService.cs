using Domain.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Specifications.CategorySpecifications;
using System.Dynamic;
using Infrastructure.Data;
using System.Xml;

namespace Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetObjectFromCategoryService> CreateCategory(Category createCategoryParams)
        {
            var returnObject = new GetObjectFromCategoryService();

            if (createCategoryParams.ParentCategoryId != null)
            {
                var categoryExists = await _unitOfWork.Repository<Category>().GetByIdAsync((int)createCategoryParams.ParentCategoryId);
                if (categoryExists == null)
                {
                    returnObject.StatusCode = 404;
                    returnObject.Message = "The ParentCategoryId is not found.";
                    return returnObject;
                }
            }

            var specParams = new GetCategorySpecificationParams();

            specParams.Search = createCategoryParams.Name;

            var spec = new GetCategoriesWithParentsSpecification(specParams);

            var categoryExist = await _unitOfWork.Repository<Category>().GetEntityWithSpec(spec);

            if (categoryExist != null)
            {
                returnObject.StatusCode = 409;

                returnObject.Message = "The category name is already exist.";

                return returnObject;
            }

            await _unitOfWork.Repository<Category>().InsertAsync(createCategoryParams);

            var result = await _unitOfWork.Complete();

            if (result <= 0)
            {
                returnObject.StatusCode = 400;

                returnObject.Message = "Bad request.";

                return returnObject;
            }

            returnObject.StatusCode = 201;

            returnObject.Message = "Category has been created successfully.";

            returnObject.CategoryResult = createCategoryParams;

            return returnObject;
        }

        public async Task<GetObjectFromCategoryService> UpdateCategory(Category updateCategoryParams)
        {
            var returnObject = new GetObjectFromCategoryService();

            if (updateCategoryParams.ParentCategoryId != null)
            {
                var categoryExists = await _unitOfWork.Repository<Category>().GetByIdAsync((int)updateCategoryParams.ParentCategoryId);
                if (categoryExists == null)
                {
                    returnObject.StatusCode = 404;
                    returnObject.Message = "The ParentCategoryId is not found.";
                    return returnObject;
                }
            }

            var specParams = new GetCategorySpecificationParams();

            var currentTimestamp = DateTimeOffset.Now;

            if (updateCategoryParams.UpdatedDate != null) currentTimestamp = (DateTimeOffset)updateCategoryParams.UpdatedDate;

            specParams.Search = updateCategoryParams.Name;

            var spec = new GetCategoriesWithParentsSpecification(specParams);

            var categoryExist = await _unitOfWork.Repository<Category>().GetEntityWithSpec(spec);

            if (categoryExist != null && categoryExist.Id != updateCategoryParams.Id)
            {
                returnObject.StatusCode = 409;

                returnObject.Message = "The category name is already exist.";

                return returnObject;
            }
            var result = await _unitOfWork.Complete();

            if (result<=0 && updateCategoryParams.UpdatedDate == currentTimestamp)
            {
                returnObject.StatusCode = 304;
                returnObject.Message = "Not Modified.";
                return returnObject;
            }

            if (result <= 0)
            {
                returnObject.StatusCode = 400;
                returnObject.Message = "Bad request.";
                return returnObject;
            }

            returnObject.StatusCode = 204;

            returnObject.Message = "Category has been updated successfully.";

            returnObject.CategoryResult = updateCategoryParams;

            return returnObject;
        }
    }
}
