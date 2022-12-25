using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications
{
    public class GetCategorySpecificationParams : BaseGetSpecificationParams
    {
        public int? ParentCategoryId { get; set; }
    }
}
