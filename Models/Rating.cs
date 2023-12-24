using System;
using System.Collections.Generic;

namespace OnlineStore.Models;

public partial class Rating
{
    public decimal Ratingid { get; set; }

    public decimal? Userid { get; set; }

    public decimal? Productid { get; set; }

    public decimal? Rating1 { get; set; }

    public DateTime? Ratingdate { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}
