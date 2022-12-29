using System.ComponentModel;

namespace API.DTOs
{
    // I used DisplayName because I want to display a correct and meaningful name in Swagger Schemas
    [DisplayName("Return Vehicle")]
    public class VehicleToReturnDto : BaseToReturnDTO
    {
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? PictureUrl { get; set; }
    }
}
