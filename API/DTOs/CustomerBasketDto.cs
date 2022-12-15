using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    // I used DisplayName because I want to display a correct and meaningful name in Swagger Schemas
    [DisplayName("Customer Basket")]
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; }

        public int VehicleId { get; set; }
        public string? VehicleName { get; set; }
        public decimal VehiclePrice { get; set; }
        public string? PictureUrl { get; set; }
        public List<BasketItemDto> Items { get; set; } = new List<BasketItemDto>();
        public int? DeliveryMethodId { get; set; }
        public string? ClientSecret { get; set; }
        public string? PaymentIntentId { get; set; }
        public decimal ShippingPrice { get; set; }
        
    }
}
