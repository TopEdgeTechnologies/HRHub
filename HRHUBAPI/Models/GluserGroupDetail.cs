using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class GluserGroupDetail
{
    public int UserGroupDetailId { get; set; }

    public int? UserGroupId { get; set; }

    public int? FormId { get; set; }

    public bool? Assign { get; set; }

    public bool? IsEdit { get; set; }

    public bool IsDelete { get; set; }

    public bool? IsPrint { get; set; }

    public bool? IsNew { get; set; }
}
