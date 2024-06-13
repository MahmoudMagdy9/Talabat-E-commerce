using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
    public class Order : BaseEntity
    {
        private readonly string _paymentIntentId;

        public Order()
        {
        }

        public Order(ICollection<OrderItem> orderItems, string buyerEmail, Address shippingAddress,
            DeliveryMethod deliveryMethod, decimal subtotal , string paymentIntentId)
        {
            _paymentIntentId = paymentIntentId;
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems;
            Subtotal = subtotal;
        }
        public string BuyerEmail { get; set; } = null!;

        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public Address ShippingAddress { get; set; } = null!;


        //public int DeliveryMethodId { get; set; } // we can use this, but we will use the below one to set the delivery method

        //navigation property [one] 
        public DeliveryMethod DeliveryMethod { get; set; } = null!;


        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
        public decimal Subtotal { get; set; } // the total price of all items in the order
        public decimal Delivery { get; set; }

        //[NotMapped]
        //public decimal Total => Subtotal + DeliveryMethod.Cost;// the total price of all items in the order + delivery cost
        //or
        public decimal GetTotal() => Subtotal + DeliveryMethod.Cost;


        public string PaymentIntentId { get; set; } = null!;
 




    }
}
