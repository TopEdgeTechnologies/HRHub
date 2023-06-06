﻿using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class OffBoardingProcessSetting
{
    public int OffboardingProcessSettingId { get; set; }

    public int? CompanyId { get; set; }

    public int NeedClearenceFromDepartmentId { get; set; }

    public int? NeedClearenceFromDesignationId { get; set; }

    public bool? AllowExitInterview { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
