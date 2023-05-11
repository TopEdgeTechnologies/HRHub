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
            try
            {
                var dbResult = await hrhubContext.Departments.FirstOrDefaultAsync(x => x.IsDeleted == false && x.DepartmentId == department.DepartmentId);
                if (dbResult != null && dbResult.DepartmentId > 0)
                {
                    dbResult.DepartmentId = department.DepartmentId;
                    dbResult.Title = department.Title;
                    dbResult.ShortCode = department.ShortCode;
                    dbResult.LogoAttachment = department.LogoAttachment;
                    dbResult.Status = department.Status;
                    dbResult.UpdatedBy = department.UpdatedBy;
                    dbResult.UpdatedOn = DateTime.Now;
                    await hrhubContext.SaveChangesAsync();
                    dbResult.TransFlag = 2;
                    return dbResult;
                }
                else
                {
                    department.CreatedOn = DateTime.Now;
                    department.IsDeleted = false;
                    hrhubContext.Departments.Add(department);
                    await hrhubContext.SaveChangesAsync();
                    department.TransFlag = 1;
                    return department;
                }
            }
            catch { throw; }
        }

        public async Task<bool> DeleteDepartment(int id, HrhubContext hrhubContext)
        {
            try
            {
                bool recordDeleted = false;
                var dbResult = await hrhubContext.Departments.FirstOrDefaultAsync(x => x.IsDeleted == false && x.DepartmentId == id);
                if (dbResult != null)
                {
                    dbResult.IsDeleted = true;
                    dbResult.UpdatedOn = DateTime.Now;
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
                    var dbResul = await hrhubContext.Departments.FirstOrDefaultAsync(x => x.IsDeleted == false && x.CompanyId == CompanyId && x.Title == title && x.DepartmentId != Id);
                    if (dbResul != null)
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