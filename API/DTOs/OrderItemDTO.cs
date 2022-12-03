using System.ComponentModel;

namespace API.DTOs
{
    [DisplayName("Order Item")]
    public class OrderItemDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
    }
}
