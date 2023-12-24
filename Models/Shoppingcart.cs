using System;
using System.Collections.Generic;

namespace OnlineStore.Models;

public partial class Shoppingcart
{
    public decimal Shoppingcartid { get; set; }

    public decimal? Userid { get; set; }

    public decimal? Productid { get; set; }

    public decimal? Quantity { get; set; }

    public decimal? Colorid { get; set; }

    public decimal? Sizeid { get; set; }

    public virtual Productcolor? Color { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Productsize? Size { get; set; }

    public virtual User? User { get; set; }
}
