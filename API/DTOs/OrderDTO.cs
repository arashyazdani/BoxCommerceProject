using System.ComponentModel;

namespace API.DTOs
{
    // I used DisplayName because I want to display a correct and meaningful name in Swagger Schemas
    [DisplayName("Order")]
    public class OrderDTO
    {
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public AddressDTO ShipToAddress { get; set; }
        public int VehicleId { get; set; }
        public string VehicleName { get; set; }
        public string? PictureUrl { get; set; }
    }
}
