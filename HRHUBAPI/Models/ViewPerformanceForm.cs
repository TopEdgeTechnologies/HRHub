using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class ViewPerformanceForm
{
    public int? AnswerId { get; set; }

    public int? CompanyId { get; set; }

    public string? PerformanceFormTitle { get; set; }

    public int ReviewFormId { get; set; }

    public int SectionId { get; set; }

    public string? SectionTitle { get; set; }

    public string? QuestionTitle { get; set; }

    public int? SectionQuestionId { get; set; }

    public int? ReviewerDesignationId { get; set; }

    public int? ReviewerStaffId { get; set; }

    public int? ReviewedStaffId { get; set; }
}
