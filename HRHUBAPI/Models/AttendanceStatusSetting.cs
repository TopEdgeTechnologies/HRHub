using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class AttendanceStatusSetting
{
    public int AttendanceStatusSettingId { get; set; }

    public int? CompanyId { get; set; }

    public int? AttendanceStatusId { get; set; }

    public int? WorkingMinutesFrom { get; set; }

    public int? WorkingMinutesTo { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
