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
    }
}
