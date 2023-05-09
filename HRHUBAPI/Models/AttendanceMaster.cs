using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class AttendanceMaster
{
    public int? AttendanceId { get; set; }

    public int? StaffId { get; set; }

    public DateTime? AttendanceDate { get; set; }
}
