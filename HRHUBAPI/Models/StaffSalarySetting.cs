using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class StaffSalarySetting
{
    public int StaffSalarySettingId { get; set; }

    public int? CompanyId { get; set; }

    public bool? IsIncomeTaxApplicable { get; set; }

    public bool? IsOverTimeApplicable { get; set; }

    public int? SalaryCategoryId { get; set; }

    public string? PaysOnLastWorkingDayOfMonth { get; set; }

    public string? SalaryFrequency { get; set; }

    public string? PaysOn { get; set; }
}
