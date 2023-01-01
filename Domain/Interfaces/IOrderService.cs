using Domain.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethod, string basketId, OrderAddress shippingAddress, CancellationToken cancellationToken = default(CancellationToken));

        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail, CancellationToken cancellationToken = default(CancellationToken));

        Task<Order> GetOrderByIdAsync(int id, string buyerEmail, CancellationToken cancellationToken = default(CancellationToken));

        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
