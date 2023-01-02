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
                (
                    (string.IsNullOrEmpty(vehicleParams.Search) &&
                     string.IsNullOrEmpty(vehicleParams.ProductName) &&
                     vehicleParams.ProductId == null &&
                     vehicleParams.IsDiscontinued == false)
                    ||
                    (string.IsNullOrEmpty(vehicleParams.Search) &&
                     string.IsNullOrEmpty(vehicleParams.ProductName) &&
                     (vehicleParams.ProductId != null && vehicleParams.ProductId == x.VehiclesParts.FirstOrDefault(c => c.ProductId == vehicleParams.ProductId)!.ProductId) &&
                     vehicleParams.IsDiscontinued == false)
                    ||
                    (!string.IsNullOrEmpty(vehicleParams.Search) &&
                     x.Name.ToLower().Contains(vehicleParams.Search.ToLower()) &&
                     string.IsNullOrEmpty(vehicleParams.ProductName) &&
                     vehicleParams.ProductId == null &&
                     vehicleParams.IsDiscontinued == false)
                    ||
                    (!string.IsNullOrEmpty(vehicleParams.Search) &&
                     x.Name.ToLower().Contains(vehicleParams.Search.ToLower()) &&
                     string.IsNullOrEmpty(vehicleParams.ProductName) &&
                     (vehicleParams.ProductId != null && vehicleParams.ProductId == x.VehiclesParts.FirstOrDefault(c => c.ProductId == vehicleParams.ProductId)!.ProductId) &&
                     vehicleParams.IsDiscontinued == false)
                    ||
                    (string.IsNullOrEmpty(vehicleParams.Search) &&
                     !string.IsNullOrEmpty(vehicleParams.ProductName) &&
                     x.VehiclesParts.FirstOrDefault(c => c.Product.Name.Contains(vehicleParams.ProductName.ToLower()) == vehicleParams.ProductName.Contains(vehicleParams.ProductName)).Product.Name.ToLower().Contains(vehicleParams.ProductName) &&
                     vehicleParams.ProductId == null &&
                     vehicleParams.IsDiscontinued == false)
                    ||
                    (string.IsNullOrEmpty(vehicleParams.Search) &&
                     !string.IsNullOrEmpty(vehicleParams.ProductName) &&
                     x.VehiclesParts.FirstOrDefault(c => c.Product.Name.Contains(vehicleParams.ProductName.ToLower()) == vehicleParams.ProductName.Contains(vehicleParams.ProductName)).Product.Name.ToLower().Contains(vehicleParams.ProductName) &&
                     (vehicleParams.ProductId != null && vehicleParams.ProductId == x.VehiclesParts.FirstOrDefault(c => c.ProductId == vehicleParams.ProductId)!.ProductId) &&
                     vehicleParams.IsDiscontinued == false)
                    ||
                    (!string.IsNullOrEmpty(vehicleParams.Search) &&
                     x.Name.ToLower().Contains(vehicleParams.Search.ToLower()) &&
                     !string.IsNullOrEmpty(vehicleParams.ProductName) &&
                     x.VehiclesParts.FirstOrDefault(c => c.Product.Name.Contains(vehicleParams.ProductName.ToLower()) == vehicleParams.ProductName.Contains(vehicleParams.ProductName)).Product.Name.ToLower().Contains(vehicleParams.ProductName) &&
                     vehicleParams.ProductId == null &&
                     vehicleParams.IsDiscontinued == false)
                    ||
                    (!string.IsNullOrEmpty(vehicleParams.Search) &&
                     x.Name.ToLower().Contains(vehicleParams.Search.ToLower()) &&
                     !string.IsNullOrEmpty(vehicleParams.ProductName) &&
                     x.VehiclesParts.FirstOrDefault(c => c.Product.Name.Contains(vehicleParams.ProductName.ToLower()) == vehicleParams.ProductName.Contains(vehicleParams.ProductName)).Product.Name.ToLower().Contains(vehicleParams.ProductName) &&
                     (vehicleParams.ProductId != null && vehicleParams.ProductId == x.VehiclesParts.FirstOrDefault(c => c.ProductId == vehicleParams.ProductId)!.ProductId) &&
                     vehicleParams.IsDiscontinued == false)
                    ||
                    (string.IsNullOrEmpty(vehicleParams.Search) &&
                     string.IsNullOrEmpty(vehicleParams.ProductName) &&
                     vehicleParams.ProductId == null &&
                     vehicleParams.IsDiscontinued == true &&
                     (vehicleParams.IsDiscontinued == x.VehiclesParts.FirstOrDefault(c => c.Product.IsDiscontinued == vehicleParams.IsDiscontinued)!.Product.IsDiscontinued))
                    ||
                    (string.IsNullOrEmpty(vehicleParams.Search) &&
                     string.IsNullOrEmpty(vehicleParams.ProductName) &&
                     (vehicleParams.ProductId != null && vehicleParams.ProductId == x.VehiclesParts.FirstOrDefault(c => c.ProductId == vehicleParams.ProductId)!.ProductId) &&
                     vehicleParams.IsDiscontinued == true &&
                     (vehicleParams.IsDiscontinued == x.VehiclesParts.FirstOrDefault(c => c.Product.IsDiscontinued == vehicleParams.IsDiscontinued)!.Product.IsDiscontinued))
                    ||
                    (!string.IsNullOrEmpty(vehicleParams.Search) &&
                     x.Name.ToLower().Contains(vehicleParams.Search.ToLower()) &&
                     string.IsNullOrEmpty(vehicleParams.ProductName) &&
                     vehicleParams.ProductId == null &&
                     vehicleParams.IsDiscontinued == true &&
                     (vehicleParams.IsDiscontinued == x.VehiclesParts.FirstOrDefault(c => c.Product.IsDiscontinued == vehicleParams.IsDiscontinued)!.Product.IsDiscontinued))
                    ||
                    (!string.IsNullOrEmpty(vehicleParams.Search) &&
                     x.Name.ToLower().Contains(vehicleParams.Search.ToLower()) &&
                     string.IsNullOrEmpty(vehicleParams.ProductName) &&
                     (vehicleParams.ProductId != null && vehicleParams.ProductId == x.VehiclesParts.FirstOrDefault(c => c.ProductId == vehicleParams.ProductId)!.ProductId) &&
                     vehicleParams.IsDiscontinued == true &&
                     (vehicleParams.IsDiscontinued == x.VehiclesParts.FirstOrDefault(c => c.Product.IsDiscontinued == vehicleParams.IsDiscontinued)!.Product.IsDiscontinued))
                    ||
                    (string.IsNullOrEmpty(vehicleParams.Search) &&
                     !string.IsNullOrEmpty(vehicleParams.ProductName) &&
                     x.VehiclesParts.FirstOrDefault(c => c.Product.Name.Contains(vehicleParams.ProductName.ToLower()) == vehicleParams.ProductName.Contains(vehicleParams.ProductName)).Product.Name.ToLower().Contains(vehicleParams.ProductName) &&
                     vehicleParams.ProductId == null &&
                     vehicleParams.IsDiscontinued == true &&
                     (vehicleParams.IsDiscontinued == x.VehiclesParts.FirstOrDefault(c => c.Product.IsDiscontinued == vehicleParams.IsDiscontinued)!.Product.IsDiscontinued))
                    ||
                    (string.IsNullOrEmpty(vehicleParams.Search) &&
                     !string.IsNullOrEmpty(vehicleParams.ProductName) &&
                     x.VehiclesParts.FirstOrDefault(c => c.Product.Name.Contains(vehicleParams.ProductName.ToLower()) == vehicleParams.ProductName.Contains(vehicleParams.ProductName)).Product.Name.ToLower().Contains(vehicleParams.ProductName) &&
                     (vehicleParams.ProductId != null && vehicleParams.ProductId == x.VehiclesParts.FirstOrDefault(c => c.ProductId == vehicleParams.ProductId)!.ProductId) &&
                     vehicleParams.IsDiscontinued == true &&
                     (vehicleParams.IsDiscontinued == x.VehiclesParts.FirstOrDefault(c => c.Product.IsDiscontinued == vehicleParams.IsDiscontinued)!.Product.IsDiscontinued))
                    ||
                    (!string.IsNullOrEmpty(vehicleParams.Search) &&
                     x.Name.ToLower().Contains(vehicleParams.Search.ToLower()) &&
                     !string.IsNullOrEmpty(vehicleParams.ProductName) &&
                     x.VehiclesParts.FirstOrDefault(c => c.Product.Name.Contains(vehicleParams.ProductName.ToLower()) == vehicleParams.ProductName.Contains(vehicleParams.ProductName)).Product.Name.ToLower().Contains(vehicleParams.ProductName) &&
                     vehicleParams.ProductId == null &&
                     vehicleParams.IsDiscontinued == true &&
                     (vehicleParams.IsDiscontinued == x.VehiclesParts.FirstOrDefault(c => c.Product.IsDiscontinued == vehicleParams.IsDiscontinued)!.Product.IsDiscontinued))
                    ||
                    (!string.IsNullOrEmpty(vehicleParams.Search) &&
                     x.Name.ToLower().Contains(vehicleParams.Search.ToLower()) &&
                     !string.IsNullOrEmpty(vehicleParams.ProductName) &&
                     x.VehiclesParts.FirstOrDefault(c => c.Product.Name.Contains(vehicleParams.ProductName.ToLower()) == vehicleParams.ProductName.Contains(vehicleParams.ProductName)).Product.Name.ToLower().Contains(vehicleParams.ProductName) &&
                     (vehicleParams.ProductId != null && vehicleParams.ProductId == x.VehiclesParts.FirstOrDefault(c => c.ProductId == vehicleParams.ProductId)!.ProductId) &&
                     vehicleParams.IsDiscontinued == true &&
                     (vehicleParams.IsDiscontinued == x.VehiclesParts.FirstOrDefault(c => c.Product.IsDiscontinued == vehicleParams.IsDiscontinued)!.Product.IsDiscontinued))
                 )
            )
        {
            
        }
    }
}
