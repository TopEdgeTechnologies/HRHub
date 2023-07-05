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



    public partial class Leave
    {
        [NotMapped]
        public string? LeaveTypeTitle { get; set; }
        [NotMapped]
        public string? Days { get; set; }

        [NotMapped]
        public string? LeaveStartDate { get; set; }
        [NotMapped]
        public string? LeaveEndDate { get; set; }
        [NotMapped]
        public string? LeaveAppliedOnDate { get; set; }
        [NotMapped]
        public string? LeaveStatus { get; set; }
        [NotMapped]
        public string? StaffRegistrationNo { get; set; }
        [NotMapped]
        public string? StaffName { get; set; }
        [NotMapped]
        public string? StaffSnap { get; set; }
        [NotMapped]
        public string? Department { get; set; }
        [NotMapped]
        public string? Designation { get; set; }
        
        [NotMapped]
        public string? LeaveStatusTitle { get; set; }
        
        [NotMapped]
        public string? CssClass { get; set; }

        [NotMapped]
        public int[]? ForwardToStaffID { get; set; }

        [NotMapped]
        public IEnumerable<LeaveType>? ListleaveTypes { get; set; }
        [NotMapped]
        public IEnumerable<LeaveStatus>? ListleaveStatus { get; set; }
        [NotMapped]
        public IEnumerable<LeaveApproval>? ListLeaveApprovalData { get; set; }

        [NotMapped]
        public int? ForwardByStaffID { get; set; }
        [NotMapped]
        public decimal? RemainingLeaves { get; set; }
        [NotMapped]
        public decimal? ConsumedLeaves { get; set; }
        [NotMapped]
        public IEnumerable<Leave>? ListAllleaves { get; set; }
        [NotMapped]
        public IEnumerable<Leave>? ListNewOrPendingleaves { get; set; }
        public async Task<List<Leave>> GetLeave(int CompanyId,int StaffId, HrhubContext _context)
        {
            try
            {

                DbConnection _db = new DbConnection();

                string query = "EXEC HR.sp_Get_StaffLeaves " + CompanyId + " , " +StaffId+ " , 0 , 0 , '' , 0 ";  
                DataTable dt = _db.ReturnDataTable(query);

                var list = dt.AsEnumerable()
                    .Select(row => new Leave
                    {
                        LeaveId = Convert.ToInt32(row["LeaveId"]),
                        LeaveTypeId = Convert.ToInt32(row["LeaveTypeId"]),
                        LeaveTypeTitle = row["LeaveTypeTitle"].ToString(),
                        LeaveStartDate = Convert.ToDateTime(row["StartDate"]).ToString("dd-MMM-yyyy"),
                        LeaveEndDate = Convert.ToDateTime(row["EndDate"]).ToString("dd-MMM-yyyy"),
                        Days = Convert.ToBoolean(row["MarkAs_HalfLeave"]) == true ? "Half Day" : Convert.ToBoolean(row["MarkAs_ShortLeave"]) == true ? "Quarter Day"
                                              : ((Convert.ToDateTime(row["EndDate"]) - Convert.ToDateTime(row["StartDate"])).Days + 1).ToString() == "1" ?
                                              ((Convert.ToDateTime(row["EndDate"]) - Convert.ToDateTime(row["StartDate"])).Days + 1).ToString() + " Day" : ((Convert.ToDateTime(row["EndDate"]) - Convert.ToDateTime(row["StartDate"])).Days + 1).ToString() + " Days",
                        LeaveSubject = row["LeaveSubject"].ToString(),
                        LeaveAppliedOnDate = Convert.ToDateTime(row["AppliedOn"]).ToString("dd-MMM-yyyy"),
                        LeaveStatusId = Convert.ToInt32(row["LeaveStatusId"]),
						LeaveStatusTitle = row["LeaveStatusTitle"].ToString(),
						CssClass = row["CssClass"].ToString(),
						RemainingLeaves = Convert.ToDecimal(row["RemainingLeaves"]),
                        ConsumedLeaves = Convert.ToDecimal(row["ConsumedLeaves"]),
                        MarkAsHalfLeave = Convert.ToBoolean(row["MarkAs_HalfLeave"]),
                        MarkAsShortLeave = Convert.ToBoolean(row["MarkAs_ShortLeave"]),

                        StaffId = Convert.ToInt32(row["StaffID"]),
                        StaffRegistrationNo = row["RegistrationNo"].ToString(),
                        StaffName = row["FirstName"].ToString() + " " + row["LastName"].ToString(),
                        StaffSnap = string.IsNullOrWhiteSpace(row["SnapPath"].ToString()) ? "/Images/Avatar.png" : row["SnapPath"].ToString(),
                        Department = row["Department"].ToString(),
                        Designation = row["Designation"].ToString()

                    }).OrderByDescending(x=>x.LeaveId)
                    .ToList();
                return list;

                //var list = await (from l in _context.Leaves
                //                  join lt in _context.LeaveTypes on l.LeaveTypeId equals lt.LeaveTypeId
                //                  join s in _context.Staff on l.StaffId equals s.StaffId
                //                  join dep in _context.Departments on s.DepartmentId equals dep.DepartmentId
                //                  join di in _context.Designations on s.DesignationId equals di.DesignationId

                //                  where l.IsDeleted == false
                //                  && lt.IsDeleted == false
                //                  && s.CompanyId == CompanyId
                //                  select new Leave()
                //                  {
                //                      LeaveId = l.LeaveId,
                //                      LeaveTypeTitle = lt.Title,
                //                      LeaveStartDate = Convert.ToDateTime(l.StartDate).ToString("dd-MMM-yyyy"),
                //                      LeaveEndDate = Convert.ToDateTime(l.EndDate).ToString("dd-MMM-yyyy"),
                //                      Days = l.MarkAsHalfLeave == true ? "Half Day" : l.MarkAsShortLeave == true ? "Quarter Day" 
                //                      : ((Convert.ToDateTime(l.EndDate) - Convert.ToDateTime(l.StartDate)).Days + 1).ToString() == "1"?
                //                      ((Convert.ToDateTime(l.EndDate) - Convert.ToDateTime(l.StartDate)).Days + 1).ToString() + " Day" : ((Convert.ToDateTime(l.EndDate) - Convert.ToDateTime(l.StartDate)).Days + 1).ToString() + " Days",
                //                      LeaveSubject = l.LeaveSubject,
                //                      LeaveAppliedOnDate = Convert.ToDateTime(l.AppliedOn).ToString("dd-MMM-yyyy"),
                //                      LeaveStatusId = l.LeaveStatusId,

                //                      StaffId = s.StaffId,
                //                      StaffRegistrationNo = s.RegistrationNo,
                //                      StaffName = s.FirstName,
                //                      Department = dep.Title,
                //                      Designation = di.Title

                //                  }).ToListAsync();

                //return list;



            }
            catch (Exception ex)
            {

                throw;

            }
        }

        public async Task<List<Leave>> GetHRLeave(int CompanyId,int LoginStaffId, HrhubContext _context)
        {
            try
            {

                DbConnection _db = new DbConnection();

                string query = "EXEC HR.sp_Get_StaffHRLeaves " + CompanyId + " , "+ LoginStaffId + " , 0 , 0 , '' , 0 ";
                DataTable dt = _db.ReturnDataTable(query);

                var list = dt.AsEnumerable()
                    .Select(row => new Leave
                    {
                        LeaveId = Convert.ToInt32(row["LeaveId"]),
                        LeaveTypeId = Convert.ToInt32(row["LeaveTypeId"]),
                        LeaveTypeTitle = row["LeaveTypeTitle"].ToString(),
                        LeaveStartDate = Convert.ToDateTime(row["StartDate"]).ToString("dd-MMM-yyyy"),
                        LeaveEndDate = Convert.ToDateTime(row["EndDate"]).ToString("dd-MMM-yyyy"),
                        Days = Convert.ToBoolean(row["MarkAs_HalfLeave"]) == true ? "Half Day" : Convert.ToBoolean(row["MarkAs_ShortLeave"]) == true ? "Quarter Day"
                                              : ((Convert.ToDateTime(row["EndDate"]) - Convert.ToDateTime(row["StartDate"])).Days + 1).ToString() == "1" ?
                                              ((Convert.ToDateTime(row["EndDate"]) - Convert.ToDateTime(row["StartDate"])).Days + 1).ToString() + " Day" : ((Convert.ToDateTime(row["EndDate"]) - Convert.ToDateTime(row["StartDate"])).Days + 1).ToString() + " Days",
                        LeaveSubject = row["LeaveSubject"].ToString(),
                        LeaveAppliedOnDate = Convert.ToDateTime(row["AppliedOn"]).ToString("dd-MMM-yyyy"),
                        LeaveStatusId = Convert.ToInt32(row["LeaveStatusId"]),
                        RemainingLeaves = Convert.ToDecimal(row["RemainingLeaves"]),
                        ConsumedLeaves = Convert.ToDecimal(row["ConsumedLeaves"]),
                        MarkAsHalfLeave = Convert.ToBoolean(row["MarkAs_HalfLeave"]),
                        MarkAsShortLeave = Convert.ToBoolean(row["MarkAs_ShortLeave"]),

                        StaffId = Convert.ToInt32(row["StaffID"]),
                        StaffRegistrationNo = row["RegistrationNo"].ToString(),
                        StaffName = row["FirstName"].ToString() + " " + row["LastName"].ToString(),
                        StaffSnap = string.IsNullOrWhiteSpace(row["SnapPath"].ToString()) ? "/Images/Avatar.png" : row["SnapPath"].ToString(),
                        Department = row["Department"].ToString(),
                        Designation = row["Designation"].ToString()

                    }).OrderByDescending(x => x.LeaveId)
                    .ToList();
                return list;


            }
            catch (Exception ex)
            {

                throw;

            }
        }

        //public async Task<List<Leave>> GetNewOrPendingLeave(int CompanyId, HrhubContext _context)
        //{
        //    try
        //    {

        //        DbConnection _db = new DbConnection();

        //        string query = "EXEC HR.sp_Get_StaffLeaves " + CompanyId;
        //        DataTable dt = _db.ReturnDataTable(query);

        //        var list = dt.AsEnumerable()
        //            .Select(row => new Leave
        //            {
        //                LeaveId = Convert.ToInt32(row["LeaveId"]),
        //                LeaveTypeTitle = row["LeaveTypeTitle"].ToString(),
        //                LeaveStartDate = Convert.ToDateTime(row["StartDate"]).ToString("dd-MMM-yyyy"),
        //                LeaveEndDate = Convert.ToDateTime(row["EndDate"]).ToString("dd-MMM-yyyy"),
        //                Days = Convert.ToBoolean(row["MarkAs_HalfLeave"]) == true ? "Half Day" : Convert.ToBoolean(row["MarkAs_ShortLeave"]) == true ? "Quarter Day"
        //                                      : ((Convert.ToDateTime(row["EndDate"]) - Convert.ToDateTime(row["StartDate"])).Days + 1).ToString() == "1" ?
        //                                      ((Convert.ToDateTime(row["EndDate"]) - Convert.ToDateTime(row["StartDate"])).Days + 1).ToString() + " Day" : ((Convert.ToDateTime(row["EndDate"]) - Convert.ToDateTime(row["StartDate"])).Days + 1).ToString() + " Days",
        //                LeaveSubject = row["LeaveSubject"].ToString(),
        //                LeaveAppliedOnDate = Convert.ToDateTime(row["AppliedOn"]).ToString("dd-MMM-yyyy"),
        //                LeaveStatusId = Convert.ToInt32(row["LeaveStatusId"]),
        //                RemainingLeaves = Convert.ToDecimal(row["RemainingLeaves"]),
        //                ConsumedLeaves = Convert.ToDecimal(row["ConsumedLeaves"]),

        //                StaffRegistrationNo = row["RegistrationNo"].ToString(),
        //                StaffName = row["FirstName"].ToString() + " " + row["LastName"].ToString(),
        //                Department = row["Department"].ToString(),
        //                Designation = row["Designation"].ToString()

        //            }).Where(x=> x.LeaveStatusId == 1 || x.LeaveStatusId == 2)
        //            .ToList();
        //        return list;





        //    }
        //    catch (Exception ex)
        //    {

        //        throw;

        //    }
        //}

        public async Task<List<Leave>> GetLeaveDetailById(int id, HrhubContext _context)
        {
            try
            {


                var list = await (from l in _context.Leaves
                                  join lt in _context.LeaveTypes on l.LeaveTypeId equals lt.LeaveTypeId

                                  where l.LeaveId == id && l.IsDeleted == false
                                    && lt.IsDeleted == false

                                  select new Leave()
                                  {
                                      LeaveId = l.LeaveId,
                                      LeaveTypeTitle = lt.Title,
                                      LeaveStartDate = Convert.ToDateTime(l.StartDate).ToString("dd-MMM-yyyy"),
                                      LeaveEndDate = Convert.ToDateTime(l.EndDate).ToString("dd-MMM-yyyy"),
                                      Days = l.MarkAsHalfLeave == true ? "Half Day" : l.MarkAsShortLeave == true ? "Quarter Day"
                                      : ((Convert.ToDateTime(l.EndDate) - Convert.ToDateTime(l.StartDate)).Days + 1).ToString() == "1" ?
                                      ((Convert.ToDateTime(l.EndDate) - Convert.ToDateTime(l.StartDate)).Days + 1).ToString() + " Day" : ((Convert.ToDateTime(l.EndDate) - Convert.ToDateTime(l.StartDate)).Days + 1).ToString() + " Days",
                                      LeaveSubject = l.LeaveSubject,
                                      ApplicationHtml = l.ApplicationHtml,
                                      LeaveAppliedOnDate = Convert.ToDateTime(l.AppliedOn).ToString("dd-MMM-yyyy"),
                                      LeaveStatusId = l.LeaveStatusId


                                  }).ToListAsync();

                if (list != null)
                {
                    return list;
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

        // Search Leave 

        public async Task<List<Leave>> SearchLeaves(int CompanyId, int StaffId, int LeaveTypeId, int LeaveStatusId, DateTime Month,bool DateFilter, HrhubContext _context)
        {
            try
            {
                DbConnection _db = new DbConnection();

                string query = "EXEC HR.sp_Get_StaffLeaves " + CompanyId + " , " + StaffId + " , " + LeaveTypeId + " , " + LeaveStatusId + " ,'" + Month + "' , "+ DateFilter;
                DataTable dt = _db.ReturnDataTable(query);

                var list = dt.AsEnumerable()
                    .Select(row => new Leave
                    {
                        LeaveId = Convert.ToInt32(row["LeaveId"]),
                        LeaveTypeId = Convert.ToInt32(row["LeaveTypeId"]),
                        LeaveTypeTitle = row["LeaveTypeTitle"].ToString(),
                        LeaveStartDate = Convert.ToDateTime(row["StartDate"]).ToString("dd-MMM-yyyy"),
                        LeaveEndDate = Convert.ToDateTime(row["EndDate"]).ToString("dd-MMM-yyyy"),
                        Days = Convert.ToBoolean(row["MarkAs_HalfLeave"]) == true ? "Half Day" : Convert.ToBoolean(row["MarkAs_ShortLeave"]) == true ? "Quarter Day"
                                              : ((Convert.ToDateTime(row["EndDate"]) - Convert.ToDateTime(row["StartDate"])).Days + 1).ToString() == "1" ?
                                              ((Convert.ToDateTime(row["EndDate"]) - Convert.ToDateTime(row["StartDate"])).Days + 1).ToString() + " Day" : ((Convert.ToDateTime(row["EndDate"]) - Convert.ToDateTime(row["StartDate"])).Days + 1).ToString() + " Days",
                        LeaveSubject = row["LeaveSubject"].ToString(),
                        LeaveAppliedOnDate = Convert.ToDateTime(row["AppliedOn"]).ToString("dd-MMM-yyyy"),
                        LeaveStatusId = Convert.ToInt32(row["LeaveStatusId"]),
                        RemainingLeaves = Convert.ToDecimal(row["RemainingLeaves"]),
                        ConsumedLeaves = Convert.ToDecimal(row["ConsumedLeaves"]),
                        MarkAsHalfLeave = Convert.ToBoolean(row["MarkAs_HalfLeave"]),
                        MarkAsShortLeave = Convert.ToBoolean(row["MarkAs_ShortLeave"]),

                        StaffId = Convert.ToInt32(row["StaffID"]),
                        StaffRegistrationNo = row["RegistrationNo"].ToString(),
                        StaffName = row["FirstName"].ToString() + " " + row["LastName"].ToString(),
                        StaffSnap = string.IsNullOrWhiteSpace(row["SnapPath"].ToString()) ? "/Images/Avatar.png" : row["SnapPath"].ToString(),
                        Department = row["Department"].ToString(),
                        Designation = row["Designation"].ToString()

                    })
                    .ToList();
                return list;




                //var query = from cs in _context.Leaves
                //            join d in _context.Designations on cs.DesignationId equals d.DesignationId

                //            where cs.Name == Name && cs.DesignationId == DesignationId && cs.ExperienceInYears == ExperienceId && cs.CompanyId == CompanyId && cs.IsDeleted == false
                //            select new Candidate
                //            {
                //                CandidateId = cs.CandidateId,
                //                Name = cs.Name,
                //                DesignationId = cs.DesignationId,
                //                DesignationTitle = d.Title,
                //                CoverLetter = cs.CoverLetter,
                //                Email = cs.Email,
                //                Phone = cs.Phone,
                //                CurrentCompany = cs.CurrentCompany,
                //                CurrentDesignation = cs.CurrentDesignation,
                //                CurrentSalary = cs.CurrentSalary,
                //                ExpectedSalary = cs.ExpectedSalary,
                //                ExperienceInMonths = cs.ExperienceInMonths,
                //                ExperienceInYears = cs.ExperienceInYears,
                //                Dob = cs.Dob,
                //                Gender = cs.Gender,
                //                City = cs.City,
                //                Address = cs.Address,
                //                Qualification = cs.Qualification,
                //                ApplyDate = cs.ApplyDate,
                //                Picture = string.IsNullOrWhiteSpace(cs.Picture) ? "" : cs.Picture,
                //                CompanyId = cs.CompanyId,
                //                StatusId = cs.StatusId,
                //                CreatedOn = cs.CreatedOn,
                //                CreatedBy = cs.CreatedBy,
                //                AttachmentPath = string.IsNullOrWhiteSpace(cs.AttachmentPath) ? "" : cs.AttachmentPath




                //            };

                return list != null ? list.OrderByDescending(x => x.LeaveId).ToList() : new List<Leave>();

            }
            catch (Exception ex)
            {

                throw;

            }
        }

        public async Task<Leave> GetLeaveById(int id, HrhubContext _context)
        {
            try
            {

                //var result = await _context.Leaves.FirstOrDefaultAsync(x => x.LeaveId == id && x.IsDeleted == false);

                var result = await (from cs in _context.Leaves
                            join s in _context.Staff on cs.StaffId equals s.StaffId
                             join d in _context.Designations on s.DesignationId equals d.DesignationId
                             join dep in _context.Departments on s.DepartmentId equals dep.DepartmentId

                             where cs.LeaveId == id && cs.IsDeleted == false
                            select new Leave
                            {
                                LeaveId = cs.LeaveId,
                                AppliedOn = cs.AppliedOn,
                                LeaveTypeId = cs.LeaveTypeId,
                                StartDate = cs.StartDate,
                                EndDate = cs.EndDate,
                                LeaveStatusId = cs.LeaveStatusId,
                                ApplicationHtml = cs.ApplicationHtml,
                                LeaveSubject = cs.LeaveSubject,
                                MarkAsHalfLeave = cs.MarkAsHalfLeave,
                                MarkAsShortLeave = cs.MarkAsShortLeave,

                                LeaveStartDate = Convert.ToDateTime(cs.StartDate).ToString("dd-MMM-yyyy"),
                                LeaveEndDate = Convert.ToDateTime(cs.EndDate).ToString("dd-MMM-yyyy"),

                                IsDeleted = cs.IsDeleted,

                                StaffId = cs.StaffId,
                                StaffRegistrationNo = s.RegistrationNo,
                                StaffName = s.FirstName + " " + s.LastName,
                                StaffSnap = string.IsNullOrWhiteSpace(s.SnapPath) ? "/Images/Avatar.png" : s.SnapPath,
                                Department = dep.Title,
                                Designation = d.Title


                            }).FirstOrDefaultAsync();



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

                throw ex;

            }
        }


        public async Task<Leave> PostLeave(Leave LeaveInfo, HrhubContext _context)
        {
            try
            {
                string msg = "";
                var checkLeaveInfo = await _context.Leaves.FirstOrDefaultAsync(x => x.LeaveId == LeaveInfo.LeaveId && x.IsDeleted == false);
                if (checkLeaveInfo != null && checkLeaveInfo.LeaveId > 0)
                {
                    checkLeaveInfo.LeaveId = LeaveInfo.LeaveId;
                    checkLeaveInfo.LeaveTypeId = LeaveInfo.LeaveTypeId;
                    checkLeaveInfo.StartDate = LeaveInfo.StartDate;
                    checkLeaveInfo.EndDate = LeaveInfo.EndDate;
                    checkLeaveInfo.LeaveSubject = LeaveInfo.LeaveSubject;
                    checkLeaveInfo.ApplicationHtml = LeaveInfo.ApplicationHtml;
                    checkLeaveInfo.UpdatedOn = DateTime.Now;
                    checkLeaveInfo.UpdatedBy = LeaveInfo.CreatedBy;

                    await _context.SaveChangesAsync();

                }
                else
                {
                    LeaveInfo.CreatedOn = DateTime.Now;
                    _context.Leaves.Add(LeaveInfo);
                }
                await _context.SaveChangesAsync();

                return checkLeaveInfo;


            }
            catch (Exception ex)
            {

                throw;

            }
        }

        public async Task<Leave> PostLeaveApproval(Leave LeaveInfo, HrhubContext _context)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    string msg = "";

                    List<LeaveApproval> lsobjAca = new List<LeaveApproval>();

                    int a = 0;
                    if (LeaveInfo.ForwardToStaffID != null)
                    {

                        foreach (var item in LeaveInfo.ForwardToStaffID)
                        {

                            LeaveApproval objAca = new LeaveApproval();


                            objAca.LeaveId = LeaveInfo.LeaveId;
                            objAca.ForwardedByStaffId = LeaveInfo.ForwardByStaffID;
                            objAca.ForwardDate = DateTime.Now;
                            objAca.ApprovalByStaffId = LeaveInfo.ForwardToStaffID.ToArray()[a];
                            objAca.LeaveStatusId = 2; // Pending;
                            lsobjAca.Add(objAca);


                            a++;

                        }


                        _context.LeaveApprovals.AddRange(lsobjAca);
                        await _context.SaveChangesAsync();
                    }


                    var checkLeaveInfo = await _context.Leaves.FirstOrDefaultAsync(x => x.LeaveId == LeaveInfo.LeaveId && x.IsDeleted == false);
                    if (checkLeaveInfo != null && checkLeaveInfo.LeaveId > 0)
                    {
                        checkLeaveInfo.LeaveStatusId = 2; // Pending

                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        LeaveInfo.CreatedOn = DateTime.Now;
                        _context.Leaves.Add(LeaveInfo);
                    }
                    await _context.SaveChangesAsync();
                    dbContextTransaction.Commit();
                    return checkLeaveInfo;


                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;

                }
            }
        }

        public async Task<LeaveApproval> GetStaffComments(int Id,int statusid, int staffid, HrhubContext _context)
        {
            try
            {
                var LeaveInfo = await _context.LeaveApprovals.FirstOrDefaultAsync(x => x.LeaveId == Id && x.LeaveStatusId == statusid && x.ApprovalByStaffId == staffid);

                return LeaveInfo != null ? LeaveInfo : new LeaveApproval();

            }
            catch (Exception ex)
            {

                throw;

            }
        }
        public async Task<bool> DeleteLeaveInfo(int id,int UserId, HrhubContext _context)
        {
            try
            {
                bool check = false;
                var LeaveInfo = await _context.Leaves.FirstOrDefaultAsync(x => x.LeaveId == id && x.IsDeleted == false);

                if (LeaveInfo != null)
                {
                    LeaveInfo.IsDeleted = true;
                    LeaveInfo.UpdatedBy = UserId;
                    LeaveInfo.UpdatedOn = DateTime.Now;
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

        public async Task<decimal> CheckLeaveCount(int StaffId, int LeaveTypeID, HrhubContext _context)
        {
            try
            {

                DbConnection _db = new DbConnection();

                //string query = "Select [HR].[Get_RemainingLeavesByStaffID]( " + StaffId + " , " + LeaveTypeID + " ) as RemainingLeave";
				string query = "Select RemainingLeaves FROM [HR].[Get_RemainingLeavesByStaffID]( " + StaffId + " , " + LeaveTypeID + " )";
				decimal RemainingLeave = Convert.ToDecimal(_db.ReturnColumn(query, "RemainingLeaves"));
                
                return RemainingLeave;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<bool> AlreadyExist(int LeaveInfoId, string email, HrhubContext _context)
        {
            try
            {
                //if (LeaveInfoId > 0)
                //{
                //    var result = await _context.Leaves.FirstOrDefaultAsync(x => x.Email == email && x.LeaveId != LeaveInfoId && x.IsDeleted==false);
                //    if (result != null)
                //    {
                //        return true;
                //    }


                //}
                //else
                //{
                //    var result = await _context.Leaves.FirstOrDefaultAsync(x => x.Email == email && x.IsDeleted == false);
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



        public async Task<LeaveApproval> SaveLeaveApprovalDetail(LeaveApproval obj, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    if(obj.ApprovedByDesignationID == obj.FinalApprovalDesignationID)
                    {
                        // Update Leave status 
                        var Info = await hrhubContext.Leaves.FirstOrDefaultAsync(x => x.LeaveId == obj.LeaveId && x.IsDeleted == false);
                        if (Info != null && Info.LeaveId > 0)
                        {
                            Info.LeaveStatusId = obj.LeaveStatusId;
                            Info.UpdatedBy = obj.CreatedBy;
                            Info.UpdatedOn = DateTime.Now;

                            await hrhubContext.SaveChangesAsync();
                        }

                        // Leave Attendance Entry
                        if (obj.LeaveStatusId == 3)
                        {
                            DateTime SelectedDate_From = Convert.ToDateTime(obj.StartDate);
                            DateTime SelectedDate_To = Convert.ToDateTime(obj.EndDate);


                            for (DateTime SelectedDate = SelectedDate_From; SelectedDate.Date <= SelectedDate_To.Date; SelectedDate = SelectedDate.AddDays(1))
                            {
                                #region Attendance Entry
                                AttendanceMaster objModel = new Models.AttendanceMaster();
                                objModel.AttendanceDate = SelectedDate;

                                objModel.StaffId = obj.ApplicantStaffId;
                                objModel.MarkAsShortLeave = obj.MarkAsShortLeave;
                                objModel.MarkAsHalfLeave = obj.MarkAsHalfLeave;
                                objModel.AttendanceStatusId = 4;
                                objModel.LeaveTypeId = obj.LeaveTypeId;
                                objModel.Remarks = "Leave";
                                objModel.CreatedBy = obj.CreatedBy;
                                objModel.CreatedOn = DateTime.Now;

                                hrhubContext.AttendanceMasters.Add(objModel);
                                #endregion

                            }
                        }
                    }

                    // Insertion in LeaveApproval Table
                    var dbResult = await hrhubContext.LeaveApprovals.FirstOrDefaultAsync(x => x.LeaveId == obj.LeaveId && x.ApprovalByStaffId == obj.ApprovalByStaffId);
                    if (dbResult != null)
                    {
                        dbResult.LeaveId = obj.LeaveId;
                        dbResult.ApprovalByStaffId = obj.ApprovalByStaffId;
                        dbResult.ApprovalDate = obj.ApprovalDate;
                        dbResult.LeaveStatusId = obj.LeaveStatusId;
                        dbResult.Remarks = obj.Remarks;

                        await hrhubContext.SaveChangesAsync();
                        dbContextTransaction.Commit();
                        return dbResult;
                    }
                    else
                    {

                        hrhubContext.LeaveApprovals.Add(obj);
                        await hrhubContext.SaveChangesAsync();
                       
                    }
                    dbContextTransaction.Commit();
                    return obj;

                }
                catch(Exception ex)
                {
                    dbContextTransaction.Rollback(); throw ex;
                }

            }
        }

        public async Task<List<LeaveStatus>> GetLeaveStatus(HrhubContext hrhubContext)
        {
            try
            {
                List<LeaveStatus> objleaveStatus = new List<LeaveStatus>();
                objleaveStatus = await hrhubContext.LeaveStatuses.Where(x => x.IsDeleted == false && x.Status == true).ToListAsync();
                return objleaveStatus;
            }
            catch { throw; }
        }


        public async Task<LeaveApprovalSetting> GetLeaveApprovalSetting(int CompanyId, HrhubContext _context)
        {

            try
            {
                return await _context.LeaveApprovalSettings.FirstOrDefaultAsync(x => x.CompanyId == CompanyId && x.IsDeleted == false);

            }
            catch (Exception ex)
            {

                throw;

            }
        }


    }
}
