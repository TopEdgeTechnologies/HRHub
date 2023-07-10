using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRHUBAPI.Models
{
    public partial class StaffDependent
    {
        #region NotMapped

        [NotMapped]
        public int? TranFlag { get; set; }

        [NotMapped]
        public IEnumerable<StaffDependent>? StaffDependentList { get; set; }

        [NotMapped]
        public IEnumerable<string>? RelationMaleList { get; set; }

        [NotMapped]
        public IEnumerable<string>? FirstNameMaleList { get; set; }

        [NotMapped]
        public IEnumerable<string>? LastNameMaleList { get; set; }

        [NotMapped]
        public IEnumerable<string>? OccupationMaleList { get; set; }

        [NotMapped]
        public IEnumerable<string>? RelationFemaleList { get; set; }

        [NotMapped]
        public IEnumerable<string>? FirstNameFemaleList { get; set; }

        [NotMapped]
        public IEnumerable<string>? LastNameFemaleList { get; set; }

        [NotMapped]
        public IEnumerable<string>? OccupationFemaleList { get; set; }

        #endregion

        public async Task<List<StaffDependent>> GetStaffDependent(HrhubContext hrhubContext)
        {
            try
            {
                return await hrhubContext.StaffDependents.ToListAsync();
            }
            catch (Exception ex) { throw; }
        }

        public async Task<StaffDependent> GetStaffDependentById(int Id, HrhubContext hrhubContext)
        {
            try
            {
                var dbResult = await hrhubContext.StaffDependents.FirstOrDefaultAsync(x => x.DependentId == Id);
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

        public async Task<List<StaffDependent>> GetStaffDependentByStaffId(int StaffId, HrhubContext hrhubContext)
        {
            try
            {
                List<StaffDependent> ListStaffDependent = new List<StaffDependent>();
                ListStaffDependent = await hrhubContext.StaffDependents.Where(x => x.StaffId == StaffId).ToListAsync();
                if (ListStaffDependent != null)
                {
                    return ListStaffDependent;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex) { throw; }
        }

        public async Task<StaffDependent> PostStaffDependent(StaffDependent objStaffDependent, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    var result = hrhubContext.StaffDependents.Where(x => x.StaffId == objStaffDependent.StaffId).ToList();
                    if (result != null && result.Count > 0)
                    {
                        hrhubContext.RemoveRange(result);
                        await hrhubContext.SaveChangesAsync();
                    }

                    List<StaffDependent> ListStaffDependent = new List<StaffDependent>();

                    int i = 0;
                    if (objStaffDependent.RelationMaleList != null) 
                    {
                        foreach (var item in objStaffDependent.RelationMaleList)
                        {
                            if (objStaffDependent.RelationMaleList.ToArray()[i] != null)
                            {
                                StaffDependent obj = new StaffDependent();
                                obj.StaffId = objStaffDependent.StaffId;
                                obj.Relation = objStaffDependent.RelationMaleList.ToArray()[i];
                                obj.FirstName = string.IsNullOrWhiteSpace(objStaffDependent.FirstNameMaleList?.ToArray()[i]) ? null : objStaffDependent.FirstNameMaleList.ToArray()[i];
                                obj.LastName = string.IsNullOrWhiteSpace(objStaffDependent.LastNameMaleList?.ToArray()[i]) ? null : objStaffDependent.LastNameMaleList.ToArray()[i];
                                obj.Occupation = string.IsNullOrWhiteSpace(objStaffDependent.OccupationMaleList?.ToArray()[i]) ? null : objStaffDependent.OccupationMaleList.ToArray()[i];
                                obj.Gender = "Male";
                                obj.CreatedBy = objStaffDependent.CreatedBy;
                                obj.CreatedOn = DateTime.Now;

                                ListStaffDependent.Add(obj);
                            }
                            i++;
                        }
                    }

                    int j = 0;
                    if (objStaffDependent.RelationFemaleList != null)
                    {
                        foreach (var item in objStaffDependent.RelationFemaleList)
                        {
                            if (objStaffDependent.RelationFemaleList.ToArray()[j] != null)
                            {
                                StaffDependent obj = new StaffDependent();
                                obj.StaffId = objStaffDependent.StaffId;
                                obj.Relation = objStaffDependent.RelationFemaleList.ToArray()[j];
                                obj.FirstName = string.IsNullOrWhiteSpace(objStaffDependent.FirstNameFemaleList?.ToArray()[j]) ? null : objStaffDependent.FirstNameFemaleList.ToArray()[j];
                                obj.LastName = string.IsNullOrWhiteSpace(objStaffDependent.LastNameFemaleList?.ToArray()[j]) ? null : objStaffDependent.LastNameFemaleList.ToArray()[j];
                                obj.Occupation = string.IsNullOrWhiteSpace(objStaffDependent.OccupationFemaleList?.ToArray()[j]) ? null : objStaffDependent.OccupationFemaleList.ToArray()[j];
                                obj.Gender = "Female";
                                obj.CreatedBy = objStaffDependent.CreatedBy;
                                obj.CreatedOn = DateTime.Now;

                                ListStaffDependent.Add(obj);
                            }
                            j++;
                        }
                    }

                    if (ListStaffDependent.Count > 0)
                    {
                        await hrhubContext.AddRangeAsync(ListStaffDependent);
                        await hrhubContext.SaveChangesAsync();
                        objStaffDependent.TranFlag = 1;
                    }
                    dbContextTransaction.Commit();
                    return objStaffDependent;
                }
                catch (Exception ex) { dbContextTransaction.Rollback(); throw; }
            }
        }

        public async Task<bool> DeleteStaffDependent(int Id, int UserId, HrhubContext hrhubContext)
        {
            using (var dbContextTransaction = hrhubContext.Database.BeginTransaction())
            {
                try
                {
                    var dbResult = await hrhubContext.StaffDependents.FirstOrDefaultAsync(x => x.DependentId == Id);
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

        public async Task<bool> AlreadyExists(int Id, int StaffId, string FirstName, HrhubContext hrhubContext)
        {
            try
            {
                if (Id > 0)
                {
                    var dbResult = await hrhubContext.StaffDependents.FirstOrDefaultAsync(x => x.StaffId == StaffId && x.FirstName == FirstName && x.DependentId != Id);
                    if (dbResult != null)
                    {
                        return true;
                    }
                }
                else
                {
                    var dbResult = await hrhubContext.StaffDependents.FirstOrDefaultAsync(x => x.StaffId == StaffId && x.FirstName == FirstName);
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
