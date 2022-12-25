using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.Specifications.CategorySpecifications
{
    public abstract class CreateOrUpdateCategoryParams
    {
        [Display(Name = "Priority")]
        [Range(1, int.MaxValue, ErrorMessage = "Priority must be between 1 to 999,999,999")]
        public int? Priority { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required")]
        [RegularExpression(@"^[\u0061-\u007a\u0041-\u005a\u0030-\u0039\u0600-\u06ff\u0750-\u077f\ufb50-\ufdff\u08a0\u2014\u08ff\ufe70\u002d\ufefc\u200b\u200c\s]{0,50}$", ErrorMessage = "Invalid name. Name must be less than 50 character.")]
        public string Name { get; set; }

        [Display(Name = "Details")]
        [RegularExpression(@"^[\u0061-\u007a\u0041-\u005a\u0030-\u0039\u0600-\u06ff\u0750-\u077f\ufb50-\ufdff\u08a0\u2014\u08ff\ufe70\u002d\ufefc\u200b\u200c\s]{0,500}$", ErrorMessage = "Invalid Details. Name must be less than 500 character.")]
        public string? Details { get; set; }


        [Display(Name = "Enabled")]
        [Required(ErrorMessage = "Enabled is required")]
        public bool Enabled { get; set; } = true;

        [Display(Name = "Parent Id")]
        [Range(1, int.MaxValue)]
        public int? ParentCategoryId { get; set; }
    }
}
