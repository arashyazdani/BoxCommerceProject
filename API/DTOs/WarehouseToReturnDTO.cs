using System.ComponentModel;

namespace API.DTOs
{
    [DisplayName("Warehouse")]
    public class WarehouseToReturnDto
    {
        public int Id { get; set; }
        public int? Priority { get; set; }
        public string Name { get; set; }
        public string? Details { get; set; }
        public string? Address { get; set; }
        public bool Enabled { get; set; } = true;
    }
}
