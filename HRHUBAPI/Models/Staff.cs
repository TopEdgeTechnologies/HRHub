using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public int? DesignationId { get; set; }

    public int? DepartmentId { get; set; }

    public string? RegistrationNo { get; set; }

    public string? NationalIdnumber { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? FatherName { get; set; }

    public string? ContactNumber1 { get; set; }

    public string? ContactNumber2 { get; set; }

    public string? EmergencyContact1 { get; set; }

    public string? EmergencyContact2 { get; set; }

    public DateTime? Dob { get; set; }

    public string? Gender { get; set; }

    public string? MaterialStatus { get; set; }

    public string? BloodGroup { get; set; }

    public string? Email { get; set; }

    public string? PresentAddress { get; set; }

    public string? PermanentAddress { get; set; }

    public DateTime? JoiningDate { get; set; }

    public DateTime? ResigningDate { get; set; }

    public DateTime? TerminationDate { get; set; }

    public string? SalaryMethod { get; set; }

    public decimal? SalaryAmount { get; set; }

    public string? AccountTitle { get; set; }

    public string? BankAccountNumber { get; set; }

    public string? BankName { get; set; }

    public string? BankLocation { get; set; }

    public string? BankCode { get; set; }

    public int? TaxPayerNumber { get; set; }

    public bool Status { get; set; }

    public string? JobDescription { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public int? CompanyId { get; set; }
}
