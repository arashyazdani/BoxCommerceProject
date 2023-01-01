using API.DTOs;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Domain.Entities.OrderAggregate;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/order")]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDTO orderDto, CancellationToken cancellationToken = default(CancellationToken))
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();

            var address = _mapper.Map<AddressDTO, OrderAddress>(orderDto.ShipToAddress);

            var order = await _orderService.CreateOrderAsync(email, orderDto.DeliveryMethodId, orderDto.BasketId, address, cancellationToken);

            if (order == null) return BadRequest(new ApiResponse(400, "Problem creating order"));

            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderDTO>>> GetOrdersForUser(CancellationToken cancellationToken = default(CancellationToken))
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();

            var orders = await _orderService.GetOrdersForUserAsync(email, cancellationToken);

            return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDTO>>(orders));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDTO>> GetOrderByIdForUser(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();

            var order = await _orderService.GetOrderByIdAsync(id, email, cancellationToken);

            if (order == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Order, OrderToReturnDTO>(order);
        }

        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Ok(await _orderService.GetDeliveryMethodsAsync(cancellationToken));
        }
    }
}
