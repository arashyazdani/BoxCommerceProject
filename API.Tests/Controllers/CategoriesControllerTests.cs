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
using Microsoft.EntityFrameworkCore.ChangeTracking;

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

        [Theory]
        [CategoryListTest]
        public async Task Test_OK_And_NotFound_ObjectResult_GetCategories(List<Category> categoryList, Type expectedActionResultType)
        {
            //Arrange
            //var categoryList = FakeData.CreateListOfTestCategory();

            //var spec = new GetCategoriesWithParentsSpecification(1);

            _unitOfWork.Setup(x => x.Repository<Category>()
                    .ListWithSpecAsync(It.IsAny<ISpecification<Category>>()))
                .ReturnsAsync(categoryList)
                .Verifiable();

            var controller = new CategoriesController(_unitOfWork.Object, _mapper.Object, _responseCache.Object);
            
            var specParams = new CategorySpecificationParams();

            // Act

            var result = await controller.GetCategories(specParams);

            // Assert

            result.Result.ShouldBeOfType(expectedActionResultType);
            
        }

        [Fact]
        public async Task Get_Category_By_Id_Should_Not_Be_Null()
        {
            //Arrange
            Category newCategory = FakeData.CreateTestCategory();

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


        [Fact]
        public async Task CreateCategory_Should_Be_201_NoContent()
        {
            // Arrange
            var newCategory = FakeData.InsertTestCategory();
            var categoryEntity = FakeData.CreateTestCategory();
            var categoryReturnDto = FakeData.CreateTestCategoryToReturnDto();

            _unitOfWork.Setup(x => x.Repository<Category>().InsertAsync(It.IsAny<Category>()))
                .Returns(Task.FromResult(newCategory)).Verifiable();
            _unitOfWork.Setup(x=>x.Complete()).ReturnsAsync(1).Verifiable();

            _mapper.Setup(x => x.Map<Category, CategoryToReturnDto>(It.IsAny<Category>())).Returns(categoryReturnDto);
            _mapper.Setup(x => x.Map<CreateCategoryParams, Category>(It.IsAny<CreateCategoryParams>())).Returns(categoryEntity);

            var controller = new CategoriesController(_unitOfWork.Object, _mapper.Object, _responseCache.Object);



            var createdCategory = new CreatedAtRouteResult("GetCategory", new { id = 1 },
                new ApiResponse(201, "Category has been created successfully.", categoryReturnDto));

            // Act

            var result = await controller.CreateCategory(newCategory);

            // Assert

            result.Result.ShouldBeEquivalentTo(createdCategory);
        }

        [Fact]
        public async Task CreateCategory_Should_Be_BadRequest()
        {
            // Arrange
            var newCategory = FakeData.InsertTestCategory();
            var categoryEntity = FakeData.CreateTestCategory();
            var categoryReturnDto = FakeData.CreateTestCategoryToReturnDto();

            _unitOfWork.Setup(x => x.Repository<Category>().InsertAsync(It.IsAny<Category>()))
                .Returns(Task.FromResult(newCategory)).Verifiable();

            _mapper.Setup(x => x.Map<Category, CategoryToReturnDto>(It.IsAny<Category>())).Returns(categoryReturnDto);
            _mapper.Setup(x => x.Map<CreateCategoryParams, Category>(It.IsAny<CreateCategoryParams>())).Returns(categoryEntity);

            var controller = new CategoriesController(_unitOfWork.Object, _mapper.Object, _responseCache.Object);



            var createdCategory = new CreatedAtRouteResult("GetCategory", new { id = 1 },
                new ApiResponse(201, "Category has been created successfully.", categoryReturnDto));

            // Act

            var result = await controller.CreateCategory(newCategory);

            // Assert

            result.Result.ShouldBeOfType(typeof(BadRequestObjectResult));
        }

        [Fact]
        public async Task CreateCategory_Should_Be_NotFound()
        {
            // Arrange
            var newCategory = FakeData.InsertTestCategory();
            newCategory.ParentCategoryId = 200;
            var categoryEntity = FakeData.CreateTestCategory();
            var categoryReturnDto = FakeData.CreateTestCategoryToReturnDto();

            _unitOfWork.Setup(x => x.Repository<Category>().InsertAsync(It.IsAny<Category>()))
                .Returns(Task.FromResult(newCategory)).Verifiable();

            _mapper.Setup(x => x.Map<Category, CategoryToReturnDto>(It.IsAny<Category>())).Returns(categoryReturnDto);
            _mapper.Setup(x => x.Map<CreateCategoryParams, Category>(It.IsAny<CreateCategoryParams>())).Returns(categoryEntity);

            var controller = new CategoriesController(_unitOfWork.Object, _mapper.Object, _responseCache.Object);



            var createdCategory = new CreatedAtRouteResult("GetCategory", new { id = 1 },
                new ApiResponse(201, "Category has been created successfully.", categoryReturnDto));

            // Act

            var result = await controller.CreateCategory(newCategory);

            // Assert

            result.Result.ShouldBeOfType(typeof(NotFoundObjectResult));
        }
    }
}
