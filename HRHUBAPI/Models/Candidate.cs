using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class Candidate
{
    public int CandidateId { get; set; }

    public string? Name { get; set; }

    public string? CoverLetter { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? CurrentDesignation { get; set; }

    public string? CurrentCompany { get; set; }

    public decimal? CurrentSalary { get; set; }

    public decimal? ExpectedSalary { get; set; }

    public string? ReasonToLeft { get; set; }

    public DateTime? Dob { get; set; }

    public int? ExperienceInYears { get; set; }

    public int? ExperienceInMonths { get; set; }

    public string Gender { get; set; } = null!;

    public string? City { get; set; }

    public string? Address { get; set; }

    public string? Qualification { get; set; }

    public int DesignationId { get; set; }

    public DateTime? ApplyDate { get; set; }

    public int StatusId { get; set; }

    public string? AttachmentPath { get; set; }

    public bool Status { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public int? CompanyId { get; set; }
}
