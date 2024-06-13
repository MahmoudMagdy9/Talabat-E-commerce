using System.ComponentModel.DataAnnotations;

namespace Talabat.API.DTO
{
    public class RegisterDTO
    {
        [Required] 
        public string DisplayName { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [Phone]
        public string Phone{ get; set; } = null!;

        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{8,15}$",
            ErrorMessage =
                "Password must have 1 uppercase, 1 lowercase, 1 number, 1 non alphanumeric and at least 8 characters")]
        public string Password { get; set; } = null!;

    }
}
