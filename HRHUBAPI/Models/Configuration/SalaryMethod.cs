using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRHUBAPI.Models
{
    public partial class SalaryMethod
    {
        [NotMapped]
        public int? TranFlag { get; set; }

        [NotMapped]
        public IEnumerable<SalaryMethod>? SalaryMethodList { get; set; }

        public async Task<List<SalaryMethod>> GetSalaryMethod(HrhubContext hrhubContext)
        {
            try
            {
                List<SalaryMethod> SalaryMethodes = new List<SalaryMethod>();
                SalaryMethodes = await hrhubContext.SalaryMethods.Where(x => x.IsDeleted == false).ToListAsync();
                return SalaryMethodes;
            }
            catch (Exception ex) { throw; }
        }

        public async Task<SalaryMethod> GetSalaryMethodById(int Id, HrhubContext hrhubContext)
        {
            try
            {
                var dbResult = await hrhubContext.SalaryMethods.FirstOrDefaultAsync(x => x.IsDeleted == false && x.SalaryMethodId == Id);
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

        public async Task<SalaryMethod> PostSalaryMethod(SalaryMethod objSalaryMethod, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    var dbResult = await hrhubContext.SalaryMethods.FirstOrDefaultAsync(x => x.IsDeleted == false && x.SalaryMethodId == objSalaryMethod.SalaryMethodId);
                    if (dbResult != null && objSalaryMethod.SalaryMethodId > 0)
                    {
                        dbResult.SalaryMethodId = objSalaryMethod.SalaryMethodId;
                        dbResult.Title = objSalaryMethod.Title;
                        dbResult.UpdatedBy = objSalaryMethod.UpdatedBy;
                        dbResult.UpdatedOn = DateTime.Now;
                        dbResult.IsDeleted = false;

                        await hrhubContext.SaveChangesAsync();
                        dbResult.TranFlag = 2;
                        dbContextTransaction.Commit();
                        return dbResult;
                    }
                    else
                    {
                        objSalaryMethod.CreatedOn = DateTime.Now;
                        objSalaryMethod.IsDeleted = false;

                        hrhubContext.Add(objSalaryMethod);
                        await hrhubContext.SaveChangesAsync();
                        objSalaryMethod.TranFlag = 1;
                        dbContextTransaction.Commit();
                        return objSalaryMethod;
                    }
                }
                catch (Exception ex) { dbContextTransaction.Rollback(); throw; }
            }
        }

        public async Task<bool> DeleteSalaryMethod(int Id, int UserId, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    //bool recordDeleted = false;
                    var dbResult = await hrhubContext.SalaryMethods.FirstOrDefaultAsync(x => x.IsDeleted == false && x.SalaryMethodId == Id);
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

        public async Task<bool> AlreadyExists(int Id, string Title, HrhubContext hrhubContext)
        {
            try
            {
                if (Id > 0)
                {
                    var dbResult = await hrhubContext.SalaryMethods.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Title == Title && x.SalaryMethodId != Id);
                    if (dbResult != null)
                    {
                        return true;
                    }
                }
                else
                {
                    var dbResult = await hrhubContext.SalaryMethods.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Title == Title);
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
