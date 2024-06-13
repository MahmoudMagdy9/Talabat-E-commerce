using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specification
{
    public class OrderWithPaymentIntentSpecifications : BaseSpecification<Order>
    {
        public OrderWithPaymentIntentSpecifications(string paymentIntentId) : base(o => o.PaymentIntentId == paymentIntentId)
        {
            Includes.Add(o => o.OrderItems);
            Includes.Add(o => o.DeliveryMethod);

        }
    }
  
}
