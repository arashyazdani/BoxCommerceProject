using System.ComponentModel;
using Domain.Entities;

namespace API.DTOs
{
    [DisplayName("Return Category")]
    public class CategoryToReturnDto
    {
        public int Id { get; set; }
        public int? Priority { get; set; }
        public string Name { get; set; }
        public string? Details { get; set; }
        public bool Enabled { get; set; } = true;
        public int? ParentCategoryId { get; set; }
        public string ParentName { get; set; }
    }
}
