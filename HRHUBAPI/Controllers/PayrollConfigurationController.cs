using HRHUBAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRHUBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PayrollConfigurationController : ControllerBase
    {
        private readonly HrhubContext _context;
        private IWebHostEnvironment _webHostEnvironment;

        public PayrollConfigurationController(HrhubContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        #region SalaryMethod

        [HttpGet("GetSalaryMethod")]
        public async Task<ActionResult<List<SalaryMethod>>> GetSalaryMethod()
        {
            return await new StaffSalary().GetSalaryMethod(_context);
        }

        #endregion

    }
}
