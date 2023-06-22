using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class XStaffLeaveAllocation
{
    public int LeaveAllocationId { get; set; }

    public int? StaffId { get; set; }

    public int? LeaveTypeId { get; set; }

    public int? AllowedLeaves { get; set; }

    public bool? Status { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
