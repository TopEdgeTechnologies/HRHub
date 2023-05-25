using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRHUBAPI.Models
{
    public partial class ComponentInfo
    {
        [NotMapped]
        public int? TranFlag { get; set; }

        [NotMapped]
        public IEnumerable<ComponentInfo>? ComponentInfoList { get; set; }

        public async Task<List<ComponentInfo>> GetComponentInfo(HrhubContext hrhubContext)
        {
            try
            {
                List<ComponentInfo> ComponentInfo = new List<ComponentInfo>();
                ComponentInfo = await hrhubContext.ComponentInfos.Where(x => x.IsDeleted == false).ToListAsync();
                return ComponentInfo;
            }
            catch (Exception ex) { throw; }
        }

        public async Task<ComponentInfo> GetComponentInfoById(int Id, HrhubContext hrhubContext)
        {
            try
            {
                var dbResult = await hrhubContext.ComponentInfos.FirstOrDefaultAsync(x => x.IsDeleted == false && x.ComponentId == Id);
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

        public async Task<ComponentInfo> PostComponentInfo(ComponentInfo objComponentInfo, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    var dbResult = await hrhubContext.ComponentInfos.FirstOrDefaultAsync(x => x.IsDeleted == false && x.ComponentId == objComponentInfo.ComponentId);
                    if (dbResult != null && objComponentInfo.ComponentId > 0)
                    {
                        dbResult.ComponentId = objComponentInfo.ComponentId;
                        dbResult.ComponentGroupId = objComponentInfo.ComponentGroupId;
                        dbResult.Title = objComponentInfo.Title;
                        dbResult.CalculationMethod = objComponentInfo.CalculationMethod;
                        dbResult.CompanyContribution = objComponentInfo.CompanyContribution;
                        dbResult.Category = objComponentInfo.Category;
                        dbResult.Type = objComponentInfo.Type;
                        dbResult.Status = objComponentInfo.Status;
                        dbResult.UpdatedBy = objComponentInfo.UpdatedBy;
                        dbResult.UpdatedOn = DateTime.Now;
                        dbResult.IsDeleted = false;

                        await hrhubContext.SaveChangesAsync();
                        dbResult.TranFlag = 2;
                        dbContextTransaction.Commit();
                        return dbResult;
                    }
                    else
                    {
                        objComponentInfo.CreatedOn = DateTime.Now;
                        objComponentInfo.IsDeleted = false;

                        hrhubContext.Add(objComponentInfo);
                        await hrhubContext.SaveChangesAsync();
                        objComponentInfo.TranFlag = 1;
                        dbContextTransaction.Commit();
                        return objComponentInfo;
                    }
                }
                catch (Exception ex) { dbContextTransaction.Rollback(); throw; }
            }
        }

        public async Task<bool> DeleteComponentInfo(int Id, int UserId, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    //bool recordDeleted = false;
                    var dbResult = await hrhubContext.ComponentInfos.FirstOrDefaultAsync(x => x.IsDeleted == false && x.ComponentId == Id);
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
                    var dbResult = await hrhubContext.ComponentInfos.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Title == Title && x.ComponentId != Id);
                    if (dbResult != null)
                    {
                        return true;
                    }
                }
                else
                {
                    var dbResult = await hrhubContext.ComponentInfos.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Title == Title);
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
