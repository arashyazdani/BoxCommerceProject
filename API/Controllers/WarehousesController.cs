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
using Infrastructure.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

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

        [Cached(600)]
        [HttpGet]
        public async Task<ActionResult<Pagination<WarehouseToReturnDto>>> GetWarehouses([FromQuery] GetWarehouseSpecificationParams specParams)
        {

            var spec = new GetWarehousesSpecification(specParams);

            var countSpec = new GetWarehousesForCountSpecification(specParams);

            var totalItems = await _unitOfWork.Repository<Warehouse>().CountAsync(countSpec);

            var warehouses = await _unitOfWork.Repository<Warehouse>().ListWithSpecAsync(spec);

            if (warehouses == null || warehouses.Count == 0) return NotFound(new ApiResponse(404));

            var data = _mapper.Map<IReadOnlyList<Warehouse>, IReadOnlyList<WarehouseToReturnDto>>(warehouses);

            var returnWarehouses =
                new Pagination<WarehouseToReturnDto>(specParams.PageIndex, specParams.PageSize, totalItems, data);

            return new OkObjectResult(new ApiResponse(200, "Ok", returnWarehouses));

        }

        [HttpPost]
        public async Task<ActionResult<WarehouseToReturnDto>> CreateWarehouse([FromQuery] CreateWarehouseParams createWarehouseParams)
        {

            var warehouseEntity = _mapper.Map<CreateWarehouseParams, Warehouse>(createWarehouseParams);

            var insertResult = await _warehouseService.CreateWarehouse(warehouseEntity);

            switch (insertResult.StatusCode)
            {
                case 400:
                    return BadRequest(new ApiResponse(insertResult.StatusCode));

                case 409:
                    return Conflict(new ApiResponse(insertResult.StatusCode, insertResult.Message));
            }

            var returnDto = _mapper.Map<Warehouse, WarehouseToReturnDto>(insertResult.ResultObject);

            return new CreatedAtRouteResult("GetWarehouse", new { id = warehouseEntity.Id }, new ApiResponse(201, "Warehouse has been created successfully.", returnDto));

        }

        [HttpPut]
        public async Task<ActionResult> UpdateWarehouse([FromQuery] UpdateWarehouseParams updateWarehouseParams)
        {
            var specWarehouse = new GetWarehousesSpecification(updateWarehouseParams.Id);

            var warehouse = await _unitOfWork.Repository<Warehouse>().GetEntityWithSpec(specWarehouse);

            if (warehouse == null) return NotFound(new ApiResponse(404, "The warehouse is not found."));

            _mapper.Map(updateWarehouseParams, warehouse);

            var updateResult = await _warehouseService.UpdateWarehouse(warehouse);

            switch (updateResult.StatusCode)
            {
                case 400:
                    return BadRequest(new ApiResponse(updateResult.StatusCode));

                case 409:
                    return Conflict(new ApiResponse(updateResult.StatusCode, updateResult.Message));

                case 304:
                    return new StatusCodeResult(304);
            }

            await _responseCache.DeleteRangeOfKeysAsync("warehouses");

            return NoContent();

        }

        [HttpPatch("id")]
        public async Task<ActionResult> PartiallyUpdateWarehouse(int id, JsonPatchDocument<UpdateWarehouseParams> updateWarehouseParams)
        {

            var specWarehouse = new GetWarehousesSpecification(id);

            var warehouse = await _unitOfWork.Repository<Warehouse>().GetEntityWithSpec(specWarehouse);

            if (warehouse == null) return NotFound(new ApiResponse(404, "The warehouse is not found."));

            var warehouseToPatch = _mapper.Map<UpdateWarehouseParams>(warehouse);

            updateWarehouseParams.ApplyTo(warehouseToPatch, ModelState);

            if (!TryValidateModel(warehouseToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(warehouseToPatch, warehouse);

            var updateResult = await _warehouseService.UpdateWarehouse(warehouse);

            switch (updateResult.StatusCode)
            {
                case 404:
                    return NotFound(new ApiResponse(updateResult.StatusCode, updateResult.Message));

                case 400:
                    return BadRequest(new ApiResponse(updateResult.StatusCode));

                case 409:
                    return Conflict(new ApiResponse(updateResult.StatusCode, updateResult.Message));

                case 304:
                    return new StatusCodeResult(304);
            }

            await _responseCache.DeleteRangeOfKeysAsync("warehouses");

            return NoContent();

        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWarehouseById(int id)
        {

            var spec = new GetWarehousesSpecification(id);

            var warehouse = await _unitOfWork.Repository<Warehouse>().GetEntityWithSpec(spec);

            if (warehouse == null) return NotFound(new ApiResponse(404));

            await _unitOfWork.Repository<Warehouse>().DeleteAsync(id);

            var result = await _unitOfWork.Complete();

            if (result <= 0) return BadRequest(new ApiResponse(400));

            await _responseCache.DeleteRangeOfKeysAsync("warehouses");

            return NoContent();

        }

        public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices
                .GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    }
}
