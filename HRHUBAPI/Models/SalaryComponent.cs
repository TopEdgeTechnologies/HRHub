using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class SalaryComponent
{
    public int SalaryComponentId { get; set; }

    public int? CompanyId { get; set; }

    public string? Title { get; set; }

    public string? Category { get; set; }

    public string? CalculationMethod { get; set; }

    public decimal? CalculationValue { get; set; }

    public decimal? CompanyContribution { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
