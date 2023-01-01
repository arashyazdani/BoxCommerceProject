using System.ComponentModel;
using Domain.Entities.BaseEntities;

namespace Domain.Entities
{
    public class Product : BaseFields
    {
        public virtual Category Category { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool IsDiscontinued { get; set; }

        public virtual ICollection<VehiclesPart> VehiclesParts { get; set; }

        public virtual ICollection<ProductsInventory> ProductsInventories { get; set; }
    }
}
