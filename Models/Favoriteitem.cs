using System;
using System.Collections.Generic;

namespace OnlineStore.Models;

public partial class Favoriteitem
{
    public decimal Id { get; set; }

    public decimal Userid { get; set; }

    public decimal Productid { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
