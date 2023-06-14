using HRHUBAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRHUBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolicyController : ControllerBase
    {

        private readonly HrhubContext _context;

        public PolicyController(HrhubContext context)
        {
            _context = context;

        }


        #region Policy
        [HttpGet("GetPolicyInfos")]
        public async Task<ActionResult<List<Policy>>> GetPolicyInfos()
        {

            return await new Policy().GetPolicy(_context);
        }
        [HttpGet("GetsPolicyById/{Id}")]
        public async Task<ActionResult<Policy>> GetsPolicyById(int Id)
        {
            return await new Policy().GetPolicyById(Id, _context);
        }
        #endregion
    }
}
