using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.Specifications.WarehouseSpecifications
{
    public class UpdateWarehouseParams : BaseCreateOrUpdateParams
    {
        [Required(ErrorMessage = "Warehouse Id is required")]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Display(Name = "Address")]
        [RegularExpression(@"^[\u0061-\u007a\u0041-\u005a\u0030-\u0039\u0600-\u06ff\u0750-\u077f\ufb50-\ufdff\u08a0\u2014\u08ff\ufe70\u002d\ufefc\u200b\u200c\s]{0,300}$", ErrorMessage = "Invalid Address. Name must be less than 300 character.")]
        public string? Address { get; set; }
    }
}
