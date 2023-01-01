using Domain.Entities;
using Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IVehicleService
    {
        Task<GetObjectFromServicesSpecification> CreateVehicle(Vehicle createVehicleParams, CancellationToken cancellationToken = default(CancellationToken));
        Task<GetObjectFromServicesSpecification> UpdateVehicle(Vehicle updateVehicleParams, CancellationToken cancellationToken = default(CancellationToken));
    }
}
