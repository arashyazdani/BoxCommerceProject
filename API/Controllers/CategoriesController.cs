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
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Vehicle> _vehicleRepository;

        public CategoriesController(IGenericRepository<Category> categoryRepository, IMapper mapper, IGenericRepository<Vehicle> vehicleRepository)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _vehicleRepository = vehicleRepository;
        }

        [HttpGet("GetCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pagination<CategoryToReturnDTO>>> GetCategories([FromQuery] CategorySpecificationParams specParams)
        {
            try
            {
                var spec = new GetCategoriesWithParentsSpecification(specParams);

                var countSpec = new GetCategoriesForCountWithParentsSpecification(specParams);

                var totalItems = await _categoryRepository.CountAsync(countSpec);

                var categories = await _categoryRepository.ListWithSpecAsync(spec);

                if (categories == null || categories.Count == 0) return NotFound(new ApiResponse(404));

                var data = _mapper.Map<IReadOnlyList<Category>, IReadOnlyList<CategoryToReturnDTO>>(categories);

                var returnCategories =
                    new Pagination<CategoryToReturnDTO>(specParams.PageIndex, specParams.PageSize, totalItems, data);

                return new ObjectResult(new ApiResponse(200, "Ok", returnCategories));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpGet("GetCategoryById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CategoryToReturnDTO>> GetCategoryById(int id)
        {
            try
            {
                var spec = new GetCategoriesWithParentsSpecification(id);

                var category = await _categoryRepository.GetEntityWithSpec(spec);

                if (category == null) return NotFound(new ApiResponse(404));

                var returnCategories = _mapper.Map<Category, CategoryToReturnDTO>(category);

                return new ObjectResult(new ApiResponse(200, "Ok", returnCategories));

            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

    }
}
