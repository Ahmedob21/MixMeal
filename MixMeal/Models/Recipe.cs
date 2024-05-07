using System;
using System.Collections.Generic;

namespace MixMeal.Models;

public partial class Recipe
{
    public decimal Recipeid { get; set; }

    public string Recipename { get; set; } = null!;

    public string Recipedescription { get; set; } = null!;

    public string Ingredients { get; set; } = null!;

    public string Instructions { get; set; } = null!;

    public string Imagepath { get; set; } = null!;

    public DateTimeOffset Publishdate { get; set; }

    public decimal Price { get; set; }

    public decimal Categoryid { get; set; }

    public decimal Chefid { get; set; }

    public decimal Recipestatusid { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual User Chef { get; set; } = null!;

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

    public virtual Status Recipestatus { get; set; } = null!;
}
