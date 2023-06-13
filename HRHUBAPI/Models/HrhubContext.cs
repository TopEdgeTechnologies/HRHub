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

    public virtual DbSet<ActivityLog> ActivityLogs { get; set; }

    public virtual DbSet<ActivityType> ActivityTypes { get; set; }

    public virtual DbSet<Announcement> Announcements { get; set; }

    public virtual DbSet<Appraisal> Appraisals { get; set; }

    public virtual DbSet<AppraisalConfigurationSetting> AppraisalConfigurationSettings { get; set; }

    public virtual DbSet<AttendanceDetail> AttendanceDetails { get; set; }

    public virtual DbSet<AttendanceMaster> AttendanceMasters { get; set; }

    public virtual DbSet<AttendanceStatus> AttendanceStatuses { get; set; }

    public virtual DbSet<Candidate> Candidates { get; set; }

    public virtual DbSet<CandidateScreening> CandidateScreenings { get; set; }

    public virtual DbSet<CandidateSkill> CandidateSkills { get; set; }

    public virtual DbSet<ClearenceProcess> ClearenceProcesses { get; set; }

    public virtual DbSet<ClearenceProcessStatus> ClearenceProcessStatuses { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<ComponentGroup> ComponentGroups { get; set; }

    public virtual DbSet<ComponentInfo> ComponentInfos { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Designation> Designations { get; set; }

    public virtual DbSet<EmploymentType> EmploymentTypes { get; set; }

    public virtual DbSet<ExceptionLog> ExceptionLogs { get; set; }

    public virtual DbSet<GluserGroup> GluserGroups { get; set; }

    public virtual DbSet<GluserGroupDetail> GluserGroupDetails { get; set; }

    public virtual DbSet<Holiday> Holidays { get; set; }

    public virtual DbSet<Leave> Leaves { get; set; }

    public virtual DbSet<LeaveApproval> LeaveApprovals { get; set; }

    public virtual DbSet<LeaveApprovalSetting> LeaveApprovalSettings { get; set; }

    public virtual DbSet<LeaveStatus> LeaveStatuses { get; set; }

    public virtual DbSet<LeaveType> LeaveTypes { get; set; }

    public virtual DbSet<LoanApplication> LoanApplications { get; set; }

    public virtual DbSet<LoanApplicationProcess> LoanApplicationProcesses { get; set; }

    public virtual DbSet<LoanRepayment> LoanRepayments { get; set; }

    public virtual DbSet<LoanStatus> LoanStatuses { get; set; }

    public virtual DbSet<LoanType> LoanTypes { get; set; }

    public virtual DbSet<OffBoardingProcessSetting> OffBoardingProcessSettings { get; set; }

    public virtual DbSet<OffBoardingType> OffBoardingTypes { get; set; }

    public virtual DbSet<PerformanceForm> PerformanceForms { get; set; }

    public virtual DbSet<Policy> Policies { get; set; }

    public virtual DbSet<PolicyCategory> PolicyCategories { get; set; }

    public virtual DbSet<PolicyConfiguration> PolicyConfigurations { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<SalaryMethod> SalaryMethods { get; set; }

    public virtual DbSet<SalaryStatus> SalaryStatuses { get; set; }

    public virtual DbSet<SalaryStatusProcess> SalaryStatusProcesses { get; set; }

    public virtual DbSet<Section> Sections { get; set; }

    public virtual DbSet<SectionAnswer> SectionAnswers { get; set; }

    public virtual DbSet<SectionQuestion> SectionQuestions { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<StaffAttachment> StaffAttachments { get; set; }

    public virtual DbSet<StaffContract> StaffContracts { get; set; }

    public virtual DbSet<StaffCustomField> StaffCustomFields { get; set; }

    public virtual DbSet<StaffLeaveAllocation> StaffLeaveAllocations { get; set; }

    public virtual DbSet<StaffOffBoarding> StaffOffBoardings { get; set; }

    public virtual DbSet<StaffReviewFormProcessed> StaffReviewFormProcesseds { get; set; }

    public virtual DbSet<StaffSalary> StaffSalaries { get; set; }

    public virtual DbSet<StaffSalaryComponent> StaffSalaryComponents { get; set; }

    public virtual DbSet<StaffSalaryDetail> StaffSalaryDetails { get; set; }

    public virtual DbSet<StaffSalarySetting> StaffSalarySettings { get; set; }

    public virtual DbSet<StaffStatus> StaffStatuses { get; set; }

    public virtual DbSet<StatusInfo> StatusInfos { get; set; }

    public virtual DbSet<TaxSlabSetting> TaxSlabSettings { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserForm> UserForms { get; set; }

    public virtual DbSet<VInfoStaff> VInfoStaffs { get; set; }

    public virtual DbSet<ViewPerformanceForm> ViewPerformanceForms { get; set; }

    public virtual DbSet<WeekendRule> WeekendRules { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivityLog>(entity =>
        {
            entity.ToTable("ActivityLog");

            entity.Property(e => e.ActivityLogId).HasColumnName("ActivityLogID");
            entity.Property(e => e.ActivityTypeId).HasColumnName("ActivityTypeID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<ActivityType>(entity =>
        {
            entity.ToTable("ActivityType");

            entity.Property(e => e.ActivityTypeId).HasColumnName("ActivityTypeID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.Title).IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<Announcement>(entity =>
        {
            entity.ToTable("Announcement");

            entity.Property(e => e.AnnouncementId).HasColumnName("AnnouncementID");
            entity.Property(e => e.AnnouncementDate).HasColumnType("date");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.Title).IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<Appraisal>(entity =>
        {
            entity.ToTable("Appraisal", "Performance");

            entity.Property(e => e.AppraisalId).HasColumnName("AppraisalID");
            entity.Property(e => e.AppraisalDate).HasColumnType("date");
            entity.Property(e => e.AppraisalPercentage).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.CurrentMonthlySalary).HasColumnType("money");
            entity.Property(e => e.NewMonthlySalary).HasColumnType("money");
            entity.Property(e => e.ReviewFormId).HasColumnName("ReviewFormID");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<AppraisalConfigurationSetting>(entity =>
        {
            entity.HasKey(e => e.AppraisalSettingId);

            entity.ToTable("AppraisalConfigurationSetting", "Performance");

            entity.Property(e => e.AppraisalSettingId).HasColumnName("AppraisalSettingID");
            entity.Property(e => e.AppraisalPercentage).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.AverageFrom).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.AverageTo).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DOrder).HasColumnName("dOrder");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

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
            entity.Property(e => e.CssClass).IsUnicode(false);
            entity.Property(e => e.IconClass).IsUnicode(false);
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
            entity.Property(e => e.AttachmentPath).IsUnicode(false);
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

        modelBuilder.Entity<ClearenceProcess>(entity =>
        {
            entity.ToTable("ClearenceProcess");

            entity.Property(e => e.ClearenceProcessId).HasColumnName("ClearenceProcessID");
            entity.Property(e => e.Attachment).IsUnicode(false);
            entity.Property(e => e.ClearenceProcessStatusId).HasColumnName("ClearenceProcessStatusID");
            entity.Property(e => e.OffBoardingId).HasColumnName("OffBoardingID");
            entity.Property(e => e.ProcessDate).HasColumnType("datetime");
            entity.Property(e => e.Remarks).IsUnicode(false);
            entity.Property(e => e.RemarksFromStaffId).HasColumnName("RemarksFrom_StaffID");
        });

        modelBuilder.Entity<ClearenceProcessStatus>(entity =>
        {
            entity.ToTable("ClearenceProcessStatus");

            entity.Property(e => e.ClearenceProcessStatusId).HasColumnName("ClearenceProcessStatusID");
            entity.Property(e => e.Title).IsUnicode(false);
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

        modelBuilder.Entity<ComponentGroup>(entity =>
        {
            entity.HasKey(e => e.ComponentGroupId).HasName("PK_FixedSalaryComponents");

            entity.ToTable("ComponentGroup", "Payroll");

            entity.Property(e => e.ComponentGroupId)
                .ValueGeneratedNever()
                .HasColumnName("ComponentGroupID");
            entity.Property(e => e.Category)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.Title).IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<ComponentInfo>(entity =>
        {
            entity.HasKey(e => e.ComponentId).HasName("PK_Components");

            entity.ToTable("ComponentInfo", "Payroll");

            entity.Property(e => e.ComponentId).HasColumnName("ComponentID");
            entity.Property(e => e.CalculationMethod).IsUnicode(false);
            entity.Property(e => e.Category).IsUnicode(false);
            entity.Property(e => e.CompanyContribution).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.ComponentGroupId).HasColumnName("ComponentGroupID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Title).IsUnicode(false);
            entity.Property(e => e.Type).IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
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
            entity.Property(e => e.Grade).HasMaxLength(50);
            entity.Property(e => e.Title).IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<EmploymentType>(entity =>
        {
            entity.ToTable("EmploymentType");

            entity.Property(e => e.EmploymentTypeId).HasColumnName("EmploymentTypeID");
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

            entity.Property(e => e.SettingId).HasColumnName("SettingID");
            entity.Property(e => e.AllowApplyHalfDayLeave).HasColumnName("Allow_ApplyHalfDayLeave");
            entity.Property(e => e.AllowApplyShortDayLeave).HasColumnName("Allow_ApplyShortDayLeave");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.FinalApprovalByDesignationId).HasColumnName("FinalApprovalBy_DesignationID");
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
            entity.Property(e => e.Cssclass)
                .IsUnicode(false)
                .HasColumnName("CSSClass");
            entity.Property(e => e.GenderBased).IsUnicode(false);
            entity.Property(e => e.Title).IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<LoanApplication>(entity =>
        {
            entity.ToTable("LoanApplication", "Payroll");

            entity.Property(e => e.LoanApplicationId).HasColumnName("LoanApplicationID");
            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.ApplicationDate).HasColumnType("date");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.InstallmentPerMonth).HasColumnType("money");
            entity.Property(e => e.LoanStatusId).HasColumnName("LoanStatusID");
            entity.Property(e => e.LoanTypeId).HasColumnName("LoanTypeID");
            entity.Property(e => e.PaymentDate).HasColumnType("date");
            entity.Property(e => e.Reason).IsUnicode(false);
            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<LoanApplicationProcess>(entity =>
        {
            entity.HasKey(e => e.ProcessId);

            entity.ToTable("LoanApplicationProcess", "Payroll");

            entity.Property(e => e.ProcessId).HasColumnName("ProcessID");
            entity.Property(e => e.ApprovedByStaffId).HasColumnName("ApprovedBy_StaffID");
            entity.Property(e => e.LoanApplicationId).HasColumnName("LoanApplicationID");
            entity.Property(e => e.LoanStatusId).HasColumnName("LoanStatusID");
            entity.Property(e => e.ProcessDate).HasColumnType("date");
            entity.Property(e => e.Remarks).IsUnicode(false);
        });

        modelBuilder.Entity<LoanRepayment>(entity =>
        {
            entity.ToTable("LoanRepayments", "Payroll");

            entity.Property(e => e.LoanRepaymentId).HasColumnName("LoanRepaymentID");
            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.LoanApplicationId).HasColumnName("LoanApplicationID");
            entity.Property(e => e.PaymentDate).HasColumnType("date");
            entity.Property(e => e.StaffSalaryId).HasColumnName("StaffSalaryID");
        });

        modelBuilder.Entity<LoanStatus>(entity =>
        {
            entity.ToTable("LoanStatus", "Payroll");

            entity.Property(e => e.LoanStatusId).HasColumnName("LoanStatusID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.Title).IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<LoanType>(entity =>
        {
            entity.ToTable("LoanType", "Payroll");

            entity.Property(e => e.LoanTypeId).HasColumnName("LoanTypeID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Title).IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<OffBoardingProcessSetting>(entity =>
        {
            entity.Property(e => e.OffboardingProcessSettingId).HasColumnName("OffboardingProcessSettingID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.NeedClearenceFromDepartmentId).HasColumnName("NeedClearenceFrom_DepartmentID");
            entity.Property(e => e.NeedClearenceFromDesignationId).HasColumnName("NeedClearenceFrom_DesignationID");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<OffBoardingType>(entity =>
        {
            entity.ToTable("OffBoardingType");

            entity.Property(e => e.OffboardingTypeId).HasColumnName("OffboardingTypeID");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.Title).IsUnicode(false);
        });

        modelBuilder.Entity<PerformanceForm>(entity =>
        {
            entity.HasKey(e => e.ReviewFormId);

            entity.ToTable("PerformanceForm", "Performance");

            entity.Property(e => e.ReviewFormId).HasColumnName("ReviewFormID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.StartDate).HasColumnType("date");
            entity.Property(e => e.Title).IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<Policy>(entity =>
        {
            entity.HasKey(e => e.PolicyId).HasName("PK_LeavePolicy_Defined");

            entity.ToTable("Policy");

            entity.Property(e => e.PolicyId).HasColumnName("PolicyID");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.PolicyCategoryId).HasColumnName("PolicyCategoryID");
            entity.Property(e => e.Title).IsUnicode(false);
        });

        modelBuilder.Entity<PolicyCategory>(entity =>
        {
            entity.ToTable("PolicyCategory");

            entity.Property(e => e.PolicyCategoryId).HasColumnName("PolicyCategoryID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Title).IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<PolicyConfiguration>(entity =>
        {
            entity.HasKey(e => e.PolicyConfigurationId).HasName("PK_LeavePolicy");

            entity.ToTable("PolicyConfiguration");

            entity.Property(e => e.PolicyConfigurationId).HasColumnName("PolicyConfigurationID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.PolicyId).HasColumnName("PolicyID");
            entity.Property(e => e.Title).IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.ToTable("Question", "Performance");

            entity.Property(e => e.QuestionId).HasColumnName("QuestionID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Title).IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<SalaryMethod>(entity =>
        {
            entity.HasKey(e => e.SalaryMethodId).HasName("PK_SalaryCategory");

            entity.ToTable("SalaryMethod", "Payroll");

            entity.Property(e => e.SalaryMethodId).HasColumnName("SalaryMethodID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Title).IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<SalaryStatus>(entity =>
        {
            entity.ToTable("SalaryStatus", "Payroll");

            entity.Property(e => e.SalaryStatusId).HasColumnName("SalaryStatusID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Title).IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<SalaryStatusProcess>(entity =>
        {
            entity.ToTable("SalaryStatusProcess", "Payroll");

            entity.Property(e => e.SalaryStatusProcessId).HasColumnName("SalaryStatusProcessID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.ProcessDate).HasColumnType("date");
            entity.Property(e => e.Remarks).IsUnicode(false);
            entity.Property(e => e.SalaryStatusId).HasColumnName("SalaryStatusID");
            entity.Property(e => e.StaffSalaryId).HasColumnName("StaffSalaryID");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<Section>(entity =>
        {
            entity.ToTable("Section", "Performance");

            entity.Property(e => e.SectionId).HasColumnName("SectionID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.ReviewFormId).HasColumnName("ReviewFormID");
            entity.Property(e => e.Title).IsUnicode(false);
            entity.Property(e => e.TotalWeightage).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<SectionAnswer>(entity =>
        {
            entity.HasKey(e => e.AnswerId);

            entity.ToTable("SectionAnswer", "Performance");

            entity.Property(e => e.AnswerId).HasColumnName("AnswerID");
            entity.Property(e => e.AnswerComments).IsUnicode(false);
            entity.Property(e => e.AnswerWeightage).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.ReviewedStaffId).HasColumnName("Reviewed_StaffID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.ReviewerDesignationId).HasColumnName("Reviewer_DesignationID");
            entity.Property(e => e.ReviewerStaffId).HasColumnName("Reviewer_StaffID");
            entity.Property(e => e.SectionQuestionId).HasColumnName("SectionQuestionID");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<SectionQuestion>(entity =>
        {
            entity.ToTable("SectionQuestion", "Performance");

            entity.Property(e => e.SectionQuestionId).HasColumnName("SectionQuestionID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.QuestionId).HasColumnName("QuestionID");
            entity.Property(e => e.SectionId).HasColumnName("SectionID");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.Weightage).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.ToTable("Staff", "HR");

            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.AccountTitle).IsUnicode(false);
            entity.Property(e => e.BankAccountNumber).IsUnicode(false);
            entity.Property(e => e.BankCode).IsUnicode(false);
            entity.Property(e => e.BankLocation).IsUnicode(false);
            entity.Property(e => e.BankName).IsUnicode(false);
            entity.Property(e => e.BloodGroup).IsUnicode(false);
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.ContactNumber1).IsUnicode(false);
            entity.Property(e => e.ContactNumber2).IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.DesignationId).HasColumnName("DesignationID");
            entity.Property(e => e.Dob)
                .HasColumnType("date")
                .HasColumnName("DOB");
            entity.Property(e => e.Email).IsUnicode(false);
            entity.Property(e => e.EmergencyContact1).IsUnicode(false);
            entity.Property(e => e.EmergencyContact2).IsUnicode(false);
            entity.Property(e => e.EmploymentTypeId).HasColumnName("EmploymentTypeID");
            entity.Property(e => e.FatherName).IsUnicode(false);
            entity.Property(e => e.FirstName).IsUnicode(false);
            entity.Property(e => e.Gender).IsUnicode(false);
            entity.Property(e => e.JobDescription).IsUnicode(false);
            entity.Property(e => e.JobTitle).IsUnicode(false);
            entity.Property(e => e.JoiningDate).HasColumnType("date");
            entity.Property(e => e.LastName).IsUnicode(false);
            entity.Property(e => e.MaterialStatus).IsUnicode(false);
            entity.Property(e => e.NationalIdnumber)
                .IsUnicode(false)
                .HasColumnName("NationalIDNumber");
            entity.Property(e => e.PermanentAddress).IsUnicode(false);
            entity.Property(e => e.PresentAddress).IsUnicode(false);
            entity.Property(e => e.RegistrationNo).IsUnicode(false);
            entity.Property(e => e.ResigningDate).HasColumnType("date");
            entity.Property(e => e.SalaryAmount).HasColumnType("money");
            entity.Property(e => e.SalaryMethodId)
                .IsUnicode(false)
                .HasColumnName("SalaryMethodID");
            entity.Property(e => e.SnapPath).IsUnicode(false);
            entity.Property(e => e.StaffStatusId).HasColumnName("StaffStatusID");
            entity.Property(e => e.TerminationDate).HasColumnType("date");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.WorkMode).IsUnicode(false);
        });

        modelBuilder.Entity<StaffAttachment>(entity =>
        {
            entity.HasKey(e => e.StaffDocumentId);

            entity.ToTable("StaffAttachments", "HR");

            entity.Property(e => e.StaffDocumentId).HasColumnName("StaffDocumentID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DocumentPath).IsUnicode(false);
            entity.Property(e => e.DocumentTitle).IsUnicode(false);
            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<StaffContract>(entity =>
        {
            entity.Property(e => e.StaffContractId).HasColumnName("StaffContractID");
            entity.Property(e => e.AdditionalDetails).IsUnicode(false);
            entity.Property(e => e.Attachment).IsUnicode(false);
            entity.Property(e => e.ContractDuration).IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.EmploymentTypeId).HasColumnName("EmploymentTypeID");
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.StartDate).HasColumnType("date");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<StaffCustomField>(entity =>
        {
            entity.Property(e => e.StaffCustomFieldId)
                .ValueGeneratedNever()
                .HasColumnName("StaffCustomFieldID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DataType).IsUnicode(false);
            entity.Property(e => e.DefaultValue).IsUnicode(false);
            entity.Property(e => e.FieldName).IsUnicode(false);
            entity.Property(e => e.PlaceholderHelpText).IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<StaffLeaveAllocation>(entity =>
        {
            entity.HasKey(e => e.LeaveAllocationId);

            entity.ToTable("StaffLeaveAllocation");

            entity.Property(e => e.LeaveAllocationId).HasColumnName("LeaveAllocationID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.LeaveTypeId).HasColumnName("LeaveTypeID");
            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<StaffOffBoarding>(entity =>
        {
            entity.HasKey(e => e.OffBoardingId);

            entity.ToTable("StaffOffBoarding");

            entity.Property(e => e.OffBoardingId).HasColumnName("OffBoardingID");
            entity.Property(e => e.ApplicationDate).HasColumnType("date");
            entity.Property(e => e.ApplicationHtml)
                .IsUnicode(false)
                .HasColumnName("Application_HTML");
            entity.Property(e => e.CreatedOn).HasColumnType("date");
            entity.Property(e => e.InteriewRemarks)
                .IsUnicode(false)
                .HasColumnName("Interiew_Remarks");
            entity.Property(e => e.InterviewDate).HasColumnType("date");
            entity.Property(e => e.InterviewDoneByStaffId).HasColumnName("InterviewDoneByStaffID");
            entity.Property(e => e.LastWorkingDay).HasColumnType("date");
            entity.Property(e => e.OffboardingTypeId).HasColumnName("OffboardingTypeID");
            entity.Property(e => e.Reason).IsUnicode(false);
            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.UpdatedOn).HasColumnType("date");
        });

        modelBuilder.Entity<StaffReviewFormProcessed>(entity =>
        {
            entity.HasKey(e => e.ReviewFormProcessedId);

            entity.ToTable("StaffReviewFormProcessed", "Performance");

            entity.Property(e => e.ReviewFormProcessedId).HasColumnName("ReviewFormProcessedID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.EarnedWeightage).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ReviewFormId).HasColumnName("ReviewFormID");
            entity.Property(e => e.ReviewedStaffId).HasColumnName("Reviewed_StaffID");
            entity.Property(e => e.TotalWeightage).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<StaffSalary>(entity =>
        {
            entity.ToTable("StaffSalary", "Payroll");

            entity.Property(e => e.StaffSalaryId).HasColumnName("StaffSalaryID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.GrossSalary).HasColumnType("money");
            entity.Property(e => e.NetSalary).HasColumnType("money");
            entity.Property(e => e.SalaryMonth).HasColumnType("date");
            entity.Property(e => e.SalaryStatusId).HasColumnName("SalaryStatusID");
            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.TotalDeductions).HasColumnType("money");
            entity.Property(e => e.TotalEarnings).HasColumnType("money");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<StaffSalaryComponent>(entity =>
        {
            entity.HasKey(e => e.StaffSalaryComponentId).HasName("PK_StaffSalaryComponents");

            entity.ToTable("StaffSalaryComponent", "Payroll");

            entity.Property(e => e.StaffSalaryComponentId).HasColumnName("StaffSalaryComponentID");
            entity.Property(e => e.CompanyContributionAmount).HasColumnType("money");
            entity.Property(e => e.CompanyContributionCalculationMethod).IsUnicode(false);
            entity.Property(e => e.CompanyContributionValue).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ComponentAmount).HasColumnType("money");
            entity.Property(e => e.ComponentId).HasColumnName("ComponentID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.PercentageValue).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<StaffSalaryDetail>(entity =>
        {
            entity.ToTable("StaffSalaryDetail", "Payroll");

            entity.Property(e => e.StaffSalaryDetailId).HasColumnName("StaffSalaryDetailID");
            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.ComponentId).HasColumnName("ComponentID");
            entity.Property(e => e.StaffSalaryId).HasColumnName("StaffSalaryID");
        });

        modelBuilder.Entity<StaffSalarySetting>(entity =>
        {
            entity.ToTable("StaffSalarySettings", "Payroll");

            entity.Property(e => e.StaffSalarySettingId).HasColumnName("StaffSalarySettingID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.SalaryFrequency).IsUnicode(false);
        });

        modelBuilder.Entity<StaffStatus>(entity =>
        {
            entity.ToTable("StaffStatus");

            entity.Property(e => e.StaffStatusId).HasColumnName("StaffStatusID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Title).IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<StatusInfo>(entity =>
        {
            entity.HasKey(e => e.StatusId);

            entity.ToTable("StatusInfo", "Recuriment");

            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.BackGroundClass).IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Title).IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<TaxSlabSetting>(entity =>
        {
            entity.HasKey(e => e.SlabId);

            entity.ToTable("TaxSlabSetting", "Payroll");

            entity.Property(e => e.SlabId).HasColumnName("SlabID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.FixedAmount).HasColumnType("money");
            entity.Property(e => e.MaxIncome)
                .HasColumnType("money")
                .HasColumnName("Max_Income");
            entity.Property(e => e.MinIncome)
                .HasColumnType("money")
                .HasColumnName("Min_Income");
            entity.Property(e => e.TaxRatePercentage).HasColumnType("money");
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
            entity.Property(e => e.ReferenceId).HasColumnName("ReferenceID");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<VInfoStaff>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_Info_Staff", "HR");

            entity.Property(e => e.AccountTitle).IsUnicode(false);
            entity.Property(e => e.BankAccountNumber).IsUnicode(false);
            entity.Property(e => e.BankCode).IsUnicode(false);
            entity.Property(e => e.BankLocation).IsUnicode(false);
            entity.Property(e => e.BankName).IsUnicode(false);
            entity.Property(e => e.BloodGroup).IsUnicode(false);
            entity.Property(e => e.CompanyAddress).IsUnicode(false);
            entity.Property(e => e.CompanyCurrency).IsUnicode(false);
            entity.Property(e => e.CompanyEmail).IsUnicode(false);
            entity.Property(e => e.CompanyEmployeeWebCheckIn).HasColumnName("CompanyEmployeeWebCheckIN");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CompanyLanguage).IsUnicode(false);
            entity.Property(e => e.CompanyLogoAttachment).IsUnicode(false);
            entity.Property(e => e.CompanyName).IsUnicode(false);
            entity.Property(e => e.CompanyPhone).IsUnicode(false);
            entity.Property(e => e.CompanyWebUrl)
                .IsUnicode(false)
                .HasColumnName("CompanyWebURL");
            entity.Property(e => e.ContactNumber1).IsUnicode(false);
            entity.Property(e => e.ContactNumber2).IsUnicode(false);
            entity.Property(e => e.ContactPerson).IsUnicode(false);
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.DepartmentLogoAttachment).IsUnicode(false);
            entity.Property(e => e.DepartmentShortCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DepartmentTitle).IsUnicode(false);
            entity.Property(e => e.DesignationGrade).HasMaxLength(50);
            entity.Property(e => e.DesignationId).HasColumnName("DesignationID");
            entity.Property(e => e.DesignationTitle).IsUnicode(false);
            entity.Property(e => e.Dob)
                .HasColumnType("date")
                .HasColumnName("DOB");
            entity.Property(e => e.Email).IsUnicode(false);
            entity.Property(e => e.EmergencyContact1).IsUnicode(false);
            entity.Property(e => e.EmergencyContact2).IsUnicode(false);
            entity.Property(e => e.EmploymentTypeId).HasColumnName("EmploymentTypeID");
            entity.Property(e => e.FatherName).IsUnicode(false);
            entity.Property(e => e.FirstName).IsUnicode(false);
            entity.Property(e => e.Gender).IsUnicode(false);
            entity.Property(e => e.JobDescription).IsUnicode(false);
            entity.Property(e => e.JobTitle).IsUnicode(false);
            entity.Property(e => e.JoiningDate).HasColumnType("date");
            entity.Property(e => e.LastName).IsUnicode(false);
            entity.Property(e => e.MaterialStatus).IsUnicode(false);
            entity.Property(e => e.NationalIdnumber)
                .IsUnicode(false)
                .HasColumnName("NationalIDNumber");
            entity.Property(e => e.PermanentAddress).IsUnicode(false);
            entity.Property(e => e.PresentAddress).IsUnicode(false);
            entity.Property(e => e.RegistrationNo).IsUnicode(false);
            entity.Property(e => e.ResigningDate).HasColumnType("date");
            entity.Property(e => e.SalaryAmount).HasColumnType("money");
            entity.Property(e => e.SalaryFrequency).IsUnicode(false);
            entity.Property(e => e.SalaryMethodId).HasColumnName("SalaryMethodID");
            entity.Property(e => e.SalaryMethodTitle).IsUnicode(false);
            entity.Property(e => e.SnapPath).IsUnicode(false);
            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.StaffSalarySettingId).HasColumnName("StaffSalarySettingID");
            entity.Property(e => e.StaffStatusId).HasColumnName("StaffStatusID");
            entity.Property(e => e.TerminationDate).HasColumnType("date");
            entity.Property(e => e.WorkMode).IsUnicode(false);
        });

        modelBuilder.Entity<ViewPerformanceForm>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("view_PerformanceForm");

            entity.Property(e => e.AnswerId).HasColumnName("AnswerID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.PerformanceFormTitle).IsUnicode(false);
            entity.Property(e => e.QuestionTitle).IsUnicode(false);
            entity.Property(e => e.ReviewFormId).HasColumnName("ReviewFormID");
            entity.Property(e => e.ReviewedStaffId).HasColumnName("Reviewed_StaffID");
            entity.Property(e => e.ReviewerDesignationId).HasColumnName("Reviewer_DesignationID");
            entity.Property(e => e.ReviewerStaffId).HasColumnName("Reviewer_StaffID");
            entity.Property(e => e.SectionId).HasColumnName("SectionID");
            entity.Property(e => e.SectionQuestionId).HasColumnName("SectionQuestionID");
            entity.Property(e => e.SectionTitle).IsUnicode(false);
        });

        modelBuilder.Entity<WeekendRule>(entity =>
        {
            entity.ToTable("WeekendRule", "HR");

            entity.Property(e => e.WeekendRuleId).HasColumnName("WeekendRuleID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DayName).IsUnicode(false);
            entity.Property(e => e.IconClass).IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
