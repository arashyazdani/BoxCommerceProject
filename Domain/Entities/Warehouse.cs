using System.ComponentModel;
using Domain.Entities.BaseEntities;

namespace Domain.Entities
{
    public class Warehouse : BaseFields
    {
        public string? Address { get; set; }

        public virtual ICollection<VehiclesInventory> VehiclesInventories { get; set; }
        public virtual ICollection<ProductsInventory> ProductsInventories { get; set; }
    }
}
