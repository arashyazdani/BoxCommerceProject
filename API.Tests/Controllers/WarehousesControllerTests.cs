using API.Controllers;
using AutoMapper;
using Domain.Interfaces;
using Domain.Specifications;
using Infrastructure.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Tests.DataAttributes.WarehouseAttributes;
using Domain.Entities;
using Shouldly;
using Domain.Specifications.WarehouseSpecifications;
using Microsoft.AspNetCore.Mvc;
using API.Tests.FakeData;
using API.DTOs;
using API.Errors;

namespace API.Tests.Controllers
{
    public class WarehousesControllerTests
    {
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IResponseCacheService> _responseCache;
        private readonly Mock<IWarehouseService> _warehouseService;

        public WarehousesControllerTests()
        {
            _mapper = new Mock<IMapper>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _responseCache = new Mock<IResponseCacheService>();
            _warehouseService = new Mock<IWarehouseService>();
        }

        [Theory]
        [GetWarehouseTests]
        public async Task GetWarehouseById_Test_OK_And_NotFound_ObjectResult_And_ExceptionFormat(int id, Warehouse newWarehouse, Type expectedActionResultType)
        {
            // Arrange

            _unitOfWork.Setup(x => x.Repository<Warehouse>()
                    .GetEntityWithSpec(It.IsAny<ISpecification<Warehouse>>()))
                    .ReturnsAsync(newWarehouse)
                    .Verifiable();

            var controller = new WarehousesController(_unitOfWork.Object, _mapper.Object, _responseCache.Object, _warehouseService.Object);

            // Act and Assert

            if (typeof(FormatException) == expectedActionResultType)
            {
                await Assert.ThrowsAsync<FormatException>(() => controller.GetWarehouseById(int.Parse("Not Integer")));
            }
            else
            {
                var result = await controller.GetWarehouseById(id);

                result.Result.ShouldBeOfType(expectedActionResultType);
            }

        }

        [Theory]
        [GetWarehouseListTests]
        public async Task GetWarehouses_Test_OK_And_NotFound_ObjectResult_And_FormatException(List<Warehouse> warehouseList, Type expectedActionResultType)
        {
            //Arrange

            _unitOfWork.Setup(x => x.Repository<Warehouse>()
                    .ListWithSpecAsync(It.IsAny<ISpecification<Warehouse>>()))
                .ReturnsAsync(warehouseList)
                .Verifiable();

            var controller = new WarehousesController(_unitOfWork.Object, _mapper.Object, _responseCache.Object, _warehouseService.Object);

            // Act and Assert
            if (typeof(FormatException) == expectedActionResultType)
            {
                 Assert.Throws<FormatException>(FakeCommonData<GetWarehouseSpecificationParams>.CreateFormatException);
            }
            else
            {
                // Act
                var specParams = new GetWarehouseSpecificationParams();

                var result = await controller.GetWarehouses(specParams);

                // Assert

                result.Result.ShouldBeOfType(expectedActionResultType);
            }
            
        }

        [Theory]
        [CreateWarehouseTests]
        public async Task CreateWarehouse_Test_Ok_And_NotFound_And_NoContent_And_Conflict_ObjectResult(CreateWarehouseParams newWarehouse, Warehouse warehouseEntity, WarehouseToReturnDto warehouseToReturnDto, Type expectedActionResultType, GetObjectFromServicesSpecification createWarehouseObject)
        {
            // Arrange

            _unitOfWork.Setup(x => x.Repository<Warehouse>().InsertAsync(It.IsAny<Warehouse>()))
                .Returns(Task.FromResult(newWarehouse)).Verifiable();

            if (expectedActionResultType != typeof(BadRequestObjectResult)) _unitOfWork.Setup(x => x.Complete()).ReturnsAsync(1).Verifiable();

            _mapper.Setup(x => x.Map<Warehouse, WarehouseToReturnDto>(It.IsAny<Warehouse>())).Returns(warehouseToReturnDto);

            var createdWarehouse = new CreatedAtRouteResult("GetWarehouse", new { id = 1 }, new ApiResponse(201, "Warehouse has been created successfully.", warehouseToReturnDto));

            _mapper.Setup(x => x.Map<CreateWarehouseParams, Warehouse>(It.IsAny<CreateWarehouseParams>())).Returns(warehouseEntity);

            _warehouseService.Setup(x =>
                    x.CreateWarehouse(It.IsAny<Warehouse>()))
                .ReturnsAsync(createWarehouseObject)
                .Verifiable();

            var controller = new WarehousesController(_unitOfWork.Object, _mapper.Object, _responseCache.Object, _warehouseService.Object);

            // Act

            var result = await controller.CreateWarehouse(newWarehouse);

            // Assert

            if (expectedActionResultType == typeof(CreatedAtRouteResult)) result.Result.ShouldBeEquivalentTo(createdWarehouse);

            result.Result.ShouldBeOfType(expectedActionResultType);

        }

    }
}
