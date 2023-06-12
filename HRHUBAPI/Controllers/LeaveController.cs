using HRHUBAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using System.Text.Json.Nodes;

namespace HRHUBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LeaveController : ControllerBase
    {
        private readonly HrhubContext _context;

        public LeaveController(HrhubContext context)
        {
            _context = context;

        }

        #region LeaveInfo

        [HttpGet("GetLeaveInfos{CompanyId}/{StaffId}")]
        public async Task<ActionResult<List<Leave>>> GetLeaveInfos(int CompanyId,int StaffId)
        {

            return await new Leave().GetLeave(CompanyId,StaffId, _context);
        }

        [HttpGet("GetHRLeaveInfos{CompanyId}/{StaffId}")]
        public async Task<ActionResult<List<Leave>>> GetHRLeaveInfos(int CompanyId, int StaffId)
        {

            return await new Leave().GetHRLeave(CompanyId, StaffId, _context);
        }

        //[HttpGet("GetNewOrPendingLeaveInfos{CompanyId}")]
        //public async Task<ActionResult<List<Leave>>> GetNewOrPendingLeaveInfos(int CompanyId)
        //{

        //    return await new Leave().GetNewOrPendingLeave(CompanyId, _context);
        //}


        [HttpGet("GetLeaveDetailInfoId{id}")]
        public async Task<ActionResult<Leave>> GetLeaveDetailInfoId(int id)
        {
            var result = await new Leave().GetLeaveDetailById(id, _context);
            if (result != null)
                return Ok(result);

            return NotFound();


        }

        [HttpGet("GetLeaveInfoId{id}")]
        public async Task<ActionResult<Leave>> GetLeaveInfoId(int id)
        {
            var result = await new Leave().GetLeaveById(id, _context);
            if (result != null)
                return Ok(result);

            return NotFound();


        }

        [HttpPost("LeaveAddOrCreate")]
        public async Task<ActionResult<Leave>> LeaveAddOrCreate(Leave obj)
        {


            var result = await new Leave().PostLeave(obj, _context);
            if (result != null && result.LeaveId > 0)
                return Ok(new
                {
                    Success = true,
                    Message = "Data Update Successfully!"
                });
            else
                return Ok(new
                {
                    Success = true,
                    Message = "Data Insert Successfully!"
                });

        }

        [HttpGet("GetStaffLeaveApprovalComments{id}/{approvalstatusid}/{loginstaffid}")]
        public async Task<ActionResult<List<LeaveApproval>>> GetStaffLeaveApprovalComments(int Id, int approvalstatusid, int loginstaffid)
        {
            var result = await new Leave().GetStaffComments(Id, approvalstatusid, loginstaffid, _context);
            if (result != null)
                return Ok(result);

            return NotFound();

        }

        [HttpDelete("DeleteLeaveInfo{id}")]
        public async Task<ActionResult<bool>> DeleteLeaveInfo(int id)
        {
            var result = await new Leave().DeleteLeaveInfo(id, _context);
            if (id > 0)
                return Ok(new
                {

                    Success = true,
                    Message = "Data Delete Successfully!"


                });

            return NotFound("Data Not Found!");
        }


        [HttpGet("LeaveCheckData{staffid}/{leavetypeid}")]
        public async Task<ActionResult<JsonObject>> LeaveCheckData(int staffid,int leavetypeid)
        {
            var result = await new Leave().CheckLeaveCount(staffid, leavetypeid, _context);
            if (result !=null)
                return Ok(result);

            return NotFound("Data Not Found!");


        }


        //[HttpGet("LeaveCheckData{id}/{cnic}")]
        //public async Task<ActionResult<JsonObject>> LeaveCheckData(int id, string email)
        //{
        //    if (await new Leave().AlreadyExist(id, email, _context))
        //    {
        //        return Ok(new
        //        {

        //            Success = true,
        //            Message = "Leave Already Exist!"


        //        });
        //    }
        //    else
        //    {

        //        return Ok(new
        //        {

        //            Success = false,
        //            Message = "Not found"


        //        });
        //    }

        //}

        [HttpPost("SaveLeaveApprovalDetail")]  //{id}/{leavestatusid}/{remarks}/{staffid}
        public async Task<ActionResult<JsonObject>> SaveLeaveApprovalDetail(LeaveApproval obj)
        {

            var dbResult = await new Leave().SaveLeaveApprovalDetail(obj, _context);
            if (dbResult != null)
            {
                return Ok(new
                {
                    Success = true,
                    Message = obj.LeaveStatusId == 3 ? "Leave Approved Successfully" : "Leave Rejectd Successfully"
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


        [HttpPost("SaveForwardLeaveDetail")]
        public async Task<ActionResult<Leave>> SaveForwardLeaveDetail(Leave obj)
        {


            var result = await new Leave().PostLeaveApproval(obj, _context);
            if (result != null && result.LeaveId > 0)
                return Ok(new
                {
                    Success = true,
                    Message = "Data Update Successfully!"
                });
            else
                return Ok(new
                {
                    Success = true,
                    Message = "Data Insert Successfully!"
                });

        }

        [HttpGet("GetLeaveApprovalByLeaveId{id}")]
        public async Task<ActionResult<List<LeaveApproval>>> GetLeaveApprovalByLeaveId(int id)
        {

            return await new LeaveApproval().GetLeaveAprrovalDetail(id, _context);
        }


		//Load dropdown LeaveStatus
		[HttpGet("GetLeaveStatusInfos")]
		public async Task<ActionResult<List<LeaveStatus>>> GetLeaveStatusInfos()
		{

			return await new Leave().GetLeaveStatus(_context);
		}


        [HttpGet("GetLeaveApprovalSettingInfos{CompanyId}")]
        public async Task<ActionResult<LeaveApprovalSetting>> GetLeaveApprovalSettingInfos(int CompanyId)
        {

            return await new Leave().GetLeaveApprovalSetting(CompanyId, _context);
        }

        // search leave by LeaveType , LeaveStatus and Date

        [HttpGet("SearchAllLeaves{CompanyId}/{StaffId}/{LeaveTypeId}/{LeaveStatusId}/{Month}/{DateFilter}")]
        public async Task<ActionResult<List<Leave>>> SearchAllLeaves(int CompanyId, int StaffId,int LeaveTypeId, int LeaveStatusId, DateTime Month, bool DateFilter)
        {
            var result = await new Leave().SearchLeaves(CompanyId, StaffId, LeaveTypeId, LeaveStatusId, Month, DateFilter, _context);
            if (result != null)
                return Ok(result);

            return NotFound(); 


        }

        #endregion

    }
}
