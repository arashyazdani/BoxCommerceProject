using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications.VehicleSpecifications
{
    [DisplayName("Update VehiclesParts")]
    public class AddOrUpdateVehiclesPartsSpecificationParams
    {
        [Required(ErrorMessage = "Vehicle Id is required")]
        [Range(1, int.MaxValue)]
        public int VehicleId { get; set; }

        [Required(ErrorMessage = "Product Id is required")]
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

    }
}
