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
using API.Tests.DataAttributes.CategoryAttributes;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using API.Tests.FakeData;

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
        [GetCategoryTests]
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
        [GetCategoryListTests]
        public async Task GetCategories_Test_OK_And_NotFound_ObjectResult(List<Category> categoryList, Type expectedActionResultType)
        {
            //Arrange

            _unitOfWork.Setup(x => x.Repository<Category>()
                    .ListWithSpecAsync(It.IsAny<ISpecification<Category>>()))
                .ReturnsAsync(categoryList)
                .Verifiable();

            var controller = new CategoriesController(_unitOfWork.Object, _mapper.Object, _responseCache.Object);
            
            var specParams = new GetCategorySpecificationParams();

            // Act

            var result = await controller.GetCategories(specParams);

            // Assert

            result.Result.ShouldBeOfType(expectedActionResultType);
            
        }

        [Fact]
        public async Task Get_Category_By_Id_Should_Not_Be_Null()
        {
            //Arrange
            Category newCategory = FakeCategories<Category>.FakeCategoryData(null, new Category());

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

        [Theory]
        [UpdateCategoryTest]
        public async Task UpdateCategory_Test_Ok_And_NotFound_And_Nocontent_ObjectResult(UpdateCategoryParams updateCategory, Category categoryEntity, Type expectedActionResultType)
        {
            // Arrange

            if (expectedActionResultType != typeof(NotFoundObjectResult)) _unitOfWork.Setup(x => x.Repository<Category>()
                    .GetEntityWithSpec(It.IsAny<ISpecification<Category>>()))
                    .ReturnsAsync(categoryEntity)
                    .Verifiable();
            _unitOfWork.Setup(x => x.Repository<Category>().Update(It.IsAny<Category>())).Verifiable();

            if(expectedActionResultType != typeof(BadRequestObjectResult)) _unitOfWork.Setup(x => x.Complete()).ReturnsAsync(1).Verifiable();

            var controller = new CategoriesController(_unitOfWork.Object, _mapper.Object, _responseCache.Object);

            // Act
            var result = await controller.UpdateCategory(updateCategory);

            // Assert
            result.ShouldBeOfType(expectedActionResultType);

        }


        [Theory]
        [UpdateCategoryTest]
        public async Task PartiallyUpdateCategory_Test_Ok_And_NotFound_And_Nocontent_ObjectResult(UpdateCategoryParams updateCategory, Category categoryEntity, Type expectedActionResultType)
        {
            // Arrange
            var jsonUpdateCategory = new JsonPatchDocument<UpdateCategoryParams>();

            jsonUpdateCategory.Replace(x => x.ParentCategoryId, 1);

            if (expectedActionResultType != typeof(NotFoundObjectResult)) _unitOfWork.Setup(x => x.Repository<Category>().GetEntityWithSpec(It.IsAny<ISpecification<Category>>())).ReturnsAsync(categoryEntity).Verifiable();

            _unitOfWork.Setup(x => x.Repository<Category>().GetByIdAsync(It.IsAny<int>())).ReturnsAsync(categoryEntity).Verifiable();

            _unitOfWork.Setup(x => x.Repository<Category>().Update(It.IsAny<Category>())).Verifiable();

            if (expectedActionResultType != typeof(BadRequestObjectResult)) _unitOfWork.Setup(x => x.Complete()).ReturnsAsync(1).Verifiable();

            _mapper.Setup(x => x.Map<UpdateCategoryParams>(It.IsAny<Category>())).Returns(updateCategory);

            _mapper.Setup(x => x.Map<UpdateCategoryParams, Category>(It.IsAny<UpdateCategoryParams>())).Returns(categoryEntity);


            var controller = new CategoriesController(_unitOfWork.Object, _mapper.Object, _responseCache.Object);

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                        It.IsAny<string>(),
                        It.IsAny<UpdateCategoryParams>()));
            controller.ObjectValidator = objectValidator.Object;
            // Act
            var result = await controller.PartiallyUpdateCategory(1, jsonUpdateCategory);

            // Assert
            result.ShouldBeOfType(expectedActionResultType);

        }

        [Theory]
        [DeleteCategoryTest]
        public async Task DeleteCategory_Test_Ok_And_NotFound_And_Nocontent_ObjectResult(int id, Category categoryEntity, Type expectedActionResultType)
        {
            // Arrange
            if(expectedActionResultType != typeof(NotFoundObjectResult)) _unitOfWork.Setup(x => x.Repository<Category>().GetEntityWithSpec(It.IsAny<ISpecification<Category>>())).ReturnsAsync(categoryEntity).Verifiable();

            _unitOfWork.Setup(x => x.Repository<Category>().DeleteAsync(It.IsAny<Category>())).Verifiable();

            if (expectedActionResultType != typeof(BadRequestObjectResult)) _unitOfWork.Setup(x => x.Complete()).ReturnsAsync(1).Verifiable();

            var controller = new CategoriesController(_unitOfWork.Object, _mapper.Object, _responseCache.Object);

            // Act
            var result = await controller.DeleteCategoryById(id);

            // Assert
            result.ShouldBeOfType(expectedActionResultType);

        }

    }
}
