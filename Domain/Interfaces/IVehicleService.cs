using Domain.Entities;
using Domain.Specifications;
using Domain.Specifications.VehicleSpecifications;
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

        Task<GetObjectFromServicesSpecification> AddOrUpdateVehiclesParts(AddOrUpdateVehiclesPartsSpecificationParams updateVehiclesPartParams, CancellationToken cancellationToken = default(CancellationToken));

    }
}
