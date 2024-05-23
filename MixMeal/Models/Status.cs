using System;
using System.Collections.Generic;

namespace MixMeal.Models;

public partial class Status
{
    public decimal Statusid { get; set; }

    public string Statusname { get; set; } = null!;

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();

    public virtual ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
