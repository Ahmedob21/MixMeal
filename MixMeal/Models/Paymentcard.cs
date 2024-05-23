using System;
using System.Collections.Generic;

namespace MixMeal.Models;

public partial class Paymentcard
{
    public decimal Cardid { get; set; }

    public string Cardname { get; set; } = null!;

    public string Cardnumber { get; set; } = null!;

    public string Cvv { get; set; } = null!;

    public decimal Balance { get; set; }

    public DateTime Expiredate { get; set; }
}
