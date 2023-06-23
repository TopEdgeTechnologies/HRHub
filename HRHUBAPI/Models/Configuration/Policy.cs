using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System;

using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.ComponentModel.Design;
using System.Data;

namespace HRHUBAPI.Models
{
    public partial class Policy
    {


        [NotMapped]
        public int? Flag { get; set; }
        [NotMapped]
        public int PolicyConfigurationId { get; set; }
        [NotMapped]
        public int? CompanyId { get; set; }
        [NotMapped]
        public int? LateMinutes { get; set; }
        [NotMapped]
        public bool? HalfLeave { get; set; }
        [NotMapped]
        public bool? QuarterLeave { get; set; }
        [NotMapped]
        public bool? HalfDayLateMinutes { get; set; }
        [NotMapped]
        public bool? AllowGraceMinutes { get; set; }
        [NotMapped]
        public int? GraceMinutes { get; set; }

        [NotMapped]
        public IEnumerable<Policy>? ListPolicy { get; set; }
        public async Task<List<Policy>> GetPolicy(HrhubContext _context)
        {
            try
            {

                var list = await _context.Policies.Where(x => x.Status == true).ToListAsync();

                return list;

            }
            catch (Exception ex)
            {

                throw;

            }
        }

        public async Task<Policy> GetPolicyById(int id, HrhubContext _context)
        {
            try
            {

                var result = await _context.Policies.FirstOrDefaultAsync(x => x.PolicyId == id && x.Status == true);
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

        public async Task<List<Policy>> GetPolicybyCategoryId(int PolicyCategoryId, HrhubContext _context)
        {
            try
            {
                var list = await _context.Policies.Where(x => x.Status == true && x.PolicyCategoryId == PolicyCategoryId).ToListAsync();

                return list;

            }
            catch (Exception ex)
            {

                throw;

            }
        }



        //    public async Task<bool> DeletePolicyInfo(int id,int Userid, HrhubContext _context)
        //    {
        //        try
        //        {
        //            bool check = false;
        //            var PolicyInfo = await _context.Policys.FirstOrDefaultAsync(x => x.PolicyId == id  && x.IsDeleted == false);

        //            if (PolicyInfo != null)
        //            {
        //                PolicyInfo.IsDeleted= true;   
        //                PolicyInfo.UpdatedOn= DateTime.Now;
        //                PolicyInfo.UpdatedBy = Userid;
        //                check = true;

        //            }
        //            await _context.SaveChangesAsync();
        //            return check;
        //        }
        //        catch (Exception)
        //        {

        //            throw;
        //        }


        //    }


        #region AttendanceSettings
        public async Task<Company> PostAttendanceSetting(Company obj, HrhubContext _context)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var checkCompanyInfo = await _context.Companies.FirstOrDefaultAsync(x => x.CompanyId == obj.CompanyId && x.IsDeleted == false);
                    if (checkCompanyInfo != null && checkCompanyInfo.CompanyId > 0)
                    {
                        checkCompanyInfo.OfficeStartTime = obj.OfficeStartTime;
                        checkCompanyInfo.OfficeEndTime = obj.OfficeEndTime;
                        checkCompanyInfo.EmployeeWebCheckIn = obj.EmployeeWebCheckIn;
                        checkCompanyInfo.AttendanceByRosters = obj.AttendanceByRosters;
                        checkCompanyInfo.UpdatedOn = DateTime.Now;
                        checkCompanyInfo.UpdatedBy = Convert.ToInt32(obj.UserId);

                        await _context.SaveChangesAsync();

                    }
                    #region Weekend entry
                    if (obj.WeekendList != null)
                    {

                        foreach (var item in obj.WeekendList)
                        {

                            var checkweekendInfo = await _context.WeekendRules.FirstOrDefaultAsync(x => x.WeekendRuleId == item.WeekendRuleId && x.IsDeleted == false);
                            if (checkweekendInfo != null && checkweekendInfo.WeekendRuleId > 0)
                            {
                                checkweekendInfo.DayName = item.DayName;
                                checkweekendInfo.Status = item.Status;
                                checkweekendInfo.UpdatedBy = Convert.ToInt32(obj.UserId);
                                checkweekendInfo.UpdatedOn = DateTime.Now;
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                WeekendRule wobj = new WeekendRule();
                                wobj.DayName = item.DayName;
                                wobj.Status = item.Status;
                                wobj.CompanyId = obj.CompanyId;
                                wobj.IsDeleted = false;
                                wobj.CreatedBy = Convert.ToInt32(obj.UserId);
                                wobj.CreatedOn = DateTime.Now;
                                _context.WeekendRules.Add(wobj);
                                await _context.SaveChangesAsync();
                            }

                        }

                    }
                    #endregion
                    dbContextTransaction.Commit();
                    return checkCompanyInfo;


                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;

                }
            }
        }

        #region PolicyConfiguration
        public async Task<List<Policy>> GetPolicyByCompanyId(int companyid, int PolicyCategoryId, HrhubContext _context)
        {
            try
            {
                var list = await (from p in _context.PolicyConfigurations
                                  join pp in _context.Policies on p.PolicyId equals pp.PolicyId

                                  where p.IsDeleted == false
                                  && p.CompanyId == companyid
                                  && p.Status == true && pp.Status == true
                                  && pp.PolicyCategoryId == PolicyCategoryId // Attendance policy
                                  select new Policy()
                                  {
                                      PolicyId = pp.PolicyId,
                                      PolicyConfigurationId = p.PolicyConfigurationId,
                                      Title = p.Title,
                                      Status = p.Status,
                                      Description = pp.Description

                                  }).ToListAsync();

                return list;

            }
            catch (Exception ex)
            {

                throw;

            }
        }
        public async Task<List<Policy>> GetAttendancePolicy(HrhubContext _context)
        {
            try
            {

                var list = await _context.Policies.Where(x => x.Status == true && x.PolicyCategoryId == 2).ToListAsync(); // Attendance Policy

                return list;

            }
            catch (Exception ex)
            {

                throw;

            }
        }
        public async Task<bool> AlreadyExist(int PolicyConfigurationId, int policyId, int CompanyId, HrhubContext _context)
        {
            try
            {
                if (PolicyConfigurationId > 0)
                {
                    var result = await _context.PolicyConfigurations.FirstOrDefaultAsync(x => x.PolicyId == policyId && x.CompanyId == CompanyId && x.PolicyConfigurationId != PolicyConfigurationId && x.IsDeleted == false);
                    if (result != null)
                    {
                        return true;
                    }
                }
                else
                {
                    var result = await _context.PolicyConfigurations.FirstOrDefaultAsync(x => x.PolicyId == policyId && x.CompanyId == CompanyId && x.IsDeleted == false);
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
        public async Task<Policy> GetAttendancePolicyConfigurationById(int id, int CompanyId, HrhubContext _context)
        {
            try
            {

                var list = await (from p in _context.PolicyConfigurations
                                  join pp in _context.Policies on p.PolicyId equals pp.PolicyId
                                  join c in _context.Companies on p.CompanyId equals c.CompanyId
                                  join l in _context.LeaveApprovalSettings on p.CompanyId equals l.CompanyId


                                  where p.PolicyConfigurationId == id && p.IsDeleted == false
                                  && p.CompanyId == CompanyId && p.Status == true && pp.Status == true
                                  && pp.PolicyCategoryId == 2

                                  select new Policy()
                                  {
                                      PolicyId = pp.PolicyId,
                                      PolicyConfigurationId = p.PolicyConfigurationId,
                                      Title = p.Title,
                                      Status = p.Status,
                                      Description = pp.Description,
                                      //HalfLeave = l.AllowApplyHalfDayLeave,
                                      //QuarterLeave = l.AllowApplyShortDayLeave,
                                      LateMinutes = c.MarkHalfDayAfterLateMinutes,
                                      HalfDayLateMinutes = c.IsMarkHalfDayAllow,
                                      GraceMinutes = c.StartTimeGraceMinutes,
                                      AllowGraceMinutes = c.IsGraceMinutesAllowed

                                  }).FirstOrDefaultAsync();

                return list;

            }
            catch (Exception ex)
            {

                throw;

            }
        }
        public async Task<PolicyConfiguration> PostAttendancePolicyConfiguration(int id, int policyId, string title, int CompanyId, int UserId, bool HalfDayAfterLateMinutes, int LateMinutes, bool AllowGraceMinutes, int GraceMinutes, HrhubContext _context)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    string msg = "";
                    var checkPolicyInfo = await _context.PolicyConfigurations.FirstOrDefaultAsync(x => x.PolicyConfigurationId == id && x.IsDeleted == false);
                    if (checkPolicyInfo != null && checkPolicyInfo.PolicyConfigurationId > 0)
                    {
                        checkPolicyInfo.PolicyId = policyId;
                        checkPolicyInfo.Title = title;
                        checkPolicyInfo.UpdatedOn = DateTime.Now;
                        checkPolicyInfo.Status = true;
                        checkPolicyInfo.UpdatedBy = UserId;

                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        PolicyConfiguration obj = new PolicyConfiguration();
                        obj.PolicyId = policyId;
                        obj.Title = title;
                        obj.CompanyId = CompanyId;
                        obj.Status = true;
                        obj.IsDeleted = false;
                        obj.CreatedBy = UserId;
                        obj.CreatedOn = DateTime.Now;
                        _context.PolicyConfigurations.Add(obj);
                        await _context.SaveChangesAsync();

                    }

                    #region updation in company table
                    if (policyId == 12) // Tardiness policy
                    {
                        var checkCompanyInfo = await _context.Companies.FirstOrDefaultAsync(x => x.CompanyId == CompanyId && x.IsDeleted == false);
                        if (checkCompanyInfo != null && checkCompanyInfo.CompanyId > 0)
                        {
                            checkCompanyInfo.IsGraceMinutesAllowed = AllowGraceMinutes;
                            checkCompanyInfo.StartTimeGraceMinutes = GraceMinutes;
                            checkCompanyInfo.UpdatedOn = DateTime.Now;
                            checkCompanyInfo.UpdatedBy = UserId;

                            await _context.SaveChangesAsync();

                        }
                    }
                    else if (policyId == 13) // Tardiness Consequences & Accumulated
                    {
                        var checkCompanyInfo = await _context.Companies.FirstOrDefaultAsync(x => x.CompanyId == CompanyId && x.IsDeleted == false);
                        if (checkCompanyInfo != null && checkCompanyInfo.CompanyId > 0)
                        {
                            checkCompanyInfo.MarkHalfDayAfterLateMinutes = LateMinutes;
                            checkCompanyInfo.IsMarkHalfDayAllow = HalfDayAfterLateMinutes;
                            checkCompanyInfo.UpdatedOn = DateTime.Now;
                            checkCompanyInfo.UpdatedBy = UserId;

                            await _context.SaveChangesAsync();

                        }
                    }
                    #endregion

                    await _context.SaveChangesAsync();
                    dbContextTransaction.Commit();
                    return checkPolicyInfo;


                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;

                }
            }
        }
        public async Task<PolicyConfiguration> UpdateStatusByPolicyConfigurationId(PolicyConfiguration ObjPolicyConfigurationInfo, HrhubContext _context)
        {
            try
            {
                string msg = "";
                var checkPolicyConfigurationInfo = await _context.PolicyConfigurations.FirstOrDefaultAsync(x => x.PolicyConfigurationId == ObjPolicyConfigurationInfo.PolicyConfigurationId && x.IsDeleted == false);
                if (checkPolicyConfigurationInfo != null && checkPolicyConfigurationInfo.PolicyConfigurationId > 0)
                {
                    checkPolicyConfigurationInfo.UpdatedOn = DateTime.Now;
                    checkPolicyConfigurationInfo.Status = ObjPolicyConfigurationInfo.Status;
                    checkPolicyConfigurationInfo.UpdatedBy = ObjPolicyConfigurationInfo.UpdatedBy;

                    await _context.SaveChangesAsync();
                    return ObjPolicyConfigurationInfo;

                }
                return ObjPolicyConfigurationInfo;
            }
            catch (Exception ex)
            {

                throw;

            }
        }


        #endregion


        #endregion




        #region LeaveSettings
        public async Task<LeaveApprovalSetting> PostLeaveSetting(LeaveApprovalSetting obj, HrhubContext _context)
        {
            try
            {
                var checkSettingInfo = await _context.LeaveApprovalSettings.FirstOrDefaultAsync(x => x.CompanyId == obj.CompanyId && x.IsDeleted == false);
                if (checkSettingInfo != null && checkSettingInfo.SettingId > 0)
                {
                    checkSettingInfo.FinalApprovalByDesignationId = obj.FinalApprovalByDesignationId;
                    checkSettingInfo.LeaveApprovalLeaveStatusId = obj.LeaveApprovalLeaveStatusId;
                    checkSettingInfo.UpdatedOn = DateTime.Now;
                    checkSettingInfo.UpdatedBy = obj.CreatedBy;

                    await _context.SaveChangesAsync();

                }

                return checkSettingInfo;
            }
            catch (Exception ex)
            {
                throw;

            }
        }
        public async Task<LeaveApprovalSetting> GetLeaveSettingByCompanyId(int companyid, HrhubContext _context)
        {
            try
            {
                var result = await _context.LeaveApprovalSettings.FirstOrDefaultAsync(x => x.CompanyId == companyid && x.IsDeleted == false);
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
        public async Task<PolicyConfiguration> PostLeavePolicyConfiguration(int id, int policyId, string title, int CompanyId, int UserId, bool HalfLeave, bool QuarterLeave, HrhubContext _context)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    string msg = "";
                    var checkPolicyInfo = await _context.PolicyConfigurations.FirstOrDefaultAsync(x => x.PolicyConfigurationId == id && x.IsDeleted == false);
                    if (checkPolicyInfo != null && checkPolicyInfo.PolicyConfigurationId > 0)
                    {
                        checkPolicyInfo.PolicyId = policyId;
                        checkPolicyInfo.Title = title;
                        checkPolicyInfo.UpdatedOn = DateTime.Now;
                        checkPolicyInfo.Status = true;
                        checkPolicyInfo.UpdatedBy = UserId;

                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        PolicyConfiguration obj = new PolicyConfiguration();
                        obj.PolicyId = policyId;
                        obj.Title = title;
                        obj.CompanyId = CompanyId;
                        obj.Status = true;
                        obj.IsDeleted = false;
                        obj.CreatedBy = UserId;
                        obj.CreatedOn = DateTime.Now;
                        _context.PolicyConfigurations.Add(obj);
                        await _context.SaveChangesAsync();

                    }

                    #region updation in leaves setting 
                    if (policyId == 9) // Tardiness policy
                    {
                        var checkLeaveInfo = await _context.LeaveApprovalSettings.FirstOrDefaultAsync(x => x.CompanyId == CompanyId && x.IsDeleted == false);
                        if (checkLeaveInfo != null && checkLeaveInfo.SettingId > 0)
                        {
                            checkLeaveInfo.AllowApplyHalfDayLeave = HalfLeave;
                            checkLeaveInfo.AllowApplyShortDayLeave = QuarterLeave;
                            checkLeaveInfo.UpdatedOn = DateTime.Now;
                            checkLeaveInfo.UpdatedBy = UserId;

                            await _context.SaveChangesAsync();

                        }
                    }
                    #endregion

                    await _context.SaveChangesAsync();
                    dbContextTransaction.Commit();
                    return checkPolicyInfo;


                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;

                }
            }
        }

        public async Task<Policy> GetLeavePolicyConfigurationById(int id, int CompanyId, HrhubContext _context)
        {
            try
            {

                var list = await (from p in _context.PolicyConfigurations
                                  join pp in _context.Policies on p.PolicyId equals pp.PolicyId
                                  join l in _context.LeaveApprovalSettings on p.CompanyId equals l.CompanyId


                                  where p.PolicyConfigurationId == id && p.IsDeleted == false
                                  && p.CompanyId == CompanyId && p.Status == true && pp.Status == true
                                  && pp.PolicyCategoryId == 1 // Leave Category id

                                  select new Policy()
                                  {
                                      PolicyId = pp.PolicyId,
                                      PolicyConfigurationId = p.PolicyConfigurationId,
                                      Title = p.Title,
                                      Status = p.Status,
                                      Description = pp.Description,
                                      HalfLeave = l.AllowApplyHalfDayLeave,
                                      QuarterLeave = l.AllowApplyShortDayLeave

                                  }).FirstOrDefaultAsync();

                return list;

            }
            catch (Exception ex)
            {

                throw;

            }
        }

        #endregion

        #region Payroll Settings

        public async Task<List<TaxSlabSetting>> GetTaxSlabSettingByCompanyId(int CompanyId, HrhubContext hrhubContext)
        {

            try
            {
                return await hrhubContext.TaxSlabSettings.Where(x => x.CompanyId == CompanyId && x.IsDeleted == false).ToListAsync();

            }
            catch (Exception ex)
            {

                throw;

            }

        }



        #endregion


    }
}
