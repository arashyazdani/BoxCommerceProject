using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Specifications.CategorySpecifications
{
    public class GetObjectFromCategoryService
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public Category? CategoryResult { get; set; }
    }
}
