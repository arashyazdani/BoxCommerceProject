using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Specifications;
using Domain.Specifications.ProductSpecifications;
using Domain.Specifications.CategorySpecifications;
//using Domain.Specifications.CategorySpecifications;

namespace Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetObjectFromServicesSpecification> CreateProduct(Product createProductParams)
        {
            var returnObject = new GetObjectFromServicesSpecification();

            if (createProductParams.CategoryId != null)
            {
                var categoryExists = await _unitOfWork.Repository<Category>().GetByIdAsync((int)createProductParams.CategoryId);

                if (categoryExists == null)
                {
                    returnObject.StatusCode = 404;
                    returnObject.Message = "The CategoryId is not found.";
                    return returnObject;
                }
            }

            var specParams = new GetProductSpecificationParams();

            specParams.Search = createProductParams.Name;

            var spec = new GetProductsWithCategoriesSpecification(specParams);

            var productExist = await _unitOfWork.Repository<Product>().GetEntityWithSpec(spec);

            if (productExist != null)
            {
                returnObject.StatusCode = 409;

                returnObject.Message = "The Product name is already exist.";

                return returnObject;
            }

            await _unitOfWork.Repository<Product>().InsertAsync(createProductParams);

            var result = await _unitOfWork.Complete();

            if (result <= 0)
            {
                returnObject.StatusCode = 400;

                returnObject.Message = "Bad request.";

                return returnObject;
            }

            returnObject.StatusCode = 201;

            returnObject.Message = "Product has been created successfully.";

            returnObject.ResultObject = createProductParams;

            return returnObject;
        }

        public async Task<GetObjectFromServicesSpecification> UpdateProduct(Product updateProductParams)
        {
            var returnObject = new GetObjectFromServicesSpecification();

            var categoryExists = await _unitOfWork.Repository<Category>().GetByIdAsync((int)updateProductParams.CategoryId);

            if (categoryExists == null)
            {
                returnObject.StatusCode = 404;

                returnObject.Message = "The CategoryId is not found.";

                return returnObject;
            }

            var specParams = new GetProductSpecificationParams();

            var currentTimestamp = DateTimeOffset.Now;

            if (updateProductParams.UpdatedDate != null) currentTimestamp = (DateTimeOffset)updateProductParams.UpdatedDate;

            specParams.Search = updateProductParams.Name;

            var spec = new GetProductsWithCategoriesSpecification(specParams);

            var productExist = await _unitOfWork.Repository<Product>().GetEntityWithSpec(spec);

            if (productExist != null && productExist.Id != updateProductParams.Id)
            {
                returnObject.StatusCode = 409;

                returnObject.Message = "The product name is already exist.";

                return returnObject;
            }

            var result = await _unitOfWork.Complete();

            if (result <= 0 && updateProductParams.UpdatedDate == currentTimestamp)
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

            returnObject.Message = "Product has been updated successfully.";

            returnObject.ResultObject = updateProductParams;

            return returnObject;
        }
    }
}
