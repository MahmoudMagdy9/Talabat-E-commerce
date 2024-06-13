using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specification.Order_Specs
{
    public class OrderSpecifications : BaseSpecification<Order>
    {

        public OrderSpecifications(string buyerEmail) : base(o => o.BuyerEmail == buyerEmail)
        {
            Includes.Add(o => o.OrderItems);
            Includes.Add(o => o.DeliveryMethod);
            AddOrderByDes(o => o.OrderDate);
        }

        public OrderSpecifications(int id , string buyerEmail) : base(o => o.Id == id && o.BuyerEmail == buyerEmail)
        {
            Includes.Add(o => o.OrderItems);
            Includes.Add(o => o.DeliveryMethod);
         }

     
    }
}
