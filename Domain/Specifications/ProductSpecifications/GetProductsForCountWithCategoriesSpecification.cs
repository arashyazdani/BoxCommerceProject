using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications.ProductSpecifications
{
    public class GetProductsForCountWithCategoriesSpecification : BaseSpecification<Product>
    {
        public GetProductsForCountWithCategoriesSpecification(GetProductSpecificationParams productParams) : base(x =>
            (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
            (!productParams.CategoryId.HasValue || x.CategoryId == productParams.CategoryId))
        { }
    }
}
