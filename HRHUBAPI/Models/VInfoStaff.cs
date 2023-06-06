using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class VInfoStaff
{
    public int StaffId { get; set; }

    public int? DesignationId { get; set; }

    public int? DepartmentId { get; set; }

    public string? JobTitle { get; set; }

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

    public string? SalaryMethodId { get; set; }

    public decimal? SalaryAmount { get; set; }

    public string? AccountTitle { get; set; }

    public string? BankAccountNumber { get; set; }

    public string? BankName { get; set; }

    public string? BankLocation { get; set; }

    public string? BankCode { get; set; }

    public int? TaxPayerNumber { get; set; }

    public string? JobDescription { get; set; }

    public string? SnapPath { get; set; }

    public bool Status { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public int? CompanyId { get; set; }

    public int? EmploymentTypeId { get; set; }

    public int? StaffStatusId { get; set; }

    public string? WorkMode { get; set; }

    public int? Expr1 { get; set; }

    public string? Title { get; set; }

    public bool? Expr2 { get; set; }

    public int? Expr3 { get; set; }

    public int? Expr4 { get; set; }

    public DateTime? Expr5 { get; set; }

    public DateTime? Expr6 { get; set; }

    public int Expr7 { get; set; }

    public string? Expr8 { get; set; }

    public bool Expr9 { get; set; }

    public byte[]? Grade { get; set; }

    public int? Seq { get; set; }

    public bool? Expr10 { get; set; }

    public int? Expr11 { get; set; }

    public int? Expr12 { get; set; }

    public DateTime? Expr13 { get; set; }

    public DateTime? Expr14 { get; set; }

    public int? Expr15 { get; set; }

    public int Expr16 { get; set; }

    public string? Expr17 { get; set; }

    public string? ShortCode { get; set; }

    public int? Expr18 { get; set; }

    public string? LogoAttachment { get; set; }

    public bool Expr19 { get; set; }

    public bool? Expr20 { get; set; }

    public int? Expr21 { get; set; }

    public int? Expr22 { get; set; }

    public DateTime? Expr23 { get; set; }

    public DateTime? Expr24 { get; set; }

    public int? Expr25 { get; set; }

    public int Expr26 { get; set; }

    public string? ContactPerson { get; set; }

    public string? CompanyName { get; set; }

    public string? Address { get; set; }

    public string? Expr27 { get; set; }

    public string? Phone { get; set; }

    public string? WebUrl { get; set; }

    public string? Expr28 { get; set; }

    public string? Language { get; set; }

    public string? Currency { get; set; }

    public bool? AttendanceByRosters { get; set; }

    public TimeSpan? OfficeStartTime { get; set; }

    public TimeSpan? OfficeEndTime { get; set; }

    public bool? EmployeeWebCheckIn { get; set; }

    public int? StartTimeGraceMinutes { get; set; }

    public int? MarkHalfDayAfterLateMinutes { get; set; }

    public bool Expr29 { get; set; }

    public bool? Expr30 { get; set; }

    public int? Expr31 { get; set; }

    public int? Expr32 { get; set; }

    public DateTime? Expr33 { get; set; }

    public DateTime? Expr34 { get; set; }

    public int StaffSalarySettingId { get; set; }

    public int? Expr35 { get; set; }

    public bool? IsIncomeTaxApplicable { get; set; }

    public bool? IsOverTimeApplicable { get; set; }

    public bool? IsShortMinutesDeductionApplicable { get; set; }

    public string? SalaryFrequency { get; set; }
}
