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
        #region StaffLoan

        [HttpGet("GetLoanInfos{CompanyId}/{StaffId}")]
        public async Task<ActionResult<List<LoanApplication>>> GetLoanInfos(int CompanyId, int StaffId)
        {
            return await new LoanApplication().GetLoanInfo(CompanyId, StaffId, _context);
        }
        [HttpGet("GetLoanStatusInfos")]
        public async Task<ActionResult<List<LoanStatus>>> GetLoanStatusInfos()
        {
            return await new LoanApplication().GetLoanStatus( _context);
        }
        [HttpGet("GetLoanInfoId{id}")]
        public async Task<ActionResult<Leave>> GetLoanInfoId(int id)
        {
            var result = await new LoanApplication().GetLoanById(id, _context);
            if (result != null)
                return Ok(result);

            return NotFound();


        }
        [HttpPost("LoanAddOrCreate")]
        public async Task<ActionResult<LoanApplication>> LoanAddOrCreate(LoanApplication obj)
        {
            var result = await new LoanApplication().PostLoan(obj, _context);
            if (result != null && result.LoanApplicationId > 0)
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

        // search data Loan by Status , loantype and dates

        [HttpGet("SearchAllLoanData{CompanyId}/{StaffId}/{LoanTypeId}/{LoanStatusId}/{ApplicationDateFrom}/{ApplicationDateTo}")]
        public async Task<ActionResult<List<LoanApplication>>> SearchAllLoanData(int CompanyId, int StaffId, int LoanTypeId, int LoanStatusId, DateTime ApplicationDateFrom, DateTime ApplicationDateTo)
        {
            var result = await new LoanApplication().SearchLoan(CompanyId, StaffId, LoanTypeId, LoanStatusId, ApplicationDateFrom, ApplicationDateTo, _context);
            if (result != null)
                return Ok(result);

            return NotFound(); 


        }
        [HttpDelete("DeleteLoanInfo{id}/{UserId}")]
        public async Task<ActionResult<bool>> DeleteLoanInfo(int id, int UserId)
        {
            var result = await new LoanApplication().DeleteLoanInfo(id, UserId ,_context);
            if (id > 0)
                return Ok(new
                {

                    Success = true,
                    Message = "Data Delete Successfully!"


                });

            return NotFound("Data Not Found!");
        }
        [HttpGet("GetLoanApprovalByLoanId{id}")]
        public async Task<ActionResult<List<LoanApplicationProcess>>> GetLoanApprovalByLoanId(int id)
        {

            return await new LoanApplicationProcess().GetLoanAprrovalDetail(id, _context);
        
        }
        [HttpPost("SaveForwardLoanDetail")]
        public async Task<ActionResult<LoanApplication>> SaveForwardLoanDetail(LoanApplication obj)
        {


            var result = await new LoanApplicationProcess().PostLoanApprovalProcess(obj, _context);
            if (result != null && result.LoanApplicationId > 0)
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
        [HttpPost("SaveLoanApprovalDetail")]  //{id}/{leavestatusid}/{remarks}/{staffid}
        public async Task<ActionResult<JsonObject>> SaveLoanApprovalDetail(LoanApplicationProcess obj)
        {

            var dbResult = await new LoanApplicationProcess().SaveLoanApprovalDetail(obj, _context);
            if (dbResult != null)
            {
                return Ok(new
                {
                    Success = true,
                    Message = obj.LoanStatusId == 3 ? "Loan Approved Successfully" : "Loan Rejected Successfully"
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
        
        [HttpGet("GetStaffLoanRemainingAmount{StaffId}")]
        public async Task<ActionResult<List<LoanType>>> GetStaffLoanRemainingAmount(int StaffId)
        {

            return await new LoanApplication().GetStaffLoanRemainingAmountDetail(StaffId, _context);
        }
        #endregion
    }
}
