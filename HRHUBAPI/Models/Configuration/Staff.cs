using HRHUBAPI.Models.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Globalization;

namespace HRHUBAPI.Models
{
    public partial class Staff
    {
        [NotMapped] 
        public IEnumerable<Staff>? StaffList { get; set; }
        
        [NotMapped]
        public int TranFlag { get; set; }

        [NotMapped] 
        public int SNO { get; set; }
        
        [NotMapped]
        public string Department { get; set; } = string.Empty;

        [NotMapped]
        public string Designation { get; set; } = string.Empty;

		[NotMapped]
		public IEnumerable< Designation> DepartmentList { get; set; }

		[NotMapped]
		public IEnumerable<Designation> dDesignationList { get; set; }


		//    public async Task<List<Staff>> GetStaffByCompanyId_DB(int CompanyId, HrhubContext hrhubContext)
		//    {
		//        try
		//        {
		//            List<Staff> staff = new List<Staff> ();
		//            staff = await hrhubContext.Staff.Where(x => x.IsDeleted == false && x.CompanyId == CompanyId).ToListAsync();
		//return staff;   
		//        } catch { throw; }
		//    }

		public async Task<List<Staff>> GetStaffByCompanyId(int CompanyId)
		{
			DbConnection _db = new DbConnection();
			try
			{
                string query = "EXEC HR.sp_Get_StaffList " + CompanyId;
				DataTable dt = _db.ReturnDataTable(query);

				var staff = dt.AsEnumerable()
					.Select(row => new Staff
					{
						SNO = Convert.ToInt32(row["SNO"]),
                        FirstName = row["FirstName"].ToString(),
                        LastName = row["LastName"].ToString(),
                        Email = row["Email"].ToString(),
						StaffId = Convert.ToInt32(row["StaffId"]),
						Department = row["Department"].ToString(),
						Designation = row["Designation"].ToString(),
						ContactNumber1 = row["ContactNumber1"].ToString(),
						ContactNumber2 = row["ContactNumber2"].ToString(),
						JoiningDate = Convert.ToDateTime(row["JoiningDate"]),
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
                if(dbResult != null)
                {
                    return dbResult;
                }
                else
                {
                    return null;
                }
            } catch { throw; }
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
            try
            {
                var dbResult = await hrhubContext.Staff.FirstOrDefaultAsync(x => x.IsDeleted == false && x.StaffId == staff.StaffId);
                if(dbResult != null && staff.StaffId > 0)
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
                    dbResult.SalaryMethod = staff.SalaryMethod;
                    dbResult.SalaryAmount = staff.SalaryAmount;
                    dbResult.AccountTitle = staff.AccountTitle;
                    dbResult.BankAccountNumber  = staff.BankAccountNumber;  
                    dbResult.BankName = staff.BankName;
                    dbResult.BankLocation = staff.BankLocation;
                    dbResult.TaxPayerNumber = staff.TaxPayerNumber;
                    dbResult.Status = staff.Status;
                    dbResult.JobDescription = staff.JobDescription;
                    dbResult.UpdatedBy = staff.UpdatedBy;
                    dbResult.UpdatedOn = DateTime.Now;

                    await hrhubContext.SaveChangesAsync();
                    dbResult.TranFlag = 2;
                    return dbResult;
                }
                else
                {
                    staff.CreatedOn = DateTime.Now;
                    staff.IsDeleted = false;

                    hrhubContext.Staff.Add(staff);
                    await hrhubContext.SaveChangesAsync();
                    return staff;
                }
            }catch { throw; }
        }

        public async Task<bool> StaffDelete(int id, HrhubContext hrhubContext)
        {
            try
            {
                bool recordDeleted = false;
                var dbResult = await hrhubContext.Staff.FirstOrDefaultAsync(x => x.IsDeleted == false && x.StaffId == id);
                if(dbResult != null)
                {
                    dbResult.IsDeleted = true;
                    dbResult.UpdatedOn = DateTime.Now;
                    recordDeleted = true;
                }
                await hrhubContext.SaveChangesAsync();
                return recordDeleted;
            } catch { throw; }
        }
        
        public async Task<bool> StaffAlreadyExists(int CompanyId, int id, string ContactNumber, HrhubContext hrhubContext)
        {
            try
            {
                if(id > 0) 
                {
                    var dbResult = await hrhubContext.Staff.FirstOrDefaultAsync(x => x.IsDeleted == false && x.CompanyId == CompanyId && x.ContactNumber1 == ContactNumber && x.StaffId != id);
                    if(dbResult != null)
                    {
                        return true;
                    }
                }
                else
                {
                    var dbReult = await hrhubContext.Staff.FirstOrDefaultAsync(x => x.IsDeleted == false && x.CompanyId == CompanyId && x.ContactNumber1 == ContactNumber);
                    if( dbReult != null )
                    {
                        return true;
                    }
                }
                return false;
            }catch { throw; }
        }



	}
}
