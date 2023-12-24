using System;
using System.Collections.Generic;

namespace OnlineStore.Models;

public partial class Logininfo
{
    public decimal Loginid { get; set; }

    public decimal? Userid { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public decimal? Userroleid { get; set; }

    public virtual User? User { get; set; }

    public virtual Role? Userrole { get; set; }
}
