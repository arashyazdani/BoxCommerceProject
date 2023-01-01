using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.OrderAggregate;
using Domain.Interfaces;
using Domain.Specifications;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRepo, IUnitOfWork unitOfWork, IPaymentService paymentService)
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, OrderAddress shippingAddress, CancellationToken cancellationToken)
        {
            // get basket from the repo
            var basket = await _basketRepo.GetBasketAsync(basketId);
            // get items from the product repo
            var items = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id, cancellationToken);
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name);
                var orderItem = new OrderItem(itemOrdered, productItem.Price);
                items.Add(orderItem);
            }

            // get delivery method from repo
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId, cancellationToken);

            // get vehicle information
            var vehicle = await _unitOfWork.Repository<Vehicle>().GetByIdAsync(basket.VehicleId, cancellationToken);

            if (vehicle == null)
            {
                return null;
            }

            // calculate subtotal
            var subtotal = items.Sum(item => item.Price);

            // check to see if order exists
            var spec = new OrderByPaymentIntentIdSpecification(basket.PaymentIntentId);
            var existingOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec, cancellationToken);

            if (existingOrder != null)
            {
                _unitOfWork.Repository<Order>().Delete(existingOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basket.PaymentIntentId, cancellationToken);
            }

            // create order
            var order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subtotal, basket.PaymentIntentId, vehicle.Name, vehicle.Id, vehicle.PictureUrl);
            await _unitOfWork.Repository<Order>().InsertAsync(order, cancellationToken);

            // save to db
            var result = await _unitOfWork.Complete(cancellationToken);
            if (result <= 0) return null;

            // return order
            return order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail, CancellationToken cancellationToken)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(buyerEmail);
            return await _unitOfWork.Repository<Order>().ListWithSpecAsync(spec, cancellationToken);
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail, CancellationToken cancellationToken)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);
            return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec, cancellationToken);
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync(CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync(cancellationToken);
        }
    }
}
