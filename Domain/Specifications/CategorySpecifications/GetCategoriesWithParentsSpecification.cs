using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Specifications.CategorySpecifications
{
    public class GetCategoriesWithParentsSpecification : BaseSpecification<Category>
    {
        public GetCategoriesWithParentsSpecification(GetCategorySpecificationParams categoryParams) : base(x =>
            (string.IsNullOrEmpty(categoryParams.Search) || x.Name.ToLower().Contains(categoryParams.Search)) &&
            (!categoryParams.ParentCategoryId.HasValue || x.ParentCategoryId == categoryParams.ParentCategoryId))
        {
            AddInclude(x => x.Parent);
            AddOrderBy(x => x.Name);
            ApplyPaging(categoryParams.PageSize * (categoryParams.PageIndex - 1), categoryParams.PageSize);

            if (!string.IsNullOrEmpty(categoryParams.Sort))
            {
                switch (categoryParams.Sort)
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

        public GetCategoriesWithParentsSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.Parent);
        }
    }
}
