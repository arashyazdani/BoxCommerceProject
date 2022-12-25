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
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/products")]
    public class ProductsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IResponseCacheService _responseCache;

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper, IResponseCacheService responseCache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _responseCache = responseCache;
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

            if (createProductParams.CategoryId != null)
            {
                var categoryExists = await _unitOfWork.Repository<Category>().GetByIdAsync((int)createProductParams.CategoryId);

                if (categoryExists == null) return NotFound(new ApiResponse(404, "The CategoryId is not found."));
            }

            var productEntity = _mapper.Map<CreateProductParams, Product>(createProductParams);

            await _unitOfWork.Repository<Product>().InsertAsync(productEntity);

            var result = await _unitOfWork.Complete();

            if (result <= 0) return BadRequest(new ApiResponse(400));

            var returnDto = _mapper.Map<Product, ProductToReturnDto>(productEntity);

            return new CreatedAtRouteResult("GetProduct", new { id = productEntity.Id }, new ApiResponse(201, "Product has been created successfully.", returnDto));
        }

    }
}
