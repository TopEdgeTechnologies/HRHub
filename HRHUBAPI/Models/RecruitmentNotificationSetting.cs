using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class RecruitmentNotificationSetting
{
    public int? NotificationId { get; set; }

    public int? CompanyId { get; set; }

    public bool? OnCandidateEnrollment { get; set; }

    public int? OnCandidateEnrollmentTemplateId { get; set; }

    public bool? OnStatusChange { get; set; }

    public int? OnStatusChangeTemplateId { get; set; }

    public bool? OnApproved { get; set; }

    public int? OnApprovedTemplateId { get; set; }

    public bool? OnRejection { get; set; }

    public int? OnRejectionTemplateId { get; set; }
}
