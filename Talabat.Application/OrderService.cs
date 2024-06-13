using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specification;
using Talabat.Core.Specification.Order_Specs;

namespace Talabat.Application
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IPaymentService _paymentService;
        //private readonly IGenericRepository<Product> _productRepository;
        //private readonly IGenericRepository<DeliveryMethod> _deliveryMethodsRepository;
        //private readonly IGenericRepository<Order> _ordersRepository;

        public OrderService(IBasketRepository basketRepository,
            IUnitOfWork unitOfWork,
            IPaymentService paymentService)

        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            // 1.Get Basket From Baskets Repo
            var basket = await _basketRepository.GetBasketAsync(basketId);

            // 2. Get Selected Items at Basket From Products Repo

            var orderItems = new List<OrderItem>();

            if (basket?.Items.Count > 0)
            {
                var productRepository = _unitOfWork.Repository<Product>();

                foreach (var item in basket.Items)
                {
                    var productItem = await productRepository.GetByIdAsync(item.Id);

                    var productItemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);

                    var orderItem = new OrderItem(productItemOrdered, productItem.Price, item.Quantity);

                    orderItems.Add(orderItem);
                }
            }

            // 3. Calculate SubTotal

            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);


            // 4. Get Delivery Method From DeliveryMethods Repo
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            var orderRepo = _unitOfWork.Repository<Order>();
            var orderSpecs = new OrderWithPaymentIntentSpecifications(basketId);
            var existingOrder = await orderRepo.GetEntityWithSpecAsync(orderSpecs);

            if (existingOrder != null)
            {
                orderRepo.Delete(existingOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            }

            // 5. Create Order
            var order = new Order(orderItems, buyerEmail, shippingAddress, deliveryMethod, subTotal ,basket.PaymentIntentId);
            await _unitOfWork.Repository<Order>().AddAsync(order);


            // 6. Save To Database  

            var result = await _unitOfWork.CompleteAsync();

            return result <= 0 ? null : order;
        }

        public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var OrderSpec = new OrderSpecifications(buyerEmail);
            var Orders = _unitOfWork.Repository<Order>().GetAllWithSpecAsync(OrderSpec);
            return Orders;
        }

        public Task<Order?> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
        {
            var orderRepo = _unitOfWork.Repository<Order>();
            var orderSpec = new OrderSpecifications(orderId, buyerEmail);
            var order = orderRepo.GetEntityWithSpecAsync(orderSpec);

            return order;

        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync() 
            => await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();


    }
}

