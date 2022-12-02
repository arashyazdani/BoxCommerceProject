using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.DTOs
{
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
