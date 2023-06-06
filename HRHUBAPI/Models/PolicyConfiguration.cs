using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class PolicyConfiguration
{
    public int PolicyConfigurationId { get; set; }

    public string? Title { get; set; }

    public int? PolicyId { get; set; }

    public int? CompanyId { get; set; }

    public bool Status { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
