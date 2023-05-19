using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class ActivityLog
{
    public int ActivityLogId { get; set; }

    public int? CompanyId { get; set; }

    public int? UserId { get; set; }

    public int? ActivityTypeId { get; set; }

    public DateTime? TimeStamp { get; set; }

    public string? Description { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
