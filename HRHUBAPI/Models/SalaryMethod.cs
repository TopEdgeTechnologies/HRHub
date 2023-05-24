using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class SalaryMethod
{
    public int SalaryMethodId { get; set; }

    public string Title { get; set; } = null!;

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
