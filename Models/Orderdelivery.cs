using System;
using System.Collections.Generic;

namespace OnlineStore.Models;

public partial class Orderdelivery
{
    public decimal Orderdeliveriesid { get; set; }

    public decimal Orderid { get; set; }

    public decimal Delivercompanyid { get; set; }

    public string Orderdeliverystatus { get; set; } = null!;

    public virtual Delivercompany Delivercompany { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
