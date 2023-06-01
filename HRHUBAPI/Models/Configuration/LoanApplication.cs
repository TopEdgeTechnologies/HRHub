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



    public partial class LoanApplication
    {
        [NotMapped]
        public string? LoanTypeTitle { get; set; }
        [NotMapped]
        public string? Days { get; set; }

        [NotMapped]
        public string? LoanPaymentDate { get; set; }
        [NotMapped]
        public string? LoanApplicationDate { get; set; }
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
        public int[]? ForwardToStaffID { get; set; }

        [NotMapped]
        public IEnumerable<LoanType>? ListLoanTypes { get; set; }
        [NotMapped]
        public IEnumerable<LoanStatus>? ListLoanStatus { get; set; }
        [NotMapped]
        public IEnumerable<LoanApplicationProcess>? ListLoanApprovalData { get; set; }

        [NotMapped]
        public int? ForwardByStaffID { get; set; }
        [NotMapped]
        public IEnumerable<LoanApplication>? ListLoanData { get; set; }
        [NotMapped]
        public IEnumerable<LoanApplication>? ListNewOrPendingleaves { get; set; }
        public async Task<List<LoanApplication>> GetLoanInfo(int CompanyId,int StaffId, HrhubContext _context)
        {
            try
            {

                var list = await (from l in _context.LoanApplications
                                  join lt in _context.LoanTypes on l.LoanTypeId equals lt.LoanTypeId
                                  join s in _context.Staff on l.StaffId equals s.StaffId
                                  join dep in _context.Departments on s.DepartmentId equals dep.DepartmentId
                                  join di in _context.Designations on s.DesignationId equals di.DesignationId

                                  where l.IsDeleted == false
                                  && lt.IsDeleted == false
                                  && s.CompanyId == CompanyId       
                                  && s.StaffId == StaffId       
                                  select new LoanApplication()
                                  {
                                      LoanApplicationId = l.LoanApplicationId,
                                      LoanTypeTitle = lt.Title,
                                      LoanApplicationDate = Convert.ToDateTime(l.ApplicationDate).ToString("dd-MMM-yyyy"),
                                      //LoanPaymentDate = String.IsNullOrWhiteSpace(l.PaymentDate.ToString()) ? DateTime.Now: Convert.ToDateTime(l.PaymentDate).ToString("dd-MMM-yyyy"),
                                      Amount = l.Amount,
                                      NoOfInstallments = l.NoOfInstallments,
                                      InstallmentPerMonth = l.InstallmentPerMonth,
                                      Reason = l.Reason,
                                      LoanStatusId = l.LoanStatusId,

                                      StaffId = s.StaffId,
                                      StaffRegistrationNo = s.RegistrationNo,
                                      StaffSnap = string.IsNullOrWhiteSpace(s.SnapPath) ? "/Images/Avatar.png" : s.SnapPath.ToString(),
                                      StaffName = s.FirstName,
                                      Department = dep.Title,
                                      Designation = di.Title

                                  }).OrderByDescending(x => x.LoanApplicationId).ToListAsync();

                return list;



            }
            catch (Exception ex)
            {

                throw;

            }
        }

        public async Task<List<LoanApplication>> GetLoanDetailById(int id, HrhubContext _context)
        {
            try
            {


                var list = await (from l in _context.LoanApplications
                                  join lt in _context.LoanTypes on l.LoanTypeId equals lt.LoanTypeId

                                  //where l.LeaveId == id && l.IsDeleted == false
                                  //  && lt.IsDeleted == false

                                  select new LoanApplication()
                                  {
                                      //LoanTypeId = l.LoanTypeId,
                                      //LoanTypeTitle = lt.Title,
                                      //LeaveStartDate = Convert.ToDateTime(l.StartDate).ToString("dd-MMM-yyyy"),
                                      //LeaveEndDate = Convert.ToDateTime(l.EndDate).ToString("dd-MMM-yyyy"),
                                      //Days = l.MarkAsHalfLeave == true ? "Half Day" : l.MarkAsShortLeave == true ? "Quarter Day"
                                      //: ((Convert.ToDateTime(l.EndDate) - Convert.ToDateTime(l.StartDate)).Days + 1).ToString() == "1" ?
                                      //((Convert.ToDateTime(l.EndDate) - Convert.ToDateTime(l.StartDate)).Days + 1).ToString() + " Day" : ((Convert.ToDateTime(l.EndDate) - Convert.ToDateTime(l.StartDate)).Days + 1).ToString() + " Days",
                                      //Reason = l.re,
                                      //ApplicationHtml = l.ApplicationHtml,
                                      //LeaveAppliedOnDate = Convert.ToDateTime(l.AppliedOn).ToString("dd-MMM-yyyy"),
                                      //LeaveStatusId = l.LeaveStatusId


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

        // Search Loan 

        public async Task<List<LoanApplication>> SearchLoan(int CompanyId, int StaffId, int LoanTypeId, int LoanStatusId, DateTime ApplicationDateFrom, DateTime ApplicationDateTo, HrhubContext _context)
        {
            try
            {
                var list = await (from l in _context.LoanApplications
                                  join lt in _context.LoanTypes on l.LoanTypeId equals lt.LoanTypeId
                                  join s in _context.Staff on l.StaffId equals s.StaffId
                                  join dep in _context.Departments on s.DepartmentId equals dep.DepartmentId
                                  join di in _context.Designations on s.DesignationId equals di.DesignationId

                                  where l.IsDeleted == false
                                  && lt.IsDeleted == false
                                  && l.LoanTypeId == LoanTypeId
                                  && l.LoanStatusId == LoanStatusId
                                  && (l.ApplicationDate >= ApplicationDateFrom && l.ApplicationDate <= ApplicationDateTo)
                                  && s.CompanyId == CompanyId
                                  && s.StaffId == StaffId
                                  select new LoanApplication()
                                  {
                                      LoanApplicationId = l.LoanApplicationId,
                                      LoanTypeTitle = lt.Title,
                                      LoanApplicationDate = Convert.ToDateTime(l.ApplicationDate).ToString("dd-MMM-yyyy"),
                                      LoanPaymentDate = Convert.ToDateTime(l.PaymentDate).ToString("dd-MMM-yyyy"),
                                      Amount = l.Amount,
                                      NoOfInstallments = l.NoOfInstallments,
                                      InstallmentPerMonth = l.InstallmentPerMonth,
                                      Reason = l.Reason,
                                      LoanStatusId = l.LoanStatusId,

                                      StaffId = s.StaffId,
                                      StaffRegistrationNo = s.RegistrationNo,
                                      StaffName = s.FirstName,
                                      StaffSnap = string.IsNullOrWhiteSpace(s.SnapPath) ? "/Images/Avatar.png" : s.SnapPath.ToString(),
                                      Department = dep.Title,
                                      Designation = di.Title

                                  }).ToListAsync();


                return list != null ? list.OrderByDescending(x => x.LoanApplicationId).ToList() : new List<LoanApplication>();











            }
            catch (Exception ex)
            {

                throw;

            }
        }

        public async Task<LoanApplication> GetLoanById(int id, HrhubContext _context)
        {
            try
            {

                var result = await _context.LoanApplications.FirstOrDefaultAsync(x => x.LoanApplicationId == id && x.IsDeleted == false);

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


        public async Task<LoanApplication> PostLoan(LoanApplication obj, HrhubContext _context)
        {
            try
            {
                string msg = "";
                var checkLoanInfo = await _context.LoanApplications.FirstOrDefaultAsync(x => x.LoanApplicationId == obj.LoanApplicationId && x.IsDeleted == false);
                if (checkLoanInfo != null && checkLoanInfo.LoanApplicationId > 0)
                {
                    checkLoanInfo.LoanApplicationId = obj.LoanApplicationId;
                    checkLoanInfo.LoanTypeId = obj.LoanTypeId;
                    checkLoanInfo.ApplicationDate = obj.ApplicationDate;
                    checkLoanInfo.Amount = obj.Amount;
                    checkLoanInfo.NoOfInstallments = obj.NoOfInstallments;
                    checkLoanInfo.InstallmentPerMonth = obj.InstallmentPerMonth;
                    checkLoanInfo.Reason = obj.Reason;
                    checkLoanInfo.UpdatedOn = DateTime.Now;
                    checkLoanInfo.UpdatedBy = obj.CreatedBy;

                    await _context.SaveChangesAsync();

                }
                else
                {
                    obj.CreatedBy =obj.CreatedBy;
                    obj.CreatedOn = DateTime.Now;
                    _context.LoanApplications.Add(obj);
                }
                await _context.SaveChangesAsync();

                return checkLoanInfo;


            }
            catch (Exception ex)
            {

                throw;

            }
        }


        public async Task<LoanApplication> PostLeaveApproval(LoanApplication LeaveInfo, HrhubContext _context)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    string msg = "";

                    List<LoanApplication> lsobjAca = new List<LoanApplication>();

                    int a = 0;
                    if (LeaveInfo.ForwardToStaffID != null)
                    {

                        foreach (var item in LeaveInfo.ForwardToStaffID)
                        {

                            LoanApplication objAca = new LoanApplication();


                            objAca.LoanApplicationId = LeaveInfo.LoanApplicationId;
                            //objAca.ForwardedByStaffId = LeaveInfo.ForwardByStaffID;
                            //objAca.ForwardDate = DateTime.Now;
                            //objAca.ApprovalByStaffId = LeaveInfo.ForwardToStaffID.ToArray()[a];
                            //objAca.LeaveStatusId = 2; // Pending;
                            lsobjAca.Add(objAca);


                            a++;

                        }


                        _context.LoanApplications.AddRange(lsobjAca);
                        await _context.SaveChangesAsync();
                    }


                    var checkLeaveInfo = await _context.LoanApplications.FirstOrDefaultAsync(x => x.LoanApplicationId == LeaveInfo.LoanApplicationId);// && x.IsDeleted == false);
                    if (checkLeaveInfo != null && checkLeaveInfo.LoanApplicationId > 0)
                    {
                       // checkLeaveInfo.LeaveStatusId = 2; // Pending

                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        //LeaveInfo.CreatedOn = DateTime.Now;
                        _context.LoanApplications.Add(LeaveInfo);
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

        public async Task<bool> DeleteLoanInfo(int id,int userid, HrhubContext _context)
        {
            try
            {
                bool check = false;
                var LoanInfo = await _context.LoanApplications.FirstOrDefaultAsync(x => x.LoanApplicationId == id && x.IsDeleted == false);

                if (LoanInfo != null)
                {
                    LoanInfo.IsDeleted = true;
                    LoanInfo.UpdatedBy = userid;
                    LoanInfo.UpdatedOn = DateTime.Now;
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

        // using this in leave apply form
        public async Task<List<LoanType>> GetStaffLoanRemainingAmountDetail(int StaffId, HrhubContext _context)
        {

            DbConnection _db = new DbConnection();
            try
            {
                string query = "EXEC sp_Get_StaffRemainingLoan " + StaffId;
                DataTable dt = _db.ReturnDataTable(query);

                var loantypes = dt.AsEnumerable()
                    .Select(row => new LoanType
                    {
                        LoanTypeId = Convert.ToInt32(row["LoanTypeId"]),
                        Title = row["Title"].ToString(),
                        TotalLoanAmount = Convert.ToDecimal(row["TotalLoanAmount"]),
                        LoanPaidAmount = Convert.ToDecimal(row["LoanPaidAmount"]),
                        RemainingLoanAmount = Convert.ToDecimal(row["RemainingLoanAmount"])
                    })
                    .ToList();
                return loantypes;
            }
            catch (Exception ex)
            { throw; }



            //try
            //{
            //    List<LeaveType> list = new List<LeaveType>();


            //    list = await (from l in _context.StaffLeaveAllocations
            //                  join lt in _context.LeaveTypes on l.LeaveTypeId equals lt.LeaveTypeId

            //                  where l.IsDeleted == false
            //                  && lt.IsDeleted == false
            //                  && l.StaffId == StaffId
            //                  select new LeaveType()
            //                  {
            //                      LeaveTypeId = lt.LeaveTypeId,
            //                      Title = lt.Title,
            //                      NoOfLeaves = l.AllowedLeaves

            //                  }).ToListAsync();

            //    return list;

            //}
            //catch (Exception ex)
            //{

            //    throw;

            //}
        }




        public async Task<bool> CheckLeave(int StaffId, int LoanTypeID, HrhubContext _context)
        {
            try
            {

                DbConnection _db = new DbConnection();

                string query = "Select [dbo].[Get_RemainingLeavesByStaffID]( " + StaffId + " , " + LoanTypeID + " ) as RemainingLeave";
                int RemainingLeave = Convert.ToInt32(_db.ReturnColumn(query, "RemainingLeave"));
                if (RemainingLeave == 0)
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



      










        //Load dropdown Leave data Id vise
        //public async Task<List<LoanApplication>> GetLeaveIdVise(int LeaveId,HrhubContext _context)
        //{
        //    try
        //    {
        //        //var list = await _context.LeaveInfos.Where(x=>x.IsDeleted==false).ToListAsync();
        //        var list = await (from c in _context.Leaves
        //                          join cl in _context.ClassInfos on c.AppliedForClassId equals cl.ClassId
        //                          join g in _context.GroupInfos on c.GroupId equals g.GroupId
        //                          join s in _context.Sessions on c.SessionId equals s.SessionId
        //Load dropdown LeaveStatus
        public async Task<List<LoanStatus>> GetLoanStatus(HrhubContext hrhubContext)
        {
            try
            {
                List<LoanStatus> objloanStatus = new List<LoanStatus>();
                return await hrhubContext.LoanStatuses.Where(x => x.IsDeleted == false && x.Status == true).ToListAsync();
            }
            catch { throw; }
        }




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






        //Load dropdown Leave data Id vise
        //public async Task<List<LoanApplication>> GetLeaveIdVise(int LeaveId,HrhubContext _context)
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
        //                          select new LoanApplication()
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
