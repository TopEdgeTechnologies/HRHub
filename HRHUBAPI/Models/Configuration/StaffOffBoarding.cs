using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System;

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualBasic;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using HRHUBAPI.Models.Configuration;
using System.Runtime.Intrinsics.Arm;
using System.ComponentModel.Design;

namespace HRHUBAPI.Models
{



    public partial class StaffOffBoarding
    {
        [NotMapped]
        public string? OffBoardingTypeTitle { get; set; }
        [NotMapped]
        public string? AppliedOn { get; set; }
        [NotMapped]
        public string? StaffRegistrationNo { get; set; }
        [NotMapped]
        public int? StaffStatusId { get; set; }
        [NotMapped]
        public string? StaffName { get; set; }
        [NotMapped]
        public string? StaffSnap { get; set; }
        [NotMapped]
        public string? Department { get; set; }
        [NotMapped]
        public string? Designation { get; set; }
        [NotMapped]
        public IEnumerable<StaffOffBoarding>? ListAllStaffOffBoardings { get; set; }
        [NotMapped]
        public IEnumerable<Department>? ListDepartments { get; set; }
        [NotMapped]
        public IEnumerable<Designation>? ListDesignations { get; set; }
        [NotMapped]
        public IEnumerable<StaffOffBoarding>? ListStaffOffBoardingsAction { get; set; }

        [NotMapped]
        public IEnumerable<OffBoardingType>? ListOffBoardingTypes { get; set; }
        [NotMapped]
        public IEnumerable<Staff>? ListStaffs { get; set; }
        public async Task<List<StaffOffBoarding>> GetStaffOffBoarding(int CompanyId, HrhubContext _context)
        {
            try
            {
                var list = await (from l in _context.StaffOffBoardings
                                  join lt in _context.OffBoardingTypes on l.OffboardingTypeId equals lt.OffboardingTypeId
                                  join s in _context.Staff on l.StaffId equals s.StaffId
                                  join dep in _context.Departments on s.DepartmentId equals dep.DepartmentId
                                  join di in _context.Designations on s.DesignationId equals di.DesignationId

                                  where l.IsDeleted == false
                                  && s.CompanyId == CompanyId
                                  && (s.StaffStatusId == 2 || s.StaffStatusId == 3 || s.StaffStatusId == 5 || s.StaffStatusId == 6)
                                  select new StaffOffBoarding()
                                  {
                                      OffBoardingId = l.OffBoardingId,
                                      OffBoardingTypeTitle = lt.Title,
                                      AppliedOn = Convert.ToDateTime(l.ApplicationDate).ToString("dd-MMM-yyyy"),
                                      Reason = l.Reason,

                                      StaffId = s.StaffId,
                                      StaffRegistrationNo = s.RegistrationNo,
                                      StaffName = s.FirstName + " " + s.LastName,
                                      StaffSnap = string.IsNullOrWhiteSpace(s.SnapPath) ? "/Images/StaffImageEmpty.jpg" : s.SnapPath,
                                      Department = dep.Title,
                                      Designation = di.Title,
                                      StaffStatusId = s.StaffStatusId,

                                  }).ToListAsync();

                return list;
            }
            catch (Exception ex)
            {

                throw;

            }
        }


        public async Task<List<StaffOffBoarding>> SearchStaffOffBoardings(int CompanyId, int DepartmentId, int DesignationId, DateTime Date, HrhubContext _context)
        {
            try
            {
                var list = await (from l in _context.StaffOffBoardings
                                  join lt in _context.OffBoardingTypes on l.OffboardingTypeId equals lt.OffboardingTypeId
                                  join s in _context.Staff on l.StaffId equals s.StaffId
                                  join dep in _context.Departments on s.DepartmentId equals dep.DepartmentId
                                  join di in _context.Designations on s.DesignationId equals di.DesignationId

                                  where l.IsDeleted == false
                                  && s.CompanyId == CompanyId
                                  && s.DepartmentId == DepartmentId && s.DesignationId == DesignationId
                                  && l.ApplicationDate == Date
                                  && (s.StaffStatusId == 2 || s.StaffStatusId == 3 || s.StaffStatusId == 5 || s.StaffStatusId == 6)


                                  select new StaffOffBoarding()
                                  {
                                      OffBoardingId = l.OffBoardingId,
                                      OffBoardingTypeTitle = lt.Title,
                                      AppliedOn = Convert.ToDateTime(l.ApplicationDate).ToString("dd-MMM-yyyy"),
                                      Reason = l.Reason,

                                      StaffId = s.StaffId,
                                      StaffRegistrationNo = s.RegistrationNo,
                                      StaffName = s.FirstName + " " + s.LastName,
                                      StaffSnap = string.IsNullOrWhiteSpace(s.SnapPath) ? "/Images/StaffImageEmpty.jpg" : s.SnapPath,
                                      Department = dep.Title,
                                      Designation = di.Title,
                                      StaffStatusId = s.StaffStatusId,

                                  }).ToListAsync();

                return list;
            }
            catch (Exception ex)
            {

                throw;

            }
        }

        public async Task<List<OffBoardingType>> GetOffBoardingType(HrhubContext _context)
        {
            try
            {
                return await _context.OffBoardingTypes.ToListAsync();  //.Where(x => x.IsDeleted == false && x.CompanyId == CompanyId)

            }
            catch (Exception ex)
            {

                throw;

            }
        }


        //public async Task<List<StaffOffBoarding>> GetStaffOffBoardingAction(int CompanyId, HrhubContext _context)
        //{
        //    try
        //    {
        //        var list = await (from l in _context.StaffOffBoardings
        //                          join lt in _context.OffBoardingTypes on l.OffboardingTypeId equals lt.OffboardingTypeId
        //                          join s in _context.Staff on l.StaffId equals s.StaffId
        //                          join dep in _context.Departments on s.DepartmentId equals dep.DepartmentId
        //                          join di in _context.Designations on s.DesignationId equals di.DesignationId

        //                          where l.IsDeleted == false
        //                          && s.CompanyId == CompanyId
        //                          && s.StaffId == 2 // active
        //                          select new StaffOffBoarding()
        //                          {
        //                              OffBoardingId = l.OffBoardingId,
        //                              OffBoardingTypeTitle = lt.Title,
        //                              AppliedOn = Convert.ToDateTime(l.ApplicationDate).ToString("dd-MMM-yyyy"),
        //                              Reason = l.Reason,

        //                              StaffId = s.StaffId,
        //                              StaffRegistrationNo = s.RegistrationNo,
        //                              StaffName = s.FirstName + " " + s.LastName,
        //                              StaffSnap = string.IsNullOrWhiteSpace(s.SnapPath) ? "/Images/StaffImageEmpty.jpg" : s.SnapPath,
        //                              Department = dep.Title,
        //                              Designation = di.Title,
        //                              StaffStatusId = s.StaffStatusId,

        //                          }).ToListAsync();

        //        return list;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;

        //    }
        //}






        //public async Task<List<StaffOffBoarding>> GetStaffOffBoardingDetailById(int id, HrhubContext _context)
        //{
        //    try
        //    {


        //        var list = await (from l in _context.StaffOffBoardings
        //                          join lt in _context.StaffOffBoardingTypes on l.StaffOffBoardingTypeId equals lt.StaffOffBoardingTypeId

        //                          where l.StaffOffBoardingId == id && l.IsDeleted == false
        //                            && lt.IsDeleted == false

        //                          select new StaffOffBoarding()
        //                          {
        //                              StaffOffBoardingId = l.StaffOffBoardingId,
        //                              StaffOffBoardingTypeTitle = lt.Title,
        //                              StaffOffBoardingStartDate = Convert.ToDateTime(l.StartDate).ToString("dd-MMM-yyyy"),
        //                              StaffOffBoardingEndDate = Convert.ToDateTime(l.EndDate).ToString("dd-MMM-yyyy"),
        //                              Days = l.MarkAsHalfStaffOffBoarding == true ? "Half Day" : l.MarkAsShortStaffOffBoarding == true ? "Quarter Day"
        //                              : ((Convert.ToDateTime(l.EndDate) - Convert.ToDateTime(l.StartDate)).Days + 1).ToString() == "1" ?
        //                              ((Convert.ToDateTime(l.EndDate) - Convert.ToDateTime(l.StartDate)).Days + 1).ToString() + " Day" : ((Convert.ToDateTime(l.EndDate) - Convert.ToDateTime(l.StartDate)).Days + 1).ToString() + " Days",
        //                              StaffOffBoardingSubject = l.StaffOffBoardingSubject,
        //                              ApplicationHtml = l.ApplicationHtml,
        //                              StaffOffBoardingAppliedOnDate = Convert.ToDateTime(l.AppliedOn).ToString("dd-MMM-yyyy"),
        //                              StaffOffBoardingStatusId = l.StaffOffBoardingStatusId


        //                          }).ToListAsync();

        //        if (list != null)
        //        {
        //            return list;
        //        }
        //        else
        //        {
        //            return null;

        //        }



        //    }
        //    catch (Exception ex)
        //    {

        //        throw;

        //    }
        //}

        // Search StaffOffBoarding 



        //public async Task<StaffOffBoarding> GetStaffOffBoardingById(int id, HrhubContext _context)
        //{
        //    try
        //    {

        //        var result = await _context.StaffOffBoardings.FirstOrDefaultAsync(x => x.StaffOffBoardingId == id && x.IsDeleted == false);

        //        if (result != null)
        //        {
        //            return result;
        //        }
        //        else
        //        {
        //            return null;

        //        }



        //    }
        //    catch (Exception ex)
        //    {

        //        throw;

        //    }
        //}


        public async Task<StaffOffBoarding> PostStaffOffBoarding(StaffOffBoarding StaffOffBoardingInfo, HrhubContext _context)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    string msg = "";
                    var checkStaffOffBoardingInfo = await _context.StaffOffBoardings.FirstOrDefaultAsync(x => x.OffBoardingId == StaffOffBoardingInfo.OffBoardingId && x.IsDeleted == false);
                    if (checkStaffOffBoardingInfo != null && checkStaffOffBoardingInfo.OffBoardingId > 0)
                    {
                        checkStaffOffBoardingInfo.OffBoardingId = StaffOffBoardingInfo.OffBoardingId;
                        checkStaffOffBoardingInfo.StaffId = StaffOffBoardingInfo.StaffId;
                        checkStaffOffBoardingInfo.OffboardingTypeId = StaffOffBoardingInfo.OffboardingTypeId;
                        checkStaffOffBoardingInfo.ApplicationDate = StaffOffBoardingInfo.ApplicationDate;
                        checkStaffOffBoardingInfo.LastWorkingDay = StaffOffBoardingInfo.LastWorkingDay;
                        checkStaffOffBoardingInfo.Reason = StaffOffBoardingInfo.Reason;
                        checkStaffOffBoardingInfo.IsImmediate = StaffOffBoardingInfo.IsImmediate;
                        checkStaffOffBoardingInfo.ApplicationHtml = StaffOffBoardingInfo.ApplicationHtml;
                        checkStaffOffBoardingInfo.UpdatedOn = DateTime.Now;
                        checkStaffOffBoardingInfo.UpdatedBy = StaffOffBoardingInfo.CreatedBy;

                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        StaffOffBoardingInfo.CreatedBy = StaffOffBoardingInfo.CreatedBy;
                        StaffOffBoardingInfo.CreatedOn = DateTime.Now;
                        _context.StaffOffBoardings.Add(StaffOffBoardingInfo);
                    }
                    await _context.SaveChangesAsync();


                    var checkStaffInfo = await _context.Staff.FirstOrDefaultAsync(x => x.StaffId == StaffOffBoardingInfo.StaffId && x.IsDeleted == false);
                    if (checkStaffInfo != null && checkStaffInfo.StaffId > 0)
                    {
                        checkStaffInfo.StaffStatusId = 2; // Off Boarding
                        checkStaffInfo.UpdatedOn = DateTime.Now;
                        checkStaffInfo.UpdatedBy = StaffOffBoardingInfo.CreatedBy;
                        await _context.SaveChangesAsync();

                    }
                    await _context.SaveChangesAsync();
                    dbContextTransaction.Commit();

                    return checkStaffOffBoardingInfo;


                }
                catch (Exception ex)
                {

                    throw;

                }
            }
        }

        //public async Task<StaffOffBoarding> PostStaffOffBoardingApproval(StaffOffBoarding StaffOffBoardingInfo, HrhubContext _context)
        //{
        //    using (var dbContextTransaction = _context.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            string msg = "";

        //            List<StaffOffBoardingApproval> lsobjAca = new List<StaffOffBoardingApproval>();

        //            int a = 0;
        //            if (StaffOffBoardingInfo.ForwardToStaffID != null)
        //            {

        //                foreach (var item in StaffOffBoardingInfo.ForwardToStaffID)
        //                {

        //                    StaffOffBoardingApproval objAca = new StaffOffBoardingApproval();


        //                    objAca.StaffOffBoardingId = StaffOffBoardingInfo.StaffOffBoardingId;
        //                    objAca.ForwardedByStaffId = StaffOffBoardingInfo.ForwardByStaffID;
        //                    objAca.ForwardDate = DateTime.Now;
        //                    objAca.ApprovalByStaffId = StaffOffBoardingInfo.ForwardToStaffID.ToArray()[a];
        //                    objAca.StaffOffBoardingStatusId = 2; // Pending;
        //                    lsobjAca.Add(objAca);


        //                    a++;

        //                }


        //                _context.StaffOffBoardingApprovals.AddRange(lsobjAca);
        //                await _context.SaveChangesAsync();
        //            }


        //            var checkStaffOffBoardingInfo = await _context.StaffOffBoardings.FirstOrDefaultAsync(x => x.StaffOffBoardingId == StaffOffBoardingInfo.StaffOffBoardingId && x.IsDeleted == false);
        //            if (checkStaffOffBoardingInfo != null && checkStaffOffBoardingInfo.StaffOffBoardingId > 0)
        //            {
        //                checkStaffOffBoardingInfo.StaffOffBoardingStatusId = 2; // Pending

        //                await _context.SaveChangesAsync();

        //            }
        //            else
        //            {
        //                StaffOffBoardingInfo.CreatedOn = DateTime.Now;
        //                _context.StaffOffBoardings.Add(StaffOffBoardingInfo);
        //            }
        //            await _context.SaveChangesAsync();
        //            dbContextTransaction.Commit();
        //            return checkStaffOffBoardingInfo;


        //        }
        //        catch (Exception ex)
        //        {

        //            throw;

        //        }
        //    }
        //}

        public async Task<bool> DeleteStaffOffBoardingInfo(int id, HrhubContext _context)
        {
            try
            {
                bool check = false;
                var StaffOffBoardingInfo = await _context.StaffOffBoardings.FirstOrDefaultAsync(x => x.OffBoardingId == id && x.IsDeleted == false);

                if (StaffOffBoardingInfo != null)
                {
                    StaffOffBoardingInfo.IsDeleted = true;
                    StaffOffBoardingInfo.UpdatedOn = DateTime.Now;
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

        public async Task<bool> CheckStaffOffBoarding(int StaffId, int StaffOffBoardingTypeID, HrhubContext _context)
        {
            try
            {

                DbConnection _db = new DbConnection();

                string query = "Select [dbo].[Get_RemainingStaffOffBoardingsByStaffID]( " + StaffId + " , " + StaffOffBoardingTypeID + " ) as RemainingStaffOffBoarding";
                int RemainingStaffOffBoarding = Convert.ToInt32(_db.ReturnColumn(query, "RemainingStaffOffBoarding"));
                if (RemainingStaffOffBoarding == 0)
                {
                    return true;
                }


                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<bool> AlreadyExist(int StaffOffBoardingInfoId, string email, HrhubContext _context)
        {
            try
            {
                //if (StaffOffBoardingInfoId > 0)
                //{
                //    var result = await _context.StaffOffBoardings.FirstOrDefaultAsync(x => x.Email == email && x.StaffOffBoardingId != StaffOffBoardingInfoId && x.IsDeleted==false);
                //    if (result != null)
                //    {
                //        return true;
                //    }


                //}
                //else
                //{
                //    var result = await _context.StaffOffBoardings.FirstOrDefaultAsync(x => x.Email == email && x.IsDeleted == false);
                //    if (result != null)
                //    {
                //        return true;
                //    }

                //}

                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        //public async Task<List<StaffOffBoardingStatus>> GetStaffOffBoardingStatus(HrhubContext hrhubContext)
        //{
        //    try
        //    {
        //        List<StaffOffBoardingStatus> objStaffOffBoardingStatus = new List<StaffOffBoardingStatus>();
        //        objStaffOffBoardingStatus = await hrhubContext.StaffOffBoardingStatuses.Where(x => x.IsDeleted == false && x.Status == true).ToListAsync();
        //        return objStaffOffBoardingStatus;
        //    }
        //    catch { throw; }
        //}




        //Load dropdown WeekendRule
        public async Task<List<WeekendRule>> GetWeekendRule(HrhubContext hrhubContext)
        {
            try
            {
                List<WeekendRule> objWeekendRule = new List<WeekendRule>();
                objWeekendRule = await hrhubContext.WeekendRules.Where(x => x.IsDeleted == false && x.Status == true).ToListAsync();
                return objWeekendRule;
            }
            catch { throw; }
        }







        //Load dropdown StaffOffBoarding data Id vise
        //public async Task<List<StaffOffBoarding>> GetStaffOffBoardingIdVise(int StaffOffBoardingId,HrhubContext _context)
        //{
        //    try
        //    {
        //        //var list = await _context.StaffOffBoardingInfos.Where(x=>x.IsDeleted==false).ToListAsync();
        //        var list = await (from c in _context.StaffOffBoardings
        //                          join cl in _context.ClassInfos on c.AppliedForClassId equals cl.ClassId
        //                          join g in _context.GroupInfos on c.GroupId equals g.GroupId
        //                          join s in _context.Sessions on c.SessionId equals s.SessionId

        //                          where c.IsDeleted == false
        //                          && cl.IsDeleted == false
        //                          && g.IsDeleted == false
        //                          && s.IsDeleted == false
        //                          && c.StaffOffBoardingId == StaffOffBoardingId
        //                          select new StaffOffBoarding()
        //                          {
        //                              StaffOffBoardingId = c.StaffOffBoardingId,
        //                              Name = c.Name,
        //                              AppliedForClassId = cl.ClassId,
        //                              ClassTitle = cl.Title,
        //                              GroupId = g.GroupId,
        //                              GroupName = g.Title,
        //                              SessionId = s.SessionId,
        //                              SessionName = s.Title,
        //                              Cnic = c.Cnic,
        //                              AdmissionDate = c.AdmissionDate,
        //                              StaffOffBoardingNo = c.StaffOffBoardingNo,
        //                              Dob = c.Dob,
        //                              FatherName = c.FatherName,
        //                              Gender = c.Gender,
        //                              Address= c.Address,
        //                              City= c.City,
        //                              Mobile = c.Mobile,
        //                              Email= c.Email,
        //                              PreviousSchool= c.PreviousSchool,
        //                              FatherQualification = c.FatherQualification,
        //                              MotherQualification = c.MotherQualification,
        //                              MotherName= c.MotherName,
        //                              ParentStaffId= c.ParentStaffId,
        //                              FirstName = c.FirstName,
        //                              LastName = c.LastName,
        //                              IsActive = c.IsActive


        //                          }).ToListAsync();

        //        return list;



        //    }
        //    catch (Exception ex)
        //    {

        //        throw;

        //    }
        //}





    }
}
