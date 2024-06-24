using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace MixMeal.Models;

public partial class Contactuspagecontent
{
    public decimal Contactuspagecontentid { get; set; }

    public string? Contenttype { get; set; }

    public string? Content { get; set; }

    public decimal? Position { get; set; }

    public string? Imagepath { get; set; }
    [NotMapped]
    [DisplayName("Image")]
    public IFormFile? ImageFile { get; set; }
}
