﻿using HRHUBAPI.Models;
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

		// filter 
		[HttpGet("StaffAttendanceOverViewList{StaffId}/{DepartmentId}/{monthId}/{yearId}")]
		public async Task<ActionResult<List<dynamic>>> StaffAttendanceOverViewList(int StaffId, int DepartmentId, int monthId, int yearId)
		{

			var Result = await new AttendanceMaster().GetAttendanceOverViewList(StaffId, DepartmentId, monthId,yearId, _context);

			return Result;

		}

        // filter
        [HttpGet("StaffAttendanceLeaveWiseList{StaffId}/{DepartmentId}/{datefrom}/{dateTo}")]
        public async Task<ActionResult<List<dynamic>>> StaffAttendanceLeaveWiseList(int StaffId, int DepartmentId, string datefrom, string dateTo)
        {

            var Result = await new AttendanceMaster().GetAttendanceLeaveWiseList(StaffId, DepartmentId, datefrom, dateTo, _context);

            return Result;

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
