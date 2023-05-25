using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class ComponentInfo
{
    public int ComponentId { get; set; }

    public int? CompanyId { get; set; }

    public int? ComponentGroupId { get; set; }

    public string? Title { get; set; }

    public string? CalculationMethod { get; set; }

    public decimal? CompanyContribution { get; set; }

    public string? Category { get; set; }

    public string? Type { get; set; }

    public bool? Status { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
