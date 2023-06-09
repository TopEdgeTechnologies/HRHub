using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System;

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualBasic;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using HRHUBAPI.Models.Configuration;
using System.Runtime.Intrinsics.Arm;
using System.ComponentModel.Design;

namespace HRHUBAPI.Models
{
    public partial class StaffReviewFormProcessed
    {
        [NotMapped]
        public decimal? AppraisalPercentage { get; set; }
        [NotMapped]
        public bool? IsAppraisalSet { get; set; }
        [NotMapped]
        public decimal? CurrentSalary { get; set; }
        [NotMapped]
        public string? Weightage { get; set; }
        [NotMapped]
        public string? AppraisalDate { get; set; }
        public string? StaffRegistrationNo { get; set; }
        [NotMapped]
        public string? StaffName { get; set; }
        [NotMapped]
        public string? StaffSnap { get; set; }
        [NotMapped]
        public string? Department { get; set; }
        [NotMapped]
        public string? Designation { get; set; }
        [NotMapped]
        public IEnumerable<StaffReviewFormProcessed>? ListStaffAppraisal { get; set; }
        public async Task<List<StaffReviewFormProcessed>> GetPerformanceAppraisal(HrhubContext _context)
        {
            try
            {
                DbConnection _db = new DbConnection();

                string query = "EXEC Get_StaffPerformaceAppraisalList";
                DataTable dt = _db.ReturnDataTable(query);

                var list = dt.AsEnumerable()
                    .Select(row => new StaffReviewFormProcessed
                    {
                        ReviewFormId = Convert.ToInt32(row["ReviewFormId"]),
                        ReviewedStaffId = Convert.ToInt32(row["Reviewed_StaffID"]),
                        StaffName = row["StaffName"].ToString(),
                        StaffSnap = string.IsNullOrWhiteSpace(row["SnapPath"].ToString()) ? "/Images/Avatar.png" : row["SnapPath"].ToString(),
                        StaffRegistrationNo = row["RegistrationNo"].ToString(),
                        Department = row["Department"].ToString(),
                        Designation = row["Designation"].ToString(),
                        CurrentSalary = Convert.ToDecimal(row["SalaryAmount"]),
                        Weightage = row["EarnedWeightage"].ToString() + "/" + row["TotalWeightage"].ToString(),
                        AppraisalPercentage = Convert.ToDecimal(row["AppraisalPercentage"]),
                        IsAppraisalSet = Convert.ToBoolean(row["IsAppraisalSet"])

                    }).ToList();

                return list;

            }
            catch (Exception ex)
            {

                throw;

            }
        }
        public async Task<List<Section>> GetReviewSections(int Id, int staffid, HrhubContext _context)
        {
            try
            {
                DbConnection _db = new DbConnection();

                string query = "EXEC Get_StaffSectionWeightageList " + Id + " , "+ staffid;
                DataTable dt = _db.ReturnDataTable(query);

                var list = dt.AsEnumerable()
                    .Select(row => new Section
                    {

                        Title = row["Title"].ToString(),
                        TotalWeightage = Convert.ToDecimal(row["TotalWeightage"])
                       

                    }).ToList();



                //var list = from s in _context.Sections
                //           join r in _context.StaffReviewFormProcesseds on s.ReviewFormId equals r.ReviewFormId

                //           where s.ReviewFormId == Id && r.ReviewedStaffId == staffid
                //           select new Section
                //           {
                //               Title = s.Title,
                //               TotalWeightage = s.TotalWeightage,
                //               EarnedWeightage = s.EarnedWeightage

                //           };

        //        return list != null ? list.ToList() : new List<Section>();

            }
            catch (Exception ex)
            {

                throw;

            }
        }
        public async Task<Appraisal> PostStaffAppraisal(List<Appraisal> listAppraisalInfo, HrhubContext _context)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var checkappraisalInfo = new Appraisal();
                    if (listAppraisalInfo.Count() > 0)
                    {
                        // save Appraisal data 
                        _context.Appraisals.AddRange(listAppraisalInfo);
                        await _context.SaveChangesAsync();

                        // Update salary in staff table
                        foreach (var item in listAppraisalInfo)
                        {
                            var checkStaffInfo = await _context.Staff.FirstOrDefaultAsync(x => x.StaffId == item.StaffId && x.IsDeleted == false);
                            if (checkStaffInfo != null && checkStaffInfo.StaffId > 0)
                            {
                                checkStaffInfo.SalaryAmount = item.NewMonthlySalary;
                                checkStaffInfo.UpdatedBy = item.CreatedBy;
                                checkStaffInfo.UpdatedOn = DateTime.Now;

                                await _context.SaveChangesAsync();

                            }
                        }
                    }

                    await _context.SaveChangesAsync();
                    dbContextTransaction.Commit();
                    return checkappraisalInfo;

                }
                catch (Exception ex)
                {

                    throw;

                }
            }
        }
        public async Task<Appraisal> EditStaffAppraisal(Appraisal objAppraisalInfo, HrhubContext _context)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    decimal incrementedsalary = 0;
                    var checkAppraisalInfo = await _context.Appraisals.FirstOrDefaultAsync(x => x.StaffId == objAppraisalInfo.StaffId && x.ReviewFormId == objAppraisalInfo.ReviewFormId);
                    if (checkAppraisalInfo != null && checkAppraisalInfo.AppraisalId > 0)
                    {
                        checkAppraisalInfo.AppraisalPercentage = objAppraisalInfo.AppraisalPercentage;
                        decimal raiseamount = Convert.ToDecimal((checkAppraisalInfo.CurrentMonthlySalary * objAppraisalInfo.AppraisalPercentage) / 100);
                        decimal currentMonthlySalary = Convert.ToDecimal(checkAppraisalInfo.CurrentMonthlySalary);
                        incrementedsalary = raiseamount + currentMonthlySalary;
                        checkAppraisalInfo.NewMonthlySalary = incrementedsalary;
                        checkAppraisalInfo.UpdatedBy = objAppraisalInfo.CreatedBy;
                        checkAppraisalInfo.UpdatedOn = DateTime.Now;

                        await _context.SaveChangesAsync();

                    }
                    else{
                        objAppraisalInfo.AppraisalDate = DateTime.Now;
                        objAppraisalInfo.CreatedOn = DateTime.Now;
                        _context.Appraisals.Add(objAppraisalInfo);
                    }



                    var checkStaffInfo = await _context.Staff.FirstOrDefaultAsync(x => x.StaffId == objAppraisalInfo.StaffId && x.IsDeleted == false);
                    if (checkStaffInfo != null && checkStaffInfo.StaffId > 0)
                    {
                        checkStaffInfo.SalaryAmount = objAppraisalInfo.NewMonthlySalary;
                        checkStaffInfo.UpdatedBy = objAppraisalInfo.CreatedBy;
                        checkStaffInfo.UpdatedOn = DateTime.Now;

                        await _context.SaveChangesAsync();

                    }

                    dbContextTransaction.Commit();
                    return checkAppraisalInfo;

                }
                catch (Exception ex)
                {

                    throw;

                }
            }
        }

    }
}
