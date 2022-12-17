using API.DTOs;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Specifications;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CategoriesController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoriesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [Cached(600)]
        [HttpGet]

        public async Task<ActionResult<Pagination<CategoryToReturnDto>>> GetCategories([FromQuery] CategorySpecificationParams specParams)
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

        [Cached(600)]
        [HttpGet("{id}", Name = "GetCategory")]
        public async Task<ActionResult<CategoryToReturnDto>> GetCategoryById(int id)
        {

            var spec = new GetCategoriesWithParentsSpecification(id);

            var category = await _unitOfWork.Repository<Category>().GetEntityWithSpec(spec);

            if (category == null) return NotFound(new ApiResponse(404));

            var returnCategories = _mapper.Map<Category, CategoryToReturnDto>(category);

            return new OkObjectResult(new ApiResponse(200, "Ok", returnCategories));

        }

        [HttpPost]
        public async Task<ActionResult<CategoryToReturnDto>> CreateCategory([FromQuery] CreateCategoryParams createCategoryParams)
        {
            if (createCategoryParams.ParentCategoryId != null)
            {
                var spec = new GetCategoriesWithParentsSpecification((int)createCategoryParams.ParentCategoryId);

                var categoryExists = await _unitOfWork.Repository<Category>().GetEntityWithSpec(spec);
                //var categoryExists = await GetCategoryById((int)createCategoryParams.ParentCategoryId);
                if (categoryExists == null) return NotFound(new ApiResponse(404,"The ParentCategoryId is not found."));
            }

            var categoryEntity = _mapper.Map<CreateCategoryParams, Category>(createCategoryParams);

            await _unitOfWork.Repository<Category>().InsertAsync(categoryEntity);

            var result = await _unitOfWork.Complete();

            if (result <= 0) return BadRequest(new ApiResponse(400));

            var returnDto = _mapper.Map<Category, CategoryToReturnDto>(categoryEntity);

            return new CreatedAtRouteResult("GetCategory", new { id = categoryEntity.Id }, new ApiResponse(201, "Category has been created successfully.", returnDto));

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CategoryToReturnDto>> DeleteCategoryById(int id)
        {

            var spec = new GetCategoriesWithParentsSpecification(id);

            var category = await _unitOfWork.Repository<Category>().GetEntityWithSpec(spec);

            if (category == null) return NotFound(new ApiResponse(404));

            await _unitOfWork.Repository<Category>().DeleteAsync(id);

            var result = await _unitOfWork.Complete();

            if (result <= 0) return BadRequest(new ApiResponse(400));


            return new OkObjectResult(new ApiResponse(200, "Category has been deleted."));

        }
    }
}
