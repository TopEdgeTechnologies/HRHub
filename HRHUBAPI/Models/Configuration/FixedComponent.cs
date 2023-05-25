using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRHUBAPI.Models
{
    public partial class FixedComponent
    {
        [NotMapped]
        public int? TranFlag { get; set; }

        public async Task<List<FixedComponent>> GetFixedComponent(HrhubContext hrhubContext)
        {
            try
            {
                List<FixedComponent> FixedComponentes = new List<FixedComponent>();
                FixedComponentes = await hrhubContext.FixedComponents.Where(x => x.IsDeleted == false).ToListAsync();
                return FixedComponentes;
            }
            catch (Exception ex) { throw; }
        }

        public async Task<FixedComponent> GetFixedComponentById(int Id, HrhubContext hrhubContext)
        {
            try
            {
                var dbResult = await hrhubContext.FixedComponents.FirstOrDefaultAsync(x => x.IsDeleted == false && x.FixedComponentId == Id);
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

        public async Task<FixedComponent> PostFixedComponent(FixedComponent objFixedComponent, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    var dbResult = await hrhubContext.FixedComponents.FirstOrDefaultAsync(x => x.IsDeleted == false && x.FixedComponentId == objFixedComponent.FixedComponentId);
                    if (dbResult != null && objFixedComponent.FixedComponentId > 0)
                    {
                        dbResult.FixedComponentId = objFixedComponent.FixedComponentId;
                        dbResult.Title = objFixedComponent.Title;
                        dbResult.UpdatedBy = objFixedComponent.UpdatedBy;
                        dbResult.UpdatedOn = DateTime.Now;
                        dbResult.IsDeleted = false;

                        await hrhubContext.SaveChangesAsync();
                        dbResult.TranFlag = 2;
                        dbContextTransaction.Commit();
                        return dbResult;
                    }
                    else
                    {
                        objFixedComponent.CreatedOn = DateTime.Now;
                        objFixedComponent.IsDeleted = false;

                        hrhubContext.Add(objFixedComponent);
                        await hrhubContext.SaveChangesAsync();
                        objFixedComponent.TranFlag = 1;
                        dbContextTransaction.Commit();
                        return objFixedComponent;
                    }
                }
                catch (Exception ex) { dbContextTransaction.Rollback(); throw; }
            }
        }

        public async Task<bool> DeleteFixedComponent(int Id, int UserId, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    //bool recordDeleted = false;
                    var dbResult = await hrhubContext.FixedComponents.FirstOrDefaultAsync(x => x.IsDeleted == false && x.FixedComponentId == Id);
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
                    var dbResult = await hrhubContext.FixedComponents.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Title == Title && x.FixedComponentId != Id);
                    if (dbResult != null)
                    {
                        return true;
                    }
                }
                else
                {
                    var dbResult = await hrhubContext.FixedComponents.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Title == Title);
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
