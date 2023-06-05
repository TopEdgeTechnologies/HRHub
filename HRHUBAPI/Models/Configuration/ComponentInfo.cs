﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HRHUBAPI.Models
{
    public partial class ComponentInfo
    {
        [NotMapped]
        public int? TranFlag { get; set; }

        [NotMapped]
        public string? GroupTitle { get; set; }
        
        [NotMapped]
        public IEnumerable<ComponentInfo>? ComponentInfoList { get; set; }

        //public async Task<List<ComponentInfo>> GetComponentInfo(HrhubContext hrhubContext)
        //{
        //    try
        //    {
        //        var queryList = from ci in hrhubContext.ComponentInfos
        //                        join cg in hrhubContext.ComponentGroups
        //                        on ci.ComponentGroupId equals cg.ComponentGroupId into joinedData
        //                        from result in joinedData.DefaultIfEmpty()
        //                        orderby result.Title != null ? 1 : 2, result.Title
        //                        select new ComponentInfo
        //                        {
        //                            ComponentId = ci.ComponentId,
        //                            ComponentGroupId = ci.ComponentGroupId,
        //                            GroupTitle = result.Title != null ? result.Title : string.Empty,
        //                            Title = ci.Title,
        //                            CalculationMethod = ci.CalculationMethod,
        //                            CompanyContribution = ci.CompanyContribution,
        //                            Category = ci.Category,
        //                            Type = ci.Type,
        //                            Status = ci.Status
        //                        };

        //        return await queryList.ToListAsync();

        //        //List<ComponentInfo> ComponentInfo = new List<ComponentInfo>();
        //        //ComponentInfo = await hrhubContext.ComponentInfos.Where(x => x.IsDeleted == false).ToListAsync();
        //        //return ComponentInfo;
        //    }
        //    catch (Exception ex) { throw; }
        //}
        public async Task<List<ComponentInfo>> GetBenefitInfo(int CompanyId, HrhubContext hrhubContext)
        {
            try
            {

                //var queryList = from ss in hrhubContext.StaffSalaryComponents
                //                join ComponentInfo ci in hrhubContext.StaffSalaryComponents on ss.ComponentId equals ci.ComponentId


                //                where ci.IsBenefit == true && ci.CompanyId == CompanyId && ci.IsDeleted == false
                //                select new ComponentInfo()
                //                {
                //                    ComponentId = ci.ComponentId,
                //                    Title = ci.Title,
                //                    Status= ci.Status


                //                };
                //return queryList != null ? queryList.OrderByDescending(x => x.ComponentId).ToList() : new List<ComponentInfo>();
                // }

                List<ComponentInfo> ComponentInfo = new List<ComponentInfo>();
                ComponentInfo = await hrhubContext.ComponentInfos.Where(x => x.IsDeleted == false && x.IsBenefit == true && x.CompanyId == CompanyId).ToListAsync();
                return ComponentInfo;
            }
            catch (Exception ex) { throw; }
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






      



    }
}
