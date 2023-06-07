using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRHUBAPI.Models
{
    public partial class StaffSalaryComponent
    {
        [NotMapped]
        public int? TranFlag { get; set; }

        [NotMapped]
        public string? Category { get; set; }

        [NotMapped]
        public string? ComponentTitle { get; set; }    


        public async Task<List<StaffSalaryComponent>> GetSalaryComponent(int CompanyId, HrhubContext hrhubContext)
        {
            try
            {
                List<StaffSalaryComponent> staffSalaryComponent = new List<StaffSalaryComponent>();
                staffSalaryComponent = await hrhubContext.StaffSalaryComponents.ToListAsync();
                return staffSalaryComponent;
            }
            catch (Exception ex) { throw; }
        }

        public async Task<StaffSalaryComponent> GetStaffSalaryComponentById(int Id, HrhubContext hrhubContext)
        {
            try
            {
                var dbResult = await hrhubContext.StaffSalaryComponents.FirstOrDefaultAsync(x => x.StaffSalaryComponentId == Id);
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

        public async Task<StaffSalaryComponent> PostStaffSalaryComponent(StaffSalaryComponent objStaffSalaryComponent, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    var dbResult = await hrhubContext.StaffSalaryComponents.FirstOrDefaultAsync(x => x.StaffSalaryComponentId == objStaffSalaryComponent.StaffSalaryComponentId);
                    if (dbResult != null && objStaffSalaryComponent.StaffSalaryComponentId > 0)
                    {
                        dbResult.StaffSalaryComponentId = objStaffSalaryComponent.StaffSalaryComponentId;
                        dbResult.StaffId = objStaffSalaryComponent.StaffId;
                        dbResult.ComponentId = objStaffSalaryComponent.ComponentId;
                        dbResult.PercentageValue = objStaffSalaryComponent.PercentageValue;
                        dbResult.ComponentAmount = objStaffSalaryComponent.ComponentAmount;

						await hrhubContext.SaveChangesAsync();
                        dbResult.TranFlag = 2;
                        dbContextTransaction.Commit();
                        return dbResult;
                    }
                    else
                    {
						hrhubContext.Add(objStaffSalaryComponent);
                        await hrhubContext.SaveChangesAsync();
                        objStaffSalaryComponent.TranFlag = 1;
                        dbContextTransaction.Commit();
                        return objStaffSalaryComponent;
                    }
                }
                catch (Exception ex) { dbContextTransaction.Rollback(); throw; }
            }
        }

        public async Task<bool> DeleteStaffSalaryComponent(int Id, int UserId, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    var dbResult = await hrhubContext.StaffSalaryComponents.FirstOrDefaultAsync(x => x.StaffSalaryComponentId == Id);
                    if (dbResult != null)
                    {
                        await hrhubContext.SaveChangesAsync();
                        dbContextTransaction.Commit();
                    }
                    return true;
                }
                catch (Exception e) { dbContextTransaction.Rollback(); return false; throw; }
            }
        }

        public async Task<bool> AlreadyExists(int Id, int StaffId, int ComponentId, HrhubContext hrhubContext)
        {
            try
            {
                if (Id > 0)
                {
                    var dbResult = await hrhubContext.StaffSalaryComponents.FirstOrDefaultAsync(x => x.StaffId == StaffId && x.ComponentId == ComponentId && x.StaffSalaryComponentId != Id);
                    if (dbResult != null)
                    {
                        return true;
                    }
                }
                else
                {
                    var dbResult = await hrhubContext.StaffSalaryComponents.FirstOrDefaultAsync(x => x.StaffId == StaffId && x.ComponentId == ComponentId);
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
