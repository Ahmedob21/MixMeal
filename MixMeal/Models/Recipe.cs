using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MixMeal.Models;

public partial class Recipe
{
    public decimal Recipeid { get; set; }

    public string Recipename { get; set; } = null!;

    public string? Imagepath { get; set; }
    [NotMapped]
    public IFormFile? ImageFile { get; set; }
    public DateTimeOffset Publishdate { get; set; }

    public decimal Price { get; set; }

    public decimal Categoryid { get; set; }

    public decimal Chefid { get; set; }

    public decimal Recipestatusid { get; set; }

    public string Recipedescription { get; set; } = null!;

    public string Ingredientname { get; set; } = null!;

    public string Instructions { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual User Chef { get; set; } = null!;

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

    public virtual Status Recipestatus { get; set; } = null!;
}
