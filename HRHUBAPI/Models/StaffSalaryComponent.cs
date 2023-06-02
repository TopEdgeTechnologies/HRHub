using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class StaffSalaryComponent
{
    public int StaffSalaryComponentId { get; set; }

    public int? StaffId { get; set; }

    public int? ComponentId { get; set; }

    public decimal? PercentageValue { get; set; }

    public decimal? ComponentAmount { get; set; }

    public string? CompanyContributionCalculationMethod { get; set; }

    public decimal? CompanyContributionValue { get; set; }

    public decimal? CompanyContributionAmount { get; set; }
}
