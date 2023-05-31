using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class AttendanceStatus
{
    public int AttendanceStatusId { get; set; }

    public string? Title { get; set; }

    public int? DefinedMinutes { get; set; }

    public bool Status { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public string? CssClass { get; set; }

    public string? IconClass { get; set; }
}
