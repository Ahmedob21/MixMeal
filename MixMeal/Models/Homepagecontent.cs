using System;
using System.Collections.Generic;

namespace MixMeal.Models;

public partial class Homepagecontent
{
    public decimal Homepagecontentid { get; set; }

    public string? Contenttype { get; set; }

    public string? Content { get; set; }

    public decimal? Position { get; set; }

    public string? Imagepath { get; set; }
}
