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
                    // Get Vehicles and VehicleParts without any conditions
                    (string.IsNullOrEmpty(vehicleParams.Search) &&
                     string.IsNullOrEmpty(vehicleParams.ProductName) &&
                     vehicleParams.ProductId == null &&
                     vehicleParams.IsDiscontinued == false)
                    ||
                    // Get Vehicles and VehicleParts with ProductId
                    (string.IsNullOrEmpty(vehicleParams.Search) &&
                     string.IsNullOrEmpty(vehicleParams.ProductName) &&
                     (vehicleParams.ProductId != null && vehicleParams.ProductId == x.VehiclesParts.FirstOrDefault(c => c.ProductId == vehicleParams.ProductId)!.ProductId) &&
                     vehicleParams.IsDiscontinued == false)
                    ||
                    // Get Vehicles and VehicleParts with Vehicle Name
                    (!string.IsNullOrEmpty(vehicleParams.Search) &&
                     x.Name.ToLower().Contains(vehicleParams.Search.ToLower()) &&
                     string.IsNullOrEmpty(vehicleParams.ProductName) &&
                     vehicleParams.ProductId == null &&
                     vehicleParams.IsDiscontinued == false)
                    ||
                    // Get Vehicles and VehicleParts with ProductId and Vehicle Name
                    (!string.IsNullOrEmpty(vehicleParams.Search) &&
                     x.Name.ToLower().Contains(vehicleParams.Search.ToLower()) &&
                     string.IsNullOrEmpty(vehicleParams.ProductName) &&
                     (vehicleParams.ProductId != null && vehicleParams.ProductId == x.VehiclesParts.FirstOrDefault(c => c.ProductId == vehicleParams.ProductId)!.ProductId) &&
                     vehicleParams.IsDiscontinued == false)
                    ||
                    // Get Vehicles and VehicleParts with Product Name
                    (string.IsNullOrEmpty(vehicleParams.Search) &&
                     !string.IsNullOrEmpty(vehicleParams.ProductName) &&
                     x.VehiclesParts.FirstOrDefault(c => c.Product.Name.Contains(vehicleParams.ProductName.ToLower()) == vehicleParams.ProductName.Contains(vehicleParams.ProductName)).Product.Name.ToLower().Contains(vehicleParams.ProductName) &&
                     vehicleParams.ProductId == null &&
                     vehicleParams.IsDiscontinued == false)
                    ||
                    // Get Vehicles and VehicleParts with ProductId and Product Name
                    (string.IsNullOrEmpty(vehicleParams.Search) &&
                     !string.IsNullOrEmpty(vehicleParams.ProductName) &&
                     x.VehiclesParts.FirstOrDefault(c => c.Product.Name.Contains(vehicleParams.ProductName.ToLower()) == vehicleParams.ProductName.Contains(vehicleParams.ProductName)).Product.Name.ToLower().Contains(vehicleParams.ProductName) &&
                     (vehicleParams.ProductId != null && vehicleParams.ProductId == x.VehiclesParts.FirstOrDefault(c => c.ProductId == vehicleParams.ProductId)!.ProductId) &&
                     vehicleParams.IsDiscontinued == false)
                    ||
                    // Get Vehicles and VehicleParts with Product Name and Vehicle Name
                    (!string.IsNullOrEmpty(vehicleParams.Search) &&
                     x.Name.ToLower().Contains(vehicleParams.Search.ToLower()) &&
                     !string.IsNullOrEmpty(vehicleParams.ProductName) &&
                     x.VehiclesParts.FirstOrDefault(c => c.Product.Name.Contains(vehicleParams.ProductName.ToLower()) == vehicleParams.ProductName.Contains(vehicleParams.ProductName)).Product.Name.ToLower().Contains(vehicleParams.ProductName) &&
                     vehicleParams.ProductId == null &&
                     vehicleParams.IsDiscontinued == false)
                    ||
                    // Get Vehicles and VehicleParts with ProductId and Product Name
                    (!string.IsNullOrEmpty(vehicleParams.Search) &&
                     x.Name.ToLower().Contains(vehicleParams.Search.ToLower()) &&
                     !string.IsNullOrEmpty(vehicleParams.ProductName) &&
                     x.VehiclesParts.FirstOrDefault(c => c.Product.Name.Contains(vehicleParams.ProductName.ToLower()) == vehicleParams.ProductName.Contains(vehicleParams.ProductName)).Product.Name.ToLower().Contains(vehicleParams.ProductName) &&
                     (vehicleParams.ProductId != null && vehicleParams.ProductId == x.VehiclesParts.FirstOrDefault(c => c.ProductId == vehicleParams.ProductId)!.ProductId) &&
                     vehicleParams.IsDiscontinued == false)
                    ||
                    // Get Vehicles and VehicleParts with Product IsDiscontinued true
                    (string.IsNullOrEmpty(vehicleParams.Search) &&
                     string.IsNullOrEmpty(vehicleParams.ProductName) &&
                     vehicleParams.ProductId == null &&
                     vehicleParams.IsDiscontinued == true &&
                     (vehicleParams.IsDiscontinued == x.VehiclesParts.FirstOrDefault(c => c.Product.IsDiscontinued == vehicleParams.IsDiscontinued)!.Product.IsDiscontinued))
                    ||
                    // Get Vehicles and VehicleParts with Product IsDiscontinued true and Product Name
                    (string.IsNullOrEmpty(vehicleParams.Search) &&
                     string.IsNullOrEmpty(vehicleParams.ProductName) &&
                     (vehicleParams.ProductId != null && vehicleParams.ProductId == x.VehiclesParts.FirstOrDefault(c => c.ProductId == vehicleParams.ProductId)!.ProductId) &&
                     vehicleParams.IsDiscontinued == true &&
                     (vehicleParams.IsDiscontinued == x.VehiclesParts.FirstOrDefault(c => c.Product.IsDiscontinued == vehicleParams.IsDiscontinued)!.Product.IsDiscontinued))
                    ||
                    // Get Vehicles and VehicleParts with Product IsDiscontinued true and Vehicle Name
                    (!string.IsNullOrEmpty(vehicleParams.Search) &&
                     x.Name.ToLower().Contains(vehicleParams.Search.ToLower()) &&
                     string.IsNullOrEmpty(vehicleParams.ProductName) &&
                     vehicleParams.ProductId == null &&
                     vehicleParams.IsDiscontinued == true &&
                     (vehicleParams.IsDiscontinued == x.VehiclesParts.FirstOrDefault(c => c.Product.IsDiscontinued == vehicleParams.IsDiscontinued)!.Product.IsDiscontinued))
                    ||
                    // Get Vehicles and VehicleParts with Product IsDiscontinued true and ProductId
                    (!string.IsNullOrEmpty(vehicleParams.Search) &&
                     x.Name.ToLower().Contains(vehicleParams.Search.ToLower()) &&
                     string.IsNullOrEmpty(vehicleParams.ProductName) &&
                     (vehicleParams.ProductId != null && vehicleParams.ProductId == x.VehiclesParts.FirstOrDefault(c => c.ProductId == vehicleParams.ProductId)!.ProductId) &&
                     vehicleParams.IsDiscontinued == true &&
                     (vehicleParams.IsDiscontinued == x.VehiclesParts.FirstOrDefault(c => c.Product.IsDiscontinued == vehicleParams.IsDiscontinued)!.Product.IsDiscontinued))
                    ||
                    // Get Vehicles and VehicleParts with Product IsDiscontinued true and Product Name and Vehicle Name
                    (string.IsNullOrEmpty(vehicleParams.Search) &&
                     !string.IsNullOrEmpty(vehicleParams.ProductName) &&
                     x.VehiclesParts.FirstOrDefault(c => c.Product.Name.Contains(vehicleParams.ProductName.ToLower()) == vehicleParams.ProductName.Contains(vehicleParams.ProductName)).Product.Name.ToLower().Contains(vehicleParams.ProductName) &&
                     vehicleParams.ProductId == null &&
                     vehicleParams.IsDiscontinued == true &&
                     (vehicleParams.IsDiscontinued == x.VehiclesParts.FirstOrDefault(c => c.Product.IsDiscontinued == vehicleParams.IsDiscontinued)!.Product.IsDiscontinued))
                    ||
                    // Get Vehicles and VehicleParts with Product IsDiscontinued true and ProductId
                    (string.IsNullOrEmpty(vehicleParams.Search) &&
                     !string.IsNullOrEmpty(vehicleParams.ProductName) &&
                     x.VehiclesParts.FirstOrDefault(c => c.Product.Name.Contains(vehicleParams.ProductName.ToLower()) == vehicleParams.ProductName.Contains(vehicleParams.ProductName)).Product.Name.ToLower().Contains(vehicleParams.ProductName) &&
                     (vehicleParams.ProductId != null && vehicleParams.ProductId == x.VehiclesParts.FirstOrDefault(c => c.ProductId == vehicleParams.ProductId)!.ProductId) &&
                     vehicleParams.IsDiscontinued == true &&
                     (vehicleParams.IsDiscontinued == x.VehiclesParts.FirstOrDefault(c => c.Product.IsDiscontinued == vehicleParams.IsDiscontinued)!.Product.IsDiscontinued))
                    ||
                    // Get Vehicles and VehicleParts with Product IsDiscontinued true and Product Name
                    (!string.IsNullOrEmpty(vehicleParams.Search) &&
                     x.Name.ToLower().Contains(vehicleParams.Search.ToLower()) &&
                     !string.IsNullOrEmpty(vehicleParams.ProductName) &&
                     x.VehiclesParts.FirstOrDefault(c => c.Product.Name.Contains(vehicleParams.ProductName.ToLower()) == vehicleParams.ProductName.Contains(vehicleParams.ProductName)).Product.Name.ToLower().Contains(vehicleParams.ProductName) &&
                     vehicleParams.ProductId == null &&
                     vehicleParams.IsDiscontinued == true &&
                     (vehicleParams.IsDiscontinued == x.VehiclesParts.FirstOrDefault(c => c.Product.IsDiscontinued == vehicleParams.IsDiscontinued)!.Product.IsDiscontinued))
                    ||
                    // Get Vehicles and VehicleParts with Product IsDiscontinued true and Product Name and Vehicle Name and ProductId
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
