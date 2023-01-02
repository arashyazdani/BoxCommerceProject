using Domain.Entities;

namespace Domain.Specifications.VehicleSpecifications
{
    public class GetVehiclesWithPartsSpecification : BaseSpecification<Vehicle>
    {
        public GetVehiclesWithPartsSpecification(GetVehicleSpecificationWithPartsParams vehicleParams) :
            base(x =>
                (
                    (string.IsNullOrEmpty(vehicleParams.Search) && 
                     string.IsNullOrEmpty(vehicleParams.ProductName) && 
                     vehicleParams.ProductId==null && 
                     vehicleParams.IsDiscontinued==false) 
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
            AddInclude($"{nameof(Vehicle.VehiclesParts)}");
            AddInclude($"{nameof(Vehicle.VehiclesParts)}.{nameof(VehiclesPart.Product)}");
            ApplyPaging(vehicleParams.PageSize * (vehicleParams.PageIndex - 1), vehicleParams.PageSize);

            if (!string.IsNullOrEmpty(vehicleParams.Sort))
            {
                switch (vehicleParams.Sort)
                {
                    case "ascname":
                        AddOrderBy(p => p.Name);
                        break;

                    case "descname":
                        AddOrderByDescending(p => p.Name);
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
        

        public GetVehiclesWithPartsSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude($"{nameof(Vehicle.VehiclesParts)}");
            AddInclude($"{nameof(Vehicle.VehiclesParts)}.{nameof(VehiclesPart.Product)}");
        }
    }
}
