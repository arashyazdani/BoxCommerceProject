using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications.ProductSpecifications
{
    public class UpdateProductParams : CreateOrUpdateProductParams
    {
        [Required(ErrorMessage = "Product Id is required")]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
    }
}
