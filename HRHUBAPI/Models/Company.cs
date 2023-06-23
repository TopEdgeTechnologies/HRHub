using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class Company
{
    public int CompanyId { get; set; }

    public string? ContactPerson { get; set; }

    public string? CompanyName { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? WebUrl { get; set; }

    public string? LogoAttachment { get; set; }

    public string? Language { get; set; }

    public string? Currency { get; set; }

    public bool AttendanceByRosters { get; set; }

    public TimeSpan? OfficeStartTime { get; set; }

    public TimeSpan? OfficeEndTime { get; set; }

    public bool EmployeeWebCheckIn { get; set; }

    public int? StartTimeGraceMinutes { get; set; }

    public bool? IsGraceMinutesAllowed { get; set; }

    public bool? IsMarkHalfDayAllow { get; set; }

    public int? MarkHalfDayAfterLateMinutes { get; set; }

    public string? EmailSendFrom { get; set; }

    public string? EmailPassword { get; set; }

    public int? EmailSmtpport { get; set; }

    public string? EmailServerHost { get; set; }

    public bool? LeaveDistributionIsAccrualApproach { get; set; }

    public bool? LeaveDistributionIsCalendarYearApproach { get; set; }

    public int? LeaveDistributionStartMonth { get; set; }

    public bool Status { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
