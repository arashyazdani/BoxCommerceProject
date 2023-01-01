namespace API.DTOs
{
    public class VehicleWithVehiclePartsToReturnDto : BaseToReturnDTO
    {
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public string? PictureUrl { get; set; }

        public IReadOnlyList<VehiclePartsDTO> VehicleParts { get; set; }
    }
}
