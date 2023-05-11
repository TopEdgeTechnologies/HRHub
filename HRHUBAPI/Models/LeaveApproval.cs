using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class LeaveApproval
{
    public int LeaveApprovalId { get; set; }

    public int? LeaveId { get; set; }

    public int? ForwardedByStaffId { get; set; }

    public DateTime? ForwardDate { get; set; }

    public int? ApprovalByStaffId { get; set; }

    public DateTime? ApprovalDate { get; set; }

    public string? Remarks { get; set; }

    public int? LeaveStatusId { get; set; }
}
