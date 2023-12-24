using System;
using System.Collections.Generic;

namespace OnlineStore.Models;

public partial class Category
{
    public decimal Categoryid { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Categorydiscount> Categorydiscounts { get; set; } = new List<Categorydiscount>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
