using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Specifications
{
    public class GetVehiclesSpecification : BaseSpecification<Vehicle>
    {
        public GetVehiclesSpecification()
        {
        }

        public GetVehiclesSpecification(int id) : base(x => x.Id == id)
        {

        }

    }
}
