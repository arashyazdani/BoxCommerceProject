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
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using API.Tests.FakeData;
using Domain.Specifications.CategorySpecifications;

namespace API.Tests.Controllers
{
    public class CategoriesControllerTests
    {
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<ICategoryService> _categoryService;
        private readonly Mock<IResponseCacheService> _responseCache;

        public CategoriesControllerTests()
        {
            _mapper = new Mock<IMapper>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _responseCache = new Mock<IResponseCacheService>();
            _categoryService = new Mock<ICategoryService>();

        }

        [Theory]
        [GetCategoryTests]
        public async Task GetCategoryById_Test_OK_And_NotFound_ObjectResult_And_ExceptionFormat(int id, Category newCategory, Type expectedActionResultType)
        {
            // Arrange

            _unitOfWork.Setup(x => x.Repository<Category>()
                    .GetEntityWithSpec(It.IsAny<ISpecification<Category>>(),It.IsAny<CancellationToken>()))
                .ReturnsAsync(newCategory)
                .Verifiable();

            var controller = new CategoriesController(_unitOfWork.Object, _mapper.Object, _responseCache.Object, _categoryService.Object);

            // Act and Assert

            if (typeof(FormatException) == expectedActionResultType)
            {
                await Assert.ThrowsAsync<FormatException>(() =>  controller.GetCategoryById(int.Parse("Not Integer")));
            }
            else
            {
                var result = await controller.GetCategoryById(id);

                result.Result.ShouldBeOfType(expectedActionResultType);
            }

        }

        [Theory]
        [GetCategoryListTests]
        public async Task GetCategories_Test_OK_And_NotFound_ObjectResult_And_FormatException(List<Category> categoryList, Type expectedActionResultType)
        {
            //Arrange

            _unitOfWork.Setup(x => x.Repository<Category>()
                    .ListWithSpecAsync(It.IsAny<ISpecification<Category>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(categoryList)
                .Verifiable();

            var controller = new CategoriesController(_unitOfWork.Object, _mapper.Object, _responseCache.Object, _categoryService.Object);

            // Act and Assert
            if (typeof(FormatException) == expectedActionResultType)
            {
                Assert.Throws<FormatException>(FakeCommonData<GetCategorySpecificationParams>.CreateFormatException);
            }
            else
            {
                // Act
                var specParams = new GetCategorySpecificationParams();

                var result = await controller.GetCategories(specParams);

                // Assert

                result.Result.ShouldBeOfType(expectedActionResultType);
            }

        }

        [Fact]
        public async Task Get_Category_By_Id_Should_Not_Be_Null()
        {
            //Arrange
            Category newCategory = FakeCategories<Category>.FakeCategoryData(null, new Category());

            var spec = new GetCategoriesWithParentsSpecification(1);
            _unitOfWork.Setup(x => x.Repository<Category>().GetEntityWithSpec(spec, It.IsAny<CancellationToken>())).ReturnsAsync(newCategory)
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
        public async Task CreateCategory_Test_Ok_And_NotFound_And_NoContent_And_Conflict_ObjectResult_And_FormatException(CreateCategoryParams newCategory, Category categoryEntity, CategoryToReturnDto categoryToReturnDto, Type expectedActionResultType, GetObjectFromServicesSpecification createCategoryObject)
        {
            // Arrange

            _unitOfWork.Setup(x => x.Repository<Category>().InsertAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(newCategory)).Verifiable();

            if(expectedActionResultType!=typeof(BadRequestObjectResult)) _unitOfWork.Setup(x => x.Complete(It.IsAny<CancellationToken>())).ReturnsAsync(1).Verifiable();

            _mapper.Setup(x => x.Map<Category, CategoryToReturnDto>(It.IsAny<Category>())).Returns(categoryToReturnDto);

            var createdCategory = new CreatedAtRouteResult("GetCategory", new { id = 1 }, new ApiResponse(201, "Category has been created successfully.", categoryToReturnDto));

            _mapper.Setup(x => x.Map<CreateCategoryParams, Category>(It.IsAny<CreateCategoryParams>())).Returns(categoryEntity);
            
            _categoryService.Setup(x => 
                    x.CreateCategory(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(createCategoryObject)
                .Verifiable();

            var controller = new CategoriesController(_unitOfWork.Object, _mapper.Object, _responseCache.Object, _categoryService.Object);

            // Act and Assert
            if (typeof(FormatException) == expectedActionResultType)
            {
                Assert.Throws<FormatException>(FakeCommonData<CreateCategoryParams>.CreateFormatExceptionOnCreateOrUpdate);
            }
            else
            {
                // Act

                var result = await controller.CreateCategory(newCategory);

                // Assert

                if (expectedActionResultType == typeof(CreatedAtRouteResult)) result.Result.ShouldBeEquivalentTo(createdCategory);

                result.Result.ShouldBeOfType(expectedActionResultType);
            }
            

        }

        [Theory]
        [UpdateCategoryTest]
        public async Task UpdateCategory_Test_Ok_And_NotFound_And_NoContent_And_NotModified_ObjectResult(UpdateCategoryParams updateCategory, Category categoryEntity, Type expectedActionResultType, GetObjectFromServicesSpecification updateCategoryObject)
        {
            // Arrange

            if (expectedActionResultType != typeof(NotFoundObjectResult)) _unitOfWork.Setup(x => x.Repository<Category>()
                    .GetEntityWithSpec(It.IsAny<ISpecification<Category>>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(categoryEntity)
                    .Verifiable();
            _unitOfWork.Setup(x => x.Repository<Category>().Update(It.IsAny<Category>(), It.IsAny<CancellationToken>())).Verifiable();

            if(expectedActionResultType != typeof(BadRequestObjectResult)) _unitOfWork.Setup(x => x.Complete(It.IsAny<CancellationToken>())).ReturnsAsync(1).Verifiable();

            _categoryService.Setup(x =>
                    x.UpdateCategory(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(updateCategoryObject)
                .Verifiable();

            var controller = new CategoriesController(_unitOfWork.Object, _mapper.Object, _responseCache.Object, _categoryService.Object);

            // Act
            var result = await controller.UpdateCategory(updateCategory);

            // Assert
            if(expectedActionResultType == typeof(StatusCodeResult)) updateCategoryObject.StatusCode.ShouldBe(304);
            result.ShouldBeOfType(expectedActionResultType);

        }


        [Theory]
        [UpdateCategoryTest]
        public async Task PartiallyUpdateCategory_Test_Ok_And_NotFound_And_NoContent_NotModified_ObjectResult(UpdateCategoryParams updateCategory, Category categoryEntity, Type expectedActionResultType, GetObjectFromServicesSpecification updateCategoryObject)
        {
            // Arrange
            var jsonUpdateCategory = new JsonPatchDocument<UpdateCategoryParams>();

            jsonUpdateCategory.Replace(x => x.ParentCategoryId, 1);

            if (expectedActionResultType != typeof(NotFoundObjectResult)) _unitOfWork.Setup(x => x.Repository<Category>().GetEntityWithSpec(It.IsAny<ISpecification<Category>>(), It.IsAny<CancellationToken>())).ReturnsAsync(categoryEntity).Verifiable();

            _unitOfWork.Setup(x => x.Repository<Category>().GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(categoryEntity).Verifiable();

            _unitOfWork.Setup(x => x.Repository<Category>().Update(It.IsAny<Category>(), It.IsAny<CancellationToken>())).Verifiable();

            if (expectedActionResultType != typeof(StatusCodeResult) && expectedActionResultType != typeof(BadRequestObjectResult)) _unitOfWork.Setup(x => x.Complete(It.IsAny<CancellationToken>())).ReturnsAsync(1).Verifiable();

            _mapper.Setup(x => x.Map<UpdateCategoryParams>(It.IsAny<Category>())).Returns(updateCategory);

            _mapper.Setup(x => x.Map<UpdateCategoryParams, Category>(It.IsAny<UpdateCategoryParams>())).Returns(categoryEntity);

            _categoryService.Setup(x =>
                    x.UpdateCategory(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(updateCategoryObject)
                .Verifiable();

            var controller = new CategoriesController(_unitOfWork.Object, _mapper.Object, _responseCache.Object, _categoryService.Object);

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                        It.IsAny<string>(),
                        It.IsAny<UpdateCategoryParams>()));

            controller.ObjectValidator = objectValidator.Object;

            // Act
            var result = await controller.PartiallyUpdateCategory(1, jsonUpdateCategory);

            // Assert
            if (expectedActionResultType == typeof(StatusCodeResult)) updateCategoryObject.StatusCode.ShouldBe(304);
            result.ShouldBeOfType(expectedActionResultType);

        }

        [Theory]
        [DeleteCategoryTest]
        public async Task DeleteCategory_Test_Ok_And_NotFound_And_NoContent_And_Forbidden_ObjectResult(int id, Category categoryEntity, Type expectedActionResultType)
        {
            // Arrange
            if(expectedActionResultType != typeof(NotFoundObjectResult)) _unitOfWork.Setup(x => x.Repository<Category>().GetEntityWithSpec(It.IsAny<ISpecification<Category>>(), It.IsAny<CancellationToken>())).ReturnsAsync(categoryEntity).Verifiable();

            _unitOfWork.Setup(x => x.Repository<Category>().DeleteAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>())).Verifiable();

            if (expectedActionResultType != typeof(BadRequestObjectResult)) _unitOfWork.Setup(x => x.Complete(It.IsAny<CancellationToken>())).ReturnsAsync(1).Verifiable();

            var controller = new CategoriesController(_unitOfWork.Object, _mapper.Object, _responseCache.Object, _categoryService.Object);

            // Act
            var result = await controller.DeleteCategoryById(id);

            // Assert

            if (typeof(ObjectResult)==expectedActionResultType)
            {
                ObjectResult objectResult = (ObjectResult)result;
                objectResult.StatusCode.ShouldBe(403);
            }
            else
            {
                result.ShouldBeOfType(expectedActionResultType);
            }

        }

    }
}
