using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRHUBAPI.Models
{
    public partial class StaffDependent
    {
        [NotMapped]
        public int? TranFlag { get; set; }

        [NotMapped]
        public IEnumerable<StaffDependent>? StaffDependentList { get; set; }

        public async Task<List<StaffDependent>> GetStaffDependent(HrhubContext hrhubContext)
        {
            try
            {
                return await hrhubContext.StaffDependents.ToListAsync();
            }
            catch (Exception ex) { throw; }
        }

        public async Task<StaffDependent> GetStaffDependentById(int Id, HrhubContext hrhubContext)
        {
            try
            {
                var dbResult = await hrhubContext.StaffDependents.FirstOrDefaultAsync(x => x.DependentId == Id);
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

        public async Task<StaffDependent> GetStaffDependentByStaffId(int StaffId, HrhubContext hrhubContext)
        {
            try
            {
                var dbResult = await hrhubContext.StaffDependents.FirstOrDefaultAsync(x => x.StaffId == StaffId);
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

        public async Task<StaffDependent> PostStaffDependent(StaffDependent objStaffDependent, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    var dbResult = await hrhubContext.StaffDependents.FirstOrDefaultAsync(x => x.DependentId == objStaffDependent.DependentId);
                    if (dbResult != null && objStaffDependent.DependentId > 0)
                    {
                        dbResult.DependentId = objStaffDependent.DependentId;
                        dbResult.StaffId = objStaffDependent.StaffId;
                        dbResult.FirstName = objStaffDependent.FirstName;
                        dbResult.LastName = objStaffDependent.LastName;
                        dbResult.Relation = objStaffDependent.Relation;
                        dbResult.Gender = objStaffDependent.Gender;
                        dbResult.Occupation = objStaffDependent.Occupation;
                        dbResult.UpdatedBy = objStaffDependent.UpdatedBy;
                        dbResult.UpdatedOn = DateTime.Now;

                        await hrhubContext.SaveChangesAsync();
                        dbResult.TranFlag = 2;
                        dbContextTransaction.Commit();
                        return dbResult;
                    }
                    else
                    {
                        objStaffDependent.CreatedOn = DateTime.Now;

                        hrhubContext.Add(objStaffDependent);
                        await hrhubContext.SaveChangesAsync();
                        objStaffDependent.TranFlag = 1;
                        dbContextTransaction.Commit();
                        return objStaffDependent;
                    }
                }
                catch (Exception ex) { dbContextTransaction.Rollback(); throw; }
            }
        }

        public async Task<bool> DeleteStaffDependent(int Id, int UserId, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    var dbResult = await hrhubContext.StaffDependents.FirstOrDefaultAsync(x => x.DependentId == Id);
                    if (dbResult != null)
                    {
                        dbResult.UpdatedBy = UserId;
                        dbResult.UpdatedOn = DateTime.Now;
                        await hrhubContext.SaveChangesAsync();
                        dbContextTransaction.Commit();
                    }
                    return true;
                }
                catch (Exception e) { dbContextTransaction.Rollback(); return false; throw; }
            }
        }

        public async Task<bool> AlreadyExists(int Id, int StaffId, string FirstName, HrhubContext hrhubContext)
        {
            try
            {
                if (Id > 0)
                {
                    var dbResult = await hrhubContext.StaffDependents.FirstOrDefaultAsync(x => x.StaffId == StaffId && x.FirstName == FirstName && x.DependentId != Id);
                    if (dbResult != null)
                    {
                        return true;
                    }
                }
                else
                {
                    var dbResult = await hrhubContext.StaffDependents.FirstOrDefaultAsync(x => x.StaffId == StaffId && x.FirstName == FirstName);
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
