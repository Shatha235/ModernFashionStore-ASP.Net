using System;
using System.Collections.Generic;

namespace OnlineStore.Models;

public partial class Producttype
{
    public decimal Typeid { get; set; }

    public string Typename { get; set; } = null!;

    public string? Imagepath { get; set; }

    public decimal? Categorytypeid { get; set; }

    public virtual Categorytype? Categorytype { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
