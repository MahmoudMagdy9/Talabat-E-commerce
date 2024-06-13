using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Services.Contract;

namespace Talabat.API.Controllers
{
    [Authorize]
    public class PaymentController : BaseController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [ProducesResponseType(typeof(CustomerBasket) , StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse) , StatusCodes.Status400BadRequest)]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
           var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);

           if (basket == null) return BadRequest(new ApiResponse(400, "Problem with your basket"));

           return Ok(basket);

        }

      

    }
}
