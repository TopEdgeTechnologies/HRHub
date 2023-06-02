using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class StaffOffBoarding
{
    public int OffBoardingId { get; set; }

    public int? OffboardingTypeId { get; set; }

    public int? StaffId { get; set; }

    public DateTime? ApplicationDate { get; set; }

    public string? Reason { get; set; }

    public string? ApplicationHtml { get; set; }

    public bool IsImmediate { get; set; }

    public DateTime? LastWorkingDay { get; set; }

    public string? InteriewRemarks { get; set; }

    public DateTime? InterviewDate { get; set; }

    public int? InterviewDoneByStaffId { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
