using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRHUBAPI.Models
{
    public partial class SalaryStatus
    {
        [NotMapped]
        public int? TranFlag { get; set; }

        public async Task<List<SalaryStatus>> GetSalaryStatus(HrhubContext hrhubContext)
        {
            try
            {
                List<SalaryStatus> salaryStatuses = new List<SalaryStatus>();
                salaryStatuses = await hrhubContext.SalaryStatuses.Where(x => x.IsDeleted == false).ToListAsync();
                return salaryStatuses;
            }
            catch (Exception ex) { throw; }
        }

        public async Task<SalaryStatus> GetSalaryStatusById(int Id, HrhubContext hrhubContext)
        {
            try
            {
                var dbResult = await hrhubContext.SalaryStatuses.FirstOrDefaultAsync(x => x.IsDeleted == false && x.SalaryStatusId == Id);
                if(dbResult != null)
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

        public async Task<SalaryStatus> PostSalaryStatus(SalaryStatus objsalaryStatus, HrhubContext hrhubContext)
        {
            using(var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    var dbResult = await hrhubContext.SalaryStatuses.FirstOrDefaultAsync(x => x.IsDeleted == false && x.SalaryStatusId == objsalaryStatus.SalaryStatusId);
                    if(dbResult != null && objsalaryStatus.SalaryStatusId > 0)
                    {
                        dbResult.SalaryStatusId = objsalaryStatus.SalaryStatusId;
                        dbResult.Title = objsalaryStatus.Title;
                        dbResult.UpdatedBy = objsalaryStatus.UpdatedBy;
                        dbResult.UpdatedOn = DateTime.Now;
                        dbResult.IsDeleted = false;

                        await hrhubContext.SaveChangesAsync();
                        dbResult.TranFlag = 2;
                        dbContextTransaction.Commit();
                        return dbResult;
                    }
                    else
                    {
                        objsalaryStatus.CreatedOn = DateTime.Now;
                        objsalaryStatus.IsDeleted = false;

                        hrhubContext.Add(objsalaryStatus);
                        await hrhubContext.SaveChangesAsync();
                        objsalaryStatus.TranFlag = 1;
                        dbContextTransaction.Commit();
                        return objsalaryStatus;
                    }
                }
                catch(Exception ex) { dbContextTransaction.Rollback();  throw; }
            }
        }

        public async Task<bool> DeleteSalaryStatus(int Id, int UserId, HrhubContext hrhubContext)
        {
            using(var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    //bool recordDeleted = false;
                    var dbResult = await hrhubContext.SalaryStatuses.FirstOrDefaultAsync(x => x.IsDeleted == false && x.SalaryStatusId == Id);
                    if(dbResult != null)
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
                catch(Exception e) { dbContextTransaction.Rollback(); return false; throw; }
            }
        }

        public async Task<bool> AlreadyExists(int Id, string Title, HrhubContext hrhubContext)
        {
            try
            {
                if(Id > 0)
                {
                    var dbResult = await hrhubContext.SalaryStatuses.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Title == Title && x.SalaryStatusId != Id);
                    if(dbResult != null) 
                    {
                        return true;
                    }
                }
                else
                {
                    var dbResult = await hrhubContext.SalaryStatuses.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Title == Title);
                    if (dbResult != null)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch(Exception e) { throw; }
        }
    }

}
