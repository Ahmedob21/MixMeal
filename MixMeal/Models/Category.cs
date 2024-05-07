using System;
using System.Collections.Generic;

namespace MixMeal.Models;

public partial class Category
{
    public decimal Categoryid { get; set; }

    public string Categoryname { get; set; } = null!;

    public string Categorydescription { get; set; } = null!;

    public string? Imagepath { get; set; }

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
