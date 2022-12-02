using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.DTOs
{
    [DisplayName("Basket Item")]
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        [Range(0.10, double.MaxValue, ErrorMessage = "Price must be greater then zero")]
        public decimal Price { get; set; }

        [Required]
        public string CategoryName { get; set; }
    }
}
