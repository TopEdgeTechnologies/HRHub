using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class LeaveType
{
    public int LeaveTypeId { get; set; }

    public int? CompanyId { get; set; }

    public string? Title { get; set; }

    public int? NoOfLeaves { get; set; }

    public bool IsNonPaid { get; set; }

    public string? GenderBased { get; set; }

    public bool Status { get; set; }

    public string? Cssclass { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
