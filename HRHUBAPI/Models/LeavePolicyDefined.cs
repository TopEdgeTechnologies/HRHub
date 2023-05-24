using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class LeavePolicyDefined
{
    public int PolicyId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }
}
