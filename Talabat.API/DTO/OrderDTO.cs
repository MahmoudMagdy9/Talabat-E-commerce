using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.API.DTO
{
    public class OrderDTO
    {

        [Required]
        public string BuyerEmail { get; set; } = null!;

        [Required]

        public string BasketId { get; set; } = null!;
        [Required]

        public int DeliveryMethodId { get; set; }
        [Required]

        public AddressDTO ShipAddress { get; set; } = null!;

 


    }
}
