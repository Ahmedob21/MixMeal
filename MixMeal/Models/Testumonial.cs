using System;
using System.Collections.Generic;

namespace MixMeal.Models;

public partial class Testumonial
{
    public decimal Testumonialid { get; set; }

    public string Ucomment { get; set; } = null!;

    public DateTimeOffset Posteddate { get; set; }

    public decimal Testimonialstatusid { get; set; }

    public decimal Custid { get; set; }

    public virtual User Cust { get; set; } = null!;

    public virtual Status Testimonialstatus { get; set; } = null!;
}
