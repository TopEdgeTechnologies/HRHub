using System;
using System.Collections.Generic;

namespace HRHUBAPI.Models;

public partial class VInfoStaff
{
    public int StaffId { get; set; }

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

    public int? EmploymentTypeId { get; set; }

    public int? StaffStatusId { get; set; }

    public string? WorkMode { get; set; }

    public int? SalaryMethodId { get; set; }

    public string? SalaryMethodTitle { get; set; }

    public bool? SalaryMethodIsDeleted { get; set; }

    public int DesignationId { get; set; }

    public string? DesignationTitle { get; set; }

    public bool DesignationStatus { get; set; }

    public byte[]? DesignationGrade { get; set; }

    public int? DesignationSeq { get; set; }

    public bool? DesignationIsDeleted { get; set; }

    public int Expr16 { get; set; }

    public string? DepartmentTitle { get; set; }

    public string? DepartmentShortCode { get; set; }

    public int? DepartmentSeq { get; set; }

    public string? DepartmentLogoAttachment { get; set; }

    public bool DepartmentStatus { get; set; }

    public bool? DepartmentIsDeleted { get; set; }

    public int CompanyId { get; set; }

    public string? ContactPerson { get; set; }

    public string? CompanyName { get; set; }

    public string? CompanyAddress { get; set; }

    public string? CompanyEmail { get; set; }

    public string? CompanyPhone { get; set; }

    public string? CompanyWebUrl { get; set; }

    public string? CompanyLogoAttachment { get; set; }

    public string? CompanyLanguage { get; set; }

    public string? CompanyCurrency { get; set; }

    public bool? CompanyAttendanceByRosters { get; set; }

    public TimeSpan? CompanyOfficeStartTime { get; set; }

    public TimeSpan? CompanyOfficeEndTime { get; set; }

    public bool? CompanyEmployeeWebCheckIn { get; set; }

    public int? CompanyStartTimeGraceMinutes { get; set; }

    public int? CompanyMarkHalfDayAfterLateMinutes { get; set; }

    public bool CompanyStatus { get; set; }

    public bool? CompanyIsDeleted { get; set; }

    public int StaffSalarySettingId { get; set; }

    public bool? IsIncomeTaxApplicable { get; set; }

    public bool? IsOverTimeApplicable { get; set; }

    public bool? IsShortMinutesDeductionApplicable { get; set; }

    public string? SalaryFrequency { get; set; }
}
