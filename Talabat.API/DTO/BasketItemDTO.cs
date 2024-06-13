using System.ComponentModel.DataAnnotations;

namespace Talabat.API.DTO
{
    public class BasketItemDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]

        public string ProductName { get; set; }
        [Required]

        public string PictureUrl { get; set; }
        [Required]
        [Range(0, double.MaxValue , ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }
        [Required]

        public string Category { get; set; }
       
        [Required]
        public string Brand { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
        public int Quantity { get; set; }
    }
}
