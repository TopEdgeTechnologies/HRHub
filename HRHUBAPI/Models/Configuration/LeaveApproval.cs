using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations.Schema;


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
        public string? LeaveForwardDate { get; set; }


        public async Task<List<LeaveApproval>> GetLeaveAprrovalDetail(int LeaveId, HrhubContext _context)
        {
            try
            {
               
                var list = await (from la in _context.LeaveApprovals
                                  join sa in _context.Staff on la.ApprovalByStaffId equals sa.StaffId into a
                                  from ct in a.DefaultIfEmpty()
                                  join sf in _context.Staff on la.ForwardedByStaffId equals sf.StaffId into g
                                  from cct in g.DefaultIfEmpty()
                                  join sff in _context.Designations on cct.DesignationId equals sff.DesignationId into d
                                  from cctt in d.DefaultIfEmpty()

                                  where la.LeaveId == LeaveId
                                  select new LeaveApproval()
                                  {
                                      LeaveId = la.LeaveId,
                                      ForwardedByStaffId = la.ForwardedByStaffId,
                                      ForwardByStaffName = cct.FirstName,
                                      LeaveForwardDate = la.ForwardDate == null? "DD-MMM-YYYY": Convert.ToDateTime(la.ForwardDate).ToString("dd-MMM-yyyy"),
                                      ApprovalByStaffId = la.ApprovalByStaffId,
                                      ApprovedByStaffName = ct.FirstName,
                                      LeaveApprovalDate = la.ApprovalDate == null? "DD-MMM-YYYY" : Convert.ToDateTime(la.ApprovalDate).ToString("dd-MMM-yyyy"),

                                      ApprovedByStaffSnap = string.IsNullOrWhiteSpace(cct.SnapPath.ToString()) ? "/Images/Avatar.png" : cct.SnapPath.ToString(),
                                      ApprovedByDesignation = cctt.Title,
                                      Remarks = la.Remarks,
                                      LeaveStatusId = la.LeaveStatusId

                                  }).ToListAsync();

                return list;



            }
            catch (Exception ex)
            {

                throw;

            }
        }


    }
}
