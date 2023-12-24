using System;
using System.Collections.Generic;

namespace OnlineStore.Models;

public partial class Coupon
{
    public decimal Couponid { get; set; }

    public string Couponcode { get; set; } = null!;

    public decimal Discountpercentage { get; set; }

    public DateTime? Startdate { get; set; }

    public DateTime? Enddate { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
