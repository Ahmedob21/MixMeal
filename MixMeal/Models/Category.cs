using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MixMeal.Models;

public partial class Category
{
    [Key]
    public decimal Categoryid { get; set; }
    [Required]
    [DisplayName("Name")]
    public string Categoryname { get; set; } = null!;
    [Required]
    [DisplayName("description")]
    public string Categorydescription { get; set; } = null!;

    public string? Imagepath { get; set; }
    [NotMapped]
    [DisplayName("Image")]
    public IFormFile? ImageFile { get; set; }
    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
