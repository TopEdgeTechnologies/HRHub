using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class CandidateSkill
{
    public int CandidateSkillId { get; set; }

    public string? Title { get; set; }

    public string? SkillStatus { get; set; }
}
