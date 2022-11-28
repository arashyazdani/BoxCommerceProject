using API.DTOs;
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

        [HttpGet("GetCategoryById/{id}")]
        public async Task<ActionResult<CategoryToReturnDTO>> GetCategoryById(int id)
        {
            try
            {
                var spec = new GetCategoriesWithParentsSpecification(id);

                var category = await _categoryRepository.GetEntityWithSpec(spec);

                if (category == null) return NotFound();

                return _mapper.Map<Category, CategoryToReturnDTO>(category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            

        }

        [HttpGet("vehicle/{id}")]
        public async Task<ActionResult<VehicleToReturnDTO>> Vehicle(int id)
        {
            var spec = new GetVehiclesSpecification(id);
            var vehicle = await _vehicleRepository.GetEntityWithSpec(spec);

            if (vehicle == null) return NotFound("Not Found");

            return _mapper.Map<Vehicle, VehicleToReturnDTO>(vehicle);
        }

    }
}
