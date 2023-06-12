using HRHUBAPI.Models.Configuration;
using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Data;

namespace HRHUBAPI.Models
{



    public partial class LeaveApproval
    {

        [NotMapped]
        public string? ForwardByStaffName { get; set; }
        [NotMapped]
        public string? ApprovedByStaffName { get; set; }
        [NotMapped]
        public string? ApprovedByStaffSnap{ get; set; }
        [NotMapped]
        public string? ApprovedByDesignation { get; set; }
        [NotMapped]
        public string? LeaveApprovalDate{ get; set; }
        [NotMapped]
        public int? LeaveTypeId { get; set; }
        [NotMapped]
        public int? CreatedBy { get; set; }
        [NotMapped]
        public int? ApplicantStaffId { get; set; }
        [NotMapped]
        public DateTime? StartDate { get; set; }
        [NotMapped]
        public DateTime? EndDate { get; set; }
        [NotMapped]
        public bool? MarkAsShortLeave { get; set; }
        [NotMapped]
        public bool? MarkAsHalfLeave { get; set; }
        [NotMapped]
        public string? LeaveForwardDate { get; set; }
        [NotMapped]
        public int? FinalApprovalDesignationID { get; set; }
        [NotMapped]
        public int? ApprovedByDesignationID { get; set; }

        public async Task<List<LeaveApproval>> GetLeaveAprrovalDetail(int LeaveId, HrhubContext _context)
        {
            try
            {
                DbConnection _db = new DbConnection();

                string query = "EXEC sp_Get_LeaveApprovalDetail " + LeaveId;
                DataTable dt = _db.ReturnDataTable(query);

                var list = dt.AsEnumerable()
                    .Select(row => new LeaveApproval
                    {
                        LeaveId = Convert.ToInt32(row["LeaveId"]),
                        ForwardedByStaffId = String.IsNullOrWhiteSpace(row["ForwardedBy_StaffId"].ToString())? 0 : Convert.ToInt32(row["ForwardedBy_StaffId"]),
                        ApprovalByStaffId = Convert.ToInt32(row["ApprovalBy_StaffId"]),

                        ForwardByStaffName = String.IsNullOrWhiteSpace(row["ForwardBy_StaffName"].ToString())? "" : row["ForwardBy_StaffName"].ToString(),
                        LeaveForwardDate = row["Forward_Date"] == DBNull.Value ? "DD-MMM-YYYY" : Convert.ToDateTime(row["Forward_Date"]).ToString("dd-MMM-yyyy"),

                        ApprovedByStaffName = row["ApprovedBy_StaffName"].ToString(),
                        LeaveApprovalDate = row["Approval_Date"] == DBNull.Value ? "DD-MMM-YYYY" : Convert.ToDateTime(row["Approval_Date"]).ToString("dd-MMM-yyyy"),

                        ApprovedByStaffSnap = string.IsNullOrWhiteSpace(row["SnapPath"].ToString()) ? "/Images/Avatar.png" : row["SnapPath"].ToString(),
                        ApprovedByDesignation = row["ApprovedBy_Designation"].ToString(),
                        Remarks = row["Remarks"].ToString(),
                        LeaveStatusId = Convert.ToInt32(row["LeaveStatusId"])

                    })
                    .ToList();
                return list;


                //var list = await (from la in _context.LeaveApprovals
                //                  join sa in _context.Staff on la.ApprovalByStaffId equals sa.StaffId into a
                //                  from ct in a.DefaultIfEmpty()
                //                  join sf in _context.Staff on la.ForwardedByStaffId equals sf.StaffId into g
                //                  from cct in g.DefaultIfEmpty()
                //                  join sff in _context.Designations on cct.DesignationId equals sff.DesignationId into d
                //                  from cctt in d.DefaultIfEmpty()

                //                  where la.LeaveId == LeaveId
                //                  select new LeaveApproval()
                //                  {
                //                      LeaveId = la.LeaveId,
                //                      ForwardedByStaffId = la.ForwardedByStaffId,
                //                      ForwardByStaffName = cct.FirstName,
                //                      LeaveForwardDate = la.ForwardDate == null? "DD-MMM-YYYY": Convert.ToDateTime(la.ForwardDate).ToString("dd-MMM-yyyy"),
                //                      ApprovalByStaffId = la.ApprovalByStaffId,
                //                      ApprovedByStaffName = ct.FirstName,
                //                      LeaveApprovalDate = la.ApprovalDate == null? "DD-MMM-YYYY" : Convert.ToDateTime(la.ApprovalDate).ToString("dd-MMM-yyyy"),

                //                      ApprovedByStaffSnap = string.IsNullOrWhiteSpace(cct.SnapPath) ? "/Images/Avatar.png" : cct.SnapPath.ToString(),
                //                      ApprovedByDesignation = cctt.Title,
                //                      Remarks = la.Remarks,
                //                      LeaveStatusId = la.LeaveStatusId

                //                  }).ToListAsync();

                //return list;



            }
            catch (Exception ex)
            {

                throw;

            }
        }


    }
}
