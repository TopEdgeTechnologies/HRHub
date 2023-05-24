using System.ComponentModel.DataAnnotations.Schema;

namespace HRHUBAPI.Models
{
    public partial class StaffLeaveAllocation
    {
        [NotMapped]
        public string? LeaveTypeTitle { get; set; }
        
        [NotMapped]
        public int? NoOfLeavesLimit { get; set; }
    }
}
