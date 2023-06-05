using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class SectionQuestion
{
    public int SectionQuestionId { get; set; }

    public int? QuestionId { get; set; }

    public int? SectionId { get; set; }

    public decimal? Weightage { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
