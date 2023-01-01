using Domain.Entities.BaseEntities;
using System.ComponentModel;

namespace Domain.Entities
{
    public class ProductsInventory : Auditable
    {
        // I used GUID for Id because we need a uniq number and unpredictable for using to each product.
        public Guid Id { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public int WarehouseId { get; set; }
        public virtual Product Product { get; set; }
        public int ProductId { get; set; }
        public DateTimeOffset? OrderDate { get; set; }
        public DateTimeOffset? ProductionDate { get; set;}
        public DateTimeOffset? WarehouseReceiptDate { get; set; }
        public DateTimeOffset? WarehouseRemittanceDate { get; set; }
    }
}
