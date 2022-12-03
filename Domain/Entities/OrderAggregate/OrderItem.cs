﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.BaseEntities;

namespace Domain.Entities.OrderAggregate
{
    [DisplayName("Order Item Table")]
    public class OrderItem : BaseEntity
    {
        public OrderItem()
        {
        }

        public OrderItem(ProductItemOrdered itemOrdered, decimal price)
        {
            ItemOrdered = itemOrdered;
            Price = price;
        }

        public ProductItemOrdered ItemOrdered { get; set; }
        public decimal Price { get; set; }
    }
}