using Domain.Entities.BaseEntities;

namespace Domain.Entities
{
    public class Vehicle : BaseFields
    {
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? PictureUrl { get; set; }
    }
}
