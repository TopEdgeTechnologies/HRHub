using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class StaffSalary
{
    public int StaffSalaryId { get; set; }

    public int? StaffId { get; set; }

    public DateTime? SalaryMonth { get; set; }

    public decimal? TotalDeductions { get; set; }

    public decimal? TotalEarnings { get; set; }

    public decimal? NetSalary { get; set; }

    public int? SalaryStatusId { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
