using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities;

namespace Talabat.API.DTO
{
    public class CustomerBasetDTO
    {
        [Required]
        public string Id { get; set; }
        [Required]

        public List<BasketItem> Items { get; set; }
    }
}
