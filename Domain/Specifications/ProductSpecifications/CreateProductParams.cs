using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.Specifications.ProductSpecifications
{
    public class CreateProductParams : BaseCreateOrUpdateParams
    {
        [Display(Name = "Category Id")]
        [Range(1, int.MaxValue)]
        [Required(ErrorMessage = "CategoryId is required")]
        public int? CategoryId { get; set; }

        [Display(Name = "Price")]
        [DisplayFormat(DataFormatString = "{0:0,0}")]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Display(Name = "Quantity")]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Display(Name = "IsDiscontinued")]
        [Required(ErrorMessage = "IsDiscontinued is required")]
        public bool IsDiscontinued { get; set; } = false;
    }
}
