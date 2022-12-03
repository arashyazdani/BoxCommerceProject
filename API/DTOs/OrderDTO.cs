using System.ComponentModel;

namespace API.DTOs
{
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
