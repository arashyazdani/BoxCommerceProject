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
using Domain.Specifications;
using Domain.Specifications.CategorySpecifications;
using Domain.Specifications.ProductSpecifications;
using Infrastructure.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace API.Controllers
{
    [Route("api/products")]
    public class ProductsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IResponseCacheService _responseCache;
        private readonly IProductService _productService;

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper, IResponseCacheService responseCache, IProductService productService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _responseCache = responseCache;
            _productService = productService;
        }

        [Cached(600)]
        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<ActionResult<ProductToReturnDto>> GetProductById(int id)
        {
            var spec = new GetProductsWithCategoriesSpecification(id);

            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpec(spec);

            if (product == null) return NotFound(new ApiResponse(404));

            var returnProduct = _mapper.Map<Product, ProductToReturnDto>(product);

            return new OkObjectResult(new ApiResponse(200, "Ok", returnProduct));
        }

        [Cached(600)]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] GetProductSpecificationParams specParams)
        {

            var spec = new GetProductsWithCategoriesSpecification(specParams);

            var countSpec = new GetProductsForCountWithCategoriesSpecification(specParams);

            var totalItems = await _unitOfWork.Repository<Product>().CountAsync(countSpec);

            var products = await _unitOfWork.Repository<Product>().ListWithSpecAsync(spec);

            if (products == null || products.Count == 0) return NotFound(new ApiResponse(404));

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            var returnProducts =
                new Pagination<ProductToReturnDto>(specParams.PageIndex, specParams.PageSize, totalItems, data);

            return new OkObjectResult(new ApiResponse(200, "Ok", returnProducts));

        }

        [HttpPost]
        public async Task<ActionResult<ProductToReturnDto>> CreateProduct([FromQuery] CreateProductParams createProductParams)
        {

            var productEntity = _mapper.Map<CreateProductParams, Product>(createProductParams);

            var insertResult = await _productService.CreateProduct(productEntity);

            switch (insertResult.StatusCode)
            {
                case 404:
                    return NotFound(new ApiResponse(insertResult.StatusCode, insertResult.Message));

                case 400:
                    return BadRequest(new ApiResponse(insertResult.StatusCode));

                case 409:
                    return Conflict(new ApiResponse(insertResult.StatusCode, insertResult.Message));
            }

            var returnDto = _mapper.Map<Product, ProductToReturnDto>(insertResult.ProductResult);

            return new CreatedAtRouteResult("GetProduct", new { id = productEntity.Id }, new ApiResponse(201, "Product has been created successfully.", returnDto));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProduct([FromQuery] UpdateProductParams updateProductParams)
        {
            var specProduct = new GetProductsWithCategoriesSpecification(updateProductParams.Id);

            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpec(specProduct);

            if (product == null) return NotFound(new ApiResponse(404, "The product is not found."));

            _mapper.Map(updateProductParams, product);

            var updateResult = await _productService.UpdateProduct(product);

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

            await _responseCache.DeleteRangeOfKeysAsync("Products");

            return NoContent();

        }

        [HttpPatch("id")]
        public async Task<ActionResult> PartiallyUpdateProduct(int id, JsonPatchDocument<UpdateProductParams> updateProductParams)
        {

            var specProduct = new GetProductsWithCategoriesSpecification(id);

            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpec(specProduct);

            if (product == null) return NotFound(new ApiResponse(404, "The product is not found."));

            var productToPatch = _mapper.Map<UpdateProductParams>(product);

            updateProductParams.ApplyTo(productToPatch, ModelState);

            if (!TryValidateModel(productToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(productToPatch, product);

            var updateResult = await _productService.UpdateProduct(product);

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

            await _responseCache.DeleteRangeOfKeysAsync("Products");

            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProductById(int id)
        {

            var spec = new GetProductsWithCategoriesSpecification(id);

            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpec(spec);

            if (product == null) return NotFound(new ApiResponse(404));

            await _unitOfWork.Repository<Product>().DeleteAsync(id);

            var result = await _unitOfWork.Complete();

            if (result <= 0) return BadRequest(new ApiResponse(400));

            await _responseCache.DeleteRangeOfKeysAsync("Products");

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
