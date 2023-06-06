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
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.SignalR;

namespace HRHUBAPI.Models
{



    public partial class StaffOffBoarding
    {
        [NotMapped]
        public int ClearenceProcessID { get; set; }
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
        public string? RemarksStaffName { get; set; }
        [NotMapped]
        public string? RemarksStaffSnap { get; set; }
        [NotMapped]
        public string? RemarksDepartment { get; set; }
        [NotMapped]
        public string? RemarksDesignation { get; set; }
        [NotMapped]
        public string? ContactNumber1 { get; set; }
        [NotMapped]
        public string? Email { get; set; }
        [NotMapped]
        public string? PermanentAddress { get; set; }
        [NotMapped]
        public string? NationalIdnumber { get; set; }
        [NotMapped]
        public string? JoiningDate { get; set; }
        [NotMapped]
        public int? RemarksFromStaffId { get; set; }
        [NotMapped]
        public string? Remarks { get; set; }
        [NotMapped]
        public string? Attachment { get; set; }
        [NotMapped]
        public string? ClearenceProcessDate { get; set; }
        [NotMapped]
        public bool? IsClearenceFromDepartment { get; set; }
        [NotMapped]
        public bool? AllowExitInterview { get; set; }
        [NotMapped]
        public bool? VisibleExitButton { get; set; }
        [NotMapped]
        public IEnumerable<StaffOffBoarding>? ListAllStaffOffBoardings { get; set; }
        [NotMapped]
        public IEnumerable<Department>? ListDepartments { get; set; }
        [NotMapped]
        public IEnumerable<StaffOffBoarding>? ListClearenceDepartments { get; set; }
        [NotMapped]
        public IEnumerable<Designation>? ListDesignations { get; set; }
        [NotMapped]
        public IEnumerable<OffBoardingType>? ListOffBoardingTypes { get; set; }
        [NotMapped]
        public IEnumerable<StaffOffBoarding>? ListClearenceProcess { get; set; }
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

                                  }).OrderByDescending(x => x.OffBoardingId).ToListAsync();

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

                                  }).OrderByDescending(x => x.OffBoardingId).ToListAsync();

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


        public async Task<StaffOffBoarding> GetStaffOffBoardingById(int id, HrhubContext _context)
        {
            try
            {

                var obj = await (from l in _context.StaffOffBoardings
                                 join lt in _context.OffBoardingTypes on l.OffboardingTypeId equals lt.OffboardingTypeId
                                 join s in _context.Staff on l.StaffId equals s.StaffId
                                 join dep in _context.Departments on s.DepartmentId equals dep.DepartmentId
                                 join di in _context.Designations on s.DesignationId equals di.DesignationId

                                 where l.IsDeleted == false
                                 && l.OffBoardingId == id
                                 select new StaffOffBoarding()
                                 {
                                     OffBoardingId = l.OffBoardingId,
                                     OffBoardingTypeTitle = lt.Title,
                                     AppliedOn = Convert.ToDateTime(l.ApplicationDate).ToString("dd-MMM-yyyy"),
                                     Reason = l.Reason,
                                     InterviewDoneByStaffId = String.IsNullOrWhiteSpace(l.InterviewDoneByStaffId.ToString()) ? 0: l.InterviewDoneByStaffId,
                                     InterviewDate = l.InterviewDate,

                                     StaffId = s.StaffId,
                                     StaffRegistrationNo = s.RegistrationNo,
                                     StaffName = s.FirstName + " " + s.LastName,
                                     StaffSnap = string.IsNullOrWhiteSpace(s.SnapPath) ? "/Images/StaffImageEmpty.jpg" : s.SnapPath,
                                     ContactNumber1 = s.ContactNumber1,
                                     Email = s.Email,
                                     PermanentAddress = s.PermanentAddress,
                                     NationalIdnumber = s.NationalIdnumber,
                                     JoiningDate = Convert.ToDateTime(s.JoiningDate).ToString("dd-MMM-yyyy"),
                                     Department = dep.Title,
                                     Designation = di.Title,
                                     StaffStatusId = s.StaffStatusId,

                                 }).FirstOrDefaultAsync();

                return obj;


            }
            catch (Exception ex)
            {

                throw;

            }
        }

        public async Task<List<StaffOffBoarding>> GetClearenceProcessData(int id, HrhubContext _context)
        {
            try
            {

                var list = await (from l in _context.ClearenceProcesses
                                  join s in _context.Staff on l.RemarksFromStaffId equals s.StaffId
                                  join dep in _context.Departments on s.DepartmentId equals dep.DepartmentId
                                  join di in _context.Designations on s.DesignationId equals di.DesignationId

                                  where l.OffBoardingId == id
                                  select new StaffOffBoarding()
                                  {
                                      ClearenceProcessID = l.ClearenceProcessId,
                                      OffBoardingId = l.OffBoardingId,
                                      RemarksFromStaffId = l.RemarksFromStaffId,
                                      ClearenceProcessDate = l.ProcessDate.ToString(),
                                      RemarksStaffName = s.FirstName + " " + s.LastName,
                                      RemarksStaffSnap = string.IsNullOrWhiteSpace(s.SnapPath) ? "/Images/StaffImageEmpty.jpg" : s.SnapPath,
                                      RemarksDepartment = dep.Title,
                                      RemarksDesignation = di.Title,
                                      Remarks = l.Remarks,
                                      Attachment = l.Attachment,

                                  }).OrderByDescending(x => x.ClearenceProcessID).ToListAsync();

                return list;


            }
            catch (Exception ex)
            {

                throw;

            }
        }
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

        public async Task<ClearenceProcess> PostClearenceProcess(ClearenceProcess StaffOffBoardingInfo, HrhubContext _context)
        {
            try
            {
                string msg = "";
                var checkStaffOffBoardingInfo = await _context.ClearenceProcesses.FirstOrDefaultAsync(x => x.OffBoardingId == StaffOffBoardingInfo.OffBoardingId && x.RemarksFromStaffId == StaffOffBoardingInfo.RemarksFromStaffId);
                if (checkStaffOffBoardingInfo != null && checkStaffOffBoardingInfo.OffBoardingId > 0)
                {
                    checkStaffOffBoardingInfo.OffBoardingId = StaffOffBoardingInfo.OffBoardingId;
                    checkStaffOffBoardingInfo.RemarksFromStaffId = StaffOffBoardingInfo.RemarksFromStaffId;
                    checkStaffOffBoardingInfo.Remarks = StaffOffBoardingInfo.Remarks;
                    checkStaffOffBoardingInfo.ProcessDate = StaffOffBoardingInfo.ProcessDate;
                    checkStaffOffBoardingInfo.Attachment = StaffOffBoardingInfo.Attachment;

                    await _context.SaveChangesAsync();

                }
                else
                {
                    _context.ClearenceProcesses.Add(StaffOffBoardingInfo);
                }
                await _context.SaveChangesAsync();


                return checkStaffOffBoardingInfo;


            }
            catch (Exception ex)
            {

                throw;

            }
        }
        public async Task<StaffOffBoarding> PostInterviewDetail(StaffOffBoarding StaffOffBoardingInfo, HrhubContext _context)
        {
            try
            {
                var checkStaffOffBoardingInfo = await _context.StaffOffBoardings.FirstOrDefaultAsync(x => x.OffBoardingId == StaffOffBoardingInfo.OffBoardingId && x.IsDeleted == false);
                if (checkStaffOffBoardingInfo != null && checkStaffOffBoardingInfo.OffBoardingId > 0)
                {
                    checkStaffOffBoardingInfo.OffBoardingId = StaffOffBoardingInfo.OffBoardingId;
                    checkStaffOffBoardingInfo.InterviewDoneByStaffId = StaffOffBoardingInfo.InterviewDoneByStaffId;
                    checkStaffOffBoardingInfo.InterviewDate = StaffOffBoardingInfo.InterviewDate;
                    checkStaffOffBoardingInfo.InteriewRemarks = StaffOffBoardingInfo.InteriewRemarks;

                    await _context.SaveChangesAsync();

                }

                var checkStaffInfo = await _context.Staff.FirstOrDefaultAsync(x => x.StaffId == StaffOffBoardingInfo.StaffId && x.IsDeleted == false);
                if (checkStaffInfo != null && checkStaffInfo.StaffId > 0)
                {
                    checkStaffInfo.StaffStatusId = 5;// Resigned
                    checkStaffInfo.UpdatedBy = StaffOffBoardingInfo.UpdatedBy;
                    checkStaffInfo.UpdatedOn = DateTime.Now;

                    await _context.SaveChangesAsync();

                }

                return checkStaffOffBoardingInfo;


            }
            catch (Exception ex)
            {

                throw;

            }
        }

        public async Task<List<StaffOffBoarding>> GetClearenceDepartment(int CompanyId,int id,int LoginStaffId, HrhubContext hrhubContext)
        {
            try
            {

                DbConnection _db = new DbConnection();

                string query = "EXEC Get_ClearenceDepartments " + CompanyId + " , " + id + " , "+ LoginStaffId;
                DataTable dt = _db.ReturnDataTable(query);

                var list = dt.AsEnumerable()
                    .Select(row => new StaffOffBoarding
                    {
                        ClearenceProcessDate = String.IsNullOrWhiteSpace(row["ProcessDate"].ToString())? "" : Convert.ToDateTime(row["ProcessDate"]).ToString("dd-MMM-yyyy"),
                        IsClearenceFromDepartment = Convert.ToBoolean(row["IsClearenceFromDepartment"]),
                        Department = row["Department"].ToString(),
                        AllowExitInterview = Convert.ToBoolean(row["AllowExitInterview"]),
                        VisibleExitButton = Convert.ToBoolean(row["VisibleExitButton"]),

                    }).ToList();
                return list;


            }

            catch { throw; }
        }

        public async Task<bool> GetClearenceInfo(int id, HrhubContext _context)
        {
            try
            {
                bool check = false;

                var obj = await (from l in _context.ClearenceProcesses
                                 join s in _context.Staff on l.RemarksFromStaffId equals s.StaffId
                                 join o in _context.OffBoardingProcessSettings on s.DepartmentId equals o.NeedClearenceFromDepartmentId


                                 //new { s.DepartmentId, s.DesignationId } equals new { o.NeedClearenceFromDepartmentId, o.NeedClearenceFromDesignationId }


                                 where l.OffBoardingId == id
                                 select new ClearenceProcess()
                                 {
                                     ClearenceProcessId = l.ClearenceProcessId

                                 }).FirstOrDefaultAsync();

                if (obj != null)
                {
                    check = true;
                }

                return check;
            }
            catch (Exception)
            {

                throw;
            }


        }

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


    }
}
