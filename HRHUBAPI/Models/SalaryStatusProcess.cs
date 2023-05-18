using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class SalaryStatusProcess
{
    public int SalaryStatusProcessId { get; set; }

    public int? StaffSalaryId { get; set; }

    public int? SalaryStatusId { get; set; }

    public DateTime? ProcessDate { get; set; }

    public string? Remarks { get; set; }
}
