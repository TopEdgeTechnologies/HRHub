using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class PerformanceReview
{
    public int ReviewFormId { get; set; }

    public int? CompanyId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}
