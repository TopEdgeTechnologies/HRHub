using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class AttendanceDetail
{
    public int AttendanceDetailId { get; set; }

    public int? AttendanceId { get; set; }

    public TimeSpan? TimeIn { get; set; }

    public TimeSpan? TimeOut { get; set; }

    public int? WorkingMinutes { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
