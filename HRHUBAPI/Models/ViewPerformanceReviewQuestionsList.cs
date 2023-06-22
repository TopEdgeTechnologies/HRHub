using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class ViewPerformanceReviewQuestionsList
{
    public int ReviewFormId { get; set; }

    public int SectionId { get; set; }

    public string? SectionTitle { get; set; }

    public bool IsAnswerWeightage { get; set; }

    public bool AllowSelfScoring { get; set; }

    public int? OrderNo { get; set; }

    public int QuestionId { get; set; }

    public string? QuestionTitle { get; set; }

    public int SectionQuestionId { get; set; }

    public string? PerformanceFormTitle { get; set; }

    public string? SectionDescription { get; set; }

    public decimal? Weightage { get; set; }

    public int? QuestionMaxLimit { get; set; }
}
