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
    public class StaffOffBoardingController : ControllerBase
    {
        private readonly HrhubContext _context;

        public StaffOffBoardingController(HrhubContext context)
        {
            _context = context;

        }

        #region StaffOffBoardingInfo

        [HttpGet("GetOffBoardingTypeInfos")]
        public async Task<ActionResult<List<OffBoardingType>>> GetOffBoardingTypeInfos()
        {

            return await new StaffOffBoarding().GetOffBoardingType(_context);
        }

        [HttpGet("GetStaffOffBoardingInfos{CompanyId}")]
        public async Task<ActionResult<List<StaffOffBoarding>>> GetStaffOffBoardingInfos(int CompanyId)
        {
            return await new StaffOffBoarding().GetStaffOffBoarding(CompanyId, _context);
        }

        [HttpGet("GetStaffOffBoardingInfoId{id}")]
        public async Task<ActionResult<StaffOffBoarding>> GetStaffOffBoardingInfoId(int id)
        {
            var result = await new StaffOffBoarding().GetStaffOffBoardingById(id, _context);
            if (result != null)
                return Ok(result);

            return NotFound();


        }
        [HttpGet("GetStaffClearenceProcessData{id}")]
        public async Task<ActionResult<StaffOffBoarding>> GetStaffClearenceProcessData(int id)
        {
            var result = await new StaffOffBoarding().GetClearenceProcessData(id, _context);
            if (result != null)
                return Ok(result);

            return NotFound();


        }
        [HttpGet("GetClearenceDepartmentByCompanyID{CompanyId}/{id}/{LoginStaffId}")]
        public async Task<ActionResult<List<StaffOffBoarding>>> GetClearenceDepartment(int CompanyId,int id, int LoginStaffId)
        {
            return await new StaffOffBoarding().GetClearenceDepartment(CompanyId,id, LoginStaffId, _context);
        }

        [HttpPost("StaffOffBoardingAddOrCreate")]
        public async Task<ActionResult<StaffOffBoarding>> StaffOffBoardingAddOrCreate(StaffOffBoarding obj)
        {

            var result = await new StaffOffBoarding().PostStaffOffBoarding(obj, _context);
            if (result != null && result.OffBoardingId > 0)
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
        [HttpPost("SaveClearenceProcess")]
        public async Task<ActionResult<StaffOffBoarding>> SaveClearenceProcess(ClearenceProcess obj)
        {

            var result = await new StaffOffBoarding().PostClearenceProcess(obj, _context);
            if (result != null && result.OffBoardingId > 0)
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
        [HttpPost("SaveInterviewDetail")]
        public async Task<ActionResult<StaffOffBoarding>> SaveInterviewDetail(StaffOffBoarding obj)
        {

            var result = await new StaffOffBoarding().PostInterviewDetail(obj, _context);
            if (result != null && result.OffBoardingId > 0)
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

        [HttpGet("SearchAllStaffOffBoardings{CompanyId}/{DepartmentId}/{DesignationId}/{Date}")]
        public async Task<ActionResult<List<StaffOffBoarding>>> SearchAllStaffOffBoardings(int CompanyId, int DepartmentId, int DesignationId, DateTime Date)
        {
            var result = await new StaffOffBoarding().SearchStaffOffBoardings(CompanyId, DepartmentId, DesignationId, Date, _context);
            if (result != null)
                return Ok(result);

            return NotFound();


        }


        [HttpGet("GetStaffClearenceInfo{id}")]
        public async Task<ActionResult<bool>> GetStaffClearenceInfo(int id)
        {
            var result = await new StaffOffBoarding().GetClearenceInfo(id, _context);
            return result;
        }






        #endregion



    }
}
