using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class SalaryComponent
{
    public int SalaryComponentId { get; set; }

    public int? CompanyId { get; set; }

    public int? FixedComponentId { get; set; }

    public string? Title { get; set; }

    public string? Category { get; set; }

    public string? ContributionMethod { get; set; }

    public decimal? StaffContribution { get; set; }

    public decimal? CompanyContribution { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
