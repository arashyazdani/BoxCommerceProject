using System.ComponentModel;

namespace API.DTOs
{
    // I used DisplayName because I want to display a correct and meaningful name in Swagger Schemas
    [DisplayName("Warehouse")]
    public class WarehouseToReturnDto : BaseToReturnDTO
    {
        public string? Address { get; set; }
    }
}
