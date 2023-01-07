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
using Domain.Specifications;

namespace Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetObjectFromServicesSpecification> CreateCategory(Category createCategoryParams, CancellationToken cancellationToken)
        {
            var returnObject = new GetObjectFromServicesSpecification();

            Category parentCategoryExists = new Category();
            if (createCategoryParams.ParentCategoryId != null)
            {
                var parentSpec = new GetCategoriesWithParentsSpecification((int)createCategoryParams.ParentCategoryId);

                parentCategoryExists = await _unitOfWork.Repository<Category>().GetEntityWithSpec(parentSpec, cancellationToken);

                //categoryExists = await _unitOfWork.Repository<Category>().GetByIdAsync((int)createCategoryParams.ParentCategoryId, cancellationToken);
                if (parentCategoryExists == null)
                {
                    returnObject.StatusCode = 404;
                    returnObject.Message = "The ParentCategoryId is not found.";
                    return returnObject;
                }
                createCategoryParams.ParentPath = parentCategoryExists.ParentPath + createCategoryParams.ParentCategoryId + "/";
            }

            var specParams = new GetCategorySpecificationParams();

            specParams.Search = createCategoryParams.Name;

            var spec = new GetCategoriesWithParentsSpecification(specParams);

            var categoryExist = await _unitOfWork.Repository<Category>().GetEntityWithSpec(spec, cancellationToken);

            if (categoryExist != null)
            {
                returnObject.StatusCode = 409;

                returnObject.Message = "The category name is already exist.";

                return returnObject;
            }

            await _unitOfWork.Repository<Category>().InsertAsync(createCategoryParams, cancellationToken);

            var result = await _unitOfWork.Complete(cancellationToken);

            if (result <= 0)
            {
                returnObject.StatusCode = 400;

                returnObject.Message = "Bad request.";

                return returnObject;
            }

            returnObject.StatusCode = 201;

            returnObject.Message = "Category has been created successfully.";

            returnObject.ResultObject = createCategoryParams;

            return returnObject;
        }

        public async Task<GetObjectFromServicesSpecification> UpdateCategory(Category updateCategoryParams, CancellationToken cancellationToken)
        {
            var returnObject = new GetObjectFromServicesSpecification();

            if (updateCategoryParams.ParentCategoryId != null)
            {
                var categoryExists = await _unitOfWork.Repository<Category>().GetByIdAsync((int)updateCategoryParams.ParentCategoryId, cancellationToken);

                if (categoryExists == null)
                {
                    returnObject.StatusCode = 404;

                    returnObject.Message = "The ParentCategoryId is not found.";

                    return returnObject;
                }
                updateCategoryParams.ParentPath = updateCategoryParams.ParentPath + updateCategoryParams.ParentCategoryId + "/";
            }

            var specParams = new GetCategorySpecificationParams();

            var currentTimestamp = DateTimeOffset.Now;

            if (updateCategoryParams.UpdatedDate != null) currentTimestamp = (DateTimeOffset)updateCategoryParams.UpdatedDate;

            specParams.Search = updateCategoryParams.Name;

            var spec = new GetCategoriesWithParentsSpecification(specParams);

            var categoryExist = await _unitOfWork.Repository<Category>().GetEntityWithSpec(spec, cancellationToken);

            if (categoryExist != null && categoryExist.Id != updateCategoryParams.Id)
            {
                returnObject.StatusCode = 409;

                returnObject.Message = "The category name is already exist.";

                return returnObject;
            }

            var result = await _unitOfWork.Complete(cancellationToken);

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

            returnObject.ResultObject = updateCategoryParams;

            return returnObject;
        }
    }
}
