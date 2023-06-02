using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class PerformanceReviewCategory
{
    public int ReviewCategoryId { get; set; }

    public int? CompanyId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }
}
