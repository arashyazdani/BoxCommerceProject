using API.Errors;
using AutoMapper;
using Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Controllers;
using System.Net.Http;
using API.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Domain.Specifications;
using System.Net.Sockets;
using Stripe;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using API.Helpers;
using API.Tests.DataAttribute;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Tests.Controllers
{
    public class CategoriesControllerTests
    {
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<ApiResponse> _apiResponse;
        private readonly CategoriesController _categoriesController;
        private readonly Mock<IResponseCacheService> _responseCache;

        public CategoriesControllerTests()
        {
            _apiResponse = new Mock<ApiResponse>();
            _mapper = new Mock<IMapper>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _responseCache = new Mock<IResponseCacheService>();
            _categoriesController = new CategoriesController(_unitOfWork.Object,_mapper.Object, _responseCache.Object);

        }

        [Theory]
        [CategoryTestFields]
        public async Task Test_OK_And_NotFound_ObjectResult_GetCategoryById(int id, Category newCategory, Type expectedActionResultType)
        {
            // Arrange

            var spec = new GetCategoriesWithParentsSpecification(id);

            _unitOfWork.Setup(x => x.Repository<Category>()
                    .GetEntityWithSpec(It.IsAny<ISpecification<Category>>()))
                .ReturnsAsync(newCategory)
                .Verifiable();

            var controller = new CategoriesController(_unitOfWork.Object, _mapper.Object, _responseCache.Object);

            // Act

            var result = await controller.GetCategoryById(id);

            // Assert

            result.Result.ShouldBeOfType(expectedActionResultType);
        }

        [Fact]
        public async Task GetCategoryById_Should_Be_OK_ObjectResult()
        {
            //Arrange
            Category newCategory = CreateTestCategory();

            var spec = new GetCategoriesWithParentsSpecification(1);

            _unitOfWork.Setup(x => x.Repository<Category>()
                    .GetEntityWithSpec(It.IsAny<ISpecification<Category>>()))
                .ReturnsAsync(newCategory)
                .Verifiable();

            var controller = new CategoriesController(_unitOfWork.Object, _mapper.Object, _responseCache.Object);
            
            // Act

            var result = await controller.GetCategoryById(1);

            // Assert

            result.Value?.Id.ShouldBe(newCategory.Id);
            result.Value?.Name.ShouldBeEquivalentTo(newCategory.Name);
            result.Value?.Name.ShouldBeEquivalentTo("newCategory.Name");
            result.Result.ShouldBeOfType<OkObjectResult>();
            
        }

        [Fact]
        public async Task GetCategoryById_Should_Be_NotFound_ObjectResult_()
        {
            //Arrange
            Category newCategory = CreateTestCategory();

            var spec = new GetCategoriesWithParentsSpecification(100);


            _unitOfWork.Setup(x => x.Repository<Category>()
                    .GetEntityWithSpec(It.IsAny<ISpecification<Category>>()))
                .ReturnsAsync(It.IsAny<Category>())
                .Verifiable();

            var controller = new CategoriesController(_unitOfWork.Object, _mapper.Object, _responseCache.Object);

            // Act

            var result = await controller.GetCategoryById(100);

            // Assert


            result.Result.ShouldBeOfType<NotFoundObjectResult>();

        }

        [Fact]
        public async Task GetCategoryById_Should_Be_BadRequest_ObjectResult_()
        {
            //Arrange
            Category newCategory = CreateTestCategory();
            _apiResponse.Setup(x => x.StatusCode == 400 && x.Data == new Category() && x.Message == "Bad request");

            var spec = new GetCategoriesWithParentsSpecification(-100);

            var newError = new BadRequestObjectResult(new ApiResponse(400));

            _unitOfWork.Setup(x => x.Repository<Category>()
                    .GetEntityWithSpec(It.IsAny<ISpecification<Category>>()))
                .ReturnsAsync(It.IsAny<Category>())
                .Verifiable();

            var controller = new CategoriesController(_unitOfWork.Object, _mapper.Object, _responseCache.Object);

            // Act

            var result = await controller.GetCategoryById(-100);

            // Assert


            result.Result.ShouldBeOfType<BadRequestObjectResult>();

        }

        [Fact]
        public async Task Get_Category_By_Id_Should_Not_Be_Null()
        {
            //Arrange
            Category newCategory = CreateTestCategory();

            var spec = new GetCategoriesWithParentsSpecification(1);
            _unitOfWork.Setup(x => x.Repository<Category>().GetEntityWithSpec(spec)).ReturnsAsync(newCategory)
                .Verifiable();

            //Act

            var result = await _unitOfWork.Object.Repository<Category>().GetEntityWithSpec(spec);


            //Assert

            result.ShouldNotBeNull();
            result.Id.ShouldBe(1);
            result.Priority.ShouldBe(1);
            result.Name.ShouldBeEquivalentTo(newCategory.Name);
        }

        private Category CreateTestCategory()
        {
            return new Category()
            {
                Id = 1,
                Priority = 1,
                Name = "Test Category",
                Enabled = true,
                Details = "Testing category data"
            };
        }
    }
}
