using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class SalaryStatus
{
    public int SalaryStatusId { get; set; }

    public string? Title { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
