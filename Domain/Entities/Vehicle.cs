using System.ComponentModel;
using Domain.Entities.BaseEntities;

namespace Domain.Entities
{
    public class Vehicle : BaseFields
    {
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? PictureUrl { get; set; }

        public virtual ICollection<VehiclesPart> VehiclesParts { get; set; }
        public virtual ICollection<VehiclesInventory> VehiclesInventories { get; set; }
    }
}
