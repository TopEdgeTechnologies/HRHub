using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRHUBAPI.Models
{
    public partial class SalaryStatusProcess
    {
        [NotMapped]
        public int? TranFlag { get; set; }

        public async Task<List<SalaryStatusProcess>> GetSalaryStatusProcess(HrhubContext hrhubContext)
        {
            try
            {
                List<SalaryStatusProcess> SalaryStatusProcesses = new List<SalaryStatusProcess>();
                SalaryStatusProcesses = await hrhubContext.SalaryStatusProcesses.Where(x => x.IsDeleted == false).ToListAsync();
                return SalaryStatusProcesses;
            }
            catch (Exception ex) { throw; }
        }

        public async Task<SalaryStatusProcess> GetSalaryStatusProcessById(int Id, HrhubContext hrhubContext)
        {
            try
            {
                var dbResult = await hrhubContext.SalaryStatusProcesses.FirstOrDefaultAsync(x => x.IsDeleted == false && x.SalaryStatusProcessId == Id);
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

        public async Task<SalaryStatusProcess> PostSalaryStatusProcess(SalaryStatusProcess objSalaryStatusProcess, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    var dbResult = await hrhubContext.SalaryStatusProcesses.FirstOrDefaultAsync(x => x.IsDeleted == false && x.SalaryStatusProcessId == objSalaryStatusProcess.SalaryStatusProcessId);
                    if (dbResult != null && objSalaryStatusProcess.SalaryStatusProcessId > 0)
                    {
                        dbResult.SalaryStatusProcessId = objSalaryStatusProcess.SalaryStatusProcessId;
                        dbResult.ProcessDate = objSalaryStatusProcess.ProcessDate;
                        dbResult.UpdatedBy = objSalaryStatusProcess.UpdatedBy;
                        dbResult.UpdatedOn = DateTime.Now;
                        dbResult.IsDeleted = false;

                        await hrhubContext.SaveChangesAsync();
                        dbResult.TranFlag = 2;
                        dbContextTransaction.Commit();
                        return dbResult;
                    }
                    else
                    {
                        objSalaryStatusProcess.CreatedOn = DateTime.Now;
                        objSalaryStatusProcess.IsDeleted = false;

                        hrhubContext.Add(objSalaryStatusProcess);
                        await hrhubContext.SaveChangesAsync();
                        objSalaryStatusProcess.TranFlag = 1;
                        dbContextTransaction.Commit();
                        return objSalaryStatusProcess;
                    }
                }
                catch (Exception ex) { dbContextTransaction.Rollback(); throw; }
            }
        }

        public async Task<bool> DeleteSalaryStatusProcess(int Id, int UserId, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    //bool recordDeleted = false;
                    var dbResult = await hrhubContext.SalaryStatusProcesses.FirstOrDefaultAsync(x => x.IsDeleted == false && x.SalaryStatusProcessId == Id);
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

        public async Task<bool> AlreadyExists(int Id, DateTime ProcessDate, HrhubContext hrhubContext)
        {
            try
            {
                if (Id > 0)
                {
                    var dbResult = await hrhubContext.SalaryStatusProcesses.FirstOrDefaultAsync(x => x.IsDeleted == false && x.ProcessDate == ProcessDate && x.SalaryStatusProcessId != Id);
                    if (dbResult != null)
                    {
                        return true;
                    }
                }
                else
                {
                    var dbResult = await hrhubContext.SalaryStatusProcesses.FirstOrDefaultAsync(x => x.IsDeleted == false && x.ProcessDate == ProcessDate);
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
