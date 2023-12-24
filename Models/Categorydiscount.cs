using System;
using System.Collections.Generic;

namespace OnlineStore.Models;

public partial class Categorydiscount
{
    public decimal Discountid { get; set; }

    public decimal? Categoryid { get; set; }

    public decimal Discountpercentage { get; set; }

    public DateTime? Startdate { get; set; }

    public DateTime? Enddate { get; set; }

    public virtual Category? Category { get; set; }
}
