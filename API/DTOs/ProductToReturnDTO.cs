namespace API.DTOs
{
    public class ProductToReturnDTO
    {
        public int Id { get; set; }
        public int? Priority { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int CategoryName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? Details { get; set; }
        public bool Enabled { get; set; } = true;
        public bool IsDiscontinued { get; set; } = false;
    }
}
