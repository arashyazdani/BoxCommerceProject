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
using Domain.Specifications.CategorySpecifications;
using Infrastructure.Services;
using API.Tests.DataAttributes.CategoryAttributes;

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
        public async Task GetProductById_Test_OK_And_NotFound_ObjectResult(int id, Product newProduct,
            Type expectedActionResultType)
        {
            // Arrange
            //var newProduct = FakeProducts<Product>.FakeProductData(null, new Product());

            var spec = new GetProductsWithCategoriesSpecification(id);

            //var products = FakeData<IReadOnlyList<Product>>.FakeProductList.Generate(5).ToList();
            _unitOfWork.Setup(x => x.Repository<Product>()
                    .GetEntityWithSpec(It.IsAny<ISpecification<Product>>()))
                .ReturnsAsync(newProduct)
                .Verifiable();

            var controller = new ProductsController(_unitOfWork.Object, _mapper.Object, _responseCache.Object, _productService.Object);

            // Act

            var result = await controller.GetProductById(id);

            // Assert

            result.Result.ShouldBeOfType(expectedActionResultType);
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
        public async Task CreateProduct_Test_Ok_And_NotFound_And_Nocontent_And_Conflict_ObjectResult(CreateProductParams newProduct, Product productEntity, ProductToReturnDto productToReturnDto, Type expectedActionResultType, GetObjectFromProductService createCategoryObject)
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
                .ReturnsAsync(createCategoryObject)
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
    }
}