using HRHUBAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    }
}
