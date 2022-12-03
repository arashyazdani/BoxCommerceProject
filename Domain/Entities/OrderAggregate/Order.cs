using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.BaseEntities;

namespace Domain.Entities.OrderAggregate
{
    public class Order : BaseEntity
    {
        public Order()
        {

        }

        public Order(IReadOnlyList<OrderItem> orderItems, string buyerEmail, OrderAddress shipToAddress, DeliveryMethod deliveryMethod, decimal subtotal, string paymentIntentId, string vehicleName, int vehicleId, string pictureUrl)
        {
            BuyerEmail = buyerEmail;
            ShipToAddress = shipToAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems;
            Subtotal = subtotal;
            PaymentIntentId = paymentIntentId;
            VehicleId = vehicleId;
            PictureUrl = pictureUrl;
            VehicleName = vehicleName;
        }

        public string BuyerEmail { get; set; }
        public int VehicleId { get; set; }
        public string VehicleName { get; set; }
        public string PictureUrl { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public OrderAddress ShipToAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public IReadOnlyList<OrderItem> OrderItems { get; set; }
        public decimal Subtotal { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string PaymentIntentId { get; set; }

        public decimal GetTotal()
        {
            return Subtotal + DeliveryMethod.Price;
        }
    }
}
