using System;
using System.Collections.Generic;

namespace OnlineStore.Models;

public partial class Productcolor
{
    public decimal Colorid { get; set; }

    public string Colorname { get; set; } = null!;

    public virtual ICollection<Productattribute> Productattributes { get; set; } = new List<Productattribute>();

    public virtual ICollection<Shoppingcart> Shoppingcarts { get; set; } = new List<Shoppingcart>();
}
