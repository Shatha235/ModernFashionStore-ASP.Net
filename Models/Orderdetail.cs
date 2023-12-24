﻿using System;
using System.Collections.Generic;

namespace OnlineStore.Models;

public partial class Orderdetail
{
    public decimal Orderdetailid { get; set; }

    public decimal? Orderid { get; set; }

    public decimal? Productid { get; set; }

    public decimal Quantity { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}
