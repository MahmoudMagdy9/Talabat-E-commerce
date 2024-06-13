using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.DTO;
using Talabat.API.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;

namespace Talabat.API.Controllers
{

    public class BasketController : BaseController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository , IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            return Ok(basket ?? new CustomerBasket(id));
        }
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasetDTO basket)
        {
            var mappedCustomerBasket = _mapper.Map<CustomerBasket>(basket);
            var createdOrUpdateBasket = await _basketRepository.UpdateAsync(mappedCustomerBasket);
            if (createdOrUpdateBasket is null) return BadRequest(new ApiResponse(400));

            return Ok(createdOrUpdateBasket);
        }

        [HttpDelete]
        public async Task DeleteBasket(string id) => await _basketRepository.DeleteAsync(id);

    }
}
