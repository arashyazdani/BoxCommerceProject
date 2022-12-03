using API.DTOs;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CategoriesController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Vehicle> _vehicleRepository;

        public CategoriesController(IUnitOfWork unitOfWork, IMapper mapper, IGenericRepository<Vehicle> vehicleRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _vehicleRepository = vehicleRepository;
        }

        [Cached(600)]
        [HttpGet("GetCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pagination<CategoryToReturnDto>>> GetCategories([FromQuery] CategorySpecificationParams specParams)
        {
            try
            {
                var spec = new GetCategoriesWithParentsSpecification(specParams);

                var countSpec = new GetCategoriesForCountWithParentsSpecification(specParams);

                var totalItems = await _unitOfWork.Repository<Category>().CountAsync(countSpec);

                var categories = await _unitOfWork.Repository<Category>().ListWithSpecAsync(spec);

                if (categories == null || categories.Count == 0) return NotFound(new ApiResponse(404));

                var data = _mapper.Map<IReadOnlyList<Category>, IReadOnlyList<CategoryToReturnDto>>(categories);

                var returnCategories =
                    new Pagination<CategoryToReturnDto>(specParams.PageIndex, specParams.PageSize, totalItems, data);

                //return Ok(returnCategories);
                return new OkObjectResult(new ApiResponse(200, "Ok", returnCategories));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [Cached(600)]
        [HttpGet("GetCategoryById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CategoryToReturnDto>> GetCategoryById(int id)
        {
            try
            {
                var spec = new GetCategoriesWithParentsSpecification(id);

                var category = await _unitOfWork.Repository<Category>().GetEntityWithSpec(spec);

                if (category == null) return NotFound(new ApiResponse(404));

                var returnCategories = _mapper.Map<Category, CategoryToReturnDto>(category);

                return new OkObjectResult(new ApiResponse(200, "Ok", returnCategories));

            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

    }
}
