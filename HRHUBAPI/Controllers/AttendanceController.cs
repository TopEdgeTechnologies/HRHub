using HRHUBAPI.Models;
using HRHUBAPI.Models.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Nodes;

namespace HRHUBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AttendanceController : Controller
    {

        private readonly HrhubContext _context;

        public AttendanceController(HrhubContext context)
        {
            _context = context;

        }

        [HttpGet("GetStaffAttendanceList{StaffId}/{datefrom}/{dateto}")]
        public async Task<ActionResult<List<AttendanceMaster>>> GetStaffAttendanceList(int StaffId, string datefrom, string dateto)
        {

			List<AttendanceMaster>? Result = await new AttendanceMaster().GetAttendance(StaffId, datefrom, dateto, _context);


			return Result;

		}


		[HttpGet("StaffAttendanceOverViewList{StaffId}/{datefrom}/{dateto}")]
		public async Task<ActionResult<List<AttendanceMaster>>> StaffAttendanceOverViewList(int StaffId, string datefrom, string dateto)
		{

			//List<AttendanceMaster>? Result = await new AttendanceMaster().GetAttendanceOverViewList(StaffId, datefrom, dateto, _context);


			//return Result;
			return null;

		}




		[HttpGet("MarkStaffAttendanceList{CompanyId}/{DepartmentId}/{dateto}")]
		public async Task<ActionResult<List<AttendanceMaster>>> MarkStaffAttendanceList(int CompanyId,int DepartmentId, string dateto)
		{

			return await new AttendanceMaster().GetAttendancedatevise(CompanyId, DepartmentId, dateto, _context);
		}
        [HttpPost("MarkStaffAttendance")]
		public async Task<ActionResult<AttendanceMaster>> MarkStaffAttendance(AttendanceMaster obj)
		{

			return await new AttendanceMaster().PostAttendence(obj, _context);
		} 
		[HttpPost("StaffMarksAttendance")]
		public async Task<ActionResult<AttendanceMaster>> StaffMarksAttendance(AttendanceMaster obj)
		{

			return await new AttendanceMaster().PostMarkAttendenceInBulk(obj, _context);
		}

		[HttpGet("CheckData{StaffId}")]
		public async Task<ActionResult<AttendanceDetail>> CheckData(int StaffId)
		{

			return await new AttendanceMaster().CheckStatus(StaffId, _context);
			

		}

		[HttpGet("SetStaffAttendanceStatus{StaffId}")]
		public async Task<ActionResult<List<AttendanceMaster>>> SetStaffAttendanceStatus(int StaffId)
		{

			return await new AttendanceMaster().GetattendanceStatusStaff(StaffId);
		}



	}
}
