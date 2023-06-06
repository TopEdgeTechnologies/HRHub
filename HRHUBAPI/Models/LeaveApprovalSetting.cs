using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class LeaveApprovalSetting
{
    public int SettingId { get; set; }

    public int? CompanyId { get; set; }

    public int? FinalApprovalByDesignationId { get; set; }

    public int? LeaveApprovalLeaveStatusId { get; set; }

    public bool? AllowApplyHalfDayLeave { get; set; }

    public bool? AllowApplyShortDayLeave { get; set; }

    public bool Status { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
