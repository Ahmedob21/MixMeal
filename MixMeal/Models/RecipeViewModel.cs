using System.ComponentModel.DataAnnotations.Schema;

namespace MixMeal.Models
{
    public class RecipeViewModel
    {
        public string Recipename { get; set; } = null!;

        public string? Imagepath { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public DateTimeOffset Publishdate { get; set; }

        public decimal Price { get; set; }

        public decimal Categoryid { get; set; }

        
        public string Recipedescription { get; set; } = null!;

        public string Ingredientname { get; set; } = null!;

        public string Instructions { get; set; } = null!;

    }
}
