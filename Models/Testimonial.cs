using System;
using System.Collections.Generic;

namespace OnlineStore.Models;

public partial class Testimonial
{
    public decimal Testimonialid { get; set; }

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public string Feedback { get; set; } = null!;

    public DateTime? Submissiondate { get; set; }

    public string? Approvalstatus { get; set; }

    public string TruncatedFeedback
    {
        get
        {
            const int maxLength = 100; // Adjust the maximum length as needed
            return Feedback.Length <= maxLength ? Feedback: Feedback.Substring(0, maxLength) + " ...";
        }
    }
}
