using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Specifications;

namespace Domain.Interfaces
{
    public interface IWarehouseService
    {
        Task<GetObjectFromServicesSpecification> CreateWarehouse(Warehouse createWarehouseParams, CancellationToken cancellationToken = default(CancellationToken));
        Task<GetObjectFromServicesSpecification> UpdateWarehouse(Warehouse updateWarehouseParams, CancellationToken cancellationToken = default(CancellationToken));
    }
}
