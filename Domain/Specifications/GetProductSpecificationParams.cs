using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications
{
    public class GetProductSpecificationParams : BaseGetSpecificationParams
    {
        public int? CategoryId { get; set; }
    }
}
