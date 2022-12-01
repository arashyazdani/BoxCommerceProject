﻿namespace API.DTOs
{
    public class VehicleToReturnDTO
    {
        public int Id { get; set; }
        public int? Priority { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? Details { get; set; }
        public string? PictureUrl { get; set; }
        public bool Enabled { get; set; } = true;
    }
}