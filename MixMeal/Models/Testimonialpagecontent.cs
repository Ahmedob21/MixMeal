using System;
using System.Collections.Generic;

namespace MixMeal.Models;

public partial class Testimonialpagecontent
{
    public decimal Testimonialpagecontentid { get; set; }

    public string? Contenttype { get; set; }

    public string? Content { get; set; }

    public decimal? Position { get; set; }

    public string? Imagepath { get; set; }
}
