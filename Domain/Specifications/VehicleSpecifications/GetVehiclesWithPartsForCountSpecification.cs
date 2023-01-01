using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Specifications.VehicleSpecifications
{
    public class GetVehiclesWithPartsForCountSpecification : BaseSpecification<Vehicle>
    {
        public GetVehiclesWithPartsForCountSpecification(GetVehicleSpecificationWithPartsParams vehicleParams) :
            base(x =>
                (string.IsNullOrEmpty(vehicleParams.Search) || x.Name.ToLower().Contains(vehicleParams.Search))) //&& (vehicleParams.ProductId == x.VehiclesParts.Where(c=>c.Product.Id == vehicleParams.ProductId)))
        {
            
        }
    }
}
