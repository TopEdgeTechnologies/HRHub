using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class OffBoardingProcessSetting
{
    public int OffboardingProcessSettingId { get; set; }

    public int? CompanyId { get; set; }

    public string? NeedClearenceFromTitle { get; set; }

    public int? NeedClearenceFromStaffId { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
