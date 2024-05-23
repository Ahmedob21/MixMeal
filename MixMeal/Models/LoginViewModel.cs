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
        [StringLength(40)]
        //[Range(8,40)]
        [DataType(DataType.Password)]
        public string Upassword { get; set; } = null!;
    }
}
