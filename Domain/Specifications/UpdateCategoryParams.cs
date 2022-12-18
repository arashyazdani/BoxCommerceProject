using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications
{
    public class UpdateCategoryParams : CreateOrUpdateCategoryParams
    {
        [Required(ErrorMessage = "Category Id is required")]
        public int Id { get; set; }
    }
}
