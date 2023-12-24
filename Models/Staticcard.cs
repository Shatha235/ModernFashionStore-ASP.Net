using System;
using System.Collections.Generic;

namespace OnlineStore.Models;

public partial class Staticcard
{
    public decimal Cardid { get; set; }

    public string? Cardnumber { get; set; }

    public DateTime? Expirydate { get; set; }

    public string? Cvv { get; set; }

    public decimal? Balance { get; set; }
}
