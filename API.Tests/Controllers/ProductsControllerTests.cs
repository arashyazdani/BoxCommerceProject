using API.Controllers;
using API.Errors;
using AutoMapper;
using Domain.Interfaces;
using Domain.Specifications;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.DTOs;
using API.Tests.DataAttributes;
using API.Tests.DataAttributes.ProductAttributes;
using API.Tests.FakeData;
using Domain.Entities;
using Domain.Specifications.ProductSpecifications;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Infrastructure.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace API.Tests.Controllers
{
    public class ProductsControllerTests
    {
        private readonly Mock<IMapper> _mapper;

        private readonly Mock<IUnitOfWork> _unitOfWork;

        //private readonly Mock<ApiResponse> _apiResponse;
        private readonly ProductsController _productsController;
        private readonly Mock<IResponseCacheService> _responseCache;
        private readonly Mock<IProductService> _productService;

        public ProductsControllerTests()
        {
            //_apiResponse = new Mock<ApiResponse>();
            _mapper = new Mock<IMapper>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _responseCache = new Mock<IResponseCacheService>();
            _productService = new Mock<IProductService>();
            //_productsController = new ProductsController(_unitOfWork.Object, _mapper.Object, _responseCache.Object, _productService.Object);
        }


        [Theory]
        [GetProductTests]
        public async Task GetProductById_Test_OK_And_NotFound_ObjectResult_And_ExceptionFormat(int id, Product newProduct,
            Type expectedActionResultType)
        {
            // Arrange

            _unitOfWork.Setup(x => x.Repository<Product>()
                    .GetEntityWithSpec(It.IsAny<ISpecification<Product>>()))
                .ReturnsAsync(newProduct)
                .Verifiable();

            var controller = new ProductsController(_unitOfWork.Object, _mapper.Object, _responseCache.Object, _productService.Object);

            // Act and Assert

            if (typeof(FormatException) == expectedActionResultType)
            {
                await Assert.ThrowsAsync<FormatException>(() => controller.GetProductById(int.Parse("Not Integer")));
            }
            else
            {
                var result = await controller.GetProductById(id);

                result.Result.ShouldBeOfType(expectedActionResultType);
            }
        }

        [Theory]
        [GetProductListTests]
        public async Task GetProducts_Test_OK_And_NotFound_ObjectResult(List<Product> productList,
            Type expectedActionResultType)
        {
            // Arrange

            _unitOfWork.Setup(x => x.Repository<Product>()
                    .ListWithSpecAsync(It.IsAny<ISpecification<Product>>()))
                .ReturnsAsync(productList)
                .Verifiable();

            var controller = new ProductsController(_unitOfWork.Object, _mapper.Object, _responseCache.Object, _productService.Object);

            var specParams = new GetProductSpecificationParams();

            // Act

            var result = await controller.GetProducts(specParams);

            // Assert

            result.Result.ShouldBeOfType(expectedActionResultType);
        }

        [Theory]
        [CreateProductTests]
        public async Task CreateProduct_Test_Ok_And_NotFound_And_Nocontent_And_Conflict_ObjectResult(CreateProductParams newProduct, Product productEntity, ProductToReturnDto productToReturnDto, Type expectedActionResultType, GetObjectFromProductService createProductObject)
        {
            // Arrange

            _unitOfWork.Setup(x => x.Repository<Product>().InsertAsync(It.IsAny<Product>()))
                .Returns(Task.FromResult(newProduct)).Verifiable();

            var categoryEntity = typeof(NotFoundObjectResult) != expectedActionResultType ? FakeCategories<Category>.FakeCategoryData(1, new Category()) : It.IsAny<Category>();


            _unitOfWork.Setup(x => x.Repository<Category>().GetByIdAsync(It.IsAny<int>())).ReturnsAsync(categoryEntity).Verifiable();
            

            if (expectedActionResultType != typeof(BadRequestObjectResult)) _unitOfWork.Setup(x => x.Complete()).ReturnsAsync(1).Verifiable();

            _mapper.Setup(x => x.Map<Product, ProductToReturnDto>(It.IsAny<Product>())).Returns(productToReturnDto);

            var createdProduct = new CreatedAtRouteResult("GetProduct", new { id = 1 }, new ApiResponse(201, "Product has been created successfully.", productToReturnDto));

            _mapper.Setup(x => x.Map<CreateProductParams, Product>(It.IsAny<CreateProductParams>())).Returns(productEntity);

            _productService.Setup(x =>
                    x.CreateProduct(It.IsAny<Product>()))
                .ReturnsAsync(createProductObject)
                .Verifiable();

            var controller = new ProductsController(_unitOfWork.Object, _mapper.Object, _responseCache.Object, _productService.Object);

            // Act

            var result = await controller.CreateProduct(newProduct);

            // Assert

            if (expectedActionResultType == typeof(CreatedAtRouteResult)) result.Result.ShouldBeEquivalentTo(createdProduct);

            result.Result.ShouldBeOfType(expectedActionResultType);

        }

        [Theory]
        [UpdateProductTest]
        public async Task UpdateProduct_Test_Ok_And_NotFound_And_Nocontent_And_NotModified_ObjectResult(UpdateProductParams updateProduct, Product productEntity, Type expectedActionResultType, GetObjectFromProductService updateProductObject)
        {
            // Arrange

            if (expectedActionResultType != typeof(NotFoundObjectResult)) _unitOfWork.Setup(x => x.Repository<Product>()
                    .GetEntityWithSpec(It.IsAny<ISpecification<Product>>()))
                .ReturnsAsync(productEntity)
                .Verifiable();
            _unitOfWork.Setup(x => x.Repository<Product>().Update(It.IsAny<Product>())).Verifiable();

            if (expectedActionResultType != typeof(BadRequestObjectResult)) _unitOfWork.Setup(x => x.Complete()).ReturnsAsync(1).Verifiable();

            _productService.Setup(x =>
                    x.UpdateProduct(It.IsAny<Product>()))
                .ReturnsAsync(updateProductObject)
                .Verifiable();

            var controller = new ProductsController(_unitOfWork.Object, _mapper.Object, _responseCache.Object, _productService.Object);

            // Act
            var result = await controller.UpdateProduct(updateProduct);

            // Assert
            if (expectedActionResultType == typeof(StatusCodeResult)) updateProductObject.StatusCode.ShouldBe(304);
            result.ShouldBeOfType(expectedActionResultType);
        }

        [Theory]
        [UpdateProductTest]
        public async Task PartiallyUpdateProduct_Test_Ok_And_NotFound_And_Nocontent_NotModified_ObjectResult(UpdateProductParams updateProduct, Product productEntity, Type expectedActionResultType, GetObjectFromProductService updateProductObject)
        {
            // Arrange
            var jsonUpdateProduct = new JsonPatchDocument<UpdateProductParams>();

            jsonUpdateProduct.Replace(x => x.CategoryId, 1);

            if (expectedActionResultType != typeof(NotFoundObjectResult)) _unitOfWork.Setup(x => x.Repository<Product>().GetEntityWithSpec(It.IsAny<ISpecification<Product>>())).ReturnsAsync(productEntity).Verifiable();

            _unitOfWork.Setup(x => x.Repository<Product>().GetByIdAsync(It.IsAny<int>())).ReturnsAsync(productEntity).Verifiable();

            _unitOfWork.Setup(x => x.Repository<Product>().Update(It.IsAny<Product>())).Verifiable();

            if (expectedActionResultType != typeof(StatusCodeResult) && expectedActionResultType != typeof(BadRequestObjectResult)) _unitOfWork.Setup(x => x.Complete()).ReturnsAsync(1).Verifiable();

            _mapper.Setup(x => x.Map<UpdateProductParams>(It.IsAny<Product>())).Returns(updateProduct);

            _mapper.Setup(x => x.Map<UpdateProductParams, Product>(It.IsAny<UpdateProductParams>())).Returns(productEntity);

            _productService.Setup(x =>
                    x.UpdateProduct(It.IsAny<Product>()))
                .ReturnsAsync(updateProductObject)
                .Verifiable();

            var controller = new ProductsController(_unitOfWork.Object, _mapper.Object, _responseCache.Object, _productService.Object);

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                        It.IsAny<string>(),
                        It.IsAny<UpdateProductParams>()));

            controller.ObjectValidator = objectValidator.Object;

            // Act
            var result = await controller.PartiallyUpdateProduct(1, jsonUpdateProduct);

            // Assert
            if (expectedActionResultType == typeof(StatusCodeResult)) updateProductObject.StatusCode.ShouldBe(304);
            result.ShouldBeOfType(expectedActionResultType);

        }

        [Theory]
        [DeleteProductTests]
        public async Task DeleteProduct_Test_Ok_And_NotFound_And_Nocontent_ObjectResult(int id, Product productEntity, Type expectedActionResultType)
        {
            // Arrange
            if (expectedActionResultType != typeof(NotFoundObjectResult)) _unitOfWork.Setup(x => x.Repository<Product>().GetEntityWithSpec(It.IsAny<ISpecification<Product>>())).ReturnsAsync(productEntity).Verifiable();

            _unitOfWork.Setup(x => x.Repository<Product>().DeleteAsync(It.IsAny<Product>())).Verifiable();

            if (expectedActionResultType != typeof(BadRequestObjectResult)) _unitOfWork.Setup(x => x.Complete()).ReturnsAsync(1).Verifiable();

            var controller = new ProductsController(_unitOfWork.Object, _mapper.Object, _responseCache.Object, _productService.Object);

            // Act
            var result = await controller.DeleteProductById(id);

            // Assert
            result.ShouldBeOfType(expectedActionResultType);

        }
    }
}