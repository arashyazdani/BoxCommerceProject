using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications.WarehouseSpecifications
{
    public class GetWarehousesForCountSpecification : BaseSpecification<Warehouse>
    {
        public GetWarehousesForCountSpecification(GetWarehouseSpecificationParams warehouseParams) : base(x =>
            (string.IsNullOrEmpty(warehouseParams.Search) || x.Name.ToLower().Contains(warehouseParams.Search)) &&
            (string.IsNullOrEmpty(warehouseParams.Address) || x.Address.ToLower().Contains(warehouseParams.Address)))
        {
        }
    }
}
