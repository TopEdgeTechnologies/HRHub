using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class StaffOffBoarding
{
    public int? OffBoardingId { get; set; }

    public int? OffboardingTypeId { get; set; }

    public int? StaffId { get; set; }

    public DateTime? ApplicationDate { get; set; }

    public string? Reason { get; set; }

    public string? ApplicationHtml { get; set; }
}
