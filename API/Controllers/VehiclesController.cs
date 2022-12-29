using API.DTOs;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Specifications;
using Domain.Specifications.VehicleSpecifications;
using Microsoft.AspNetCore.Mvc;

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
    }
}
