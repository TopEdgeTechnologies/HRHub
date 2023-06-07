using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class StaffContract
{
    public int StaffContractId { get; set; }

    public int? StaffId { get; set; }

    public int? EmploymentTypeId { get; set; }

    public string? ContractDuration { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Attachment { get; set; }

    public string? AdditionalDetails { get; set; }

    public bool Status { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
