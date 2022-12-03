using System.ComponentModel;

namespace Domain.Entities
{
    [DisplayName("Vehicle Parts Table")]
    public class VehiclesPart
    {
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public Vehicle Vehicle { get; set; }
        public int VehicleId { get; set; }
    }
}
