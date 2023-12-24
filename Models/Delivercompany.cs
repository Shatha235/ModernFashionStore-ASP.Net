using System;
using System.Collections.Generic;

namespace OnlineStore.Models;

public partial class Delivercompany
{
    public decimal Delivercompanyid { get; set; }

    public string Delivercompanyname { get; set; } = null!;

    public string Delivercompanyaddress { get; set; } = null!;

    public virtual ICollection<Orderdelivery> Orderdeliveries { get; set; } = new List<Orderdelivery>();
}
