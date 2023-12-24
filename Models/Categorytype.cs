using System;
using System.Collections.Generic;

namespace OnlineStore.Models;

public partial class Categorytype
{
    public decimal Categorytypeid { get; set; }

    public string Categorytypename { get; set; } = null!;

    public virtual ICollection<Producttype> Producttypes { get; set; } = new List<Producttype>();
}
