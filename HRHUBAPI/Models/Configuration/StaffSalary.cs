using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRHUBAPI.Models
{
    public partial class StaffSalary
    {
        [NotMapped]
        public int? TranFlag { get; set; }

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

        public async Task<StaffSalary> GetStaffSalaryById(int Id, HrhubContext hrhubContext)
        {
            try
            {
                var dbResult = await hrhubContext.StaffSalaries.FirstOrDefaultAsync(x => x.IsDeleted == false && x.StaffSalaryId == Id);
                if (dbResult != null)
                {
                    return dbResult;
                }
                else
                {
                    return null;
                }
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
