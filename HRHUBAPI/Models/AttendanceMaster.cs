using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class AttendanceMaster
{
    public int AttendanceId { get; set; }

    public int? StaffId { get; set; }

    public DateTime? AttendanceDate { get; set; }

    public TimeSpan? FirstPunchIn { get; set; }

    public TimeSpan? LastPunchOut { get; set; }

    public decimal? TotalWorkingMinutes { get; set; }

    public decimal? TotalDefinedMinutes { get; set; }

    public int? OverTimeMinutes { get; set; }

    public int? AttendanceStatusId { get; set; }

    public int? LeaveTypeId { get; set; }

    public int? LateMinutes { get; set; }

    public bool? MarkedAsHalfDay { get; set; }

    public bool? MarkAsHalfLeave { get; set; }

    public bool? MarkedAsShortDay { get; set; }

    public bool? MarkAsShortLeave { get; set; }

    public bool? NonPaidAttendance { get; set; }

    public string? Remarks { get; set; }

    public int? GraceLateMinutes { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
