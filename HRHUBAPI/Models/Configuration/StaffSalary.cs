using Microsoft.EntityFrameworkCore;

namespace HRHUBAPI.Models
{
    public partial class StaffSalary
    {

        #region SalaryMethod
        
        public async Task<List<SalaryMethod>> GetSalaryMethod(HrhubContext hrhubContext)
        {
            try
            {

                var dbResult = await hrhubContext.SalaryMethods.Where(x => x.IsDeleted == false).ToListAsync();
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

        #endregion

    }
}
