using System;
using System.Collections.Generic;

namespace OnlineStore.Models;

public partial class Productsize
{
    public decimal Sizeid { get; set; }

    public string Sizename { get; set; } = null!;

    public virtual ICollection<Productattribute> Productattributes { get; set; } = new List<Productattribute>();

    public virtual ICollection<Shoppingcart> Shoppingcarts { get; set; } = new List<Shoppingcart>();
}
