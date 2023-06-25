using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class EmailTemplate
{
    public int TemplateId { get; set; }

    public int? CompanyId { get; set; }

    public string? Title { get; set; }

    public string? Subject { get; set; }

    public string? Body { get; set; }

    public bool Status { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public string? Type { get; set; }
}
