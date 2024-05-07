using System;
using System.Collections.Generic;

namespace MixMeal.Models;

public partial class Userlogin
{
    public decimal UserloginId { get; set; }

    public string Email { get; set; } = null!;

    public string Upassword { get; set; } = null!;

    public decimal Userid { get; set; }

    public virtual User User { get; set; } = null!;
}
