using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications
{
    public class GetCategoriesForCountWithParentsSpecification : BaseSpecification<Category>
    {
        public GetCategoriesForCountWithParentsSpecification(CategorySpecificationParams categoryParams) : base(x =>
            (string.IsNullOrEmpty(categoryParams.Search) || x.Name.ToLower().Contains(categoryParams.Search)))
        {
        }

    }
}
