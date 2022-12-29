using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.Specifications.VehicleSpecifications
{
    public class CreateVehicleParams : BaseCreateOrUpdateParams
    {
        [Display(Name = "PictureUrl")]
        [RegularExpression(@"^([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*){0,500}$", ErrorMessage = "Invalid PictureUrl.")]
        public string? PictureUrl { get; set; }
    }
}
