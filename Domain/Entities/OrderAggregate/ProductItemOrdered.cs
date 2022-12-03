using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.OrderAggregate
{
    [DisplayName("Product Item Ordered Table")]
    public class ProductItemOrdered
    {
        public ProductItemOrdered()
        {
        }

        public ProductItemOrdered(int productItemId, string productName)
        {
            ProductItemId = productItemId;
            ProductName = productName;
        }

        public int ProductItemId { get; set; }
        public string ProductName { get; set; }
    }
}
