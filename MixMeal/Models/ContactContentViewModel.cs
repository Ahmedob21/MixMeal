using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace MixMeal.Models
{
    public class ContactContentViewModel
    {
        public string? Contenttype { get; set; }

        public string? Content { get; set; }

        [NotMapped]
        [DisplayName("Image")]
        public IFormFile? ImageFile { get; set; }
    }
}
