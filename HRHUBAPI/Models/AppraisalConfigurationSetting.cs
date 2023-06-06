using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class AppraisalConfigurationSetting
{
    public int AppraisalSettingId { get; set; }

    public int? DOrder { get; set; }

    public decimal? AverageFrom { get; set; }

    public decimal? AverageTo { get; set; }

    public decimal? AppraisalPercentage { get; set; }

    public bool Status { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
