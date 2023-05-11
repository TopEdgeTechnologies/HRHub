using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class Leave
{
    public int LeaveId { get; set; }

    public DateTime? AppliedOn { get; set; }

    public int? StaffId { get; set; }

    public int? LeaveTypeId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? LeaveStatusId { get; set; }

    public string? ApplicationHtml { get; set; }

    public string? LeaveSubject { get; set; }

    public bool MarkAsHalfLeave { get; set; }

    public bool MarkAsShortLeave { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
