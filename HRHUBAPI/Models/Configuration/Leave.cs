using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System;

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualBasic;
using Microsoft.EntityFrameworkCore.Storage;

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
        public string? Department { get; set; }
        [NotMapped]
        public string? Designation { get; set; }
        [NotMapped]
        public int[]? ForwardToStaffID { get; set; }

        [NotMapped]
        public IEnumerable<LeaveType>? ListleaveTypes { get; set; }
        [NotMapped]
        public IEnumerable<LeaveApproval>? ListLeaveApprovalData { get; set; }

        [NotMapped]
        public int? ForwardByStaffID { get; set; }
        [NotMapped]
        public IEnumerable<Leave>? ListAllleaves { get; set; }
        [NotMapped]
        public IEnumerable<Leave>? ListNewOrPendingleaves { get; set; }
        public async Task<List<Leave>> GetLeave(int CompanyId, HrhubContext _context)
        {
            try
            {

                var list = await (from l in _context.Leaves
                                  join lt in _context.LeaveTypes on l.LeaveTypeId equals lt.LeaveTypeId
                                  join s in _context.Staff on l.StaffId equals s.StaffId
                                  join dep in _context.Departments on s.DepartmentId equals dep.DepartmentId
                                  join di in _context.Designations on s.DesignationId equals di.DesignationId

                                  where l.IsDeleted == false
                                  && lt.IsDeleted == false
                                  && s.CompanyId == CompanyId
                                  select new Leave()
                                  {
                                      LeaveId = l.LeaveId,
                                      LeaveTypeTitle = lt.Title,
                                      LeaveStartDate = Convert.ToDateTime(l.StartDate).ToString("dd-MMM-yyyy"),
                                      LeaveEndDate = Convert.ToDateTime(l.EndDate).ToString("dd-MMM-yyyy"),
                                      Days = l.MarkAsHalfLeave == true ? "Half Day" : l.MarkAsShortLeave == true ? "Short Day" : ((Convert.ToDateTime(l.EndDate) - Convert.ToDateTime(l.StartDate)).Days + 1).ToString(),
                                      LeaveSubject = l.LeaveSubject,
                                      LeaveAppliedOnDate = Convert.ToDateTime(l.AppliedOn).ToString("dd-MMM-yyyy"),
                                      LeaveStatusId = l.LeaveStatusId,

                                      StaffRegistrationNo = s.RegistrationNo,
                                      StaffName = s.FirstName,
                                      Department = dep.Title,
                                      Designation = di.Title

                                  }).ToListAsync();

                return list;



            }
            catch (Exception ex)
            {

                throw;

            }
        }

        public async Task<List<Leave>> GetNewOrPendingLeave(int CompanyId, HrhubContext _context)
        {
            try
            {

                var list = await (from l in _context.Leaves
                                  join lt in _context.LeaveTypes on l.LeaveTypeId equals lt.LeaveTypeId
                                  join s in _context.Staff on l.StaffId equals s.StaffId
                                  join dep in _context.Departments on s.DepartmentId equals dep.DepartmentId
                                  join di in _context.Designations on s.DesignationId equals di.DesignationId

                                  where l.IsDeleted == false
                                  && lt.IsDeleted == false
                                  && (l.LeaveStatusId == 1 || l.LeaveStatusId == 2)
                                   && s.CompanyId == CompanyId
                                  select new Leave()
                                  {
                                      LeaveId = l.LeaveId,
                                      LeaveTypeTitle = lt.Title,
                                      LeaveStartDate = Convert.ToDateTime(l.StartDate).ToString("dd-MMM-yyyy"),
                                      LeaveEndDate = Convert.ToDateTime(l.EndDate).ToString("dd-MMM-yyyy"),
                                      Days = l.MarkAsHalfLeave == true ? "Half Day" : l.MarkAsShortLeave == true ? "Short Day" : ((Convert.ToDateTime(l.EndDate) - Convert.ToDateTime(l.StartDate)).Days + 1).ToString(),
                                      LeaveSubject = l.LeaveSubject,
                                      LeaveAppliedOnDate = Convert.ToDateTime(l.AppliedOn).ToString("dd-MMM-yyyy"),
                                      LeaveStatusId = l.LeaveStatusId,

                                      StaffRegistrationNo = s.RegistrationNo,
                                      StaffName = s.FirstName,
                                      Department = dep.Title,
                                      Designation = di.Title

                                  }).OrderByDescending(x => x.LeaveId).ToListAsync();

                return list;



            }
            catch (Exception ex)
            {

                throw;

            }
        }

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
                                      Days = l.MarkAsHalfLeave == true ? "Half Day" : l.MarkAsShortLeave == true ? "Short Day" : ((Convert.ToDateTime(l.EndDate) - Convert.ToDateTime(l.StartDate)).Days + 1).ToString(),
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

        public async Task<Leave> GetLeaveById(int id, HrhubContext _context)
        {
            try
            {

                var result = await _context.Leaves.FirstOrDefaultAsync(x => x.LeaveId == id && x.IsDeleted == false);

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

                    throw;

                }
            }
        }

        public async Task<bool> DeleteLeaveInfo(int id, HrhubContext _context)
        {
            try
            {
                bool check = false;
                var LeaveInfo = await _context.Leaves.FirstOrDefaultAsync(x => x.LeaveId == id && x.IsDeleted == false);

                if (LeaveInfo != null)
                {
                    LeaveInfo.IsDeleted = true;
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
                    var Info = await hrhubContext.Leaves.FirstOrDefaultAsync(x => x.LeaveId == obj.LeaveId && x.IsDeleted == false);
                    if (Info != null && Info.LeaveId > 0)
                    {
                        Info.LeaveStatusId = obj.LeaveStatusId;

                        await hrhubContext.SaveChangesAsync();
                    }

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
                        dbContextTransaction.Commit();
                        return obj;
                    }

                    

                    


                }
                catch
                {
                    dbContextTransaction.Rollback(); throw;
                }

            }
        }













        //Load dropdown Leave data Id vise
        //public async Task<List<Leave>> GetLeaveIdVise(int LeaveId,HrhubContext _context)
        //{
        //    try
        //    {
        //        //var list = await _context.LeaveInfos.Where(x=>x.IsDeleted==false).ToListAsync();
        //        var list = await (from c in _context.Leaves
        //                          join cl in _context.ClassInfos on c.AppliedForClassId equals cl.ClassId
        //                          join g in _context.GroupInfos on c.GroupId equals g.GroupId
        //                          join s in _context.Sessions on c.SessionId equals s.SessionId
		//Load dropdown LeaveStatus
		public async Task<List<LeaveStatus>> GetLeaveStatus(HrhubContext hrhubContext)
		{
			try
			{
				List<LeaveStatus> objleaveStatus = new List<LeaveStatus>();
				objleaveStatus = await hrhubContext.LeaveStatuses.Where(x => x.IsDeleted == false && x.Status==true).ToListAsync();
				return objleaveStatus;
			}
			catch { throw; }
		}




		//Load dropdown WeekendRule
		public async Task<List<WeekendRule>> GetWeekendRule(HrhubContext hrhubContext)
		{
			try
			{
				List<WeekendRule> objWeekendRule = new List<WeekendRule>();
				objWeekendRule = await hrhubContext.WeekendRules.Where(x => x.IsDeleted == false && x.Status==true).ToListAsync();
				return objWeekendRule;
			}
			catch { throw; }
		}












		//Load dropdown Leave data Id vise
		//public async Task<List<Leave>> GetLeaveIdVise(int LeaveId,HrhubContext _context)
		//{
		//    try
		//    {
		//        //var list = await _context.LeaveInfos.Where(x=>x.IsDeleted==false).ToListAsync();
		//        var list = await (from c in _context.Leaves
		//                          join cl in _context.ClassInfos on c.AppliedForClassId equals cl.ClassId
		//                          join g in _context.GroupInfos on c.GroupId equals g.GroupId
		//                          join s in _context.Sessions on c.SessionId equals s.SessionId

		//                          where c.IsDeleted == false
		//                          && cl.IsDeleted == false
		//                          && g.IsDeleted == false
		//                          && s.IsDeleted == false
		//                          && c.LeaveId == LeaveId
		//                          select new Leave()
		//                          {
		//                              LeaveId = c.LeaveId,
		//                              Name = c.Name,
		//                              AppliedForClassId = cl.ClassId,
		//                              ClassTitle = cl.Title,
		//                              GroupId = g.GroupId,
		//                              GroupName = g.Title,
		//                              SessionId = s.SessionId,
		//                              SessionName = s.Title,
		//                              Cnic = c.Cnic,
		//                              AdmissionDate = c.AdmissionDate,
		//                              LeaveNo = c.LeaveNo,
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
}
