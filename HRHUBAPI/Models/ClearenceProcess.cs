using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class ClearenceProcess
{
    public int ClearenceProcessId { get; set; }

    public int? OffBoardingId { get; set; }

    public int? RemarksFromStaffId { get; set; }

    public string? Remarks { get; set; }

    public int? ClearenceProcessStatusId { get; set; }
}
