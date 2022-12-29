using System.ComponentModel;

namespace API.DTOs
{
    // I used DisplayName because I want to display a correct and meaningful name in Swagger Schemas
    [DisplayName("Product Return")]
    public class ProductToReturnDto : BaseToReturnDTO

    {
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public bool IsDiscontinued { get; set; } = false;
    }
}
