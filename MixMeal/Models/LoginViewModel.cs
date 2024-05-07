using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MixMeal.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [PasswordPropertyText]
        public string Upassword { get; set; } = null!;
    }
}
