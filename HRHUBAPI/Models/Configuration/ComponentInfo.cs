using HRHUBAPI.Models.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HRHUBAPI.Models
{
    public partial class ComponentInfo
    {

        DbConnection _db = new DbConnection();



        [NotMapped]
        public int? TranFlag { get; set; }
        
        [NotMapped]
        public int? StaffCount { get; set; }

        [NotMapped]
        public string? GroupTitle { get; set; }
        
        [NotMapped]
        public IEnumerable<ComponentInfo>? ComponentInfoList { get; set; }


        public async Task<List<ComponentInfo>> GetBenefitInfo(int CompanyId, HrhubContext hrhubContext)
        {
           
                try
                {

                    List<ComponentInfo> list = new List<ComponentInfo>();

                    string query = "EXEC dbo.sp_Benefit_Count_Staff " + CompanyId + "  ";
                    DataTable dt = _db.ReturnDataTable(query);

                    list = dt.AsEnumerable()
                        .Select(row => new ComponentInfo
                        {


                            ComponentId = Convert.ToInt32(row["ComponentID"]),                           
                            Title = row["Title"].ToString(),
                            Status= Convert.ToBoolean(row["Status"]),
                            StaffCount = Convert.ToInt32(row["StaffCount"])
                           

                        }).OrderByDescending(x => x.ComponentId).ToList();
                    return list;




                }
                catch (Exception ex)
                {

                    throw;

                }

            }

       

        public async Task<ComponentInfo> GetBenefitInfoById(int Id, HrhubContext hrhubContext)
        {
            try
            {
                var dbResult = await hrhubContext.ComponentInfos.FirstOrDefaultAsync(x => x.IsDeleted == false && x.ComponentId == Id);
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

        public async Task<ComponentInfo> PostBenefitInfo(ComponentInfo objComponentInfo, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    var dbResult = await hrhubContext.ComponentInfos.FirstOrDefaultAsync(x => x.IsDeleted == false && x.ComponentId == objComponentInfo.ComponentId);
                    if (dbResult != null && objComponentInfo.ComponentId > 0)
                    {
                        dbResult.ComponentId = objComponentInfo.ComponentId;
                        dbResult.ComponentGroupId = objComponentInfo.ComponentGroupId;
                        dbResult.Title = objComponentInfo.Title;
                        dbResult.CalculationMethod = objComponentInfo.CalculationMethod;
                        dbResult.CompanyContribution = objComponentInfo.CompanyContribution;
                        dbResult.Category = objComponentInfo.Category;
                        dbResult.Type = objComponentInfo.Type;
                        dbResult.Status = objComponentInfo.Status;
                        dbResult.UpdatedBy = objComponentInfo.UpdatedBy;
                        dbResult.UpdatedOn = DateTime.Now;
                        dbResult.IsDeleted = false;

                        await hrhubContext.SaveChangesAsync();
                        dbResult.TranFlag = 2;
                        dbContextTransaction.Commit();
                        return dbResult;
                    }
                    else
                    {
                        objComponentInfo.CreatedOn = DateTime.Now;
                        objComponentInfo.IsDeleted = false;
                        objComponentInfo.IsBenefit= true;
                        objComponentInfo.Type = "Fixed";
                        hrhubContext.Add(objComponentInfo);
                        await hrhubContext.SaveChangesAsync();
                        objComponentInfo.TranFlag = 1;
                        dbContextTransaction.Commit();
                        return objComponentInfo;
                    }
                }
                catch (Exception ex) { dbContextTransaction.Rollback(); throw; }
            }
        }

        public async Task<bool> DeleteBenefitInfo(int Id, int UserId, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    //bool recordDeleted = false;
                    var dbResult = await hrhubContext.ComponentInfos.FirstOrDefaultAsync(x => x.IsDeleted == false && x.ComponentId == Id);
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

        public async Task<bool> AlreadyExists(int Id, string Title, int CompanyId, HrhubContext hrhubContext)
        {
            try
            {
                if (Id > 0)
                {
                    var dbResult = await hrhubContext.ComponentInfos.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Title == Title && x.ComponentId != Id && x.CompanyId==CompanyId);
                    if (dbResult != null)
                    {
                        return true;
                    }
                }
                else
                {
                    var dbResult = await hrhubContext.ComponentInfos.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Title == Title && x.CompanyId==CompanyId);
                    if (dbResult != null)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception e) { throw; }
        }



        #region Component in Setting

        public async Task<List<ComponentInfo>> GetComponentsInfo(int CompanyId, HrhubContext hrhubContext)
        {

            try
            {

                List<ComponentInfo> list = new List<ComponentInfo>();

                string query = "EXEC dbo.sp_Get_ComponentInfo " + CompanyId;
                DataTable dt = _db.ReturnDataTable(query);

                list = dt.AsEnumerable()
                    .Select(row => new ComponentInfo
                    {
                        ComponentId = Convert.ToInt32(row["ComponentID"]),
                        Title = row["Title"].ToString(),
                        Status = Convert.ToBoolean(row["Status"]),
                        GroupTitle = row["GroupTitle"].ToString(),
                        Type = row["Type"].ToString(),
                        Category = row["Category"].ToString(),
                        IsBenefit = Convert.ToBoolean(row["IsBenefit"])

                    }).OrderByDescending(x => x.ComponentId).ToList();
                return list;
            }
            catch (Exception ex)
            {

                throw;

            }

        }

        public async Task<ComponentInfo> PostComponent(ComponentInfo objComponentInfo, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    var dbResult = await hrhubContext.ComponentInfos.FirstOrDefaultAsync(x => x.IsDeleted == false && x.ComponentId == objComponentInfo.ComponentId);
                    if (dbResult != null && objComponentInfo.ComponentId > 0)
                    {
                        dbResult.ComponentId = objComponentInfo.ComponentId;
                        dbResult.ComponentGroupId = objComponentInfo.ComponentGroupId;
                        dbResult.Title = objComponentInfo.Title;
                        dbResult.CalculationMethod = objComponentInfo.CalculationMethod;
                        dbResult.CompanyContribution = objComponentInfo.CompanyContribution;
                        dbResult.Category = objComponentInfo.Category;
                        dbResult.Type = objComponentInfo.Type;
                        dbResult.Status = objComponentInfo.Status;
                        dbResult.UpdatedBy = objComponentInfo.UpdatedBy;
                        dbResult.UpdatedOn = DateTime.Now;
                        dbResult.IsDeleted = false;

                        await hrhubContext.SaveChangesAsync();
                        dbResult.TranFlag = 2;
                        dbContextTransaction.Commit();
                        return dbResult;
                    }
                    else
                    {
                        objComponentInfo.CreatedOn = DateTime.Now;
                        objComponentInfo.IsDeleted = false;
                        objComponentInfo.IsBenefit = false;
                        hrhubContext.Add(objComponentInfo);
                        await hrhubContext.SaveChangesAsync();
                        objComponentInfo.TranFlag = 1;
                        dbContextTransaction.Commit();
                        return objComponentInfo;
                    }
                }
                catch (Exception ex) { dbContextTransaction.Rollback(); throw; }
            }
        }

        #endregion






    }
}
