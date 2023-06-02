using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System;

using System.ComponentModel.DataAnnotations.Schema;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using HRHUBAPI.Models.Configuration;
using System.Data;

namespace HRHUBAPI.Models
{



    public partial class StaffContract
    {
		DbConnection _db = new DbConnection();

		[NotMapped]
		public string? DesigantionName { get; set; }
		[NotMapped]
        public string? EmployementType { get; set; }
		[NotMapped]
		public string? StaffImg { get; set; }
		[NotMapped]
		public string? SatffFName { get; set; }
		[NotMapped]
		public string? SatffLName { get; set; }

		[NotMapped]
		public string? ExpirayStatus { get; set; }

		[NotMapped]
		public int? Renew { get; set; }
		[NotMapped]
        public IEnumerable<StaffContract>? AllStaffContract { get; set; } 
        
        // list all active contract data
        public async Task<List<StaffContract>> GetStaffAllContract(int CompanyId,HrhubContext _context)
        {
            try
            {
				var List = from SC in _context.StaffContracts
							join s in _context.Staff on SC.StaffId equals s.StaffId
                            join D in _context.Designations on s.DesignationId equals D.DesignationId
							join et in _context.EmploymentTypes on SC.EmploymentTypeId equals et.EmploymentTypeId
                            where s.CompanyId== CompanyId  && SC.IsDeleted == false && s.IsDeleted== false && SC.Status == true && s.Status==true && D.IsDeleted== false && D.Status==true && et.IsDeleted==false && et.Status==true

							select new StaffContract
							{
								StaffContractId = SC.StaffContractId,
								EmploymentTypeId = et.EmploymentTypeId,
                                StaffId = s.StaffId,
                                SatffFName = s.FirstName,
                                SatffLName = s.LastName,
								DesigantionName = D.Title,
                                EmployementType = et.Title,
								EndDate = SC.EndDate,
								StaffImg = string.IsNullOrWhiteSpace(s.SnapPath) ? "/Images/Avatar.png" : s.SnapPath, 
								StartDate = SC.StartDate,
                                ContractDuration= SC.ContractDuration,
                                Attachment = SC.Attachment,
                                AdditionalDetails= SC.AdditionalDetails,                                
                                Status= SC.Status
                                
                            };
                             return List != null ? List.OrderByDescending(x => x.StaffContractId).ToList() : new List<StaffContract>();



			}
			catch (Exception ex)
            {

                throw;

            }
        }

		// List Expiry Contract Data
		public async Task<List<StaffContract>> GetStaffExpiredContract(int CompanyId, HrhubContext _context)
		{
			try
			{

				List<StaffContract> list = new List<StaffContract>();

				string query = "EXEC dbo.sp_Get_Expire_ContractStaff " + CompanyId + "  ";
				DataTable dt = _db.ReturnDataTable(query);

				list = dt.AsEnumerable()
					.Select(row => new StaffContract
					{


						StaffContractId = Convert.ToInt32(row["StaffContractID"]),
						EmploymentTypeId = Convert.ToInt32(row["EmployeeTypeId"]),
						EmployementType = row["EmployeeType"].ToString(),
						StaffId = Convert.ToInt32(row["StaffID"]),
						SatffFName = row["FirstName"].ToString(),
						SatffLName = row["FirstName"].ToString(),
						DesigantionName = row["DesignationName"].ToString(),
						EndDate = Convert.ToDateTime(row["EndDate"].ToString()),
						StaffImg = string.IsNullOrWhiteSpace(row["StaffImg"].ToString()) ? "/Images/Avatar.png" : row["StaffImg"].ToString(),
						StartDate = Convert.ToDateTime(row["StartDate"].ToString()),
						ContractDuration = row["ContractDuration"].ToString(),
						Attachment = string.IsNullOrWhiteSpace(row["Attachment"].ToString()) ? null : row["Attachment"].ToString(),
						AdditionalDetails = row["AdditionalDetails"].ToString(),
						ExpirayStatus = row["ExpirayStatus"].ToString(),

					}).OrderByDescending(x => x.StaffContractId).ToList();
				return list;

				


			}
			catch (Exception ex)
			{

				throw;

			}
		}

		public async Task<StaffContract> StaffContractById(int id, HrhubContext _context)
        {
            try
            {

                var result = await _context.StaffContracts.FirstOrDefaultAsync(x => x.StaffContractId == id  && x.IsDeleted==false);
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


        public async Task<StaffContract> PostStaffContract(StaffContract ObjStaffContractInfo, HrhubContext _context)
        {


			using (var dbContextTransaction = _context.Database.BeginTransaction())
			{

			try
            {


                string msg = "";
                var checkStaffContractInfo = await _context.StaffContracts.FirstOrDefaultAsync(x => x.StaffContractId == ObjStaffContractInfo.StaffContractId && x.IsDeleted == false);
                if (checkStaffContractInfo != null && checkStaffContractInfo.StaffContractId > 0 && ObjStaffContractInfo.Renew==0)
                {
                    checkStaffContractInfo.StaffContractId = ObjStaffContractInfo.StaffContractId;
                    checkStaffContractInfo.StaffId = ObjStaffContractInfo.StaffId;
                    checkStaffContractInfo.EmploymentTypeId = ObjStaffContractInfo.EmploymentTypeId;                   
                    checkStaffContractInfo.ContractDuration = ObjStaffContractInfo.ContractDuration;
                    checkStaffContractInfo.Attachment = ObjStaffContractInfo.Attachment;
                    checkStaffContractInfo.AdditionalDetails = ObjStaffContractInfo.AdditionalDetails;
                   checkStaffContractInfo.StartDate = ObjStaffContractInfo.StartDate;
                   checkStaffContractInfo.EndDate = ObjStaffContractInfo.EndDate;
                    checkStaffContractInfo.UpdatedOn = DateTime.Now;
                    checkStaffContractInfo.Status = ObjStaffContractInfo.Status;
                    checkStaffContractInfo.UpdatedBy = ObjStaffContractInfo.CreatedBy;

                    await _context.SaveChangesAsync();
                   
                  

                }
                else
                {


                var result=    _context.StaffContracts.Where(x => x.StaffId == ObjStaffContractInfo.StaffId && x.IsDeleted == false);
                    if (result!=null && result.Count()>0)
                    {
                        foreach (var item in result)
                        {
                            item.Status = false;
                            item.UpdatedOn = DateTime.Now;
                            item.UpdatedBy = ObjStaffContractInfo.CreatedBy;
                            item.IsDeleted = true;
						}
						await _context.SaveChangesAsync();
					}
                        if (ObjStaffContractInfo.Renew == 1)
                            ObjStaffContractInfo.StaffContractId = 0;


                    ///new button bit in object , check the current object staffid in contract , previous records of indiviual staffid  status inactive (0)

                    ObjStaffContractInfo.CreatedOn = DateTime.Now;
                    ObjStaffContractInfo.IsDeleted = false;
                    _context.StaffContracts.Add(ObjStaffContractInfo);
                    await _context.SaveChangesAsync();

						

                }
					await _context.SaveChangesAsync();
					dbContextTransaction.Commit();
					return ObjStaffContractInfo;



				}
            catch (Exception ex)
            {
					dbContextTransaction.Rollback();
					throw;

            }
       
        
        }
        }


        public async Task<bool> DeleteStaffContractInfo(int id,int Userid, HrhubContext _context)
        {
            try
            {
                bool check = false;
                var StaffContractInfo = await _context.StaffContracts.FirstOrDefaultAsync(x => x.StaffContractId == id  && x.IsDeleted == false);

                if (StaffContractInfo != null)
                {
                    StaffContractInfo.IsDeleted= true;   
                    StaffContractInfo.UpdatedOn= DateTime.Now;
                    StaffContractInfo.UpdatedBy = Userid;
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


        public async Task<bool> AlreadyExists(int StaffContractInfoId, DateTime enddate, int staffId, HrhubContext _context)
        {
            try
            {

                if (StaffContractInfoId > 0)
                {
                    var result = await _context.StaffContracts.FirstOrDefaultAsync(x => x.EndDate == enddate && x.StaffId == staffId && x.StaffContractId != StaffContractInfoId && x.IsDeleted == false && x.Status==true);
                    if (result != null)
                    {
                        return true;
                    }


                }
                else
                {
                    var result = await _context.StaffContracts.FirstOrDefaultAsync(x => x.EndDate == enddate && x.StaffId == staffId && x.IsDeleted == false && x.Status == true);
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



        //Load dropdown Employeement Type
        public async Task<List<EmploymentType>> GetEmploymentType(int CompnayId, HrhubContext hrhubContext)
		{
			try
			{
				List<EmploymentType> objEmploymentType = new List<EmploymentType>();
				objEmploymentType = await hrhubContext.EmploymentTypes.Where(x => x.IsDeleted == false && x.Status == true && x.CompanyId== CompnayId).ToListAsync();
				return objEmploymentType;
			}
			catch { throw; }
		}




	}
}
