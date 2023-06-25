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
}
