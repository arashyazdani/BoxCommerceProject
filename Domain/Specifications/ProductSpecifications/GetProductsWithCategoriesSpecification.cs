using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Specifications.ProductSpecifications
{
    public class GetProductsWithCategoriesSpecification : BaseSpecification<Product>
    {
        public GetProductsWithCategoriesSpecification(GetProductSpecificationParams productParams) : base(x =>
            (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
            (!productParams.CategoryId.HasValue || x.CategoryId == productParams.CategoryId))
        {
            AddInclude(x => x.Category);
            AddOrderBy(x => x.Name);
            ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);

            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                switch (productParams.Sort)
                {
                    case "ascname":
                        AddOrderBy(p => p.Name);
                        break;

                    case "descname":
                        AddOrderByDescending(p => p.Name);
                        break;

                    case "ascpriority":
                        AddOrderBy(p => p.Priority ?? 0);
                        break;

                    case "descpriority":
                        AddOrderByDescending(p => p.Priority ?? 0);
                        break;

                    case "ascid":
                        AddOrderBy(p => p.Id);
                        break;

                    case "descid":
                        AddOrderByDescending(p => p.Id);
                        break;

                    default:
                        AddOrderBy(n => n.Name);
                        break;
                }
            }
        }
        public GetProductsWithCategoriesSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.Category);
        }


    }
}
