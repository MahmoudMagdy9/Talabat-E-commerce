using System.Runtime.Serialization;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.API.DTO
{
    public class OrderToReturnDTO
    {

        public int Id { get; set; }

        public string BuyerEmail { get; set; } = null!;

        public DateTimeOffset OrderDate { get; set; }

        public string Status { get; set; }  

        public Address ShippingAddress { get; set; } = null!;
 
        public string DeliveryMethod { get; set; } = null!;

        public string DeliveryMethodCost { get; set; } = null!;

        public ICollection<OrderItemDTO> OrderItems { get; set; } = new HashSet<OrderItemDTO>();

        public decimal Subtotal { get; set; }  
 
        public decimal Total { get; set; }


        public string PaymentIntentId { get; set; } = null!;


    }

    
}
