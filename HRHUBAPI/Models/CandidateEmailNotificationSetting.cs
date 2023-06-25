using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class CandidateEmailNotificationSetting
{
    public int CandidateNotificationId { get; set; }

    public int? CompanyId { get; set; }

    public int? StatusId { get; set; }

    public int? TemplateId { get; set; }

    public bool? Status { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
