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
        #endregion

    }
}
