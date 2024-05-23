using System.ComponentModel.DataAnnotations;

namespace MixMeal.Models
{
    public class EditAccount
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Upassword { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Compare("Upassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = null!;

    }
}
