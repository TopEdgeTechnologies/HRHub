using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class EmailNotificationSetting
{
    public int NotificationId { get; set; }

    public int? CompanyId { get; set; }

    public bool OnStatusChange { get; set; }

    public bool OnSalaryGeneration { get; set; }

    public int? OnSalaryGenerationTemplateId { get; set; }

    public bool? OnBenefitEnrollment { get; set; }

    public int? OnBenefitEnrollmentTemplateId { get; set; }

    public bool? OnAnnouncements { get; set; }

    public int? OnAnnouncementsTemplateId { get; set; }

    public bool? OnLeaveApproval { get; set; }

    public int? OnLeaveApprovalTemplateId { get; set; }
}
