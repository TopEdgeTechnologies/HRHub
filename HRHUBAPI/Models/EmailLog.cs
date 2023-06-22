using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class EmailLog
{
    public int EmailId { get; set; }

    public int? CompanyId { get; set; }

    public string? EmailFrom { get; set; }

    public string? EmailTo { get; set; }

    public string? EmailCc { get; set; }

    public string? Subject { get; set; }

    public string? Body { get; set; }

    public bool? IsSent { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
