using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Specifications
{
    public class GetCategoriesWithParentsSpecification : BaseSpecification<Category>
    {
        public GetCategoriesWithParentsSpecification()
        {
            AddInclude(x=>x.Parent);
        }

        public GetCategoriesWithParentsSpecification(int id) : base(x=> x.Id==id)
        {
            AddInclude(x => x.Parent);
        }
    }
}
