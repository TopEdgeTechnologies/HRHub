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

		//[HttpGet("GetAnyProcdureData{ProcedureName}")]
		//public async Task<ActionResult> GetAnyProcdureData( string ProcedureName, [FromBody] object[] parameters)
		//{
		//	string parameterString = string.Join(",", parameters);
		//	DataTable dataTable = await Task.Run(() => _DbCom.ReturnDataTable(ProcedureName + " " + parameterString));
		//	//var result = _DbCom.GetTableRows(dataTable);
		//   var result = _DbCom.ConvertDataTableToList<List<dynamic>>(dataTable);


		//	return Ok(result);

		//}



		//[HttpGet("GetDashboardData/{CompanyId}/{FunctionName}")]
		//public async Task<ActionResult> GetDashboardDataFunction(int CompanyId, string FunctionName, [FromBody] object[] parameters)
		//{
		//	string parameterString = string.Join(",", parameters);
		//	string query = $"SELECT * FROM {FunctionName}({parameterString})";
		//          query += " OPTION (MAXRECURSION 0) ";
		//	DataTable dataTable = await Task.Run(() => _DbCom.ReturnDataTable(query));
		//	var result = _DbCom.GetTableRows(dataTable);
		//	return Ok(result);
		//}


	}
}
