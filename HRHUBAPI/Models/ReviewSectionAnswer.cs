using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class ReviewSectionAnswer
{
    public int AnswerId { get; set; }

    public int? QuestionId { get; set; }

    public int? ReviewedStaffId { get; set; }

    public int? ReviewerDesignationId { get; set; }

    public int? ReviewerStaffId { get; set; }

    public decimal? AnswerWeightage { get; set; }

    public string? AnswerComments { get; set; }
}
