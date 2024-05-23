using System;
using System.Collections.Generic;

namespace MixMeal.Models;

public partial class Gender
{
    public decimal Genderid { get; set; }

    public string Gendername { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
