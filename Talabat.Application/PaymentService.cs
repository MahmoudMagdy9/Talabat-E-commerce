using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Stripe;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Application
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration, IBasketRepository basketRepository, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

            var basket = await _basketRepository.GetBasketAsync(basketId);

            if (basket == null) return null;

            var shippingPrice = 0m;
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                basket.ShippingPrice = deliveryMethod.Cost;
                shippingPrice = deliveryMethod.Cost;


            }

            if (basket.Items.Count > 0)
            {

                foreach (var item in basket.Items)
                {

                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

                    if (item.Price != product.Price)
                        item.Price = product.Price;

                    PaymentIntentService paymentIntentService = new PaymentIntentService();

                    PaymentIntent paymentIntent;

                    if (basket.PaymentIntentId == null)
                    {
                        var options = new PaymentIntentCreateOptions
                        {
                            Amount = (long)basket.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long)shippingPrice * 100,
                            Currency = "usd",
                            PaymentMethodTypes = new List<string> { "card" }
                        };

                        paymentIntent = await paymentIntentService.CreateAsync(options);
                        basket.PaymentIntentId = paymentIntent.Id;
                        basket.ClientSecret = paymentIntent.ClientSecret;
                    }
                    else
                    {
                        var updateOptions = new PaymentIntentUpdateOptions
                        {
                            Amount = (long)basket.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long)shippingPrice * 100
                        };

                        paymentIntentService.UpdateAsync(basket.PaymentIntentId, updateOptions);
                    }

                }

            }



            await _basketRepository.UpdateAsync(basket);

            return basket;


        }
    }
}
