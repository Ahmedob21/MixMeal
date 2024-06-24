using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MixMeal.Models
{
    public class TestimonialContentViewModel
    {
        [MaxLength(40)]
        public string? Contenttype { get; set; }

        public string? Content { get; set; }
        [NotMapped]
        [DisplayName("Image")]
        public IFormFile? ImageFile { get; set; }
    }
}
