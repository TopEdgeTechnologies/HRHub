﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System;

using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;
using HRHUBAPI.Models.Configuration;
using System.Data;

namespace HRHUBAPI.Models
{



    public partial class Company
    {


        DbConnection _db = new DbConnection();

        [NotMapped]
        public int TransFlag { get; set; }
        [NotMapped]
        public int LeaveStatusId { get; set; }

        [NotMapped]
        public int StaffId { get; set; }

        [NotMapped]
        public string? Staffname { get; set; }
        [NotMapped]
        public string? StaffEmail { get; set; }

        [NotMapped]
        public string? StaffDesignation { get; set; }
        [NotMapped]
        public string? StaffDepartment { get; set; }

        [NotMapped]
        public string UserId { get; set; } = string.Empty;
        [NotMapped]
        public string Username { get; set; } = string.Empty;
        [NotMapped]
        public string UserPassword { get; set; } = string.Empty;


        [NotMapped]
        public User? SettingUser { get; set; }
        [NotMapped]
        public WeekendRule? SettingWeekendRule { get; set; }

        [NotMapped]
        public Staff? SettingStaff { get; set; }

        [NotMapped]
        public AttendanceStatus? SettingAttendanceStatus { get; set; }

        [NotMapped]
        public LeaveApproval? SettingLeaveApproval { get; set; }
        [NotMapped]
        public List<WeekendRule>? WeekendList { get; set; }
        [NotMapped]
        public string? ImgPath { get; set; }


        public async Task<List<Company>> GetCompany(int CompanyId, HrhubContext _context)
        {
            try
            {
                List<Company> list = new List<Company>();
                list = await _context.Companies.Where(x => x.IsDeleted == false && x.CompanyId == CompanyId).ToListAsync();

                return list;

            }
            catch (Exception ex)
            {

                throw;

            }
        }

        public async Task<Company> GetCompanyById(int id, HrhubContext _context)
        {
            try
            {

                var result = await _context.Companies.FirstOrDefaultAsync(x => x.CompanyId == id && x.IsDeleted == false);
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return null;

                }



            }
            catch (Exception ex)
            {

                throw;

            }
        }


        public async Task<Company> PostCompany(Company ObjCompanyInfo, HrhubContext _context)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {

                try
                {
                    string msg = "";
                    var checkCompanyInfo = await _context.Companies.FirstOrDefaultAsync(x => x.CompanyId == ObjCompanyInfo.CompanyId && x.IsDeleted == false);
                    if (checkCompanyInfo != null && checkCompanyInfo.CompanyId > 0)
                    {
                        checkCompanyInfo.CompanyId = ObjCompanyInfo.CompanyId;
                        checkCompanyInfo.CompanyName = ObjCompanyInfo.CompanyName;
                        checkCompanyInfo.ContactPerson = ObjCompanyInfo.ContactPerson;
                        checkCompanyInfo.Phone = ObjCompanyInfo.Phone;
                        checkCompanyInfo.Address = ObjCompanyInfo.Address;
                        if(ObjCompanyInfo.LogoAttachment != null && ObjCompanyInfo.LogoAttachment != "")
                        {
                            checkCompanyInfo.LogoAttachment = ObjCompanyInfo.LogoAttachment;
                        }
                        else
                        {
                            checkCompanyInfo.LogoAttachment = ObjCompanyInfo.ImgPath;
                        }
                        checkCompanyInfo.Language = ObjCompanyInfo.Language;
                        checkCompanyInfo.Email = ObjCompanyInfo.Email;
                        checkCompanyInfo.Currency = ObjCompanyInfo.Currency;
                        checkCompanyInfo.WebUrl = ObjCompanyInfo.WebUrl;
                        checkCompanyInfo.AttendanceByRosters = ObjCompanyInfo.AttendanceByRosters;
                        checkCompanyInfo.OfficeEndTime = ObjCompanyInfo.OfficeEndTime;
                        checkCompanyInfo.OfficeStartTime = ObjCompanyInfo.OfficeStartTime;
                        checkCompanyInfo.EmployeeWebCheckIn = ObjCompanyInfo.EmployeeWebCheckIn;
                        checkCompanyInfo.StartTimeGraceMinutes = ObjCompanyInfo.StartTimeGraceMinutes;
                        checkCompanyInfo.MarkHalfDayAfterLateMinutes = ObjCompanyInfo.MarkHalfDayAfterLateMinutes;
                        checkCompanyInfo.UpdatedOn = DateTime.Now;
                        checkCompanyInfo.Status = ObjCompanyInfo.Status;
                        checkCompanyInfo.UpdatedBy = ObjCompanyInfo.CreatedBy;
                        checkCompanyInfo.TransFlag = 2;
                        await _context.SaveChangesAsync();
                        dbContextTransaction.Commit();
                        return ObjCompanyInfo;

                    }
                    else
                    {
                        //----------- Intial Entry Company Register

                        ObjCompanyInfo.CreatedOn = DateTime.Now;
                        ObjCompanyInfo.IsDeleted = false;
                        ObjCompanyInfo.Language = "English";
                        ObjCompanyInfo.Currency = "0";
                        ObjCompanyInfo.StartTimeGraceMinutes = 0;
                        ObjCompanyInfo.MarkHalfDayAfterLateMinutes = 0;
                        ObjCompanyInfo.Status = true;
                        _context.Companies.Add(ObjCompanyInfo);
                        await _context.SaveChangesAsync();



                        // -----------------Save and Insert Designation records

                        Designation objDesignation = new Designation();
                        objDesignation.CompanyId = ObjCompanyInfo.CompanyId;
                        objDesignation.Title = ObjCompanyInfo.StaffDesignation;
                        objDesignation.Status = true;

                        objDesignation.CreatedOn = DateTime.Now;
                        objDesignation.CreatedBy = ObjCompanyInfo.CreatedBy;
                        objDesignation.IsDeleted = false;
                        _context.Designations.Add(objDesignation);
                        await _context.SaveChangesAsync();

                        // ---------------------------------------------



                        // ------------------Save and Insert Department records


                        Department objDepartment = new Department();
                        objDepartment.CompanyId = ObjCompanyInfo.CompanyId;
                        objDepartment.Title = ObjCompanyInfo.StaffDepartment;
                        objDepartment.Status = true;
                        objDepartment.CreatedOn = DateTime.Now;
                        objDepartment.CreatedBy = ObjCompanyInfo.CreatedBy;
                        objDepartment.IsDeleted = false;
                        _context.Departments.Add(objDepartment);
                        await _context.SaveChangesAsync();

                        // ---------------------------------------------


                        // -------------------Save and Staff records


                        Staff objStaff = new Staff();
                        objStaff.CompanyId = ObjCompanyInfo.CompanyId;
                        objStaff.FirstName = ObjCompanyInfo.ContactPerson;
                        objStaff.Email = ObjCompanyInfo.StaffEmail;
                        objStaff.DesignationId = objDesignation.DesignationId;
                        objStaff.DepartmentId = objDepartment.DepartmentId;
                        objStaff.Status = true;
                        objStaff.CreatedOn = DateTime.Now;
                        objStaff.CreatedBy = ObjCompanyInfo.CreatedBy;
                        objStaff.IsDeleted = false;
                        _context.Staff.Add(objStaff);
                        await _context.SaveChangesAsync();

                        // ---------------------------------------------


                        // --------------------Save and Insert Users records

                        User objUser = new User();
                        objUser.CompanyId = ObjCompanyInfo.CompanyId;
                        objUser.UserName = ObjCompanyInfo.Username;
                        objUser.Password = ObjCompanyInfo.UserPassword;
                        objUser.StaffId = objStaff.StaffId;
                        objUser.GroupId = 3;
                        objUser.IsActive = true;

                        objUser.CreatedOn = DateTime.Now;
                        objUser.CreateBy = ObjCompanyInfo.CreatedBy;

                        _context.Users.Add(objUser);
                        await _context.SaveChangesAsync();

                        // ---------------------------------------------


                        // ---------------get subject and body from EmailTemplate_HRHUB

                        var obj = _context.EmailTemplateHrhubs.FirstOrDefault(x => x.TemplateId == 1);
                        // template id 1 is company onboarding welcome Email 
                        string EmailSubject = obj.Subject;
                        string EmailBody = obj.Body;

                        EmailBody = EmailBody.Replace("[StaffName]", objStaff.FirstName);

                        //------------------------------------------------------------------------------





                        // ------------------------insert into Email Log table
                        EmailLog ObjEmail = new EmailLog();
                        ObjEmail.CompanyId = 1;
                        ObjEmail.Subject = EmailSubject;
                        ObjEmail.Body = EmailBody;
                        ObjEmail.EmailTo = objStaff.Email;
                        ObjEmail.IsSent = false;
                        ObjEmail.CreatedOn = DateTime.Now;

                        _context.EmailLogs.Add(ObjEmail);
                        await _context.SaveChangesAsync();






                        ObjCompanyInfo.TransFlag = 1;
                        dbContextTransaction.Commit();

                        //--------------------------------------------


                        //----------- Call Store Procedure on Company New Configuration

                        string query = "EXEC dbo.Osp_CompanyOnBoarding_FirstConfiguration " + ObjCompanyInfo.CompanyId + "," + objUser.UserId;
                        DataTable dt = _db.ReturnDataTable(query);

                        //----------------------




                    }



                    return ObjCompanyInfo;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;

                }
            }

        }

        public async Task<Company> PostCompanyProfile(Company ObjCompanyInfo, HrhubContext _context)
        {
            try
            {
                string msg = "";
                var checkCompanyInfo = await _context.Companies.FirstOrDefaultAsync(x => x.CompanyId == ObjCompanyInfo.CompanyId && x.IsDeleted == false);
                if (checkCompanyInfo != null && checkCompanyInfo.CompanyId > 0)
                {
                    checkCompanyInfo.CompanyId = ObjCompanyInfo.CompanyId;
                    checkCompanyInfo.CompanyName = ObjCompanyInfo.CompanyName;
                    checkCompanyInfo.Phone = ObjCompanyInfo.Phone;
                    checkCompanyInfo.Email = ObjCompanyInfo.Email;
                    checkCompanyInfo.Address = ObjCompanyInfo.Address;
                    if (ObjCompanyInfo.LogoAttachment != null && ObjCompanyInfo.LogoAttachment != "")
                    {
                        checkCompanyInfo.LogoAttachment = ObjCompanyInfo.LogoAttachment;
                    }
                    else
                    {
                        checkCompanyInfo.LogoAttachment = ObjCompanyInfo.ImgPath;
                    }
                    checkCompanyInfo.Language = ObjCompanyInfo.Language;
                    checkCompanyInfo.Currency = ObjCompanyInfo.Currency;
                    checkCompanyInfo.UpdatedOn = DateTime.Now;
                    checkCompanyInfo.Status = ObjCompanyInfo.Status;
                    checkCompanyInfo.UpdatedBy = ObjCompanyInfo.CreatedBy;
                    checkCompanyInfo.TransFlag = 2;
                    await _context.SaveChangesAsync();

                }

                return ObjCompanyInfo;

            }
            catch (Exception ex)
            {
                throw;

            }
        }

        public async Task<bool> DeleteCompanyInfo(int id, int Userid, HrhubContext _context)
        {
            try
            {
                bool check = false;
                var CompanyInfo = await _context.Companies.FirstOrDefaultAsync(x => x.CompanyId == id && x.IsDeleted == false);

                if (CompanyInfo != null)
                {
                    CompanyInfo.IsDeleted = true;
                    CompanyInfo.UpdatedOn = DateTime.Now;
                    CompanyInfo.UpdatedBy = Userid;
                    check = true;

                }
                await _context.SaveChangesAsync();
                return check;
            }
            catch (Exception)
            {

                throw;
            }


        }


        public async Task<bool> AlreadyCompanyEmailExist(int CompanyInfoId, string email, HrhubContext _context)
        {
            try
            {

                if (CompanyInfoId > 0)
                {
                    var result = await _context.Companies.FirstOrDefaultAsync(x => x.Email == email && x.CompanyId != CompanyInfoId && x.IsDeleted == false);
                    if (result != null)
                    {
                        return true;
                    }


                }
                else
                {
                    var result = await _context.Companies.FirstOrDefaultAsync(x => x.Email == email && x.IsDeleted == false);
                    if (result != null)
                    {
                        return true;
                    }

                }

                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> AlreadyCompanyNameExist(int CompanyInfoId, string companyName, HrhubContext _context)
        {
            try
            {

                if (CompanyInfoId > 0)
                {
                    var result = await _context.Companies.FirstOrDefaultAsync(x => x.CompanyName == companyName && x.CompanyId != CompanyInfoId && x.IsDeleted == false);
                    if (result != null)
                    {
                        return true;
                    }


                }
                else
                {
                    var result = await _context.Companies.FirstOrDefaultAsync(x => x.CompanyName == companyName && x.IsDeleted == false);
                    if (result != null)
                    {
                        return true;
                    }

                }

                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }


        //Get data user table for company setting

        public async Task<User> GetUserByCompanyId(int userId, HrhubContext _context)
        {
            try
            {

                var result = await _context.Users.FirstOrDefaultAsync(x => x.UserId == userId);
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return null;

                }



            }
            catch (Exception ex)
            {

                throw;

            }
        }




    }
}
