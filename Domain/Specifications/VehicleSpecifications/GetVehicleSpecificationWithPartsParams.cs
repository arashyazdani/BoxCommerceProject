using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications.VehicleSpecifications
{
    public class GetVehicleSpecificationWithPartsParams : BaseGetSpecificationParams
    {
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }
        public bool IsDiscontinued { get; set; } = false;
    }
}
