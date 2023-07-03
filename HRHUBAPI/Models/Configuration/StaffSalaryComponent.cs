﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRHUBAPI.Models
{
    public partial class StaffSalaryComponent
    {
        [NotMapped]
        public int? TranFlag { get; set; }

        [NotMapped]
        public string? Check { get; set; }
        [NotMapped]
        public string? Category { get; set; }

        [NotMapped]
        public string? ComponentTitle { get; set; }

        [NotMapped]
        public string? StaffFName { get; set; } 
        [NotMapped]
        public string? StaffSName { get; set; }

        public async Task<List<StaffSalaryComponent>> GetSalaryComponent(int CompanyId,int ComponentId, HrhubContext hrhubContext)
        {
            try
            {
                var List = from SC in hrhubContext.StaffSalaryComponents
                           join s in hrhubContext.Staff on SC.StaffId equals s.StaffId                          
                           where s.CompanyId == CompanyId && s.IsDeleted == false &&  s.Status == true && SC.ComponentId== ComponentId && SC.IsDeleted== false

                           select new StaffSalaryComponent
                           {

                              StaffSalaryComponentId = SC.StaffSalaryComponentId,
                              StaffId= SC.StaffId,
                              ComponentId = SC.ComponentId,
                              StaffFName = s.FirstName,
                              StaffSName = s.LastName,
                              PercentageValue = SC.PercentageValue,
                              ComponentAmount = SC.ComponentAmount,
                              CompanyContributionValue= SC.CompanyContributionValue,
                              CompanyContributionCalculationMethod = SC.CompanyContributionCalculationMethod,
                              CompanyContributionAmount = SC.CompanyContributionAmount,
                               

                           };
                return List != null ? List.OrderByDescending(x => x.StaffSalaryComponentId).ToList() : new List<StaffSalaryComponent>();



            }
            catch (Exception ex) { throw; }
        }

        public async Task<StaffSalaryComponent> GetStaffSalaryComponentById(int Id, HrhubContext hrhubContext)
        {
            try
            {
                var dbResult = await hrhubContext.StaffSalaryComponents.FirstOrDefaultAsync(x => x.StaffSalaryComponentId == Id);
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

        public async Task<StaffSalaryComponent> PostStaffSalaryComponent(StaffSalaryComponent objStaffSalaryComponent, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {

                var staffGet = await hrhubContext.Staff.FirstOrDefaultAsync(x => x.StaffId == objStaffSalaryComponent.StaffId);
                decimal GrossSalary = Convert.ToDecimal(staffGet.SalaryAmount);
                try
                {
                    var dbResult = await hrhubContext.StaffSalaryComponents.FirstOrDefaultAsync(x => x.StaffSalaryComponentId == objStaffSalaryComponent.StaffSalaryComponentId);
                    if (dbResult != null && objStaffSalaryComponent.StaffSalaryComponentId > 0)
                    {

                        if (dbResult.CompanyContributionCalculationMethod == "Percentage" || objStaffSalaryComponent.Check == "Per %")
                        {
                            dbResult.CompanyContributionCalculationMethod = "Percentage";
                            dbResult.CompanyContributionAmount = (GrossSalary * objStaffSalaryComponent.CompanyContributionValue) / 100; // for company
                            dbResult.ComponentAmount = (GrossSalary * objStaffSalaryComponent.PercentageValue) / 100; // for staff
                        }
                        else
                        {
                            dbResult.CompanyContributionCalculationMethod = "Amount";
                            dbResult.CompanyContributionAmount = objStaffSalaryComponent.CompanyContributionValue;
                            dbResult.ComponentAmount = objStaffSalaryComponent.PercentageValue;

                        }


                        dbResult.StaffSalaryComponentId = objStaffSalaryComponent.StaffSalaryComponentId;
                        dbResult.StaffId = objStaffSalaryComponent.StaffId;
                        dbResult.ComponentId = objStaffSalaryComponent.ComponentId;
                        dbResult.PercentageValue = objStaffSalaryComponent.PercentageValue;
                        dbResult.ComponentAmount = objStaffSalaryComponent.ComponentAmount;

						await hrhubContext.SaveChangesAsync();
                        dbResult.TranFlag = 2;
                        dbContextTransaction.Commit();
                        return dbResult;
                    }
                    else
                    {

                        if (objStaffSalaryComponent.Check== "Fixed")
                        {

                        }

						hrhubContext.Add(objStaffSalaryComponent);
                        objStaffSalaryComponent.IsDeleted= false;
                        objStaffSalaryComponent.CreatedOn= DateTime.Now;
                        
                        await hrhubContext.SaveChangesAsync();
                        objStaffSalaryComponent.TranFlag = 1;
                        dbContextTransaction.Commit();
                        return objStaffSalaryComponent;
                    }
                }
                catch (Exception ex) { dbContextTransaction.Rollback(); throw; }
            }
        }

        public async Task<bool> DeleteStaffSalaryComponent(int Id,int UserId, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    var dbResult = await hrhubContext.StaffSalaryComponents.FirstOrDefaultAsync(x => x.StaffSalaryComponentId == Id);
                    if (dbResult != null)
                    {
                        dbResult.UpdatedOn = DateTime.Now;
                        dbResult.UpdatedBy = UserId;
                        await hrhubContext.SaveChangesAsync();
                        dbContextTransaction.Commit();
                    }
                    return true;
                }
                catch (Exception e) { dbContextTransaction.Rollback(); return false; throw; }
            }
        }


        public async Task<StaffSalaryComponent> DeleteStaffBenefitComponent(int Id,int UserId, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    StaffSalaryComponent? dbResult = await hrhubContext.StaffSalaryComponents.FirstOrDefaultAsync(x => x.StaffSalaryComponentId == Id);
                    if (dbResult != null)
                    {
                        dbResult.IsDeleted = true;
                        dbResult.UpdatedOn= DateTime.Now;
                        dbResult.UpdatedBy = UserId;
                        await hrhubContext.SaveChangesAsync();
                        dbContextTransaction.Commit();
                    }
                    return dbResult;
                }
                catch (Exception e) { dbContextTransaction.Rollback(); return null; throw; }
            }
        }

        public async Task<bool> AlreadyExists(int Id, int StaffId, int ComponentId, HrhubContext hrhubContext)
        {
            try
            {
                if (Id > 0)
                {
                    var dbResult = await hrhubContext.StaffSalaryComponents.FirstOrDefaultAsync(x => x.StaffId == StaffId && x.ComponentId == ComponentId && x.StaffSalaryComponentId != Id && x.IsDeleted==false);
                    if (dbResult != null)
                    {
                        return true;
                    }
                }
                else
                {
                    var dbResult = await hrhubContext.StaffSalaryComponents.FirstOrDefaultAsync(x => x.StaffId == StaffId && x.ComponentId == ComponentId && x.IsDeleted==false);
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
