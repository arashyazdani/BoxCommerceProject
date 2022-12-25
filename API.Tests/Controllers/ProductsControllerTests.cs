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
using API.Tests.DataAttributes;
using API.Tests.DataAttributes.ProductAttributes;
using API.Tests.FakeData;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Shouldly;

namespace API.Tests.Controllers
{
    public class ProductsControllerTests
    {
        private readonly Mock<IMapper> _mapper;

        private readonly Mock<IUnitOfWork> _unitOfWork;

        //private readonly Mock<ApiResponse> _apiResponse;
        private readonly ProductsController _categoriesController;
        private readonly Mock<IResponseCacheService> _responseCache;

        public ProductsControllerTests()
        {
            //_apiResponse = new Mock<ApiResponse>();
            _mapper = new Mock<IMapper>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _responseCache = new Mock<IResponseCacheService>();
            _categoriesController = new ProductsController(_unitOfWork.Object, _mapper.Object, _responseCache.Object);
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

            var controller = new ProductsController(_unitOfWork.Object, _mapper.Object, _responseCache.Object);

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

            var controller = new ProductsController(_unitOfWork.Object, _mapper.Object, _responseCache.Object);

            var specParams = new GetProductSpecificationParams();

            // Act

            var result = await controller.GetCategories(specParams);

            // Assert

            result.Result.ShouldBeOfType(expectedActionResultType);
        }

        [Fact]
        public async Task CreateProduct_Should_Be_CreatedAtRouteResult()
        {
            // Arrange
            var newProduct = FakeProducts<Product>.FakeProductData(null, new Product());


            // Act

            // Assert
        }
    }
}