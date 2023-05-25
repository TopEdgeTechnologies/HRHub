using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRHUBAPI.Models
{
    public partial class Department
    {

        [NotMapped]
        public IEnumerable<Department>? Listdepartments { get; set; }

        [NotMapped]
        public int TransFlag { get; set; }

        public async Task<List<Department>> GetDepartment(int CompanyId, HrhubContext hrhubContext)
        {
            try
            {
                List<Department> departments = new List<Department>();
                departments = await hrhubContext.Departments.Where(x => x.IsDeleted == false && x.CompanyId == CompanyId).ToListAsync();
                return departments;
            }
            catch { throw; }
        }

        public async Task<Department> GetDepartmentById(int Id, HrhubContext hrhubContext)
        {
            try
            {
                var dbResult = await hrhubContext.Departments.FirstOrDefaultAsync(x => x.IsDeleted == false && x.DepartmentId == Id);
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

        public async Task<Department> PostDepartment(Department department, HrhubContext hrhubContext)
        {
            using(var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
            {
                var dbResult = await hrhubContext.Departments.FirstOrDefaultAsync(x => x.IsDeleted == false && x.DepartmentId == department.DepartmentId);
                if (dbResult != null && dbResult.DepartmentId > 0)
                {
                    dbResult.DepartmentId = department.DepartmentId;
                    dbResult.Title = department.Title;
                    dbResult.ShortCode = department.ShortCode;
                    dbResult.LogoAttachment = string.IsNullOrWhiteSpace(department.LogoAttachment) ? dbResult.LogoAttachment: department.LogoAttachment;
                    dbResult.Status = department.Status;
                    dbResult.UpdatedBy = department.UpdatedBy;
                    dbResult.UpdatedOn = DateTime.Now;
                    await hrhubContext.SaveChangesAsync();
                    dbResult.TransFlag = 2;
                    dbContextTransaction.Commit();
                    return dbResult;
                }
                else
                {
                    department.CreatedOn = DateTime.Now;
                    department.IsDeleted = false;
                    hrhubContext.Departments.Add(department);
                    await hrhubContext.SaveChangesAsync();
                    department.TransFlag = 1;
                    dbContextTransaction.Commit();
                    return department;
                }
            }
            catch { dbContextTransaction.Rollback(); throw; }
            }
        }

        public async Task<bool> DeleteDepartment(int id , int userid, HrhubContext hrhubContext)
        {
            try
            {
                bool recordDeleted = false;
                var dbResult = await hrhubContext.Departments.FirstOrDefaultAsync(x => x.IsDeleted == false && x.DepartmentId == id);
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
                    var dbResult = await hrhubContext.Departments.FirstOrDefaultAsync(x => x.IsDeleted == false && x.CompanyId == CompanyId && x.Title == title && x.DepartmentId != Id);
                    if (dbResult != null)
                    {
                        return true;
                    }
                }
                else
                {
                    var dbResult = await hrhubContext.Departments.FirstOrDefaultAsync(x => x.IsDeleted == false && x.CompanyId == CompanyId && x.Title == title);
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