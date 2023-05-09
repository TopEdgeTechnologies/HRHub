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

    public virtual DbSet<AttendanceMaster> AttendanceMasters { get; set; }

    public virtual DbSet<Candidate> Candidates { get; set; }

    public virtual DbSet<CandidateScreening> CandidateScreenings { get; set; }

    public virtual DbSet<CandidateSkill> CandidateSkills { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Designation> Designations { get; set; }

    public virtual DbSet<GluserGroup> GluserGroups { get; set; }

    public virtual DbSet<GluserGroupDetail> GluserGroupDetails { get; set; }

    public virtual DbSet<LeaveType> LeaveTypes { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<StaffAttachment> StaffAttachments { get; set; }

    public virtual DbSet<StatusInfo> StatusInfos { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserForm> UserForms { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=WebServer;Initial Catalog=HRHUB;User ID=team;Password=dynamixsolpassword;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AttendanceMaster>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("AttendanceMaster");

            entity.Property(e => e.AttendanceDate).HasColumnType("date");
            entity.Property(e => e.AttendanceId).HasColumnName("AttendanceID");
            entity.Property(e => e.StaffId).HasColumnName("StaffID");
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
            entity.Property(e => e.ExperienceInYears).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name).IsUnicode(false);
            entity.Property(e => e.Phone).IsUnicode(false);
            entity.Property(e => e.Qualification).IsUnicode(false);
            entity.Property(e => e.ResonToLeft).IsUnicode(false);
            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Company).WithMany(p => p.Candidates)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Candidate_Company");
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

            entity.HasOne(d => d.Candidate).WithMany(p => p.CandidateScreenings)
                .HasForeignKey(d => d.CandidateId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_CandidateScreening_Candidate");

            entity.HasOne(d => d.Status).WithMany(p => p.CandidateScreenings)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_CandidateScreening_StatusInfo");
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

            entity.HasOne(d => d.Candidate).WithMany(p => p.CandidateSkills)
                .HasForeignKey(d => d.CandidateId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_CandidateSkills_Candidate");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("Company");

            entity.Property(e => e.CompanyId)
                .ValueGeneratedNever()
                .HasColumnName("CompanyID");
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

            entity.Property(e => e.DepartmentId)
                .ValueGeneratedNever()
                .HasColumnName("DepartmentID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.LogoAttachment).IsUnicode(false);
            entity.Property(e => e.ShortCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Title).IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Company).WithMany(p => p.Departments)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK_Department_Company");
        });

        modelBuilder.Entity<Designation>(entity =>
        {
            entity.ToTable("Designation", "HR");

            entity.Property(e => e.DesignationId)
                .ValueGeneratedNever()
                .HasColumnName("DesignationID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Title).IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<GluserGroup>(entity =>
        {
            entity.HasKey(e => e.GroupId);

            entity.ToTable("GLUserGroup");

            entity.Property(e => e.GroupId).HasColumnName("GroupID");
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

        modelBuilder.Entity<LeaveType>(entity =>
        {
            entity.ToTable("LeaveType");

            entity.Property(e => e.LeaveTypeId)
                .ValueGeneratedNever()
                .HasColumnName("LeaveTypeID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Title).IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Company).WithMany(p => p.LeaveTypes)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_LeaveType_Company");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.ToTable("Staff", "HR");

            entity.Property(e => e.StaffId)
                .ValueGeneratedNever()
                .HasColumnName("StaffID");
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

            entity.HasOne(d => d.Company).WithMany(p => p.Staff)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Staff_Company");

            entity.HasOne(d => d.Department).WithMany(p => p.Staff)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Staff_Department");
        });

        modelBuilder.Entity<StaffAttachment>(entity =>
        {
            entity.ToTable("StaffAttachments", "HR");

            entity.Property(e => e.StaffAttachmentId)
                .ValueGeneratedNever()
                .HasColumnName("StaffAttachmentID");
            entity.Property(e => e.Attachment).IsUnicode(false);
            entity.Property(e => e.AttachmentType).IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.Title).IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Staff).WithMany(p => p.StaffAttachments)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_StaffAttachments_Staff");
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
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.GroupId).HasColumnName("GroupID");
            entity.Property(e => e.InstituteId).HasColumnName("InstituteID");
            entity.Property(e => e.Password).IsUnicode(false);
            entity.Property(e => e.SchoolId).HasColumnName("SchoolID");
            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.UserGroupId).HasColumnName("UserGroupID");
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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
