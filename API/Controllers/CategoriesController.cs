//using System.Text.Json;
using System.Xml.XPath;
using API.DTOs;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Specifications.CategorySpecifications;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Converters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace API.Controllers
{
    [Route("api/categories")]
    public class CategoriesController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IResponseCacheService _responseCache;
        private readonly ICategoryService _categoryService;

        public CategoriesController(IUnitOfWork unitOfWork, IMapper mapper, IResponseCacheService responseCache, ICategoryService categoryService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _responseCache = responseCache;
            _categoryService = categoryService;
        }

        [Cached(600)]
        [HttpGet]
        public async Task<ActionResult<Pagination<CategoryToReturnDto>>> GetCategories([FromQuery] GetCategorySpecificationParams specParams, CancellationToken cancellationToken = default(CancellationToken))
        {

            var spec = new GetCategoriesWithParentsSpecification(specParams);

            var countSpec = new GetCategoriesForCountWithParentsSpecification(specParams);

            var totalItems = await _unitOfWork.Repository<Category>().CountAsync(countSpec, cancellationToken);

            var categories = await _unitOfWork.Repository<Category>().ListWithSpecAsync(spec, cancellationToken);

            if (categories == null || categories.Count == 0) return NotFound(new ApiResponse(404));

            var data = _mapper.Map<IReadOnlyList<Category>, IReadOnlyList<CategoryToReturnDto>>(categories);

            var returnCategories =
                new Pagination<CategoryToReturnDto>(specParams.PageIndex, specParams.PageSize, totalItems, data);

            return new OkObjectResult(new ApiResponse(200, "Ok", returnCategories));

        }

        [Cached(600)]
        [HttpGet("{id}", Name = "GetCategory")]
        public async Task<ActionResult<CategoryToReturnDto>> GetCategoryById(int id, CancellationToken cancellationToken = default(CancellationToken))
        {

            var spec = new GetCategoriesWithParentsSpecification(id);

            var category = await _unitOfWork.Repository<Category>().GetEntityWithSpec(spec, cancellationToken);

            if (category == null) return NotFound(new ApiResponse(404));

            var returnCategories = _mapper.Map<Category, CategoryToReturnDto>(category);

            return new OkObjectResult(new ApiResponse(200, "Ok", returnCategories));

        }


        [HttpPost]
        public async Task<ActionResult<CategoryToReturnDto>> CreateCategory([FromQuery] CreateCategoryParams createCategoryParams, CancellationToken cancellationToken = default(CancellationToken))
        {

            var categoryEntity = _mapper.Map<CreateCategoryParams, Category>(createCategoryParams);

            var insertResult = await _categoryService.CreateCategory(categoryEntity, cancellationToken);

            switch (insertResult.StatusCode)
            {
                case 404:
                    return NotFound(new ApiResponse(insertResult.StatusCode, insertResult.Message));

                case 400:
                    return BadRequest(new ApiResponse(insertResult.StatusCode));

                case 409:
                    return Conflict(new ApiResponse(insertResult.StatusCode, insertResult.Message));
            }

            var returnDto = _mapper.Map<Category, CategoryToReturnDto>(insertResult.ResultObject);

            return new CreatedAtRouteResult("GetCategory", new { id = categoryEntity.Id }, new ApiResponse(201, "Category has been created successfully.", returnDto));

        }

        [HttpPut]
        public async Task<ActionResult> UpdateCategory([FromQuery] UpdateCategoryParams updateCategoryParams, CancellationToken cancellationToken = default(CancellationToken))
        {
            var specCategory = new GetCategoriesWithParentsSpecification(updateCategoryParams.Id);

            var category = await _unitOfWork.Repository<Category>().GetEntityWithSpec(specCategory, cancellationToken);

            if (category == null) return NotFound(new ApiResponse(404, "The category is not found."));

            _mapper.Map(updateCategoryParams, category);

            var updateResult = await _categoryService.UpdateCategory(category, cancellationToken);

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

            await _responseCache.DeleteRangeOfKeysAsync("Categories");

            return NoContent();

        }

        [HttpPatch("id")]
        public async Task<ActionResult> PartiallyUpdateCategory(int id, JsonPatchDocument<UpdateCategoryParams> updateCategoryParams, CancellationToken cancellationToken = default(CancellationToken))
        {

            var specCategory = new GetCategoriesWithParentsSpecification(id);

            var category = await _unitOfWork.Repository<Category>().GetEntityWithSpec(specCategory, cancellationToken);

            if (category == null) return NotFound(new ApiResponse(404, "The category is not found."));

            var categoryToPatch = _mapper.Map<UpdateCategoryParams>(category);

            updateCategoryParams.ApplyTo(categoryToPatch, ModelState);

            if (!TryValidateModel(categoryToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(categoryToPatch, category);

            var updateResult = await _categoryService.UpdateCategory(category, cancellationToken);

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

            await _responseCache.DeleteRangeOfKeysAsync("Categories");

            return NoContent();

        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategoryById(int id, CancellationToken cancellationToken = default(CancellationToken))
        {

            var spec = new GetCategoriesWithParentsSpecification(id);

            var category = await _unitOfWork.Repository<Category>().GetEntityWithSpec(spec, cancellationToken);

            if (category == null) return NotFound(new ApiResponse(404));

            await _unitOfWork.Repository<Category>().DeleteAsync(id);

            var result = await _unitOfWork.Complete(cancellationToken);

            if (result <= 0) return BadRequest(new ApiResponse(400));

            await _responseCache.DeleteRangeOfKeysAsync("Categories");

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
