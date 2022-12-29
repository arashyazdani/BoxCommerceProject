using AutoMapper;
using Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Controllers;
using API.Tests.DataAttributes.VehicleAttributes;
using Domain.Entities;
using Domain.Specifications;
using Shouldly;
using API.Tests.FakeData;
using Domain.Specifications.VehicleSpecifications;
using Infrastructure.Services;

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
                    .GetEntityWithSpec(It.IsAny<ISpecification<Vehicle>>()))
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
        [GetVehicleListTestsAttribute]
        public async Task GetVehicles_Test_OK_And_NotFound_ObjectResult_And_FormatException(List<Vehicle> vehicleList, Type expectedActionResultType)
        {
            //Arrange

            _unitOfWork.Setup(x => x.Repository<Vehicle>()
            .ListWithSpecAsync(It.IsAny<ISpecification<Vehicle>>()))
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
    }
}
