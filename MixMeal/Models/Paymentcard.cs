using System;
using System.Collections.Generic;

namespace MixMeal.Models;

public partial class Paymentcard
{
    public decimal Cardid { get; set; }

    public string Cardname { get; set; } = null!;

    public decimal Cardnumber { get; set; }

    public decimal Cvc { get; set; }

    public decimal Balance { get; set; }

    public decimal Cutomerid { get; set; }

    public virtual User Cutomer { get; set; } = null!;
}
