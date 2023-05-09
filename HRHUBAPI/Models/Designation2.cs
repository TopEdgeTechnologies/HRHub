using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class Designation2
{
    public int DesignationId { get; set; }

    public string? Title { get; set; }

    public bool? Status { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public int? CompanyId { get; set; }
}
