using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications.VehicleSpecifications
{
    public class UpdateVehicleParams : BaseCreateOrUpdateParams
    {
        [Required(ErrorMessage = "Vehicle Id is required")]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Display(Name = "Price")]
        [DisplayFormat(DataFormatString = "{0:0,0}")]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Display(Name = "Quantity")]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Display(Name = "PictureUrl")]
        [RegularExpression(@"^([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*){0,500}$", ErrorMessage = "Invalid PictureUrl.")]
        public string? PictureUrl { get; set; }
    }
}
