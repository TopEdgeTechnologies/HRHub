using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRHUBAPI.Models
{
    public partial class StaffSkill
    {
        [NotMapped]
        public int? TranFlag { get; set; }

        [NotMapped]
        public IEnumerable<StaffSkill>? StaffSkillList { get; set; }

        public async Task<List<StaffSkill>> GetStaffSkill(HrhubContext hrhubContext)
        {
            try
            {
                return await hrhubContext.StaffSkills.ToListAsync();
            }
            catch (Exception ex) { throw; }
        }

        public async Task<StaffSkill> GetStaffSkillById(int Id, HrhubContext hrhubContext)
        {
            try
            {
                var dbResult = await hrhubContext.StaffSkills.FirstOrDefaultAsync(x => x.SkillId == Id);
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

        public async Task<StaffSkill> GetStaffSkillByStaffId(int StaffId, HrhubContext hrhubContext)
        {
            try
            {
                var dbResult = await hrhubContext.StaffSkills.FirstOrDefaultAsync(x => x.StaffId == StaffId);
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

        public async Task<StaffSkill> PostStaffSkill(StaffSkill objStaffSkill, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    var dbResult = await hrhubContext.StaffSkills.FirstOrDefaultAsync(x => x.SkillId == objStaffSkill.SkillId);
                    if (dbResult != null && objStaffSkill.SkillId > 0)
                    {
                        dbResult.SkillId = objStaffSkill.SkillId;
                        dbResult.StaffId = objStaffSkill.StaffId;
                        dbResult.Title = objStaffSkill.Title;
                        dbResult.UpdatedBy = objStaffSkill.UpdatedBy;
                        dbResult.UpdatedOn = DateTime.Now;

                        await hrhubContext.SaveChangesAsync();
                        dbResult.TranFlag = 2;
                        dbContextTransaction.Commit();
                        return dbResult;
                    }
                    else
                    {
                        objStaffSkill.CreatedOn = DateTime.Now;

                        hrhubContext.Add(objStaffSkill);
                        await hrhubContext.SaveChangesAsync();
                        objStaffSkill.TranFlag = 1;
                        dbContextTransaction.Commit();
                        return objStaffSkill;
                    }
                }
                catch (Exception ex) { dbContextTransaction.Rollback(); throw; }
            }
        }

        public async Task<bool> DeleteStaffSkill(int Id, int UserId, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    var dbResult = await hrhubContext.StaffSkills.FirstOrDefaultAsync(x => x.SkillId == Id);
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

        public async Task<bool> AlreadyExists(int Id, int StaffId, string Title, HrhubContext hrhubContext)
        {
            try
            {
                if (Id > 0)
                {
                    var dbResult = await hrhubContext.StaffSkills.FirstOrDefaultAsync(x => x.StaffId == StaffId && x.Title == Title && x.SkillId != Id);
                    if (dbResult != null)
                    {
                        return true;
                    }
                }
                else
                {
                    var dbResult = await hrhubContext.StaffSkills.FirstOrDefaultAsync(x => x.StaffId == StaffId && x.Title == Title);
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
