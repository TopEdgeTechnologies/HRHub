using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class StaffDependent
{
    public int DependentId { get; set; }

    public int? StaffId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Relation { get; set; }

    public string? Gender { get; set; }

    public string? Occupation { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
