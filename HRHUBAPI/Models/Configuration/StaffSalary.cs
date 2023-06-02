using HRHUBAPI.Models.Configuration;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace HRHUBAPI.Models
{
    public partial class StaffSalary
    {
        #region [NotMapped]

            [NotMapped]
            public int? TranFlag { get; set; }

            [NotMapped]
            public int? SNO { get; set; }
            
            [NotMapped]
            public string? FirstName { get; set; }

            [NotMapped]
            public string? LastName { get; set; }


            [NotMapped]
            public string? RegistrationNo { get; set; }

            [NotMapped]
            public string? DesignationTitle { get; set; }
            
            [NotMapped]
            public string? JobTitle { get; set; }
        
            [NotMapped]
            public string? DepartmentTitle { get; set; }

		    [NotMapped]
		    public string? ContactNumber { get; set; }

		    [NotMapped]
		    public string? SnapPath { get; set; }
            
            [NotMapped]
		    public string? Email { get; set; }

		    [NotMapped]
            public decimal? CalculatedSalaryAmount { get; set; }

            [NotMapped]
		    public string? SalaryStatusTitle { get; set; }

            [NotMapped]
            public IEnumerable<StaffSalary>? StaffSalaryList { get; set; }

		    //[NotMapped]
		    //public string? SnapPath { get; set; }

		    [NotMapped]
		    public string? Category { get; set; }

			[NotMapped]
		    public string? ComponentTitle { get; set; }

            [NotMapped]
		    public decimal? ComponentAmount { get; set; }

		#endregion

		public async Task<List<StaffSalary>> GetStaffSalary(HrhubContext hrhubContext)
        {
            try
            {
                List<StaffSalary> StaffSalaryes = new List<StaffSalary>();
                StaffSalaryes = await hrhubContext.StaffSalaries.Where(x => x.IsDeleted == false).ToListAsync();
                return StaffSalaryes;
            }
            catch (Exception ex) { throw; }
        }

        public async Task<List<StaffSalary>> GetStaffSalaryByCompanyId(int CompanyId, int month, int year)
        {
            try
            {
                DbConnection dbConnection = new DbConnection();
                string query = " SELECT * FROM Payroll.GetStaffSalaryCardListView(" + CompanyId + ", " + month + ", " + year + ") ";
                DataTable dt = dbConnection.ReturnDataTable(query);
                var resultRows = dt.AsEnumerable().Select(row => new StaffSalary
                {
                    SNO = Convert.ToInt32(row["SNO"]),
					StaffId = Convert.ToInt32(row["StaffId"]),
					RegistrationNo = string.IsNullOrWhiteSpace(row["RegistrationNo"].ToString()) ? "" : row["RegistrationNo"].ToString(),
                    FirstName = string.IsNullOrWhiteSpace(row["FirstName"].ToString()) ? "" : row["FirstName"].ToString(),
                    LastName = string.IsNullOrWhiteSpace(row["LastName"].ToString()) ? "" : row["LastName"].ToString(),
                    DesignationTitle = string.IsNullOrWhiteSpace(row["DesignationTitle"].ToString()) ? "" : row["DesignationTitle"].ToString(),
                    JobTitle = string.IsNullOrWhiteSpace(row["JobTitle"].ToString()) ? "" : row["JobTitle"].ToString(),
                    DepartmentTitle = string.IsNullOrWhiteSpace(row["DepartmentTitle"].ToString()) ? "" : row["DepartmentTitle"].ToString(),
					SnapPath = string.IsNullOrWhiteSpace(row["SnapPath"].ToString()) ? "" : row["SnapPath"].ToString(),
					ContactNumber = string.IsNullOrWhiteSpace(row["ContactNumber"].ToString()) ? "" : row["ContactNumber"].ToString(),
					Email = string.IsNullOrWhiteSpace(row["Email"].ToString()) ? "" : row["Email"].ToString(),
					CalculatedSalaryAmount = Convert.ToDecimal(row["WorkingMinutesSalaryAmount"]),
					SalaryStatusTitle = string.IsNullOrWhiteSpace(row["SalaryStatusTitle"].ToString()) ? "" : row["SalaryStatusTitle"].ToString(),

				}).ToList();    
                return resultRows;
            }
            catch (Exception ex) { throw; }
        }

        public async Task<List<StaffSalary>> GetStaffSalaryById(int CompanyId, int month, int year, int StaffId, HrhubContext hrhubContext)
        {
			try
			{
				DbConnection dbConnection = new DbConnection();
				string query = " SELECT * FROM Payroll.GetStaffSalaryCardOutput(" + CompanyId + ", " + month + ", " + year + ", " + StaffId + ") ";
				DataTable dt = dbConnection.ReturnDataTable(query);
				var resultRows = dt.AsEnumerable().Select(row => new StaffSalary
				{
					StaffId = Convert.ToInt32(row["StaffId"]),
					RegistrationNo = string.IsNullOrWhiteSpace(row["RegistrationNo"].ToString()) ? "" : row["RegistrationNo"].ToString(),
					FirstName = string.IsNullOrWhiteSpace(row["FirstName"].ToString()) ? "" : row["FirstName"].ToString(),
					LastName = string.IsNullOrWhiteSpace(row["LastName"].ToString()) ? "" : row["LastName"].ToString(),
					DesignationTitle = string.IsNullOrWhiteSpace(row["DesignationTitle"].ToString()) ? "" : row["DesignationTitle"].ToString(),
					DepartmentTitle = string.IsNullOrWhiteSpace(row["DepartmentTitle"].ToString()) ? "" : row["DepartmentTitle"].ToString(),
					SnapPath = string.IsNullOrWhiteSpace(row["SnapPath"].ToString()) ? "" : row["SnapPath"].ToString(),
					Category = string.IsNullOrWhiteSpace(row["Category"].ToString()) ? "" : row["Category"].ToString(),
					ComponentTitle = string.IsNullOrWhiteSpace(row["ComponentTitle"].ToString()) ? "" : row["ComponentTitle"].ToString(),
					ComponentAmount = Convert.ToDecimal(row["ComponentAmount"]),
				}).ToList();
				return resultRows;
			}
			catch (Exception ex) { throw; }
		}

        public async Task<StaffSalary> PostStaffSalary(StaffSalary objStaffSalary, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    var dbResult = await hrhubContext.StaffSalaries.FirstOrDefaultAsync(x => x.IsDeleted == false && x.StaffSalaryId == objStaffSalary.StaffSalaryId);
                    if (dbResult != null && objStaffSalary.StaffSalaryId > 0)
                    {
                        dbResult.StaffSalaryId = objStaffSalary.StaffSalaryId;
                        dbResult.StaffId = objStaffSalary.StaffId;
                        dbResult.SalaryMonth = objStaffSalary.SalaryMonth;
                        dbResult.GrossSalary = objStaffSalary.GrossSalary;
                        dbResult.TotalDeductions = objStaffSalary.TotalDeductions;
                        dbResult.TotalEarnings = objStaffSalary.TotalEarnings;
                        dbResult.NetSalary = objStaffSalary.NetSalary;
                        dbResult.SalaryStatusId = objStaffSalary.SalaryStatusId;
                        dbResult.UpdatedBy = objStaffSalary.UpdatedBy;
                        dbResult.UpdatedOn = DateTime.Now;
                        dbResult.IsDeleted = false;

                        await hrhubContext.SaveChangesAsync();
                        dbResult.TranFlag = 2;
                        dbContextTransaction.Commit();
                        return dbResult;
                    }
                    else
                    {
                        objStaffSalary.CreatedOn = DateTime.Now;
                        objStaffSalary.IsDeleted = false;

                        hrhubContext.Add(objStaffSalary);
                        await hrhubContext.SaveChangesAsync();
                        objStaffSalary.TranFlag = 1;
                        dbContextTransaction.Commit();
                        return objStaffSalary;
                    }
                }
                catch (Exception ex) { dbContextTransaction.Rollback(); throw; }
            }
        }

        public async Task<bool> DeleteStaffSalary(int Id, int UserId, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    //bool recordDeleted = false;
                    var dbResult = await hrhubContext.StaffSalaries.FirstOrDefaultAsync(x => x.IsDeleted == false && x.StaffSalaryId == Id);
                    if (dbResult != null)
                    {
                        dbResult.IsDeleted = true;
                        dbResult.UpdatedBy = UserId;
                        dbResult.UpdatedOn = DateTime.Now;
                        await hrhubContext.SaveChangesAsync();
                        //recordDeleted = true;
                        dbContextTransaction.Commit();
                    }
                    return true;
                }
                catch (Exception e) { dbContextTransaction.Rollback(); return false; throw; }
            }
        }

        public async Task<bool> AlreadyExists(int Id, int StaffId, HrhubContext hrhubContext)
        {
            try
            {
                if (Id > 0)
                {
                    var dbResult = await hrhubContext.StaffSalaries.FirstOrDefaultAsync(x => x.IsDeleted == false && x.StaffId == StaffId && x.StaffSalaryId != Id);
                    if (dbResult != null)
                    {
                        return true;
                    }
                }
                else
                {
                    var dbResult = await hrhubContext.StaffSalaries.FirstOrDefaultAsync(x => x.IsDeleted == false && x.StaffId == StaffId);
                    if (dbResult != null)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception e) { throw; }
        }

    }
}
