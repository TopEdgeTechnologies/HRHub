using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class Holiday
{
    public int HolidayId { get; set; }

    public int? CompanyId { get; set; }

    public DateTime? HolidayDate { get; set; }

    public string? DayName { get; set; }

    public string? HolidayType { get; set; }

    public string? Title { get; set; }

    public bool Status { get; set; }

    public bool? IsDeleted { get; set; }

    public string? Cssclass { get; set; }

    public string? SpanClass { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
