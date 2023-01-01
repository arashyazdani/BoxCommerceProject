using API.DTOs;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Specifications;
using Domain.Specifications.ProductSpecifications;
using Domain.Specifications.VehicleSpecifications;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using System.Linq;

namespace API.Controllers
{
    [Route("api/vehicles")]
    public class VehiclesController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IResponseCacheService _responseCache;
        private readonly IVehicleService _vehicleService;

        public VehiclesController(IUnitOfWork unitOfWork, IMapper mapper, IResponseCacheService responseCache, IVehicleService vehicleService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _responseCache = responseCache;
            _vehicleService = vehicleService;
        }

        [HttpGet("{id}", Name = "GetVehicle")]
        public async Task<ActionResult<VehicleToReturnDto>> GetVehicleById(int id)
        {
            var spec = new GetVehiclesSpecification(id);

            var vehicle = await _unitOfWork.Repository<Vehicle>().GetEntityWithSpec(spec);

            if (vehicle == null) return NotFound(new ApiResponse(404));

            var returnVehicles = _mapper.Map<Vehicle, VehicleToReturnDto>(vehicle);

            return new OkObjectResult(new ApiResponse(200, "Ok", returnVehicles));
        }

        [Cached(600)]
        [HttpGet]
        public async Task<ActionResult<Pagination<VehicleToReturnDto>>> GetVehicles([FromQuery] GetVehicleSpecificationParams specParams)
        {

            var spec = new GetVehiclesSpecification(specParams);

            var countSpec = new GetVehiclesForCountSpecification(specParams);

            var totalItems = await _unitOfWork.Repository<Vehicle>().CountAsync(countSpec);

            var vehicles = await _unitOfWork.Repository<Vehicle>().ListWithSpecAsync(spec);

            if (vehicles == null || vehicles.Count == 0) return NotFound(new ApiResponse(404));

            var data = _mapper.Map<IReadOnlyList<Vehicle>, IReadOnlyList<VehicleToReturnDto>>(vehicles);

            var returnVehicles =
                new Pagination<VehicleToReturnDto>(specParams.PageIndex, specParams.PageSize, totalItems, data);

            return new OkObjectResult(new ApiResponse(200, "Ok", returnVehicles));

        }

        [Cached(600)]
        [HttpGet("{id}/vehicleparts", Name = "GetVehicleWithVehiclePartsByID")]
        public async Task<ActionResult<VehicleWithVehiclePartsToReturnDto>> GetVehicleWithVehiclePartsById(int id)
        {

            var spec = new GetVehiclesWithPartsSpecification(id);

            var vehicle = await _unitOfWork.Repository<Vehicle>().GetEntityWithSpec(spec);

            if (vehicle == null) return NotFound(new ApiResponse(404));

            var returnVehicles = _mapper.Map<Vehicle, VehicleWithVehiclePartsToReturnDto>(vehicle);

            return new OkObjectResult(new ApiResponse(200, "Ok", returnVehicles));
        }

        [Cached(600)]
        [HttpGet("vehicleparts", Name = "GetVehicleWithVehicleParts")]
        public async Task<ActionResult<Pagination<VehicleWithVehiclePartsToReturnDto>>> GetVehicleWithVehicleParts([FromQuery] GetVehicleSpecificationParams specParams)
        {

            var spec = new GetVehiclesWithPartsSpecification(specParams);

            var countSpec = new GetVehiclesWithPartsSpecification(specParams);

            var totalItems = await _unitOfWork.Repository<Vehicle>().CountAsync(countSpec);

            var vehicles = await _unitOfWork.Repository<Vehicle>().ListWithSpecAsync(spec);

            if (vehicles == null || vehicles.Count == 0) return NotFound(new ApiResponse(404));

            var data = _mapper.Map<IReadOnlyList<Vehicle>, IReadOnlyList<VehicleWithVehiclePartsToReturnDto>>(vehicles);

            var returnVehicles =
                new Pagination<VehicleWithVehiclePartsToReturnDto>(specParams.PageIndex, specParams.PageSize, totalItems, data);

            return new OkObjectResult(new ApiResponse(200, "Ok", returnVehicles));

        }

        [HttpPost]
        public async Task<ActionResult<VehicleToReturnDto>> CreateVehicle([FromQuery] CreateVehicleParams createVehicleParams)
        {

            var vehicleEntity = _mapper.Map<CreateVehicleParams, Vehicle>(createVehicleParams);

            var insertResult = await _vehicleService.CreateVehicle(vehicleEntity);

            switch (insertResult.StatusCode)
            {
                case 400:
                    return BadRequest(new ApiResponse(insertResult.StatusCode));

                case 409:
                    return Conflict(new ApiResponse(insertResult.StatusCode, insertResult.Message));
            }

            var returnDto = _mapper.Map<Vehicle, VehicleToReturnDto>(insertResult.ResultObject);

            return new CreatedAtRouteResult("GetVehicle", new { id = vehicleEntity.Id }, new ApiResponse(201, "Vehicle has been created successfully.", returnDto));

        }

        [HttpPut]
        public async Task<ActionResult> UpdateVehicle([FromQuery] UpdateVehicleParams updateVehicleParams)
        {
            var specVehicle = new GetVehiclesSpecification(updateVehicleParams.Id);

            var vehicle = await _unitOfWork.Repository<Vehicle>().GetEntityWithSpec(specVehicle);

            if (vehicle == null) return NotFound(new ApiResponse(404, "The vehicle is not found."));

            _mapper.Map(updateVehicleParams, vehicle);

            var updateResult = await _vehicleService.UpdateVehicle(vehicle);

            switch (updateResult.StatusCode)
            {
                case 400:
                    return BadRequest(new ApiResponse(updateResult.StatusCode));

                case 409:
                    return Conflict(new ApiResponse(updateResult.StatusCode, updateResult.Message));

                case 304:
                    return new StatusCodeResult(304);
            }

            await _responseCache.DeleteRangeOfKeysAsync("vehicles");

            return NoContent();

        }

        [HttpPatch("id")]
        public async Task<ActionResult> PartiallyUpdateVehicle(int id, JsonPatchDocument<UpdateVehicleParams> updateVehicleParams)
        {

            var specVehicle = new GetVehiclesSpecification(id);

            var vehicle = await _unitOfWork.Repository<Vehicle>().GetEntityWithSpec(specVehicle);

            if (vehicle == null) return NotFound(new ApiResponse(404, "The vehicle is not found."));

            var vehicleToPatch = _mapper.Map<UpdateVehicleParams>(vehicle);

            updateVehicleParams.ApplyTo(vehicleToPatch, ModelState);

            if (!TryValidateModel(vehicleToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(vehicleToPatch, vehicle);

            var updateResult = await _vehicleService.UpdateVehicle(vehicle);

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

            await _responseCache.DeleteRangeOfKeysAsync("vehicles");

            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVehicleById(int id)
        {

            var spec = new GetVehiclesSpecification(id);

            var vehicle = await _unitOfWork.Repository<Vehicle>().GetEntityWithSpec(spec);

            if (vehicle == null) return NotFound(new ApiResponse(404));

            await _unitOfWork.Repository<Vehicle>().DeleteAsync(id);

            var result = await _unitOfWork.Complete();

            if (result <= 0) return BadRequest(new ApiResponse(400));

            await _responseCache.DeleteRangeOfKeysAsync("vehicles");

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
