using Domain.Entities.OrderAggregate;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IPaymentService
    {
        Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId, CancellationToken cancellationToken = default(CancellationToken));

        Task<Order> UpdateOrderPaymentSucceeded(string paymentIntentId, CancellationToken cancellationToken = default(CancellationToken));

        Task<Order> UpdateOrderPaymentFailed(string paymentIntentId, CancellationToken cancellationToken = default(CancellationToken));
    }
}
