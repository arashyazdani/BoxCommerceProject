using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications
{
    public class GetObjectFromServicesSpecification
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public dynamic? ResultObject{ get; set; }
    }
}
