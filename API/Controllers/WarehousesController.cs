using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.DTOs;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Specifications.WarehouseSpecifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/warehouses")]
    public class WarehousesController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IResponseCacheService _responseCache;
        private readonly IWarehouseService _warehouseService;

        public WarehousesController(IUnitOfWork unitOfWork, IMapper mapper, IResponseCacheService responseCache, IWarehouseService warehouseService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _responseCache = responseCache;
            _warehouseService = warehouseService;
        }

        [Cached(600)]
        [HttpGet("{id}", Name = "GetWarehouse")]
        public async Task<ActionResult<WarehouseToReturnDto>> GetWarehouseById(int id)
        {

            var spec = new GetWarehousesSpecification(id);

            var warehouse = await _unitOfWork.Repository<Warehouse>().GetEntityWithSpec(spec);

            if (warehouse == null) return NotFound(new ApiResponse(404));

            var returnWarehouses = _mapper.Map<Warehouse, WarehouseToReturnDto>(warehouse);

            return new OkObjectResult(new ApiResponse(200, "Ok", returnWarehouses));

        }
    }
}
