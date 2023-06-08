using HRHUBAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace HRHUBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PerformanceController : ControllerBase
    {
        private readonly HrhubContext _context;

        public PerformanceController(HrhubContext context)
        {
            _context = context;

        }

        #region PerformanceAppraisal

        [HttpGet("GetPerformanceAppraisalInfos")]
        public async Task<ActionResult<List<StaffReviewFormProcessed>>> GetPerformanceAppraisalInfos()
        {

            return await new StaffReviewFormProcessed().GetPerformanceAppraisal( _context);
        }

        [HttpGet("GetPerformanceReviewSections{Id}/{staffid}")]
        public async Task<ActionResult<List<Section>>> GetPerformanceReviewSections(int Id, int staffid)
        {
            var result = await new StaffReviewFormProcessed().GetReviewSections(Id,staffid,  _context);
            if (result != null)
                return Ok(result);

            return NotFound();


        }
        [HttpPost("PostStaffAppraisal")]
        public async Task<ActionResult<Appraisal>> PostStaffAppraisal(List<Appraisal> list)
        {
            var result = await new StaffReviewFormProcessed().PostStaffAppraisal(list, _context);
            if (result != null && result.AppraisalId > 0)
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
        [HttpPost("EditStaffAppraisal")]
        public async Task<ActionResult<Appraisal>> EditStaffAppraisal(Appraisal obj)
        {
            var result = await new StaffReviewFormProcessed().EditStaffAppraisal(obj, _context);
            if (result != null && result.AppraisalId > 0)
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
        #endregion

    }
}
