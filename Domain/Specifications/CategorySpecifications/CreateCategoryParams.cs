using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.Specifications.CategorySpecifications
{
    public class CreateCategoryParams : BaseCreateOrUpdateParams
    {
        [Display(Name = "Parent Id")]
        [Range(1, int.MaxValue)]
        public int? ParentCategoryId { get; set; }
    }
}
