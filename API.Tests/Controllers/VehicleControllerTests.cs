using AutoMapper;
using Domain.Interfaces;
using Moq;
using API.Controllers;
using API.DTOs;
using API.Errors;
using API.Tests.DataAttributes.VehicleAttributes;
using Domain.Entities;
using Domain.Specifications;
using Shouldly;
using API.Tests.FakeData;
using Domain.Specifications.VehicleSpecifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Linq.Expressions;

namespace API.Tests.Controllers
{
    public class VehicleControllerTests
    {
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IResponseCacheService> _responseCache;
        private readonly Mock<IVehicleService> _vehicleService;

        public VehicleControllerTests()
        {
            _mapper = new Mock<IMapper>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _responseCache = new Mock<IResponseCacheService>();
            _vehicleService = new Mock<IVehicleService>();
        }

        [Theory]
        [GetVehicleTests]
        public async Task GetVehicleById_Test_OK_And_NotFound_ObjectResult_And_ExceptionFormat(int id, Vehicle newVehicle, Type expectedActionResultType)
        {
            // Arrange

            _unitOfWork.Setup(x => x.Repository<Vehicle>()
                    .GetEntityWithSpec(It.IsAny<ISpecification<Vehicle>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(newVehicle)
                .Verifiable();

            var controller = new VehiclesController(_unitOfWork.Object, _mapper.Object, _responseCache.Object, _vehicleService.Object);

            // Act and Assert

            if (typeof(FormatException) == expectedActionResultType)
            {
                await Assert.ThrowsAsync<FormatException>(() => controller.GetVehicleById(int.Parse("Not Integer")));
            }
            else
            {
                var result = await controller.GetVehicleById(id);

                result.Result.ShouldBeOfType(expectedActionResultType);
            }

        }

        [Theory]
        [GetVehicleWithVehiclePartsTests]
        public async Task GetVehicleWithVehiclePartsById_Test_OK_And_NotFound_ObjectResult_And_ExceptionFormat(int id, Vehicle newVehicle, Type expectedActionResultType)
        {
            // Arrange

            _unitOfWork.Setup(x => x.Repository<Vehicle>()
                    .GetEntityWithSpec(It.IsAny<ISpecification<Vehicle>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(newVehicle)
                .Verifiable();

            var controller = new VehiclesController(_unitOfWork.Object, _mapper.Object, _responseCache.Object, _vehicleService.Object);

            // Act and Assert

            if (typeof(FormatException) == expectedActionResultType)
            {
                await Assert.ThrowsAsync<FormatException>(() => controller.GetVehicleById(int.Parse("Not Integer")));
            }
            else
            {
                var result = await controller.GetVehicleWithVehiclePartsById(id);

                result.Result.ShouldBeOfType(expectedActionResultType);
            }

        }

        [Theory]
        [GetVehicleListTestsAttribute]
        public async Task GetVehicles_Test_OK_And_NotFound_ObjectResult_And_FormatException(List<Vehicle> vehicleList, Type expectedActionResultType)
        {
            //Arrange

            _unitOfWork.Setup(x => x.Repository<Vehicle>()
            .ListWithSpecAsync(It.IsAny<ISpecification<Vehicle>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicleList)
                .Verifiable();

            var controller = new VehiclesController(_unitOfWork.Object, _mapper.Object, _responseCache.Object, _vehicleService.Object);

            // Act and Assert
            if (typeof(FormatException) == expectedActionResultType)
            {
                Assert.Throws<FormatException>(FakeCommonData<GetVehicleSpecificationParams>.CreateFormatException);
            }
            else
            {
                // Act
                var specParams = new GetVehicleSpecificationParams();

                var result = await controller.GetVehicles(specParams);

                // Assert

                result.Result.ShouldBeOfType(expectedActionResultType);
            }

        }

        [Theory]
        [GetVehiclesWithPartsListTestsAttribute]
        public async Task GetVehicleWithVehicleParts_Test_OK_And_NotFound_ObjectResult_And_FormatException(List<Vehicle> vehicleList, Type expectedActionResultType)
        {
            //Arrange
            var specParams = new GetVehicleSpecificationWithPartsParams();

            if (typeof(NotFoundObjectResult) == expectedActionResultType)
            {
                specParams.ProductId =  20;
                var spec = new GetVehiclesWithPartsSpecification(specParams);
                _unitOfWork.Setup(x => x.Repository<Vehicle>()
                        .ListWithSpecAsync(spec, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(vehicleList)
                    .Verifiable();
            }
            else
            {
                _unitOfWork.Setup(x => x.Repository<Vehicle>()
                        .ListWithSpecAsync(It.IsAny<ISpecification<Vehicle>>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(vehicleList)
                    .Verifiable();
            }

            var controller = new VehiclesController(_unitOfWork.Object, _mapper.Object, _responseCache.Object, _vehicleService.Object);

            // Act and Assert
            if (typeof(FormatException) == expectedActionResultType)
            {
                Assert.Throws<FormatException>(FakeCommonData<GetVehicleSpecificationParams>.CreateFormatException);
            }
            else
            {
                // Act

                var result = await controller.GetVehicleWithVehicleParts(specParams);

                // Assert

                result.Result.ShouldBeOfType(expectedActionResultType);
            }

        }

        [Theory]
        [CreateVehicleTests]
        public async Task CreateVehicle_Test_Created_And_NotFound_And_BadRequest_And_Conflict_ObjectResult_And_FormatException(CreateVehicleParams newVehicle, Vehicle vehicleEntity, VehicleToReturnDto vehicleToReturnDto, Type expectedActionResultType, GetObjectFromServicesSpecification createVehicleObject)
        {
            // Arrange

            _unitOfWork.Setup(x => x.Repository<Vehicle>().InsertAsync(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(newVehicle)).Verifiable();

            if (expectedActionResultType != typeof(BadRequestObjectResult)) _unitOfWork.Setup(x => x.Complete(It.IsAny<CancellationToken>())).ReturnsAsync(1).Verifiable();

            _mapper.Setup(x => x.Map<Vehicle, VehicleToReturnDto>(It.IsAny<Vehicle>())).Returns(vehicleToReturnDto);

            var createdVehicle = new CreatedAtRouteResult("GetVehicle", new { id = 1 }, new ApiResponse(201, "Vehicle has been created successfully.", vehicleToReturnDto));

            _mapper.Setup(x => x.Map<CreateVehicleParams, Vehicle>(It.IsAny<CreateVehicleParams>())).Returns(vehicleEntity);

            _vehicleService.Setup(x =>
                    x.CreateVehicle(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(createVehicleObject)
                .Verifiable();

            var controller = new VehiclesController(_unitOfWork.Object, _mapper.Object, _responseCache.Object, _vehicleService.Object);

            // Act and Assert
            if (typeof(FormatException) == expectedActionResultType)
            {
                Assert.Throws<FormatException>(FakeCommonData<CreateVehicleParams>.CreateFormatExceptionOnCreateOrUpdate);
            }
            else
            {
                // Act

                var result = await controller.CreateVehicle(newVehicle);

                // Assert

                if (expectedActionResultType == typeof(CreatedAtRouteResult)) result.Result.ShouldBeEquivalentTo(createdVehicle);

                result.Result.ShouldBeOfType(expectedActionResultType);
            }

        }

        [Theory]
        [UpdateVehicleTests]
        public async Task UpdateVehicle_Test_BadRequest_And_NotFound_And_NoContent_And_NotModified_ObjectResult(UpdateVehicleParams updateVehicle, Vehicle vehicleEntity, Type expectedActionResultType, GetObjectFromServicesSpecification updateVehicleObject)
        {
            // Arrange

            if (expectedActionResultType != typeof(NotFoundObjectResult)) _unitOfWork.Setup(x => x.Repository<Vehicle>()
                    .GetEntityWithSpec(It.IsAny<ISpecification<Vehicle>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(vehicleEntity)
                .Verifiable();
            _unitOfWork.Setup(x => x.Repository<Vehicle>().Update(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>())).Verifiable();

            if (expectedActionResultType != typeof(BadRequestObjectResult)) _unitOfWork.Setup(x => x.Complete(It.IsAny<CancellationToken>())).ReturnsAsync(1).Verifiable();

            _vehicleService.Setup(x =>
                    x.UpdateVehicle(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(updateVehicleObject)
                .Verifiable();

            var controller = new VehiclesController(_unitOfWork.Object, _mapper.Object, _responseCache.Object, _vehicleService.Object);

            // Act
            var result = await controller.UpdateVehicle(updateVehicle);

            // Assert
            if (expectedActionResultType == typeof(StatusCodeResult)) updateVehicleObject.StatusCode.ShouldBe(304);
            result.ShouldBeOfType(expectedActionResultType);

        }

        [Theory]
        [UpdateVehicleTests]
        public async Task PartiallyUpdateVehicle_Test_BadRequest_And_Conflict_And_NotFound_And_NoContent_NotModified_ObjectResult(UpdateVehicleParams updateVehicle, Vehicle vehicleEntity, Type expectedActionResultType, GetObjectFromServicesSpecification updateVehicleObject)
        {
            // Arrange
            var jsonUpdateVehicle = new JsonPatchDocument<UpdateVehicleParams>();

            if (expectedActionResultType != typeof(NotFoundObjectResult)) _unitOfWork.Setup(x => x.Repository<Vehicle>().GetEntityWithSpec(It.IsAny<ISpecification<Vehicle>>(), It.IsAny<CancellationToken>())).ReturnsAsync(vehicleEntity).Verifiable();

            _unitOfWork.Setup(x => x.Repository<Vehicle>().GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(vehicleEntity).Verifiable();

            _unitOfWork.Setup(x => x.Repository<Vehicle>().Update(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>())).Verifiable();

            if (expectedActionResultType != typeof(StatusCodeResult) && expectedActionResultType != typeof(BadRequestObjectResult)) _unitOfWork.Setup(x => x.Complete(It.IsAny<CancellationToken>())).ReturnsAsync(1).Verifiable();

            _mapper.Setup(x => x.Map<UpdateVehicleParams>(It.IsAny<Vehicle>())).Returns(updateVehicle);

            _mapper.Setup(x => x.Map<UpdateVehicleParams, Vehicle>(It.IsAny<UpdateVehicleParams>())).Returns(vehicleEntity);

            _vehicleService.Setup(x =>
                    x.UpdateVehicle(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(updateVehicleObject)
                .Verifiable();

            var controller = new VehiclesController(_unitOfWork.Object, _mapper.Object, _responseCache.Object, _vehicleService.Object);

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                        It.IsAny<string>(),
                        It.IsAny<UpdateVehicleParams>()));

            controller.ObjectValidator = objectValidator.Object;

            // Act
            var result = await controller.PartiallyUpdateVehicle(1, jsonUpdateVehicle);

            // Assert
            if (expectedActionResultType == typeof(StatusCodeResult)) updateVehicleObject.StatusCode.ShouldBe(304);
            result.ShouldBeOfType(expectedActionResultType);

        }

        [Theory]
        [DeleteVehicleTests]
        public async Task DeleteVehicle_Test_NotFound_And_NoContent_And_BadRequest_ObjectResult(int id, Vehicle vehicleEntity, Type expectedActionResultType)
        {
            // Arrange
            if (expectedActionResultType != typeof(NotFoundObjectResult)) _unitOfWork.Setup(x => x.Repository<Vehicle>().GetEntityWithSpec(It.IsAny<ISpecification<Vehicle>>(), It.IsAny<CancellationToken>())).ReturnsAsync(vehicleEntity).Verifiable();
            //_vehicleService.Setup(x=>x.CheckItemsExistAsync(It.IsAny<int>(),))

            _unitOfWork.Setup(x => x.Repository<Vehicle>().DeleteAsync(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>())).Verifiable();

            if (expectedActionResultType != typeof(BadRequestObjectResult)) _unitOfWork.Setup(x => x.Complete(It.IsAny<CancellationToken>())).ReturnsAsync(1).Verifiable();

            var controller = new VehiclesController(_unitOfWork.Object, _mapper.Object, _responseCache.Object, _vehicleService.Object);

            // Act
            var result = await controller.DeleteVehicleById(id);

            // Assert
            result.ShouldBeOfType(expectedActionResultType);

        }

        [Theory]
        [AddOrUpdateVehiclesPartsTestsAttributes]
        public async Task AddOrUpdateVehiclesParts_Test_BadRequest_And_Conflict_And_NotFound_And_NoContent_NotModified_ObjectResult(
            string testType,
            AddOrUpdateVehiclesPartsSpecificationParams specificationParams, 
            GetObjectFromServicesSpecification updateVehicleObject, 
            Type expectedActionResultType, 
            Product product, 
            Vehicle vehicleWithVehiclesParts, 
            List<VehiclesPart> vehiclesPart)
        {
            //Arrange

            _unitOfWork.Setup(x => x.Repository<Vehicle>()
                    .GetEntityWithSpec(It.IsAny<ISpecification<Vehicle>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(vehicleWithVehiclesParts)
                .Verifiable();
            _unitOfWork.Setup(x => x.Repository<Product>()
                    .GetEntityWithSpec(It.IsAny<ISpecification<Product>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(product)
                .Verifiable();

            _vehicleService
                .Setup(x => x.AddOrUpdateVehiclesParts(It.IsAny<AddOrUpdateVehiclesPartsSpecificationParams>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(updateVehicleObject).Verifiable();
            
            _unitOfWork.Setup(x=>x.Repository<VehiclesPart>().SearchAsync(It.IsAny<Expression<Func<VehiclesPart, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(vehiclesPart).Verifiable();

            if (expectedActionResultType != typeof(StatusCodeResult) && expectedActionResultType != typeof(BadRequestObjectResult)) _unitOfWork.Setup(x => x.Complete(It.IsAny<CancellationToken>())).ReturnsAsync(1).Verifiable();

            var controller = new VehiclesController(_unitOfWork.Object, _mapper.Object, _responseCache.Object, _vehicleService.Object);


            // Act
            var result = await controller.AddOrUpdateVehiclesParts(specificationParams);


            // Assert

            result.ShouldBeOfType(expectedActionResultType);
            if (expectedActionResultType == typeof(StatusCodeResult)) updateVehicleObject.StatusCode.ShouldBe(304);
        }
    }
}
