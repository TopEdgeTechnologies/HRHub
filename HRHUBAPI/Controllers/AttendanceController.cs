﻿using HRHUBAPI.Models;
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

            return await new AttendanceMaster().GetAttendance(StaffId, datefrom, dateto, _context);
        }

		[HttpGet("MarkStaffAttendanceList{CompanyId}/{dateto}")]
		public async Task<ActionResult<List<AttendanceMaster>>> MarkStaffAttendanceList(int CompanyId, string dateto)
		{

			return await new AttendanceMaster().GetAttendancedatevise(CompanyId, dateto, _context);
		}
        [HttpPost("MarkStaffAttendance")]
		public async Task<ActionResult<AttendanceMaster>> MarkStaffAttendance(AttendanceMaster obj)
		{

			return await new AttendanceMaster().PostAttendence(obj, _context);
		}

		[HttpGet("CheckData")]
		public async Task<ActionResult<JsonObject>> CheckData()
		{
			if (await new AttendanceMaster().CheckStatus(_context))
			{
				return Ok(new
				{

					Success = true,					


				});
			}
			else
			{

				return Ok(new
				{

					Success = false,
					Message = "Not found"


				});
			}

		}





	}
}
