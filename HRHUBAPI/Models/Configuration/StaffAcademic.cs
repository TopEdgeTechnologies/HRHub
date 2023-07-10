using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace HRHUBAPI.Models
{
    public partial class StaffAcademic
    {
        #region NotMapped

        [NotMapped]
        public int? TranFlag { get; set; }

        [NotMapped]
        public IEnumerable<StaffAcademic>? StaffAcademicList { get; set; }

        [NotMapped]
        public IEnumerable<string>? StudyTitleList { get; set; }

        [NotMapped]
        public IEnumerable<string>? StudyInstituteList { get; set; }

        [NotMapped]
        public IEnumerable<string>? StudyDateFromList { get; set; }

        [NotMapped]
        public IEnumerable<string>? StudyDateToList { get; set; }

        [NotMapped]
        public IEnumerable<int>? StudyInProcessList { get; set; }

        [NotMapped]
        public IEnumerable<string>? WorkTitleList { get; set; }

        [NotMapped]
        public IEnumerable<string>? WorkCompanyList { get; set; }

        [NotMapped]
        public IEnumerable<string>? WorkDateFromList { get; set; }

        [NotMapped]
        public IEnumerable<string>? WorkDateToList { get; set; }

        [NotMapped]
        public IEnumerable<int>? WorkInProcessList { get; set; }

        #endregion

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
                    var result = hrhubContext.StaffAcademics.Where(x => x.StaffId == objStaffAcademic.StaffId).ToList(); 
                    if (result != null && result.Count > 0)
                    {
                        hrhubContext.RemoveRange(result);
                        await hrhubContext.SaveChangesAsync();    
                    }
                    
                    List<StaffAcademic> ListStaffAcademic = new List<StaffAcademic>();

                    int i = 0;
                    if(objStaffAcademic.StudyTitleList != null) //&& objStaffAcademic.StudyTitleList.Any()
                    {
                        foreach(var item in objStaffAcademic.StudyTitleList)
                        {
                            if(objStaffAcademic.StudyTitleList.ToArray()[i] != null)
                            {
                                StaffAcademic obj = new StaffAcademic();
                                obj.StaffId = objStaffAcademic.StaffId;
                                obj.Title = objStaffAcademic.StudyTitleList.ToArray()[i];
                                obj.InstituteName = string.IsNullOrWhiteSpace(objStaffAcademic.StudyInstituteList?.ToArray()[i]) ? null : objStaffAcademic.StudyInstituteList.ToArray()[i];  
                                obj.FromDate = string.IsNullOrWhiteSpace(objStaffAcademic.StudyDateFromList?.ToArray()[i]) ? null : Convert.ToDateTime(objStaffAcademic.StudyDateFromList.ToArray()[i]); 
                                obj.ToDate = string.IsNullOrWhiteSpace(objStaffAcademic.StudyDateToList?.ToArray()[i]) ? null : Convert.ToDateTime(objStaffAcademic.StudyDateToList.ToArray()[i]);
                                obj.InProcess = Convert.ToBoolean(objStaffAcademic.StudyInProcessList?.ToArray()[i]);
                                obj.Type = "Study";
                                obj.CreatedBy = objStaffAcademic.CreatedBy;
                                obj.CreatedOn = DateTime.Now;

                                ListStaffAcademic.Add(obj);
                            }
                            i++;
                        }
                    }

                    int j = 0;
                    if (objStaffAcademic.WorkTitleList != null)
                    {
                        foreach (var item in objStaffAcademic.WorkTitleList)
                        {
                            if(objStaffAcademic.WorkTitleList.ToArray()[j] != null)
                            {
                                StaffAcademic obj = new StaffAcademic();
                                obj.StaffId = objStaffAcademic.StaffId;
                                obj.Title = objStaffAcademic.WorkTitleList.ToArray()[j];
                                obj.InstituteName = string.IsNullOrWhiteSpace(objStaffAcademic.WorkCompanyList?.ToArray()[j]) ? null : objStaffAcademic.WorkCompanyList.ToArray()[j];
                                obj.FromDate = string.IsNullOrWhiteSpace(objStaffAcademic.WorkDateFromList?.ToArray()[j]) ? null : Convert.ToDateTime(objStaffAcademic.WorkDateFromList.ToArray()[j]);
                                obj.ToDate = string.IsNullOrWhiteSpace(objStaffAcademic.WorkDateToList?.ToArray()[j]) ? null : Convert.ToDateTime(objStaffAcademic.WorkDateToList.ToArray()[j]);
                                obj.InProcess = Convert.ToBoolean(objStaffAcademic.WorkInProcessList?.ToArray()[j]);
                                obj.Type = "Work";
                                obj.CreatedBy = objStaffAcademic.CreatedBy;
                                obj.CreatedOn = DateTime.Now;

                                ListStaffAcademic.Add(obj);
                            }
                            j++;
                        }
                    }

                    if(ListStaffAcademic.Count > 0)
                    {
                        await hrhubContext.AddRangeAsync(ListStaffAcademic);  
                        await hrhubContext.SaveChangesAsync();
                        objStaffAcademic.TranFlag = 1;
                    }
                    dbContextTransaction.Commit();
                    return objStaffAcademic;
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
