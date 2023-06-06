using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class StaffReviewFormProcessed
{
    public int ReviewFormProcessedId { get; set; }

    public int? ReviewFormId { get; set; }

    public int? ReviewedStaffId { get; set; }

    public decimal? TotalWeightage { get; set; }

    public decimal? EarnedWeightage { get; set; }
}
