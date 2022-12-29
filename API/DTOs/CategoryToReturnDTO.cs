using System.ComponentModel;
using Domain.Entities;

namespace API.DTOs
{
    // I used DisplayName because I want to display a correct and meaningful name in Swagger Schemas
    [DisplayName("Return Category")]
    public class CategoryToReturnDto : BaseToReturnDTO
    {
        public int? ParentCategoryId { get; set; }
        public string ParentName { get; set; }
    }
}
