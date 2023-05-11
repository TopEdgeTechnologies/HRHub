using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HRHUBAPI.Models;

public partial class HrhubContext : DbContext
{
    public HrhubContext()
    {
    }

    public HrhubContext(DbContextOptions<HrhubContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AttendanceDetail> AttendanceDetails { get; set; }

    public virtual DbSet<AttendanceMaster> AttendanceMasters { get; set; }

    public virtual DbSet<AttendanceStatus> AttendanceStatuses { get; set; }

    public virtual DbSet<Candidate> Candidates { get; set; }

    public virtual DbSet<CandidateScreening> CandidateScreenings { get; set; }

    public virtual DbSet<CandidateSkill> CandidateSkills { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Designation> Designations { get; set; }

    public virtual DbSet<ExceptionLog> ExceptionLogs { get; set; }

    public virtual DbSet<GluserGroup> GluserGroups { get; set; }

    public virtual DbSet<GluserGroupDetail> GluserGroupDetails { get; set; }

    public virtual DbSet<Holiday> Holidays { get; set; }

    public virtual DbSet<Leave> Leaves { get; set; }

    public virtual DbSet<LeaveApproval> LeaveApprovals { get; set; }

    public virtual DbSet<LeaveApprovalSetting> LeaveApprovalSettings { get; set; }

    public virtual DbSet<LeaveStatus> LeaveStatuses { get; set; }

    public virtual DbSet<LeaveType> LeaveTypes { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<StaffAttachment> StaffAttachments { get; set; }

    public virtual DbSet<StatusInfo> StatusInfos { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserForm> UserForms { get; set; }

    public virtual DbSet<WeekendRule> WeekendRules { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AttendanceDetail>(entity =>
        {
            entity.ToTable("AttendanceDetail", "HR");

            entity.Property(e => e.AttendanceDetailId).HasColumnName("AttendanceDetailID");
            entity.Property(e => e.AttendanceId).HasColumnName("AttendanceID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.TimeIn).HasColumnName("TimeIN");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<AttendanceMaster>(entity =>
        {
            entity.HasKey(e => e.AttendanceId);

            entity.ToTable("AttendanceMaster", "HR");

            entity.Property(e => e.AttendanceId).HasColumnName("AttendanceID");
            entity.Property(e => e.AttendanceDate).HasColumnType("date");
            entity.Property(e => e.AttendanceStatusId).HasColumnName("AttendanceStatusID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.FirstPunchIn).HasColumnName("FirstPunchIN");
            entity.Property(e => e.LeaveTypeId).HasColumnName("LeaveTypeID");
            entity.Property(e => e.MarkAsHalfLeave).HasColumnName("MarkAs_HalfLeave");
            entity.Property(e => e.MarkAsShortLeave).HasColumnName("MarkAs_ShortLeave");
            entity.Property(e => e.MarkedAsHalfDay).HasColumnName("MarkedAs_HalfDay");
            entity.Property(e => e.MarkedAsShortDay).HasColumnName("MarkedAs_ShortDay");
            entity.Property(e => e.NonPaidAttendance).HasColumnName("NonPaid_Attendance");
            entity.Property(e => e.Remarks).IsUnicode(false);
            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.TotalDefinedMinutes).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TotalWorkingMinutes).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<AttendanceStatus>(entity =>
        {
            entity.ToTable("AttendanceStatus", "HR");

            entity.Property(e => e.AttendanceStatusId).HasColumnName("AttendanceStatusID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Title).IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<Candidate>(entity =>
        {
            entity.ToTable("Candidate", "Recuriment");

            entity.Property(e => e.CandidateId).HasColumnName("CandidateID");
            entity.Property(e => e.Address).IsUnicode(false);
            entity.Property(e => e.ApplyDate).HasColumnType("date");
            entity.Property(e => e.AttachmentPath).IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CoverLetter).IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.CurrentCompany).IsUnicode(false);
            entity.Property(e => e.CurrentDesignation).IsUnicode(false);
            entity.Property(e => e.CurrentSalary).HasColumnType("money");
            entity.Property(e => e.DesignationId).HasColumnName("DesignationID");
            entity.Property(e => e.Dob)
                .HasColumnType("date")
                .HasColumnName("DOB");
            entity.Property(e => e.Email).IsUnicode(false);
            entity.Property(e => e.ExpectedSalary).HasColumnType("money");
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name).IsUnicode(false);
            entity.Property(e => e.Phone).IsUnicode(false);
            entity.Property(e => e.Picture).IsUnicode(false);
            entity.Property(e => e.Qualification).IsUnicode(false);
            entity.Property(e => e.ReasonToLeft).IsUnicode(false);
            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<CandidateScreening>(entity =>
        {
            entity.HasKey(e => e.ScreeningId);

            entity.ToTable("CandidateScreening", "Recuriment");

            entity.Property(e => e.ScreeningId).HasColumnName("ScreeningID");
            entity.Property(e => e.CandidateId).HasColumnName("CandidateID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Remarks).IsUnicode(false);
            entity.Property(e => e.ScreeningDate).HasColumnType("date");
            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<CandidateSkill>(entity =>
        {
            entity.ToTable("CandidateSkills", "Recuriment");

            entity.Property(e => e.CandidateSkillId).HasColumnName("CandidateSkillID");
            entity.Property(e => e.CandidateId).HasColumnName("CandidateID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.SkillStatus).IsUnicode(false);
            entity.Property(e => e.Title).IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("Company");

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.Address).IsUnicode(false);
            entity.Property(e => e.CompanyName).IsUnicode(false);
            entity.Property(e => e.ContactPerson).IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Currency).IsUnicode(false);
            entity.Property(e => e.Email).IsUnicode(false);
            entity.Property(e => e.EmployeeWebCheckIn).HasColumnName("EmployeeWebCheckIN");
            entity.Property(e => e.Language).IsUnicode(false);
            entity.Property(e => e.LogoAttachment).IsUnicode(false);
            entity.Property(e => e.Phone).IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.WebUrl)
                .IsUnicode(false)
                .HasColumnName("WebURL");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.ToTable("Department", "HR");

            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.LogoAttachment).IsUnicode(false);
            entity.Property(e => e.ShortCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Title).IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<Designation>(entity =>
        {
            entity.HasKey(e => e.DesignationId).HasName("PK_Designation2");

            entity.ToTable("Designation", "HR");

            entity.Property(e => e.DesignationId).HasColumnName("DesignationID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Title).IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<ExceptionLog>(entity =>
        {
            entity.HasKey(e => e.ExceptionId);

            entity.ToTable("ExceptionLog");

            entity.Property(e => e.ExceptionId).HasColumnName("ExceptionID");
            entity.Property(e => e.ErrorMessage).IsUnicode(false);
            entity.Property(e => e.ExceptionDateTime).HasColumnType("datetime");
            entity.Property(e => e.InnerException).IsUnicode(false);
            entity.Property(e => e.StackTrace).IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<GluserGroup>(entity =>
        {
            entity.HasKey(e => e.GroupId);

            entity.ToTable("GLUserGroup");

            entity.Property(e => e.GroupId).HasColumnName("GroupID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.GroupTitle)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<GluserGroupDetail>(entity =>
        {
            entity.HasKey(e => e.UserGroupDetailId);

            entity.ToTable("GLUserGroupDetail");

            entity.Property(e => e.UserGroupDetailId).HasColumnName("UserGroupDetailID");
            entity.Property(e => e.FormId).HasColumnName("FormID");
            entity.Property(e => e.UserGroupId).HasColumnName("UserGroupID");
        });

        modelBuilder.Entity<Holiday>(entity =>
        {
            entity.ToTable("Holidays", "HR");

            entity.Property(e => e.HolidayId).HasColumnName("HolidayID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DayName).IsUnicode(false);
            entity.Property(e => e.HolidayDate).HasColumnType("date");
            entity.Property(e => e.Title).IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<Leave>(entity =>
        {
            entity.ToTable("Leave", "HR");

            entity.Property(e => e.LeaveId).HasColumnName("LeaveID");
            entity.Property(e => e.ApplicationHtml)
                .IsUnicode(false)
                .HasColumnName("Application_HTML");
            entity.Property(e => e.AppliedOn).HasColumnType("date");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.LeaveStatusId).HasColumnName("LeaveStatusID");
            entity.Property(e => e.LeaveSubject).IsUnicode(false);
            entity.Property(e => e.LeaveTypeId).HasColumnName("LeaveTypeID");
            entity.Property(e => e.MarkAsHalfLeave).HasColumnName("MarkAs_HalfLeave");
            entity.Property(e => e.MarkAsShortLeave).HasColumnName("MarkAs_ShortLeave");
            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.StartDate).HasColumnType("date");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<LeaveApproval>(entity =>
        {
            entity.ToTable("LeaveApproval", "HR");

            entity.Property(e => e.LeaveApprovalId).HasColumnName("LeaveApprovalID");
            entity.Property(e => e.ApprovalByStaffId).HasColumnName("ApprovalBy_StaffID");
            entity.Property(e => e.ApprovalDate)
                .HasColumnType("date")
                .HasColumnName("Approval_Date");
            entity.Property(e => e.ForwardDate)
                .HasColumnType("date")
                .HasColumnName("Forward_Date");
            entity.Property(e => e.ForwardedByStaffId).HasColumnName("ForwardedBy_StaffId");
            entity.Property(e => e.LeaveId).HasColumnName("LeaveID");
            entity.Property(e => e.LeaveStatusId).HasColumnName("LeaveStatusID");
            entity.Property(e => e.Remarks).IsUnicode(false);
        });

        modelBuilder.Entity<LeaveApprovalSetting>(entity =>
        {
            entity.HasKey(e => e.SettingId);

            entity.ToTable("LeaveApprovalSetting", "HR");

            entity.Property(e => e.SettingId)
                .ValueGeneratedNever()
                .HasColumnName("SettingID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.FinalApprovalByStaffId).HasColumnName("FinalApprovalBy_StaffID");
            entity.Property(e => e.LeaveApprovalLeaveStatusId).HasColumnName("LeaveApproval_LeaveStatusID");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<LeaveStatus>(entity =>
        {
            entity.ToTable("LeaveStatus", "HR");

            entity.Property(e => e.LeaveStatusId).HasColumnName("LeaveStatusID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.Title).IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<LeaveType>(entity =>
        {
            entity.ToTable("LeaveType", "HR");

            entity.Property(e => e.LeaveTypeId).HasColumnName("LeaveTypeID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Title).IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.ToTable("Staff", "HR");

            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.AccountTitle)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.BankAccountNumber)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.BankLocation)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.BankName)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.BloodGroup).IsUnicode(false);
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.ContactNumber).IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.DesignationId).HasColumnName("DesignationID");
            entity.Property(e => e.Dob)
                .HasColumnType("date")
                .HasColumnName("DOB");
            entity.Property(e => e.Email)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.EmergencyContact1)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.EmergencyContact2)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.FatherName).IsUnicode(false);
            entity.Property(e => e.Gender).IsUnicode(false);
            entity.Property(e => e.JobDescription).IsUnicode(false);
            entity.Property(e => e.JoiningDate).HasColumnType("date");
            entity.Property(e => e.MaterialStatus).IsUnicode(false);
            entity.Property(e => e.Name).IsUnicode(false);
            entity.Property(e => e.NationalIdnumber)
                .IsUnicode(false)
                .HasColumnName("NationalIDNumber");
            entity.Property(e => e.PermanentAddress)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.PresentAddress)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.RegistrationNo).IsUnicode(false);
            entity.Property(e => e.ResigningDate).HasColumnType("date");
            entity.Property(e => e.SalaryPerMonth).HasColumnType("money");
            entity.Property(e => e.TerminationDate).HasColumnType("date");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<StaffAttachment>(entity =>
        {
            entity.ToTable("StaffAttachments", "HR");

            entity.Property(e => e.StaffAttachmentId).HasColumnName("StaffAttachmentID");
            entity.Property(e => e.Attachment).IsUnicode(false);
            entity.Property(e => e.AttachmentType).IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.Title).IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<StatusInfo>(entity =>
        {
            entity.HasKey(e => e.StatusId);

            entity.ToTable("StatusInfo", "Recuriment");

            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Title).IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.GroupId).HasColumnName("GroupID");
            entity.Property(e => e.Password).IsUnicode(false);
            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.UserGroupId).HasColumnName("UserGroupID");
            entity.Property(e => e.UserImage)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.UserName).IsUnicode(false);
        });

        modelBuilder.Entity<UserForm>(entity =>
        {
            entity.HasKey(e => e.Formid);

            entity.Property(e => e.Action)
                .HasMaxLength(50)
                .HasColumnName("action");
            entity.Property(e => e.Controller)
                .HasMaxLength(50)
                .HasColumnName("controller");
            entity.Property(e => e.DOrder).HasColumnName("dOrder");
            entity.Property(e => e.FormTitle).HasMaxLength(50);
            entity.Property(e => e.ImageClass)
                .HasMaxLength(50)
                .HasColumnName("imageClass");
            entity.Property(e => e.IsParent).HasColumnName("isParent");
            entity.Property(e => e.ParentId).HasColumnName("parentId");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<WeekendRule>(entity =>
        {
            entity.ToTable("WeekendRule");

            entity.Property(e => e.WeekendRuleId).HasColumnName("WeekendRuleID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DayName).IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
