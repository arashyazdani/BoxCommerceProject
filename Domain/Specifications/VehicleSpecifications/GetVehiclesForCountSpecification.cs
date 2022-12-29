using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Specifications.VehicleSpecifications
{
    public class GetVehiclesForCountSpecification : BaseSpecification<Vehicle>
    {
        public GetVehiclesForCountSpecification(GetVehicleSpecificationParams vehicleParams) : base(x =>
            (string.IsNullOrEmpty(vehicleParams.Search) || x.Name.ToLower().Contains(vehicleParams.Search)))
        {
            
        }
    }
}
