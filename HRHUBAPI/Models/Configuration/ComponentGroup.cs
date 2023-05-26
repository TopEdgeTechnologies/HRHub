using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRHUBAPI.Models
{
    public partial class ComponentGroup
    {
        [NotMapped]
        public int? TranFlag { get; set; }

        public async Task<List<ComponentGroup>> GetComponentGroup(HrhubContext hrhubContext)
        {
            try
            {
                List<ComponentGroup> componentGroup = new List<ComponentGroup>();
                componentGroup = await hrhubContext.ComponentGroups.Where(x => x.IsDeleted == false).ToListAsync();
                return componentGroup;
            }
            catch (Exception ex) { throw; }
        }

        public async Task<ComponentGroup> GetComponentGroupById(int Id, HrhubContext hrhubContext)
        {
            try
            {
                var dbResult = await hrhubContext.ComponentGroups.FirstOrDefaultAsync(x => x.IsDeleted == false && x.ComponentGroupId == Id);
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

        public async Task<ComponentGroup> PostComponentGroup(ComponentGroup objComponentGroup, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    var dbResult = await hrhubContext.ComponentGroups.FirstOrDefaultAsync(x => x.IsDeleted == false && x.ComponentGroupId == objComponentGroup.ComponentGroupId);
                    if (dbResult != null && objComponentGroup.ComponentGroupId > 0)
                    {
                        dbResult.ComponentGroupId = objComponentGroup.ComponentGroupId;
                        dbResult.Title = objComponentGroup.Title;
                        dbResult.Description = objComponentGroup.Description;
                        dbResult.Category = objComponentGroup.Category;
                        dbResult.UpdatedBy = objComponentGroup.UpdatedBy;
                        dbResult.UpdatedOn = DateTime.Now;
                        dbResult.IsDeleted = false;

                        await hrhubContext.SaveChangesAsync();
                        dbResult.TranFlag = 2;
                        dbContextTransaction.Commit();
                        return dbResult;
                    }
                    else
                    {
                        objComponentGroup.CreatedOn = DateTime.Now;
                        objComponentGroup.IsDeleted = false;

                        hrhubContext.Add(objComponentGroup);
                        await hrhubContext.SaveChangesAsync();
                        objComponentGroup.TranFlag = 1;
                        dbContextTransaction.Commit();
                        return objComponentGroup;
                    }
                }
                catch (Exception ex) { dbContextTransaction.Rollback(); throw; }
            }
        }

        public async Task<bool> DeleteComponentGroup(int Id, int UserId, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    //bool recordDeleted = false;
                    var dbResult = await hrhubContext.ComponentGroups.FirstOrDefaultAsync(x => x.IsDeleted == false && x.ComponentGroupId == Id);
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
                    var dbResult = await hrhubContext.ComponentGroups.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Title == Title && x.ComponentGroupId != Id);
                    if (dbResult != null)
                    {
                        return true;
                    }
                }
                else
                {
                    var dbResult = await hrhubContext.ComponentGroups.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Title == Title);
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
