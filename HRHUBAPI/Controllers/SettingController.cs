using HRHUBAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;

namespace HRHUBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : ControllerBase
    {

        private readonly HrhubContext _context;

        public SettingController(HrhubContext context)
        {
            _context = context;

        }


        #region AttendanceSetting
        [HttpPost("PostAttendanceSetting")]
        public async Task<ActionResult<Policy>> PostAttendanceSetting(Company Obj)
        {

            var dbResult = await new Policy().PostAttendanceSetting(Obj, _context);
            if (dbResult != null)
            {
                return Ok(new
                {
                    Success = true,
                    Message = "Data Updated Successfully"
                }); ;
            }
            else
            {
                return Ok(new
                {
                    Success = true,
                    Message = "Data Inserted Successfully"
                });
            }
        }
        #endregion


        #region LeaveSetting

        [HttpGet("GetLeaveSettingByCompanyId{CompanyId}")]
        public async Task<ActionResult<LeaveApprovalSetting>> GetLeaveSettingByCompanyId(int CompanyId)
        {
            var result = await new Policy().GetLeaveSettingByCompanyId(CompanyId, _context);
            if (result != null)
                return Ok(result);

            return NotFound();


        }
        [HttpGet("DeleteLeaveType{id}")]
        public async Task<ActionResult<bool>> DeleteLeaveType(int Id, int UserId)
        {
            if (Id > 0)
            {

                var dbResult = await new LeaveType().DeleteLeaveTypeInfo(Id, _context);
                if (dbResult == true)
                {
                    return Ok(new
                    {
                        Success = true,
                        Message = "Data Deleted Successfully"
                    });
                }
            }
            return NotFound("Data Not Found!");
        }

        [HttpPost("PostLeaveSetting")]
        public async Task<ActionResult<Policy>> PostLeaveSetting(LeaveApprovalSetting Obj)
        {

            var dbResult = await new Policy().PostLeaveSetting(Obj, _context);
            if (dbResult != null)
            {
                return Ok(new
                {
                    Success = true,
                    Message = "Data Updated Successfully"
                }); ;
            }
            else
            {
                return Ok(new
                {
                    Success = true,
                    Message = "Data Inserted Successfully"
                });
            }
        }

        #endregion

    }
}
