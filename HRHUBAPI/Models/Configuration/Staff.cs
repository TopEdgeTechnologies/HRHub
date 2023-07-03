using HRHUBAPI.Models.Configuration;
using HRHUBAPI.Models.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Globalization;

namespace HRHUBAPI.Models
{
    public partial class Staff
    {

        #region [NotMapped]

            [NotMapped]
            public int? TotalActiveStaff { get; set; }

            [NotMapped]
            public int? TotalMaleStaff { get; set; }

            [NotMapped]
            public int? TotalFemaleStaff { get; set; }
        
            [NotMapped]
            public int? TotalProbationStaff { get; set; }

            [NotMapped]
            public IEnumerable<Staff>? StaffList { get; set; }

            [NotMapped]
            public int? MTDPresentCount { get; set; }

            [NotMapped]
            public int? YTDPaidLeaveCount { get; set; }

            [NotMapped]
            public int? TotalAllowedLeaves { get; set; }

            [NotMapped]
            public int? MonthDaysCount { get; set; }

            [NotMapped]
            public string? MonthName { get; set; }

            [NotMapped]
            public int? MonthNumber { get; set; }

            [NotMapped]
            public int? Year { get; set; }

            [NotMapped]
            public int? TranFlag { get; set; }

            [NotMapped]
            public int? SNO { get; set; }

            [NotMapped]
            public string? DepartmentTitle { get; set; }

            [NotMapped]
            public string? DesignationTitle { get; set; }

            [NotMapped]
            public IEnumerable<SelectListItem>? MaterialStatusList { get; set; } 

            [NotMapped]
            public IEnumerable<SelectListItem>? BloodGroupList { get; set; } 

            [NotMapped]
            public IEnumerable<Department>? DepartmentList { get; set; }

            [NotMapped]
            public IEnumerable<Designation>? DesignationList { get; set; }

            [NotMapped]
            public IEnumerable<SalaryMethod>? SalaryMethodList { get; set; }

            [NotMapped]
            public IEnumerable<string>? DocumentTitle { get; set; }

            [NotMapped]
            public IEnumerable<IFormFile>? DocFiles { get; set; }

            [NotMapped]
            public IEnumerable<string>? DocumentPath { get; set; }

            [NotMapped]
            public IEnumerable<StaffAttachment>? StaffAttachmentList { get; set; }
   
            [NotMapped]
            public IEnumerable<StaffLeaveAllocation>? StaffLeaveAllocationslist { get; set; }

            [NotMapped]
            public IEnumerable<int>? ListLeaveTypeId { get; set; }

            [NotMapped]
            public IEnumerable<int>? ListAllowedLeaves { get; set; }

            [NotMapped]
            public IEnumerable<StaffSalaryComponent>? StaffSalaryComponentList { get; set; }

            [NotMapped]
            public IEnumerable<int>? ListComponentId { get; set; }

            [NotMapped]
            public IEnumerable<decimal>? ListComponentPercentageEarning { get; set; }
            [NotMapped]
            public IEnumerable<decimal>? ListComponentAmountEarning { get; set; }    

        #endregion

        public async Task<Staff> GetStaffStatisticsByCompanyId(int CompanyId)
		{
			DbConnection _db = new DbConnection();
			try
			{
				string query = "EXEC BI.GetStaff_Statistics " + CompanyId;
				DataTable dt = _db.ReturnDataTable(query);
				Staff StaffStatistics = new Staff();

				if (dt.Rows.Count > 0)
				{
					StaffStatistics.TotalActiveStaff = Convert.ToInt32(dt.Rows[0]["TotalActiveStaff"]);
					StaffStatistics.TotalMaleStaff = Convert.ToInt32(dt.Rows[0]["TotalMaleStaff"]);
					StaffStatistics.TotalFemaleStaff = Convert.ToInt32(dt.Rows[0]["TotalFemaleStaff"]);
					StaffStatistics.TotalProbationStaff = Convert.ToInt32(dt.Rows[0]["TotalProbationStaff"]);
				}
				return StaffStatistics;
			}
			catch { throw; }
		}

        public async Task<Staff> GetStaffAttendanceStatistics(int CompanyId, int month, int year, int StaffId)
        {
            try
            {
                DbConnection dbConnection = new DbConnection(); 
                string query = " EXEC BI.GetStaff_Attendance_Statistics " + CompanyId + ", " + month + ", " + year + ", " + StaffId ;
                
                DataTable dt = dbConnection.ReturnDataTable(query);
                Staff StaffAttendanceStatistics = new Staff();
                if(dt.Rows.Count > 0)
                {
                    StaffAttendanceStatistics.MTDPresentCount = Convert.ToInt32(dt.Rows[0]["MTDPresentCount"]);
                    StaffAttendanceStatistics.YTDPaidLeaveCount = Convert.ToInt32(dt.Rows[0]["YTDPaidLeaveCount"]);
                    StaffAttendanceStatistics.TotalAllowedLeaves = Convert.ToInt32(dt.Rows[0]["TotalAllowedPaidLeaveCount"]);
                    StaffAttendanceStatistics.MonthDaysCount = Convert.ToInt32(dt.Rows[0]["MonthDaysCount"]);
                    StaffAttendanceStatistics.MonthName = dt.Rows[0]["MonthName"].ToString();
              }
                return StaffAttendanceStatistics;
            }
            catch (Exception ex) { throw; }
        }

        public async Task<VInfoStaff> GetStaffProfilebyId(int StaffID, HrhubContext hrhubContext)
        {
    
            try
            {
                var result= await hrhubContext.VInfoStaffs.FirstOrDefaultAsync(x => x.StaffId == StaffID && x.IsDeleted == false && x.Status == true);
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch { throw; }
        }


        public async Task<List<Staff>> GetStaffByCompanyId(int CompanyId)
        {
            DbConnection _db = new DbConnection();
            try
            {
                string query = "EXEC HR.GetStaffList " + CompanyId;
                DataTable dt = _db.ReturnDataTable(query);

                var staff = dt.AsEnumerable()
                    .Select(row => new Staff
                    {
                        SnapPath = string.IsNullOrWhiteSpace(row["SnapPath"].ToString()) ? "/Images/Avatar.png" : row["SnapPath"].ToString(),
                        SNO = Convert.ToInt32(row["SNO"]),
                        FirstName = row["FirstName"].ToString(),
                        LastName =  row["LastName"].ToString(),
                        Email = row["Email"].ToString(),
                        StaffId = Convert.ToInt32(row["StaffId"]),
                        RegistrationNo = row["RegistrationNo"].ToString(),
                        DepartmentTitle = row["DepartmentTitle"].ToString(),
                        DesignationTitle = row["DesignationTitle"].ToString(),
                        ContactNumber1 = row["ContactNumber1"].ToString(),
                        ContactNumber2 = string.IsNullOrWhiteSpace(row["ContactNumber2"].ToString()) ? "" : row["ContactNumber2"].ToString(),
                        Gender = row["Gender"].ToString(),
                        //JoiningDate = DateTime.ParseExact(row["JoiningDate"].ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture),
                        Status = Convert.ToBoolean(row["Status"])
                    })
                    .ToList();
                return staff;
            }
            catch { throw; }
        }

        public async Task<Staff> GetStaffById(int Id, HrhubContext hrhubContext)
        {
            try
            {
                var dbResult = await hrhubContext.Staff.FirstOrDefaultAsync(x => x.IsDeleted == false && x.StaffId == Id);
                if (dbResult != null)
                {
                    return dbResult;
                }
                else
                {
                    return null;
                }
            }
            catch { throw; }
        }

        public async Task<List<StaffAttachment>> GetStaffDocumentDetail(int Id, HrhubContext hrhubContext)
        {
            return await hrhubContext.StaffAttachments.Where(x => x.IsDeleted == false && x.StaffId==Id).ToListAsync();     
        }

        public async Task<List<StaffLeaveAllocation>> GetStaffLeaveAllocationsDetail(int CompanyId, int Id, HrhubContext hrhubContext)
        {
            DbConnection _db = new DbConnection();
            try
            {
                string query = "EXEC HR.GetStaffWise_Allowed_Leaves " + CompanyId + ", " + Id;
                DataTable dt = _db.ReturnDataTable(query);

                var leaveAllocation = dt.AsEnumerable()
                    .Select(row => new StaffLeaveAllocation
                    {
                        //SNO = Convert.ToInt32(row["SNO"]),
                        LeaveTypeId = Convert.ToInt32(row["LeaveTypeID"]),
                        LeaveTypeTitle = row["LeaveTypeTitle"].ToString(),
                        NoOfLeavesLimit = string.IsNullOrWhiteSpace(row["NoOfLeaves"].ToString()) ? 0 : Convert.ToInt32(row["NoOfLeaves"]),
                        LeaveAllocationId = string.IsNullOrWhiteSpace(row["LeaveAllocationID"].ToString()) ? 0 : Convert.ToInt32(row["LeaveAllocationID"]),

                        StaffId = string.IsNullOrWhiteSpace(row["StaffID"].ToString()) ? 0 : Convert.ToInt32(row["StaffID"]),
                        AllowedLeaves = string.IsNullOrWhiteSpace(row["AllowedLeaves"].ToString()) ? 0 : Convert.ToInt32(row["AllowedLeaves"]),
                        //Status = row["Status"].ToString(),
                        //ContactNumber2 = string.IsNullOrWhiteSpace(row["ContactNumber2"].ToString()) ? "" : row["IsDeleted"].ToString(),
                        //IsDeleted = row["IsDeleted"].ToString(),
                        //JoiningDate = DateTime.ParseExact(row["JoiningDate"].ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture),
                        //Status = Convert.ToBoolean(row["Status"])
                    })
                    .ToList();
                //return leaveAllocation;
                Staff objStaff = new Staff();
                objStaff.StaffLeaveAllocationslist = leaveAllocation;
                return objStaff.StaffLeaveAllocationslist.ToList();
            }
            catch (Exception e) { throw; }

            //return await hrhubContext.StaffLeaveAllocations.Where(x => x.IsDeleted == false && x.StaffId == Id).ToListAsync();

            //var queryList = (from lt in hrhubContext.LeaveTypes
            //                 join la in hrhubContext.StaffLeaveAllocations
            //                    on lt.LeaveTypeId equals la.LeaveTypeId into leaveAllocations
            //                 from la in leaveAllocations.DefaultIfEmpty()
            //                 where la.StaffId == Id
            //                 select new StaffLeaveAllocation
            //                 {
            //                     LeaveAllocationId = la.LeaveAllocationId,
            //                     LeaveTypeId = lt.LeaveTypeId,
            //                     LeaveTypeTitle = lt.Title,
            //                     StaffId = la.StaffId,
            //                     AllowedLeaves = la.AllowedLeaves,

            //                 })?.ToList();

            //Staff objStaff = new Staff();   
            //objStaff.StaffLeaveAllocationslist = queryList;
            //return objStaff.StaffLeaveAllocationslist.ToList();
        }

        public async Task<List<StaffSalaryComponent>> GetStaffSalaryDetail(int CompanyId, int StaffId, HrhubContext hrhubContext)
        {
            DbConnection _db = new DbConnection();
            try
            {
                var queryList = from ci in hrhubContext.ComponentInfos
                                join ssc in (
                                    from s in hrhubContext.StaffSalaryComponents
                                    where s.StaffId == StaffId
                                    select new
                                    {
                                        s.StaffSalaryComponentId,
                                        s.ComponentId,
                                        s.PercentageValue,
                                        s.ComponentAmount
                                    }
                                ) on ci.ComponentId equals ssc.ComponentId into joinedData
                                from result in joinedData.DefaultIfEmpty()
                                where ci.CompanyId == CompanyId &&
                                      ci.Status == true &&
                                      ci.IsDeleted == false &&
                                      (ci.Category == "Earning" || ci.Category == "Deduction") &&
                                      (ci.CompanyContribution == null || ci.CompanyContribution == 0)
                                orderby ci.Category descending, ci.Title
                                select new StaffSalaryComponent
                                {
                                    ComponentId = ci.ComponentId,
                                    Category = ci.Category,
                                    ComponentTitle = ci.Title,
                                    PercentageValue = result.PercentageValue ?? 0,
                                    ComponentAmount = result.ComponentAmount ?? 0,
                                };

                return await queryList.ToListAsync();

                //var queryList = from ci in hrhubContext.ComponentInfos
                //                join ssc in (
                //                    from s in hrhubContext.StaffSalaryComponents
                //                    where s.StaffId == CompanyId
                //                    select new
                //                    {
                //                        s.StaffSalaryComponentId,
                //                        s.ComponentId,
                //                        s.PercentageValue,
                //                        s.ComponentAmount
                //                    }
                //                ) on ci.ComponentId equals ssc.ComponentId into joinedData
                //                from result in joinedData.DefaultIfEmpty()
                //                where ci.CompanyId == StaffId && 
                //                      ci.Status == true && ci.IsDeleted == false &&
                //                      (ci.Category == "Earning" || ci.Category == "Deduction") &&
                //                      (ci.CompanyContribution == null || ci.CompanyContribution == 0)
                //                orderby ci.Category descending, ci.Title
                //                select new StaffSalaryComponent
                //                {
                //                    StaffSalaryComponentId = result.StaffSalaryComponentId,
                //                    ComponentId = ci.ComponentId,
                //                    Category = ci.Category,
                //                    ComponentTitle = ci.Title,
                //                    PercentageValue = result.PercentageValue,
                //                    ComponentAmount = result.ComponentAmount
                //                };

                //return await queryList.ToListAsync();
            }
            catch (Exception e) { throw; }
        }

        // Get single record of Staff by company ID
        public async Task<Staff> GetStaffCompanyId(int CompanyId, HrhubContext hrhubContext)
		{
			try
			{
				var dbResult = await hrhubContext.Staff.FirstOrDefaultAsync(x => x.IsDeleted == false && x.CompanyId == CompanyId);
				if (dbResult != null)
				{
					return dbResult;
				}
				else
				{
					return null;
				}
			}
			catch { throw; }
		}
        
        public async Task<Staff> PostStaff(Staff staff, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    var dbResult = await hrhubContext.Staff.FirstOrDefaultAsync(x => x.IsDeleted == false && x.StaffId == staff.StaffId);
                    if (dbResult != null && staff.StaffId > 0)
                    {
                        dbResult.DesignationId = staff.DesignationId;
                        dbResult.DepartmentId = staff.DepartmentId;
                        dbResult.RegistrationNo = staff.RegistrationNo;
                        dbResult.NationalIdnumber = staff.NationalIdnumber;
                        dbResult.FirstName = staff.FirstName;
                        dbResult.LastName = staff.LastName;
                        dbResult.FatherName = staff.FatherName;
                        dbResult.Gender = staff.Gender;
                        dbResult.ContactNumber1 = staff.ContactNumber1;
                        dbResult.ContactNumber2 = staff.ContactNumber2;
                        dbResult.EmergencyContact1 = staff.EmergencyContact1;
                        dbResult.EmergencyContact2 = staff.EmergencyContact2;
                        dbResult.Dob = staff.Dob;
                        dbResult.Gender = staff.Gender;
                        dbResult.MaterialStatus = staff.MaterialStatus;
                        dbResult.BloodGroup = staff.BloodGroup;
                        dbResult.Email = staff.Email;
                        dbResult.PresentAddress = staff.PresentAddress;
                        dbResult.PermanentAddress = staff.PermanentAddress;
                        dbResult.JoiningDate = staff.JoiningDate;
                        dbResult.ResigningDate = staff.ResigningDate;
                        dbResult.TerminationDate = staff.TerminationDate;
                        dbResult.SalaryMethodId = staff.SalaryMethodId;
                        dbResult.SalaryAmount = staff.SalaryAmount;
                        dbResult.AccountTitle = staff.AccountTitle;
                        dbResult.BankAccountNumber = staff.BankAccountNumber;
                        dbResult.BankName = staff.BankName;
                        dbResult.BankLocation = staff.BankLocation;
                        dbResult.BankCode = staff.BankCode;
                        dbResult.TaxPayerNumber = staff.TaxPayerNumber;
                        dbResult.Status = staff.Status;
                        dbResult.JobDescription = staff.JobDescription;
                        dbResult.JobTitle = staff.JobTitle;
                        dbResult.SnapPath = staff.SnapPath;
                        dbResult.UpdatedBy = staff.UpdatedBy;
                        dbResult.UpdatedOn = DateTime.Now;
                        staff.IsDeleted = false;

                        await hrhubContext.SaveChangesAsync();

                        //Staff Salary
                        StaffSalaryDetailSave(staff, hrhubContext);

                        //Staff Leave Details
                        StaffLeaveAllocationsDetailSave(staff, hrhubContext);

                        // Staff Document Details
                        DocumentAttachmentDetailSave(staff, hrhubContext);
                        
                        dbResult.TranFlag = 2;
                        dbContextTransaction.Commit();
                        return dbResult;
                    }
                    else
                    {
                        staff.CreatedOn = DateTime.Now;
                        staff.IsDeleted = false;

                        hrhubContext.Staff.Add(staff);
                        await hrhubContext.SaveChangesAsync();

                        //Staff Salary
                        StaffSalaryDetailSave(staff, hrhubContext);

                        //Staff Leave Details
                        StaffLeaveAllocationsDetailSave(staff, hrhubContext);
                        
                        // Staff Document Details
                        DocumentAttachmentDetailSave(staff, hrhubContext);
                        
                        staff.TranFlag = 1;
                        dbContextTransaction.Commit();
                        return staff;
                    }
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        private bool StaffSalaryDetailSave(Staff objStaff, HrhubContext hrhubContext)
        {
            try
            {
                var result = hrhubContext.StaffSalaryComponents.Where(x => x.StaffId == objStaff.StaffId).ToList();
                if (result != null && result.Count > 0)
                {
                    hrhubContext.StaffSalaryComponents.RemoveRange(result);
                    hrhubContext.SaveChanges();
                }

                List<StaffSalaryComponent> ListStaffSalaryComponents = new List<StaffSalaryComponent>();

                int i = 0;
                foreach (var item in objStaff.ListComponentAmountEarning)
                {
                    if (item != null && item > 0)
                    {
                        StaffSalaryComponent staffSalaryComponent = new StaffSalaryComponent();
                        staffSalaryComponent.StaffId = objStaff.StaffId;
                        staffSalaryComponent.ComponentId = objStaff.ListComponentId.ToArray()[i];
                        staffSalaryComponent.ComponentAmount = item;
                        staffSalaryComponent.PercentageValue = objStaff.ListComponentPercentageEarning.ToArray()[i];

                        ListStaffSalaryComponents.Add(staffSalaryComponent);
                    }
                    i++;
                }
                hrhubContext.AddRange(ListStaffSalaryComponents);
				hrhubContext.SaveChanges();
                return true;
            }
            catch (Exception ex) { throw; }
        }

        private bool StaffLeaveAllocationsDetailSave(Staff objStaff, HrhubContext _context)
        {
            try
            {
                List<StaffLeaveAllocation> ListStaffLeaveAllocation = new List<StaffLeaveAllocation>();

                var result = _context.StaffLeaveAllocations.Where(x => x.StaffId == objStaff.StaffId).ToList();   
                if (result != null && result.Count > 0)
                {
                    _context.StaffLeaveAllocations.RemoveRange(result);
                    _context.SaveChanges(); 
                }

                int i = 0;
                foreach (var item in objStaff.ListAllowedLeaves)
                {
                    if(item != null && item > 0)
                    {
                        StaffLeaveAllocation staffLeaveAllocation = new StaffLeaveAllocation();
                        staffLeaveAllocation.LeaveTypeId =  objStaff.ListLeaveTypeId.ToArray()[i];
                        staffLeaveAllocation.StaffId = objStaff.StaffId;
                        staffLeaveAllocation.AllowedLeaves = item;
                        staffLeaveAllocation.IsDeleted = false;
                        staffLeaveAllocation.Status = true;
                        staffLeaveAllocation.CreatedBy = objStaff.CreatedBy;
                        staffLeaveAllocation.CreatedOn = objStaff.CreatedOn;

                        ListStaffLeaveAllocation.Add(staffLeaveAllocation); 
                    }
                    i++;
                }

                _context.StaffLeaveAllocations.AddRange(ListStaffLeaveAllocation);  
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex) { throw; }
        }

        private bool DocumentAttachmentDetailSave(Staff objstaff, HrhubContext _Context)
        {
            try
            {
                if(objstaff.StaffAttachmentList == null ||  objstaff.StaffAttachmentList.Count() == 0)
                {
                    return false;
                }

                foreach (var item in objstaff.StaffAttachmentList)
                {
                    item.StaffId = objstaff.StaffId;
                }

                var result = _Context.StaffAttachments.Where(x => x.StaffId == objstaff.StaffId && x.IsDeleted == false);
                if (result != null && result.Count() > 0)
                {
                    
                    _Context.RemoveRange(result);
                    _Context.SaveChanges();
                    _Context.AddRange(objstaff.StaffAttachmentList);
                    _Context.SaveChanges();
                    return true;
                }
                else
                {
                    _Context.AddRange(objstaff.StaffAttachmentList);
                    _Context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> StaffDelete(int id, HrhubContext hrhubContext)
        {
            try
            {
                bool recordDeleted = false;
                var dbResult = await hrhubContext.Staff.FirstOrDefaultAsync(x => x.IsDeleted == false && x.StaffId == id);
                if (dbResult != null)
                {
                    dbResult.IsDeleted = true;
                    dbResult.UpdatedOn = DateTime.Now;
                    recordDeleted = true;
                }
                await hrhubContext.SaveChangesAsync();
                return recordDeleted;
            }
            catch { throw; }
        }

        public async Task<bool> StaffAlreadyExists(int CompanyId, int id, string nationalId, HrhubContext hrhubContext)
        {
            try
            {
                if (id > 0)
                {
                    var dbResult = await hrhubContext.Staff.FirstOrDefaultAsync(x => x.IsDeleted == false && x.CompanyId == CompanyId && x.NationalIdnumber == nationalId && x.StaffId != id);
                    if (dbResult != null)
                    {
                        return true;
                    }
                }
                else
                {
                    var dbReult = await hrhubContext.Staff.FirstOrDefaultAsync(x => x.IsDeleted == false && x.CompanyId == CompanyId && x.NationalIdnumber == nationalId);
                    if (dbReult != null)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch { throw; }
        }



    }
}
