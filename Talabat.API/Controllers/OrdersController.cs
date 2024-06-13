using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using Talabat.API.DTO;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services.Contract;
using Order = Talabat.Core.Entities.Order_Aggregate.Order;

namespace Talabat.API.Controllers
{
    [Authorize]
    public class OrdersController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [ProducesResponseType(typeof(OrderToReturnDTO), StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDTO>> CreateOrder(OrderDTO orderDto)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var address = _mapper.Map<AddressDTO, Address>(orderDto.ShipAddress);
            var order = await _orderService.CreateOrderAsync(buyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, address);

            return order is null ? BadRequest() : Ok(_mapper.Map<Order , OrderToReturnDTO>(order));
        }

        [ProducesResponseType(typeof(OrderToReturnDTO), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDTO>>> GetOrdersForUser()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);


            var orders = await _orderService.GetOrdersForUserAsync(buyerEmail);

            return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDTO>>(orders));
        }

        [ProducesResponseType(typeof(OrderToReturnDTO), StatusCodes.Status200OK)]
        [HttpGet("{id}")] //api/orders/{id}?buyerEmail={buyerEmail}
        public async Task<ActionResult<OrderToReturnDTO>> GetOrdersForUser(int id)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var order = await _orderService.GetOrderByIdForUserAsync(id, buyerEmail);
            return order is null ? NotFound() : Ok(_mapper.Map<OrderToReturnDTO>(order) );
        }


        [ProducesResponseType(typeof(OrderToReturnDTO), StatusCodes.Status200OK)]
        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var deliveryMethods = await _orderService.GetDeliveryMethodsAsync();
            return Ok(deliveryMethods);
        }

    }
}
