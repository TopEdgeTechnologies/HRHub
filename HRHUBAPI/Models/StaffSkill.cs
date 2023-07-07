using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class StaffSkill
{
    public int SkillId { get; set; }

    public int? StaffId { get; set; }

    public string? Title { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
