using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications.CategorySpecifications
{
    public class UpdateCategoryParams : CreateOrUpdateCategoryParams
    {
        [Required(ErrorMessage = "Category Id is required")]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
    }
}
