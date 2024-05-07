using System;
using System.Collections.Generic;

namespace MixMeal.Models;

public partial class Userrole
{
    public decimal Roleid { get; set; }

    public string Rolename { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
