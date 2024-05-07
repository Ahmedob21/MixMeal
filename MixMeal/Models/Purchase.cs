using System;
using System.Collections.Generic;

namespace MixMeal.Models;

public partial class Purchase
{
    public decimal Purchaseid { get; set; }

    public DateTimeOffset Purchasedate { get; set; }

    public decimal Customerid { get; set; }

    public decimal Recipeid { get; set; }

    public virtual User Customer { get; set; } = null!;

    public virtual Recipe Recipe { get; set; } = null!;
}
