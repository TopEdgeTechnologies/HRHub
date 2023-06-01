using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class ReviewSectionQuestion
{
    public int QuestionId { get; set; }

    public int? SectionId { get; set; }

    public string? QuestionText { get; set; }

    public decimal? Weightage { get; set; }
}
