using System;
using System.Collections.Generic;

namespace OnlineStore.Models;

public partial class Order
{
    public decimal Orderid { get; set; }

    public decimal? Userid { get; set; }

    public decimal Totalprice { get; set; }

    public DateTime Orderdate { get; set; }

    public decimal? Couponid { get; set; }

    public string? Orderstatus { get; set; }

    public virtual Coupon? Coupon { get; set; }

    public virtual ICollection<Orderdelivery> Orderdeliveries { get; set; } = new List<Orderdelivery>();

    public virtual ICollection<Orderdetail> Orderdetails { get; set; } = new List<Orderdetail>();

    public virtual User? User { get; set; }
}
