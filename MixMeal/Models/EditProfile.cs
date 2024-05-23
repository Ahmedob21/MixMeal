using System.ComponentModel.DataAnnotations.Schema;

namespace MixMeal.Models
{
    public class EditProfile
    {
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!; 
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public DateTime Bithdate { get; set; }

        public string Phone { get; set; } = null!;

        public string Username { get; set; } = null!;
        public decimal Genderid { get; set; }

    }
}
