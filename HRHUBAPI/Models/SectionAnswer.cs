using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class SectionAnswer
{
    public int AnswerId { get; set; }

    public int? SectionId { get; set; }

    public int? SectionQuestionId { get; set; }

    public int? ReviewedStaffId { get; set; }

    public int? ReviewerDesignationId { get; set; }

    public int? ReviewerStaffId { get; set; }

    public decimal? AnswerWeightage { get; set; }

    public string? AnswerComments { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
