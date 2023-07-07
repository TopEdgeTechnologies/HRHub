using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class GluserGroup
{
    public int GroupId { get; set; }

    public int? CompanyId { get; set; }

    public string? GroupTitle { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public int? CreateBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public bool IsDeleted { get; set; }

    public string? GroupType { get; set; }
}
