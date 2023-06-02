using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class ReviewSection
{
    public int SectionId { get; set; }

    public int? ReviewFormId { get; set; }

    public int? ReviewCategoryId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public int? OrderNo { get; set; }

    public bool? IsWeightageSection { get; set; }

    public bool? AllowSelfScoring { get; set; }
}
