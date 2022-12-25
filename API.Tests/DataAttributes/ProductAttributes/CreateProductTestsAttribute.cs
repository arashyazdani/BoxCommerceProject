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
using Xunit.Sdk;
using Domain.Specifications.ProductSpecifications;

namespace API.Tests.DataAttributes.ProductAttributes
{
    public class CreateProductTestsAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] { FakeProducts<CreateProductParams>.FakeProductData(1, new CreateProductParams()), FakeProducts<Product>.FakeProductData(1, new Product()), FakeProducts<ProductToReturnDto>.FakeProductData(1, new ProductToReturnDto()), typeof(CreatedAtRouteResult) };
            yield return new object[] { FakeProducts<CreateProductParams>.FakeProductData(200, new CreateProductParams()), FakeProducts<Product>.FakeProductData(1, new Product()), FakeProducts<ProductToReturnDto>.FakeProductData(1, new ProductToReturnDto()), typeof(NotFoundObjectResult) };
            yield return new object[] { FakeProducts<CreateProductParams>.FakeProductData(1, new CreateProductParams()), FakeProducts<Product>.FakeProductData(1, new Product()), FakeProducts<ProductToReturnDto>.FakeProductData(1, new ProductToReturnDto()), typeof(BadRequestObjectResult) };
        }
    }
}
