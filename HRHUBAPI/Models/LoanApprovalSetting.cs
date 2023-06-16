using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class LoanApprovalSetting
{
    public int SettingId { get; set; }

    public int? CompanyId { get; set; }

    public int? LoanFinalApprovalDesignationId { get; set; }
}
