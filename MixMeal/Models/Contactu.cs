using System;
using System.Collections.Generic;

namespace MixMeal.Models;

public partial class Contactu
{
    public decimal Contactusid { get; set; }

    public string Custname { get; set; } = null!;

    public string? Subject { get; set; }

    public string Custemail { get; set; } = null!;

    public string Message { get; set; } = null!;

    public DateTimeOffset? Contactdate { get; set; }
}
