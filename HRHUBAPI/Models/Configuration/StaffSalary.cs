using HRHUBAPI.Models.Configuration;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Data;

namespace HRHUBAPI.Models
{
    public partial class StaffSalary
    {
        #region [NotMapped]

            [NotMapped]
            public int? TranFlag { get; set; }

            [NotMapped]
            public int? CompanyId { get; set; }

            [NotMapped]
            public int? SNO { get; set; }

		    [NotMapped]
		    public int? Year { get; set; }

		    [NotMapped]
		    public int? MonthNumber { get; set; }

		    [NotMapped]
		    public string? MonthName { get; set; }

		    [NotMapped]
		    public int? MonthDaysCount { get; set; }

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
		    public decimal? OV_EarningAmount { get; set; }

		    [NotMapped]
		    public decimal? OV_DeductionAmount { get; set; }

		    [NotMapped]
            public decimal? OV_PayableAmount { get; set; }

            [NotMapped]
		    public string? SalaryStatusTitle { get; set; }

            [NotMapped]
            public IEnumerable<StaffSalary>? StaffSalaryList { get; set; }

		    [NotMapped]
		    public string? Category { get; set; }

			[NotMapped]
		    public int? ComponentId { get; set; }

         	[NotMapped]
		    public string? ComponentTitle { get; set; }

            [NotMapped]
		    public decimal? ComponentAmount { get; set; }

            [NotMapped]
            public string? CompanyName { get; set; }

            [NotMapped]
            public string? CompanyAddress { get; set; }

		    [NotMapped]
		    public string? CompanyPhone { get; set; }

		    [NotMapped]
		    public string? CompanyEmail { get; set; }

		    [NotMapped]
            public string? CompanyLogoAttachment { get; set; }

            [NotMapped]
            public IEnumerable<StaffSalary>? StaffSalaryEditList { get; set; }

            [NotMapped]
            public IEnumerable<int>? ListDetailEarningComponentId { get; set; }

		    [NotMapped]
		    public IEnumerable<decimal>? ListDetailEarningComponentAmount { get; set; }

		    [NotMapped]
		    public IEnumerable<int>? ListDetailDeductionComponentId { get; set; }

		    [NotMapped]
		    public IEnumerable<decimal>? ListDetailDeductionComponentAmount { get; set; }


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

        public async Task<List<StaffSalary>> GetStaffSalaryHistory(int CompanyId, string DateFrom, string DateTo, int StaffId)
        {
            try
            {
                DbConnection dbConnection = new DbConnection();
                string query = " SELECT * FROM Payroll.GetStaffSalaryCardOutputHistory("+ CompanyId + ", '" + DateFrom + "', '" + DateTo + "'," + StaffId + ")";
				
				DataTable dt = dbConnection.ReturnDataTable(query);
                var resultRows = dt.AsEnumerable().Select(row => new StaffSalary
                {
					SNO = Convert.ToInt32(row["SNO"]),
					Year = Convert.ToInt32(row["Year"]),
					MonthNumber = Convert.ToInt32(row["MonthNumber"]),
					MonthName = string.IsNullOrWhiteSpace(row["MonthName"].ToString()) ? "" : row["MonthName"].ToString(),
					MonthDaysCount = Convert.ToInt32(row["MonthDaysCount"]),

					CompanyName = string.IsNullOrWhiteSpace(row["CompanyName"].ToString()) ? "" : row["CompanyName"].ToString(),
					CompanyAddress = string.IsNullOrWhiteSpace(row["CompanyAddress"].ToString()) ? "" : row["CompanyAddress"].ToString(),
					CompanyPhone = string.IsNullOrWhiteSpace(row["CompanyPhone"].ToString()) ? "" : row["CompanyPhone"].ToString(),
					CompanyEmail = string.IsNullOrWhiteSpace(row["CompanyEmail"].ToString()) ? "" : row["CompanyEmail"].ToString(),
					CompanyLogoAttachment = string.IsNullOrWhiteSpace(row["CompanyLogoAttachment"].ToString()) ? "" : row["CompanyLogoAttachment"].ToString(),

					StaffId = Convert.ToInt32(row["StaffId"]),
					RegistrationNo = string.IsNullOrWhiteSpace(row["RegistrationNo"].ToString()) ? "" : row["RegistrationNo"].ToString(),
					FirstName = string.IsNullOrWhiteSpace(row["FirstName"].ToString()) ? "" : row["FirstName"].ToString(),
					LastName = string.IsNullOrWhiteSpace(row["LastName"].ToString()) ? "" : row["LastName"].ToString(),
					DesignationTitle = string.IsNullOrWhiteSpace(row["DesignationTitle"].ToString()) ? "" : row["DesignationTitle"].ToString(),
					DepartmentTitle = string.IsNullOrWhiteSpace(row["DepartmentTitle"].ToString()) ? "" : row["DepartmentTitle"].ToString(),

					OV_EarningAmount = string.IsNullOrWhiteSpace(row["OV_EarningAmount"].ToString()) ? 0 : Convert.ToDecimal(row["OV_EarningAmount"]),
					OV_DeductionAmount = string.IsNullOrWhiteSpace(row["OV_DeductionAmount"].ToString()) ? 0 : Convert.ToDecimal(row["OV_DeductionAmount"]),
					OV_PayableAmount = string.IsNullOrWhiteSpace(row["OV_PayableAmount"].ToString()) ? 0 : Convert.ToDecimal(row["OV_PayableAmount"]),
					SalaryStatusTitle = string.IsNullOrWhiteSpace(row["SalaryStatusTitle"].ToString()) ? "" : row["SalaryStatusTitle"].ToString(),

	

				}).ToList();
                return resultRows;
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
					OV_EarningAmount = string.IsNullOrWhiteSpace(row["OV_EarningAmount"].ToString()) ? 0 : Convert.ToDecimal(row["OV_EarningAmount"]),
					OV_DeductionAmount = string.IsNullOrWhiteSpace(row["OV_DeductionAmount"].ToString()) ? 0 : Convert.ToDecimal(row["OV_DeductionAmount"]),
					OV_PayableAmount = string.IsNullOrWhiteSpace(row["OV_PayableAmount"].ToString()) ? 0 : Convert.ToDecimal(row["OV_PayableAmount"]),
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
                    CompanyName = string.IsNullOrWhiteSpace(row["CompanyName"].ToString()) ? "" : row["CompanyName"].ToString(),
                    CompanyAddress = string.IsNullOrWhiteSpace(row["CompanyAddress"].ToString()) ? "" : row["CompanyAddress"].ToString(),
					CompanyPhone = string.IsNullOrWhiteSpace(row["CompanyPhone"].ToString()) ? "" : row["CompanyPhone"].ToString(),
					CompanyEmail = string.IsNullOrWhiteSpace(row["CompanyEmail"].ToString()) ? "" : row["CompanyEmail"].ToString(),
                    CompanyLogoAttachment = string.IsNullOrWhiteSpace(row["CompanyLogoAttachment"].ToString()) ? "" : row["CompanyLogoAttachment"].ToString(),
					StaffId = Convert.ToInt32(row["StaffId"]),
					RegistrationNo = string.IsNullOrWhiteSpace(row["RegistrationNo"].ToString()) ? "" : row["RegistrationNo"].ToString(),
					FirstName = string.IsNullOrWhiteSpace(row["FirstName"].ToString()) ? "" : row["FirstName"].ToString(),
					LastName = string.IsNullOrWhiteSpace(row["LastName"].ToString()) ? "" : row["LastName"].ToString(),
					DesignationTitle = string.IsNullOrWhiteSpace(row["DesignationTitle"].ToString()) ? "" : row["DesignationTitle"].ToString(),
					DepartmentTitle = string.IsNullOrWhiteSpace(row["DepartmentTitle"].ToString()) ? "" : row["DepartmentTitle"].ToString(),
					Category = string.IsNullOrWhiteSpace(row["Category"].ToString()) ? "" : row["Category"].ToString(),
					ComponentId = string.IsNullOrWhiteSpace(row["ComponentID"].ToString()) ? 0 : Convert.ToInt32(row["ComponentID"]),
					ComponentTitle = string.IsNullOrWhiteSpace(row["ComponentTitle"].ToString()) ? "" : row["ComponentTitle"].ToString(),
					ComponentAmount = Convert.ToDecimal(row["ComponentAmount"]),
				}).ToList();
				return resultRows;
			}
			catch (Exception ex) { throw; }
		}

        public async Task<bool> PostStaffSalaryMaster(StaffSalary objStaffSalary)
        {
            try
            {
                DbConnection dbConnection = new DbConnection();
                string query = " EXEC Payroll.StaffSalary_BulkInsert " + objStaffSalary.CompanyId + ", '" + objStaffSalary.SalaryMonth + "', '" + objStaffSalary.SalaryMonth + "'," + objStaffSalary.CreatedBy;
                DataTable dt = dbConnection.ReturnDataTable(query);
                return Convert.ToBoolean(dt.Rows[0][0]);
            }
            catch (Exception e) { return false; }
        }

		public async Task<bool> PutStaffSalaryMaster(StaffSalary objStaffSalary)
		{
			try
			{
				DbConnection dbConnection = new DbConnection();
				string query = " EXEC Payroll.StaffSalary_BulkUpdate '" + objStaffSalary.SalaryMonth + "', " + objStaffSalary.CreatedBy;
				DataTable dt = dbConnection.ReturnDataTable(query);
				return Convert.ToBoolean(dt.Rows[0][0]);
			}
			catch (Exception e) { return false; }
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

        public async Task<bool> AlreadyExistsMaster(int month, int year, HrhubContext hrhubContext)
        {
            try
            {
                if (month > 0 && year > 0)
                {
                    var dayMonth = "01";
                    var strSalaryMonth = $"{year}-{month}-{dayMonth}";

                    var dbResult = await hrhubContext.StaffSalaries.FirstOrDefaultAsync(x => x.IsDeleted == false && x.SalaryMonth == Convert.ToDateTime(strSalaryMonth));
                    if (dbResult != null)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception e) { throw; }
        }

		public async Task<StaffSalary> PostSingleStaffSalary(StaffSalary objStaffSalary, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    var dbResult = await hrhubContext.StaffSalaries.FirstOrDefaultAsync(x => x.IsDeleted == false && x.SalaryMonth == objStaffSalary.SalaryMonth && x.StaffId == objStaffSalary.StaffId);
                    if (dbResult != null && objStaffSalary.StaffId > 0)
                    {
						objStaffSalary.StaffSalaryId = dbResult.StaffSalaryId;

                        dbResult.StaffId = objStaffSalary.StaffId;
                        dbResult.SalaryMonth = objStaffSalary.SalaryMonth;
						//dbResult.GrossSalary = objStaffSalary.GrossSalary;
						dbResult.TotalEarnings = objStaffSalary.TotalEarnings;
						dbResult.TotalDeductions = objStaffSalary.TotalDeductions;
                        dbResult.NetSalary = objStaffSalary.TotalEarnings - objStaffSalary.TotalDeductions;
                        //dbResult.SalaryStatusId = objStaffSalary.SalaryStatusId;
                        dbResult.UpdatedBy = objStaffSalary.UpdatedBy;
                        dbResult.UpdatedOn = DateTime.Now;
                        dbResult.IsDeleted = false;

                        await hrhubContext.SaveChangesAsync();

						PostSingleStaffSalaryDetail(objStaffSalary, hrhubContext);

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

						PostSingleStaffSalaryDetail(objStaffSalary, hrhubContext);

						objStaffSalary.TranFlag = 1;
                        dbContextTransaction.Commit();
                        return objStaffSalary;
                    }
                }
                catch (Exception ex) { dbContextTransaction.Rollback(); throw; }
            }
        }

        private bool PostSingleStaffSalaryDetail(StaffSalary objStaffSalary, HrhubContext hrhubContext)
        {
            try
            {
				var dbResult = hrhubContext.StaffSalaryDetails.Where(x => x.StaffSalaryId == objStaffSalary.StaffSalaryId).ToList();
                if (dbResult != null && dbResult.Count > 0) 
                {
                    hrhubContext.RemoveRange(dbResult);
                    hrhubContext.SaveChanges(); 
                }

                List<StaffSalaryDetail> ListStaffSalaryDetails = new List<StaffSalaryDetail>();

				// Earning Component Section
                if(objStaffSalary.ListDetailEarningComponentId != null && objStaffSalary.ListDetailEarningComponentId.Count() > 0)
                {
				    int i = 0;
                    foreach(var item in objStaffSalary.ListDetailEarningComponentId)
                    {
                        if(item != null && item > 0)
                        {
                            StaffSalaryDetail objStaffSalaryDetail = new StaffSalaryDetail();
                        
                            objStaffSalaryDetail.StaffSalaryId = objStaffSalary.StaffSalaryId;
						    objStaffSalaryDetail.ComponentId = objStaffSalary.ListDetailEarningComponentId.ToArray()[i];
						    objStaffSalaryDetail.Amount = objStaffSalary.ListDetailEarningComponentAmount.ToArray()[i];

                            ListStaffSalaryDetails.Add(objStaffSalaryDetail);   
					    }
                        i++;
                    }
                }
				// End Earning Component Section

				// Deduction Component Section
                if(objStaffSalary.ListDetailDeductionComponentId != null && objStaffSalary.ListDetailDeductionComponentId.Count() > 0)
                {
				    int j = 0;
                    foreach(var item in objStaffSalary.ListDetailDeductionComponentId)
                    {
                        if(item != null && item > 0)
                        {
                            StaffSalaryDetail objStaffSalaryDetail = new StaffSalaryDetail();
                            objStaffSalaryDetail.StaffSalaryId = objStaffSalary.StaffSalaryId;
                            objStaffSalaryDetail.ComponentId = objStaffSalary.ListDetailDeductionComponentId.ToArray()[j];
                            objStaffSalaryDetail.Amount = objStaffSalary.ListDetailDeductionComponentAmount.ToArray()[j];

                            ListStaffSalaryDetails.Add(objStaffSalaryDetail);
                        }
                        j++;
					}
                }
				// End Dedution Component Section

				hrhubContext.AddRange(ListStaffSalaryDetails);
				hrhubContext.SaveChanges();
                return true;

			}
			catch (Exception e) { throw; }
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
