using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MixMeal.Models
{
    public class RegisterViewModel
    {
        [DisplayName("First Name")]
        public string Firstname { get; set; } = null!;

        [DisplayName("Last Name")]
        public string Lastname { get; set; } = null!;
        [DisplayName("Image Path")]
        public string? Imagepath { get; set; }
        
        [DisplayName("Email")]
        public string Email { get; set; } = null!;
       
        public string Phone { get; set; } = null!;

        [DisplayName("Username")]
        public string Username { get; set; } = null!;

        [DisplayName("Password")]
        public string Upassword { get; set; } = null!;
        public DateTime? Bithdate { get; set; }

    }
}
