﻿using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class StaffSalaryDetail1
{
    public int StaffSalaryDetailId { get; set; }

    public int? StaffSalaryId { get; set; }

    public int? SalaryComponentId { get; set; }

    public decimal? Amount { get; set; }
}
