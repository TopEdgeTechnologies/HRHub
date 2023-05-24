using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class StaffSalarySetting
{
    public int StaffSalarySettingId { get; set; }

    public int? CompanyId { get; set; }

    public bool? IsIncomeTaxApplicable { get; set; }

    public bool? IsOverTimeApplicable { get; set; }

    public string? SalaryFrequency { get; set; }
}
