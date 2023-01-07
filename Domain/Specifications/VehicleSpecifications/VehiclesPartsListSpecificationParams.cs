using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications.VehicleSpecifications
{
    public class VehiclesPartsListSpecificationParams
    {
        [Required(ErrorMessage = "Vehicle Id is required")]
        [Range(1, int.MaxValue)]
        public int ChassisId { get; set; }
        [Required(ErrorMessage = "Engine Id is required")]
        [Range(1, int.MaxValue)]
        public int EngineId { get; set; }
        [Required(ErrorMessage = "OptionsPack Id is required")]
        [Range(1, int.MaxValue)]
        public int OptionsPackId { get; set; }
    }
}
