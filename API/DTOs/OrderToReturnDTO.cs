using Domain.Entities.OrderAggregate;

namespace API.DTOs
{
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
