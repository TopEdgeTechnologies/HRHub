using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class StaffSalaryDetail
{
    public int StaffSalaryDetailId { get; set; }

    public int? StaffSalaryId { get; set; }

    public int? ComponentId { get; set; }

    public decimal? Amount { get; set; }
}
