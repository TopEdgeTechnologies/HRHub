using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class StaffAcademic
{
    public int AcademicId { get; set; }

    public int? StaffId { get; set; }

    public string? Title { get; set; }

    public string? InstituteName { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public bool? InProcess { get; set; }

    public string? Type { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
