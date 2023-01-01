using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications.VehicleSpecifications
{
    public class GetVehiclesWithPartsSpecification : BaseSpecification<Vehicle>
    {
        public GetVehiclesWithPartsSpecification(GetVehicleSpecificationParams vehicleParams) : base(x =>
            (string.IsNullOrEmpty(vehicleParams.Search) || x.Name.ToLower().Contains(vehicleParams.Search)))
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
