using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.OrderAggregate
{
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
