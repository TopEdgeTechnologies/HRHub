using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class Appraisal
{
    public int AppraisalId { get; set; }

    public int? StaffId { get; set; }

    public DateTime? AppraisalDate { get; set; }

    public int? ReviewFormId { get; set; }

    public decimal? AppraisalPercentage { get; set; }

    public decimal? CurrentMonthlySalary { get; set; }

    public decimal? NewMonthlySalary { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
