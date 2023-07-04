using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class LeaveStatus
{
    public int LeaveStatusId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public bool Status { get; set; }

    public string? CssClass { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
