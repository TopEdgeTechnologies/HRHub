using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class CandidateScreening
{
    public int ScreeningId { get; set; }

    public int? CandidateId { get; set; }

    public int? StatusId { get; set; }

    public string? Remarks { get; set; }

    public DateTime? ScreeningDate { get; set; }

    public bool IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public string? AttachmentPath { get; set; }

    public DateTime? InterviewDateTime { get; set; }

    public string? InterviewMediumOrLocation { get; set; }
}
