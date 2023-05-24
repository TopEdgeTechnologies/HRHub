using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class LoanApplicationProcess
{
    public int? ProcessId { get; set; }

    public int? LoanApplicationId { get; set; }

    public int? ApprovedByStaffId { get; set; }

    public int? StatusId { get; set; }

    public string? Remarks { get; set; }

    public bool? IsDelete { get; set; }
}
