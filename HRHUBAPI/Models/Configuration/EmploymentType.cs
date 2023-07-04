using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRHUBAPI.Models
{
    public partial class EmploymentType
    {

        [NotMapped]
        public int TransFlag { get; set; }

        public async Task<List<EmploymentType>> GetEmploymentType(int CompanyId, HrhubContext hrhubContext)
        {
            try
            {
                List<EmploymentType> employmenttype = new List<EmploymentType>();
                employmenttype = await hrhubContext.EmploymentTypes.Where(x => x.IsDeleted == false && x.CompanyId == CompanyId).OrderByDescending(x => x.EmploymentTypeId).ToListAsync();
                return employmenttype;
            }
            catch { throw; }
        }

        public async Task<EmploymentType> GetEmploymentTypeById(int Id, HrhubContext hrhubContext)
        {
            try
            {
                var dbResult = await hrhubContext.EmploymentTypes.FirstOrDefaultAsync(x => x.IsDeleted == false && x.EmploymentTypeId == Id);
                if (dbResult != null)
                {
                    return dbResult;
                }
                else
                {
                    return null;
                }
            }
            catch { throw; }
        }
        public async Task<EmploymentType> UpdateStatusByEmploymentTypeId(EmploymentType ObjInfo, HrhubContext _context)
        {
            try
            {
                string msg = "";
                var checkEmploymentTypeInfo = await _context.EmploymentTypes.FirstOrDefaultAsync(x => x.EmploymentTypeId == ObjInfo.EmploymentTypeId && x.IsDeleted == false);
                if (checkEmploymentTypeInfo != null && checkEmploymentTypeInfo.EmploymentTypeId > 0)
                {
                    checkEmploymentTypeInfo.EmploymentTypeId = ObjInfo.EmploymentTypeId;
                    checkEmploymentTypeInfo.UpdatedOn = DateTime.Now;
                    checkEmploymentTypeInfo.Status = ObjInfo.Status;
                    checkEmploymentTypeInfo.UpdatedBy = ObjInfo.UpdatedBy;

                    await _context.SaveChangesAsync();
                    return ObjInfo;

                }
                return ObjInfo;
            }
            catch (Exception ex)
            {

                throw;

            }
        }
        public async Task<EmploymentType> PostEmploymentType(EmploymentType employmentType, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    var dbResult = await hrhubContext.EmploymentTypes.FirstOrDefaultAsync(x => x.IsDeleted == false && x.EmploymentTypeId == employmentType.EmploymentTypeId);
                    if (dbResult != null && dbResult.EmploymentTypeId > 0)
                    {
                        dbResult.EmploymentTypeId = employmentType.EmploymentTypeId;
                        dbResult.Title = employmentType.Title;
                        dbResult.Status = employmentType.Status;
                        dbResult.UpdatedBy = employmentType.UpdatedBy;
                        dbResult.UpdatedOn = DateTime.Now;
                        await hrhubContext.SaveChangesAsync();
                        dbResult.TransFlag = 2;
                        dbContextTransaction.Commit();
                        return dbResult;
                    }
                    else
                    {
                        employmentType.CreatedOn = DateTime.Now;
                        employmentType.IsDeleted = false;
                        hrhubContext.EmploymentTypes.Add(employmentType);
                        await hrhubContext.SaveChangesAsync();
                        employmentType.TransFlag = 1;
                        dbContextTransaction.Commit();
                        return employmentType;
                    }
                }
                catch { dbContextTransaction.Rollback(); throw; }
            }
        }

        public async Task<bool> DeleteEmploymentType(int id, int userid, HrhubContext hrhubContext)
        {
            try
            {
                bool recordDeleted = false;
                var dbResult = await hrhubContext.EmploymentTypes.FirstOrDefaultAsync(x => x.IsDeleted == false && x.EmploymentTypeId == id);
                if (dbResult != null)
                {
                    dbResult.IsDeleted = true;
                    dbResult.UpdatedOn = DateTime.Now;
                    dbResult.UpdatedBy = userid;

                    recordDeleted = true;
                }
                await hrhubContext.SaveChangesAsync();
                return recordDeleted;
            }
            catch { throw; }
        }

        public async Task<bool> AlreadyExists(int CompanyId, int Id, string title, HrhubContext hrhubContext)
        {
            try
            {
                if (Id > 0)
                {
                    var dbResult = await hrhubContext.EmploymentTypes.FirstOrDefaultAsync(x => x.IsDeleted == false && x.CompanyId == CompanyId && x.Title == title && x.EmploymentTypeId != Id);
                    if (dbResult != null)
                    {
                        return true;
                    }
                }
                else
                {
                    var dbResult = await hrhubContext.EmploymentTypes.FirstOrDefaultAsync(x => x.IsDeleted == false && x.CompanyId == CompanyId && x.Title == title);
                    if (dbResult != null)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch { throw; }
        }


    }
}
