using System;
using System.Collections.Generic;

namespace OnlineStore.Models;

public partial class Productimage
{
    public decimal Imageid { get; set; }

    public decimal? Productid { get; set; }

    public string Imagepath { get; set; } = null!;

    public virtual Product? Product { get; set; }
}
