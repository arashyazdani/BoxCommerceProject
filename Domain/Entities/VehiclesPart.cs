using Domain.Entities.BaseEntities;
using System.ComponentModel;

namespace Domain.Entities
{
    public class VehiclesPart : Auditable
    {
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public Vehicle Vehicle { get; set; }
        public int VehicleId { get; set; }
    }
}
