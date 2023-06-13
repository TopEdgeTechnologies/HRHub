using HRHUBAPI.Models;
using HRHUBAPI.Models.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;

namespace HRHUBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {

        private readonly HrhubContext _context;
        private readonly DbConnection _DbCom;

        public DashboardController(HrhubContext context, DbConnection DbCom)
        {
            _context = context;
            _DbCom = DbCom;

        }
       

        [HttpGet("GetDashboardData{CompanyId}/{ProcedureName}")]
        public async Task<ActionResult> GetDashboardData(int CompanyId, string ProcedureName, [FromBody] object[] parameters)
        {
            string parameterString = string.Join(",", parameters);
            DataTable dataTable = await Task.Run(() => _DbCom.ReturnDataTable(ProcedureName + " " + parameterString));
            var result = _DbCom.GetTableRows(dataTable);
            return Ok(result);

        }

    }
}
