using System;
using System.Collections.Generic;

namespace OnlineStore.Models;

public partial class Productattribute
{
    public decimal Productattributeid { get; set; }

    public decimal? Productid { get; set; }

    public decimal? Colorid { get; set; }

    public decimal? Sizeid { get; set; }

    public decimal Quantity { get; set; }

    public virtual Productcolor? Color { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Productsize? Size { get; set; }
}
