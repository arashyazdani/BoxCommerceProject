using API.DTOs;
using API.Tests.FakeData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Specifications;
using Xunit.Sdk;
using Domain.Specifications.ProductSpecifications;

namespace API.Tests.DataAttributes.ProductAttributes
{
    public class CreateProductTestsAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[]
            {
                FakeProducts<CreateProductParams>.FakeProductData(1, new CreateProductParams()), 
                FakeProducts<Product>.FakeProductData(1, new Product()), 
                FakeProducts<ProductToReturnDto>.FakeProductData(1, new ProductToReturnDto()), 
                typeof(CreatedAtRouteResult),
                FakeCommonData<GetObjectFromServicesSpecification>.FakeServiceObject(201, "Product has been created successfully.", FakeProducts<Product>.FakeProductData(1, new Product()))
            };

            yield return new object[]
            {
                FakeProducts<CreateProductParams>.FakeProductData(200, new CreateProductParams()), 
                FakeProducts<Product>.FakeProductData(1, new Product()), 
                FakeProducts<ProductToReturnDto>.FakeProductData(1, new ProductToReturnDto()), 
                typeof(NotFoundObjectResult),
                FakeCommonData<GetObjectFromServicesSpecification>.FakeServiceObject(404, "The CategoryId is not found.", FakeProducts<Product>.FakeProductData(1, new Product()))
            };

            yield return new object[]
            {
                FakeProducts<CreateProductParams>.FakeProductData(1, new CreateProductParams()), 
                FakeProducts<Product>.FakeProductData(1, new Product()), 
                FakeProducts<ProductToReturnDto>.FakeProductData(1, new ProductToReturnDto()), 
                typeof(BadRequestObjectResult),
                FakeCommonData<GetObjectFromServicesSpecification>.FakeServiceObject(400, "Bad request.", FakeProducts<Product>.FakeProductData(1, new Product()))
            };

            yield return new object[]
            {
                FakeProducts<CreateProductParams>.FakeProductData(1, new CreateProductParams()),
                FakeProducts<Product>.FakeProductData(1, new Product()),
                FakeProducts<ProductToReturnDto>.FakeProductData(1, new ProductToReturnDto()),
                typeof(FormatException),
                FakeCommonData<GetObjectFromServicesSpecification>.FakeServiceObject(400, "Bad request.", FakeProducts<Product>.FakeProductData(1, new Product()))
            };

            yield return new object[]
            {
                FakeProducts<CreateProductParams>.FakeProductData(1, new CreateProductParams()),
                FakeProducts<Product>.FakeProductData(1, new Product()),
                FakeProducts<ProductToReturnDto>.FakeProductData(1, new ProductToReturnDto()),
                typeof(ConflictObjectResult),
                FakeCommonData<GetObjectFromServicesSpecification>.FakeServiceObject(409, "The Product name is already exist.", FakeProducts<Product>.FakeProductData(1, new Product()))
            };
        }
    }
}
