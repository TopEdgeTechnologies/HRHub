using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class Section
{
    public int SectionId { get; set; }

    public int? ReviewFormId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public int? OrderNo { get; set; }

    public bool? IsAnswerWeightage { get; set; }

    public bool? AllowSelfScoring { get; set; }

    public int? QuestionMaxLimit { get; set; }

    public decimal? TotalWeightage { get; set; }

    public decimal? EarnedWeightage { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
