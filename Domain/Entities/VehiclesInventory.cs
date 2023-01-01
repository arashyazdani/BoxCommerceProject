using Domain.Entities.BaseEntities;
using System.ComponentModel;

namespace Domain.Entities
{
    public class VehiclesInventory : Auditable
    {
        // I used GUID for Id because we need a uniq number and unpredictable for using to each vehicle.
        public Guid Id { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public int WarehouseId { get; set; }
        public virtual Vehicle Vehicle { get; set; }
        public int VehicleId { get; set; }
        public Guid ChassisSerialNumber { get; set; }
        public Guid EngineSerialNumber { get; set; }
        public Guid OptionsPackSerialNumber { get; set; }
        public DateTimeOffset? OrderDate { get; set; }
        public DateTimeOffset? ProductionDate { get; set; }
        public DateTimeOffset? WarehouseReceiptDate { get; set; }
        public DateTimeOffset? WarehouseRemittanceDate { get; set; }
    }
}
