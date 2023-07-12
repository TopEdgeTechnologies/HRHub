﻿using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class StaffSalarySetting
{
    public int StaffSalarySettingId { get; set; }

    public int? CompanyId { get; set; }

    public bool? IsIncomeTaxApplicable { get; set; }

    public bool? IsOverTimeApplicable { get; set; }

    public bool? IsShortMinutesDeductionApplicable { get; set; }

    public string? SalaryFrequency { get; set; }

    public string? WeekDay { get; set; }

    public short? FirstDateNumber { get; set; }

    public short? SecondDateNumber { get; set; }
}
