using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class StaffAttachment
{
    public int StaffDocumentId { get; set; }

    public int? StaffId { get; set; }

    public string? DocumentTitle { get; set; }

    public string? DocumentPath { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
