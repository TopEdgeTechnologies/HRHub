using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class WeekendRule
{
    public int WeekendRuleId { get; set; }

    public int? CompanyId { get; set; }

    public string? DayName { get; set; }

    public bool Status { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
