namespace API.DTOs
{
    public class VehiclePartsDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal? ProductPrice { get; set; }
        public bool ProductIsDiscontinued { get; set; } = false;

    }
}
