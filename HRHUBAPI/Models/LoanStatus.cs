using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class LoanStatus
{
    public int LoanStatusId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public bool Status { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
