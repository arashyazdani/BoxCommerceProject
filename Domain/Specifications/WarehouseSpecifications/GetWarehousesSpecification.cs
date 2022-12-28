using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Specifications.WarehouseSpecifications
{
    public class GetWarehousesSpecification : BaseSpecification<Warehouse>
    {
        public GetWarehousesSpecification(GetWarehouseSpecificationParams warehouseParams) : base(x =>
            (string.IsNullOrEmpty(warehouseParams.Search) || x.Name.ToLower().Contains(warehouseParams.Search)) &&
            (string.IsNullOrEmpty(warehouseParams.Address) || x.Address.ToLower().Contains(warehouseParams.Address)))
        {
            AddOrderBy(x => x.Name);
            ApplyPaging(warehouseParams.PageSize * (warehouseParams.PageIndex - 1), warehouseParams.PageSize);

            if (!string.IsNullOrEmpty(warehouseParams.Sort))
            {
                switch (warehouseParams.Sort)
                {
                    case "ascname":
                        AddOrderBy(p => p.Name);
                        break;

                    case "descname":
                        AddOrderByDescending(p => p.Name);
                        break;
                    case "ascaddress":
                        AddOrderBy(p => p.Address!);
                        break;

                    case "desaddress":
                        AddOrderByDescending(p => p.Address!);
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

        public GetWarehousesSpecification(int id) : base(x => x.Id == id)
        {
        }

    }
}
