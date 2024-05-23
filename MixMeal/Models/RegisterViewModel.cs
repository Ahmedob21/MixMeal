using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MixMeal.Models
{
    public class RegisterViewModel
    {
        [Key]
        public decimal id { get; set; } 
        [Required]
        [DisplayName("First Name")]
        public string Firstname { get; set; } = null!;
        [Required]
        [DisplayName("Last Name")]
        public string Lastname { get; set; } = null!;
       
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        [Required]
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = null!;
        [Required]
        [DisplayName("Username")]
        [StringLength(40)]
        public string Username { get; set; } = null!;
        [Required]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string Upassword { get; set; } = null!;
        [Required]
        [DataType(DataType.Date)]
        public DateTime Bithdate { get; set; }
        [DisplayName("Gender")]
        public decimal Genderid { get; set; }

    }
}
