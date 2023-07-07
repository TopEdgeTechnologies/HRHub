using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRHUBAPI.Models
{
    public partial class StaffAcademic
    {
        [NotMapped]
        public int? TranFlag { get; set; }

        [NotMapped]
        public IEnumerable<StaffAcademic>? StaffAcademicList { get; set; }

        public async Task<List<StaffAcademic>> GetStaffAcademic(HrhubContext hrhubContext)
        {
            try
            {
                return await hrhubContext.StaffAcademics.ToListAsync();
            }
            catch (Exception ex) { throw; }
        }

        public async Task<StaffAcademic> GetStaffAcademicById(int Id, HrhubContext hrhubContext)
        {
            try
            {
                var dbResult = await hrhubContext.StaffAcademics.FirstOrDefaultAsync(x => x.AcademicId == Id);
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

        public async Task<List<StaffAcademic>> GetStaffAcademicByStaffId(int StaffId, HrhubContext hrhubContext)
        {
            try
            {
                List<StaffAcademic> ListStaffAcademic = new List<StaffAcademic>();
                ListStaffAcademic = await hrhubContext.StaffAcademics.Where(x => x.StaffId == StaffId).ToListAsync();
                if (ListStaffAcademic != null)
                {
                    return ListStaffAcademic;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex) { throw; }
        }

        public async Task<StaffAcademic> PostStaffAcademic(StaffAcademic objStaffAcademic, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    var dbResult = await hrhubContext.StaffAcademics.FirstOrDefaultAsync(x => x.AcademicId == objStaffAcademic.AcademicId);
                    if (dbResult != null && objStaffAcademic.AcademicId > 0)
                    {
                        dbResult.AcademicId = objStaffAcademic.AcademicId;
                        dbResult.StaffId = objStaffAcademic.StaffId;
                        dbResult.Title = objStaffAcademic.Title;
                        dbResult.InstituteName = objStaffAcademic.InstituteName;
                        dbResult.FromDate = objStaffAcademic.FromDate;
                        dbResult.ToDate = objStaffAcademic.ToDate;
                        dbResult.InProcess = objStaffAcademic.InProcess;
                        dbResult.Type = objStaffAcademic.Type;
                        dbResult.UpdatedBy = objStaffAcademic.UpdatedBy;
                        dbResult.UpdatedOn = DateTime.Now;

                        await hrhubContext.SaveChangesAsync();
                        dbResult.TranFlag = 2;
                        dbContextTransaction.Commit();
                        return dbResult;
                    }
                    else
                    {
                        objStaffAcademic.CreatedOn = DateTime.Now;
                        
                        hrhubContext.Add(objStaffAcademic);
                        await hrhubContext.SaveChangesAsync();
                        objStaffAcademic.TranFlag = 1;
                        dbContextTransaction.Commit();
                        return objStaffAcademic;
                    }
                }
                catch (Exception ex) { dbContextTransaction.Rollback(); throw; }
            }
        }

        public async Task<bool> DeleteStaffAcademic(int Id, int UserId, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    var dbResult = await hrhubContext.StaffAcademics.FirstOrDefaultAsync(x => x.AcademicId == Id);
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
                    var dbResult = await hrhubContext.StaffAcademics.FirstOrDefaultAsync(x => x.StaffId == StaffId && x.Title == Title && x.AcademicId != Id);
                    if (dbResult != null)
                    {
                        return true;
                    }
                }
                else
                {
                    var dbResult = await hrhubContext.StaffAcademics.FirstOrDefaultAsync(x => x.StaffId == StaffId && x.Title == Title);
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
