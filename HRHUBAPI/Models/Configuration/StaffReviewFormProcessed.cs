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
        public IEnumerable<StaffReviewFormProcessed>? ListStaffAppraisal{ get; set; }
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
                        Weightage = row["EarnedWeightage"].ToString() + "/" + row["TotalWeightage"].ToString(),
                        AppraisalPercentage = Convert.ToDecimal(row["AppraisalPercentage"])

                    }).ToList();

                return list;

            }
            catch (Exception ex)
            {

                throw;

            }
        }
        public async Task<List<Section>> GetReviewSections(int Id,int staffid, HrhubContext _context)
        {
            try
            {
                var list = from s in _context.Sections
                            join r in _context.StaffReviewFormProcesseds on s.ReviewFormId equals r.ReviewFormId

                            where s.ReviewFormId == Id && r.ReviewedStaffId == staffid
                            select new Section
                            {
                                Title = s.Title,
                                TotalWeightage = s.TotalWeightage
                             

                            };

                return list != null ? list.ToList() : new List<Section>();

            }
            catch (Exception ex)
            {

                throw;

            }
        }


    }
}
