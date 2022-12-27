using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.BaseEntities;

namespace Domain.Entities.OrderAggregate
{
    [DisplayName("Order Item Table")]
    public class OrderItem : Auditable
    {
        public OrderItem()
        {
        }

        public OrderItem(ProductItemOrdered itemOrdered, decimal price)
        {
            ItemOrdered = itemOrdered;
            Price = price;
        }

        public int Id { get; set; }
        public ProductItemOrdered ItemOrdered { get; set; }
        public decimal Price { get; set; }
    }
}
