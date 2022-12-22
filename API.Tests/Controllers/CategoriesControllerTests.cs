using API.Errors;
using AutoMapper;
using Domain.Interfaces;
using Moq;
using API.Controllers;
using API.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Domain.Specifications;
using API.Tests.DataAttribute;

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
        public async Task GetCategoryById_Test_OK_And_NotFound_ObjectResult(int id, Category newCategory, Type expectedActionResultType)
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
        public async Task GetCategories_Test_OK_And_NotFound_ObjectResult(List<Category> categoryList, Type expectedActionResultType)
        {
            //Arrange

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
            Category newCategory = FakeData<Category>.CategoryData(null);

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


        [Theory]
        [CreateCategoryTest]
        public async Task CreateCategory_Test_Ok_And_NotFound_And_Nocontent_ObjectResult(CreateCategoryParams newCategory, Category categoryEntity, CategoryToReturnDto categoryToReturnDto, Type expectedActionResultType)
        {
            // Arrange

            _unitOfWork.Setup(x => x.Repository<Category>().InsertAsync(It.IsAny<Category>()))
                .Returns(Task.FromResult(newCategory)).Verifiable();

            if(expectedActionResultType!=typeof(BadRequestObjectResult)) _unitOfWork.Setup(x => x.Complete()).ReturnsAsync(1).Verifiable();

            _mapper.Setup(x => x.Map<Category, CategoryToReturnDto>(It.IsAny<Category>())).Returns(categoryToReturnDto);

            var createdCategory = new CreatedAtRouteResult("GetCategory", new { id = 1 }, new ApiResponse(201, "Category has been created successfully.", categoryToReturnDto));

            _mapper.Setup(x => x.Map<CreateCategoryParams, Category>(It.IsAny<CreateCategoryParams>())).Returns(categoryEntity);

            var controller = new CategoriesController(_unitOfWork.Object, _mapper.Object, _responseCache.Object);


            // Act

            var result = await controller.CreateCategory(newCategory);

            // Assert

            if (expectedActionResultType == typeof(CreatedAtRouteResult)) result.Result.ShouldBeEquivalentTo(createdCategory);
            
            result.Result.ShouldBeOfType(expectedActionResultType);

        }

        //[Fact]
        //public async Task UpdateCategory_Should_Be_NoContent_Object_Result()
        //{
        //    _unitOfWork.Setup(x => x.Repository<Category>().InsertAsync(It.IsAny<Category>()))
        //        .Returns(Task.FromResult(newCategory)).Verifiable();
        //}

    }
}
