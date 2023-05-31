using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class SalaryStatusProcess
{
    public int SalaryStatusProcessId { get; set; }

    public int? StaffSalaryId { get; set; }

    public int? SalaryStatusId { get; set; }

    public DateTime? ProcessDate { get; set; }

    public string? Remarks { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
