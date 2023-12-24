using System;
using System.Collections.Generic;

namespace OnlineStore.Models;

public partial class Userreview
{
    public decimal Reviewid { get; set; }

    public decimal? Userid { get; set; }

    public decimal? Productid { get; set; }

    public string? Comments { get; set; }

    public DateTime? Reviewdate { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}
