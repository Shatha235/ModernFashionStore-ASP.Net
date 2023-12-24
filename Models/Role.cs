using System;
using System.Collections.Generic;

namespace OnlineStore.Models;

public partial class Role
{
    public decimal Roleid { get; set; }

    public string? Rolename { get; set; }

    public virtual ICollection<Logininfo> Logininfos { get; set; } = new List<Logininfo>();
}
