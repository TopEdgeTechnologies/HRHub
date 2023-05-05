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

    public virtual DbSet<Candidate> Candidates { get; set; }

    public virtual DbSet<CandidateScreening> CandidateScreenings { get; set; }

    public virtual DbSet<CandidateSkill> CandidateSkills { get; set; }

    public virtual DbSet<DesignationInfo> DesignationInfos { get; set; }

    public virtual DbSet<GluserGroup> GluserGroups { get; set; }

    public virtual DbSet<GluserGroupDetail> GluserGroupDetails { get; set; }

    public virtual DbSet<StatusInfo> StatusInfos { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserForm> UserForms { get; set; }

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
            entity.Property(e => e.SkillStatus).IsUnicode(false);
            entity.Property(e => e.Title).IsUnicode(false);
        });

        modelBuilder.Entity<DesignationInfo>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DesignationInfo");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DesignationId).HasColumnName("DesignationID");
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
