using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications.WarehouseSpecifications
{
    public class GetWarehouseSpecificationParams : BaseGetSpecificationParams
    {
        public string? Address { get; set; }
    }
}
