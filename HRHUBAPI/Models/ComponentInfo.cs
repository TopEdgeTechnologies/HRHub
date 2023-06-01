using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class ComponentInfo
{
    public int ComponentId { get; set; }

    public int? CompanyId { get; set; }

<<<<<<<< HEAD:HRHUBAPI/Models/ComponentInfo.cs
    public int? ComponentGroupId { get; set; }
========
    public int? FixedComponentId { get; set; }

    public string? Title { get; set; }
>>>>>>>> LoanModule24-May-2023:HRHUBAPI/Models/SalaryComponent.cs

    public string? Title { get; set; }

    public string? ContributionMethod { get; set; }

<<<<<<<< HEAD:HRHUBAPI/Models/ComponentInfo.cs
========
    public decimal? StaffContribution { get; set; }

>>>>>>>> LoanModule24-May-2023:HRHUBAPI/Models/SalaryComponent.cs
    public decimal? CompanyContribution { get; set; }

    public string? Category { get; set; }

    public string? Type { get; set; }

    public bool Status { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
