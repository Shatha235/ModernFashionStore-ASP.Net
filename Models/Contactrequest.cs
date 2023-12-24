using System;
using System.Collections.Generic;

namespace OnlineStore.Models;

public partial class Contactrequest
{
    public decimal Requestid { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Message { get; set; }

    public DateTime? Submissiondate { get; set; }
}
