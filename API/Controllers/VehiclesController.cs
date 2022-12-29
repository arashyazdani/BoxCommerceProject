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
    }
}
