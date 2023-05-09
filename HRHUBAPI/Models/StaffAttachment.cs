using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class StaffAttachment
{
    public int StaffAttachmentId { get; set; }

    public int? StaffId { get; set; }

    public string? Attachment { get; set; }

    public string? AttachmentType { get; set; }

    public string? Title { get; set; }

    public bool? Status { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
