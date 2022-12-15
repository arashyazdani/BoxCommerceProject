using System.ComponentModel;
using Domain.Entities.OrderAggregate;

namespace API.DTOs
{
    // I used DisplayName because I want to display a correct and meaningful name in Swagger Schemas
    [DisplayName("Order Return")]
    public class OrderToReturnDTO
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string VehicleName { get; set; }
        public string? PictureUrl { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public OrderAddress ShipToAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal ShippingPrice { get; set; }
        public IReadOnlyList<OrderItemDTO> OrderItems { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
    }
}
