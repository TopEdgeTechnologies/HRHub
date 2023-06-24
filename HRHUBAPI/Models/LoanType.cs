using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class LoanType
{
    public int LoanTypeId { get; set; }

    public int? CompanyId { get; set; }

    public string? Title { get; set; }

    public string? SnapPath { get; set; }

    public bool Status { get; set; }

    public bool IsNeedApproval { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
