using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HRHUBAPI.Models
{
    public partial class AttendanceMaster
    {

        [NotMapped]
        public string? Attendancestatus { get; set; }

		[NotMapped]
		public string? CssClass { get; set; }

		[NotMapped]
		public string? AttendanceFormateDate { get; set; }


		[NotMapped]
		public string? AttendanceFormateDateIn { get; set; }
		[NotMapped]
		public string? AttendanceFormateDateOut { get; set; }

		[NotMapped]
        public IEnumerable<AttendanceMaster> listAttendance { get; set; }

        public Task<List<AttendanceMaster>> GetAttendance(int staffid,  string datefrom, string dateTo,  HrhubContext _context)
        {
            try
            {
				
				DateTime Newdatefrom = DateTime.ParseExact(datefrom, "dd-MM-yyyy", CultureInfo.InvariantCulture);
				DateTime Newdateto = DateTime.ParseExact(dateTo, "dd-MM-yyyy", CultureInfo.InvariantCulture);






				var query = from at in _context.AttendanceMasters
                            join s in _context.AttendanceStatuses on at.AttendanceStatusId equals s.AttendanceStatusId
                            where at.StaffId == staffid && at.IsDeleted == false && at.AttendanceDate >= Newdatefrom && at.AttendanceDate <= Newdateto
							select new AttendanceMaster
                            {
                                AttendanceId = at.AttendanceId,
                                Attendancestatus = s.Title,
                                AttendanceDate = at.AttendanceDate,
                                FirstPunchIn = at.FirstPunchIn,
                                LastPunchOut = at.LastPunchOut,
                                TotalWorkingMinutes = at.TotalWorkingMinutes,
                                TotalDefinedMinutes = at.TotalDefinedMinutes,
                                AttendanceStatusId = at.AttendanceStatusId,
                                LateMinutes = at.LateMinutes,
                                CreatedBy = at.CreatedBy,
                                LeaveTypeId = at.LeaveTypeId,
                                MarkAsHalfLeave = at.MarkAsHalfLeave,
                                NonPaidAttendance = at.NonPaidAttendance,
                                OverTimeMinutes = at.OverTimeMinutes,
                                Remarks = at.Remarks,
                                StaffId = at.StaffId,
                                UpdatedBy = at.UpdatedBy,
                                CreatedOn = at.CreatedOn,
                                GraceLateMinutes = at.GraceLateMinutes,
                                UpdatedOn = at.UpdatedOn,
                                MarkedAsHalfDay = at.MarkedAsHalfDay,
                                MarkAsShortLeave = at.MarkAsShortLeave,
                                MarkedAsShortDay = at.MarkedAsShortDay,
                                IsDeleted = at.IsDeleted,
                                AttendanceFormateDate = at.AttendanceDate.Value.Date.ToString("dd-MMM-yyyy"),
								CssClass = s.CssClass

								// AttendanceFormateDateIn = at.FirstPunchIn.Value.ToString("h:mm tt"),
								// AttendanceFormateDateOut = at.LastPunchOut.Value.ToString("h:mm tt")



							};

               return Task.FromResult(query != null ? query.OrderByDescending(x => x.AttendanceDate).ToList() : new List<AttendanceMaster>());




            }
            catch (Exception ex)
            {

                throw;

            }
        }


    }
}
