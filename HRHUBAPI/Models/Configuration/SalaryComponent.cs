using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRHUBAPI.Models
{
    public partial class SalaryComponent
    {
        [NotMapped]
        public int? TranFlag { get; set; }

        public async Task<List<SalaryComponent>> GetSalaryComponentByCompanyId(int CompanyId, HrhubContext hrhubContext)
        {
            try
            {
                List<SalaryComponent> SalaryComponentes = new List<SalaryComponent>();
                SalaryComponentes = await hrhubContext.SalaryComponents.Where(x => x.IsDeleted == false && x.CompanyId == CompanyId).ToListAsync();
                return SalaryComponentes;
            }
            catch (Exception ex) { throw; }
        }

        public async Task<SalaryComponent> GetSalaryComponentById(int Id, HrhubContext hrhubContext)
        {
            try
            {
                var dbResult = await hrhubContext.SalaryComponents.FirstOrDefaultAsync(x => x.IsDeleted == false && x.SalaryComponentId == Id);
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

        public async Task<SalaryComponent> PostSalaryComponent(SalaryComponent objSalaryComponent, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    var dbResult = await hrhubContext.SalaryComponents.FirstOrDefaultAsync(x => x.IsDeleted == false && x.SalaryComponentId == objSalaryComponent.SalaryComponentId);
                    if (dbResult != null && objSalaryComponent.SalaryComponentId > 0)
                    {
                        dbResult.SalaryComponentId = objSalaryComponent.SalaryComponentId;
                        dbResult.Title = objSalaryComponent.Title;
                        dbResult.Category = objSalaryComponent.Category;
                        dbResult.ContributionMethod = objSalaryComponent.ContributionMethod;
                        dbResult.StaffContribution = objSalaryComponent.StaffContribution;
                        dbResult.CompanyContribution = objSalaryComponent.CompanyContribution;
                        dbResult.UpdatedBy = objSalaryComponent.UpdatedBy;
                        dbResult.UpdatedOn = DateTime.Now;
                        dbResult.IsDeleted = false;

                        await hrhubContext.SaveChangesAsync();
                        dbResult.TranFlag = 2;
                        dbContextTransaction.Commit();
                        return dbResult;
                    }
                    else
                    {
                        objSalaryComponent.CreatedOn = DateTime.Now;
                        objSalaryComponent.IsDeleted = false;

                        hrhubContext.Add(objSalaryComponent);
                        await hrhubContext.SaveChangesAsync();
                        objSalaryComponent.TranFlag = 1;
                        dbContextTransaction.Commit();
                        return objSalaryComponent;
                    }
                }
                catch (Exception ex) { dbContextTransaction.Rollback(); throw; }
            }
        }

        public async Task<bool> DeleteSalaryComponent(int Id, int UserId, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    //bool recordDeleted = false;
                    var dbResult = await hrhubContext.SalaryComponents.FirstOrDefaultAsync(x => x.IsDeleted == false && x.SalaryComponentId == Id);
                    if (dbResult != null)
                    {
                        dbResult.IsDeleted = false;
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

        public async Task<bool> AlreadyExists(int Id, string Title, HrhubContext hrhubContext)
        {
            try
            {
                if (Id > 0)
                {
                    var dbResult = await hrhubContext.SalaryComponents.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Title == Title && x.SalaryComponentId != Id);
                    if (dbResult != null)
                    {
                        return true;
                    }
                }
                else
                {
                    var dbResult = await hrhubContext.SalaryComponents.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Title == Title);
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
