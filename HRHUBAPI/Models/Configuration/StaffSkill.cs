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

        [NotMapped]
        public IEnumerable<string>? SkillTitleList { get; set; }

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

        public async Task<List<StaffSkill>> GetStaffSkillByStaffId(int StaffId, HrhubContext hrhubContext)
        {
            try
            {
                List<StaffSkill> ListStaffSkill = new List<StaffSkill>();
                ListStaffSkill = await hrhubContext.StaffSkills.Where(x => x.StaffId == StaffId).ToListAsync();
                if (ListStaffSkill != null)
                {
                    return ListStaffSkill;
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
                    var result = hrhubContext.StaffSkills.Where(x => x.StaffId == objStaffSkill.StaffId).ToList();
                    if (result != null && result.Count > 0)
                    {
                        hrhubContext.RemoveRange(result);
                        await hrhubContext.SaveChangesAsync();
                    }

                    List<StaffSkill> ListStaffSkill = new List<StaffSkill>();

                    int i = 0;
                    if (objStaffSkill.SkillTitleList != null)
                    {
                        foreach (var item in objStaffSkill.SkillTitleList)
                        {
                            if (objStaffSkill.SkillTitleList.ToArray()[i] != null)
                            {
                                StaffSkill obj = new StaffSkill();
                                obj.StaffId = objStaffSkill.StaffId;
                                obj.Title = objStaffSkill.SkillTitleList.ToArray()[i];
                                obj.CreatedBy = objStaffSkill.CreatedBy;
                                obj.CreatedOn = DateTime.Now;

                                ListStaffSkill.Add(obj);
                            }
                            i++;
                        }
                    }

                    if (ListStaffSkill.Count > 0)
                    {
                        await hrhubContext.AddRangeAsync(ListStaffSkill);
                        await hrhubContext.SaveChangesAsync();
                        objStaffSkill.TranFlag = 1;
                    }
                    dbContextTransaction.Commit();
                    return objStaffSkill;
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
