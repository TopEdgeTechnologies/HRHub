using HRHUBAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using System.ComponentModel;

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
            return await new SalaryMethod().GetSalaryMethod(_context);
        }

        [HttpGet("GetSalaryMethodById/{Id}")]
        public async Task<ActionResult<SalaryMethod>> GetSalaryMethodById(int Id)
        {
            return await new SalaryMethod().GetSalaryMethodById(Id, _context);
        }

        [HttpPost("PostSalaryMethod")]
        public async Task<ActionResult<SalaryMethod>> PostSalaryMethod(SalaryMethod salaryMethod)
        {
            var dbResult = await new SalaryMethod().PostSalaryMethod(salaryMethod, _context);
            if (dbResult != null && dbResult.TranFlag == 2)
            {
                return Ok(new
                {
                    success = true,
                    Message = "Data Updated Successfully"
                });
            }
            else
            {
                return Ok(new
                {
                    success = true,
                    Message = "Data Inserted Successfully"
                });
            }
        }

        [HttpGet("DeleteSalaryMethod/{Id}/{UserId}")]
        public async Task<ActionResult<bool>> DeleteSalaryMethod(int Id, int UserId)
        {
            var dbResult = await new SalaryMethod().DeleteSalaryMethod(Id, UserId, _context);
            if (dbResult == true)
            {
                return Ok(new
                {
                    success = true,
                    Message = "Data Deleted Successfully"
                });
            }
            return NotFound("Data Not Found!");
        }

        [HttpGet("SalaryMethodAlreadyExists/{Id}/{Title}")]
        public async Task<ActionResult<SalaryMethod>> SalaryMethodAlreadyExists(int Id, string Title)
        {
            var dbResult = await new SalaryMethod().AlreadyExists(Id, Title, _context);
            if (dbResult == true)
            {
                return Ok(new
                {
                    Success = true,
                    Message = "Record Already Exists"
                });
            }
            else
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Data Not Found!"
                });
            }
        }

        #endregion

        #region Salary Status

        [HttpGet("GetSalaryStatus")]
        public async Task<ActionResult<List<SalaryStatus>>> GetSalaryStatus()
        {
            return await new SalaryStatus().GetSalaryStatus(_context);
        }

        [HttpGet("GetSalaryStatusById/{Id}")]
        public async Task<ActionResult<SalaryStatus>> GetSalaryStatusById(int Id)
        {
            return await new SalaryStatus().GetSalaryStatusById(Id, _context);
        }

        [HttpPost("PostSalaryStatus")]
        public async Task<ActionResult<SalaryStatus>> PostSalaryStatus(SalaryStatus SalaryStatus)
        {
            var dbResult = await new SalaryStatus().PostSalaryStatus(SalaryStatus, _context);
            if (dbResult != null && dbResult.TranFlag == 2)
            {
                return Ok(new
                {
                    success = true,
                    Message = "Data Updated Successfully"
                });
            }
            else
            {
                return Ok(new
                {
                    success = true,
                    Message = "Data Inserted Successfully"
                });
            }
        }

        [HttpGet("DeleteSalaryStatus/{Id}/{UserId}")]
        public async Task<ActionResult<bool>> DeleteSalaryStatus(int Id, int UserId)
        {
            var dbResult = await new SalaryStatus().DeleteSalaryStatus(Id, UserId, _context);
            if (dbResult == true)
            {
                return Ok(new
                {
                    success = true,
                    Message = "Data Deleted Successfully"
                });
            }
            return NotFound("Data Not Found!");
        }

        [HttpGet("SalaryStatusAlreadyExists/{Id}/{Title}")]
        public async Task<ActionResult<SalaryStatus>> SalaryStatusAlreadyExists(int Id, string Title)
        {
            var dbResult = await new SalaryStatus().AlreadyExists(Id, Title, _context);
            if (dbResult == true)
            {
                return Ok(new
                {
                    Success = true,
                    Message = "Record Already Exists"
                });
            }
            else
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Data Not Found!"
                });
            }
        }

        #endregion

        #region Salary Status Process

        [HttpGet("GetSalaryStatusProcess")]
        public async Task<ActionResult<List<SalaryStatusProcess>>> GetSalaryStatusProcess()
        {
            return await new SalaryStatusProcess().GetSalaryStatusProcess(_context);
        }

        [HttpGet("GetSalaryStatusProcessById/{Id}")]
        public async Task<ActionResult<SalaryStatusProcess>> GetSalaryStatusProcessById(int Id)
        {
            return await new SalaryStatusProcess().GetSalaryStatusProcessById(Id, _context);
        }

        [HttpPost("PostSalaryStatusProcess")]
        public async Task<ActionResult<SalaryStatusProcess>> PostSalaryStatusProcess(SalaryStatusProcess SalaryStatusProcess)
        {
            var dbResult = await new SalaryStatusProcess().PostSalaryStatusProcess(SalaryStatusProcess, _context);
            if (dbResult != null && dbResult.TranFlag == 2)
            {
                return Ok(new
                {
                    success = true,
                    Message = "Data Updated Successfully"
                });
            }
            else
            {
                return Ok(new
                {
                    success = true,
                    Message = "Data Inserted Successfully"
                });
            }
        }

        [HttpGet("DeleteSalaryStatusProcess/{Id}/{UserId}")]
        public async Task<ActionResult<bool>> DeleteSalaryStatusProcess(int Id, int UserId)
        {
            var dbResult = await new SalaryStatusProcess().DeleteSalaryStatusProcess(Id, UserId, _context);
            if (dbResult == true)
            {
                return Ok(new
                {
                    success = true,
                    Message = "Data Deleted Successfully"
                });
            }
            return NotFound("Data Not Found!");
        }

        [HttpGet("SalaryStatusProcessAlreadyExists/{Id}/{Title}")]
        public async Task<ActionResult<SalaryStatusProcess>> SalaryStatusProcessAlreadyExists(int Id, DateTime ProcessDate)
        {
            var dbResult = await new SalaryStatusProcess().AlreadyExists(Id, ProcessDate, _context);
            if (dbResult == true)
            {
                return Ok(new
                {
                    Success = true,
                    Message = "Record Already Exists"
                });
            }
            else
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Data Not Found!"
                });
            }
        }

        #endregion

        #region Component Group

        [HttpGet("GetComponentGroup")]
        public async Task<ActionResult<List<ComponentGroup>>> GetComponentGroup()
        {
            return await new ComponentGroup().GetComponentGroup(_context);
        }

        [HttpGet("GetComponentGroupById/{Id}")]
        public async Task<ActionResult<ComponentGroup>> GetComponentGroupById(int Id)
        {
            return await new ComponentGroup().GetComponentGroupById(Id, _context);
        }

        [HttpPost("PostComponentGroup")]
        public async Task<ActionResult<ComponentGroup>> PostComponentGroup(ComponentGroup ComponentGroup)
        {
            var dbResult = await new ComponentGroup().PostComponentGroup(ComponentGroup, _context);
            if (dbResult != null && dbResult.TranFlag == 2)
            {
                return Ok(new
                {
                    success = true,
                    Message = "Data Updated Successfully"
                });
            }
            else
            {
                return Ok(new
                {
                    success = true,
                    Message = "Data Inserted Successfully"
                });
            }
        }

        [HttpGet("DeleteComponentGroup/{Id}/{UserId}")]
        public async Task<ActionResult<bool>> DeleteComponentGroup(int Id, int UserId)
        {
            var dbResult = await new ComponentGroup().DeleteComponentGroup(Id, UserId, _context);
            if (dbResult == true)
            {
                return Ok(new
                {
                    success = true,
                    Message = "Data Deleted Successfully"
                });
            }
            return NotFound("Data Not Found!");
        }

        [HttpGet("ComponentGroupAlreadyExists/{Id}/{Title}")]
        public async Task<ActionResult<ComponentGroup>> ComponentGroupAlreadyExists(int Id, string Title)
        {
            var dbResult = await new ComponentGroup().AlreadyExists(Id, Title, _context);
            if (dbResult == true)
            {
                return Ok(new
                {
                    Success = true,
                    Message = "Record Already Exists"
                });
            }
            else
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Data Not Found!"
                });
            }
        }

        #endregion

        #region Component Info

        [HttpGet("GetComponentInfo")]
        public async Task<ActionResult<List<ComponentInfo>>> GetComponentInfo()
        {
            return await new ComponentInfo().GetComponentInfo(_context);
        }

        [HttpGet("GetComponentInfoById/{Id}")]
        public async Task<ActionResult<ComponentInfo>> GetComponentInfoById(int Id)
        {
            return await new ComponentInfo().GetComponentInfoById(Id, _context);
        }

        [HttpPost("PostComponentInfo")]
        public async Task<ActionResult<ComponentInfo>> PostComponentInfo(ComponentInfo componentInfo)
        {
            var dbResult = await new ComponentInfo().PostComponentInfo(componentInfo, _context);
            if (dbResult != null && dbResult.TranFlag == 2)
            {
                return Ok(new
                {
                    success = true,
                    Message = "Data Updated Successfully"
                });
            }
            else
            {
                return Ok(new
                {
                    success = true,
                    Message = "Data Inserted Successfully"
                });
            }
        }

        [HttpGet("DeleteComponentInfo/{Id}/{UserId}")]
        public async Task<ActionResult<bool>> DeleteComponentInfo(int Id, int UserId)
        {
            var dbResult = await new ComponentInfo().DeleteComponentInfo(Id, UserId, _context);
            if (dbResult == true)
            {
                return Ok(new
                {
                    success = true,
                    Message = "Data Deleted Successfully"
                });
            }
            return NotFound("Data Not Found!");
        }

        [HttpGet("ComponentInfoAlreadyExists/{Id}/{Title}")]
        public async Task<ActionResult<ComponentInfo>> ComponentInfoAlreadyExists(int Id, string Title)
        {
            var dbResult = await new ComponentInfo().AlreadyExists(Id, Title, _context);
            if (dbResult == true)
            {
                return Ok(new
                {
                    Success = true,
                    Message = "Record Already Exists"
                });
            }
            else
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Data Not Found!"
                });
            }
        }

        #endregion

        #region Staff Salary Component

        [HttpGet("GetSalaryComponent")]
        public async Task<ActionResult<List<StaffSalaryComponent>>> GetSalaryComponent(int CompanyId)
        {
            return await new StaffSalaryComponent().GetSalaryComponent(CompanyId, _context);
        }

        [HttpGet("GetStaffSalaryComponentById/{Id}")]
        public async Task<ActionResult<StaffSalaryComponent>> GetStaffSalaryComponentById(int Id)
        {
            return await new StaffSalaryComponent().GetStaffSalaryComponentById(Id, _context);
        }

        [HttpPost("PostStaffSalaryComponent")]
        public async Task<ActionResult<StaffSalaryComponent>> PostStaffSalaryComponent(StaffSalaryComponent staffSalaryComponent)
        {
            var dbResult = await new StaffSalaryComponent().PostStaffSalaryComponent(staffSalaryComponent, _context);
            if (dbResult != null && dbResult.TranFlag == 2)
            {
                return Ok(new
                {
                    success = true,
                    Message = "Data Updated Successfully"
                });
            }
            else
            {
                return Ok(new
                {
                    success = true,
                    Message = "Data Inserted Successfully"
                });
            }
        }

        [HttpGet("DeleteStaffSalaryComponent/{Id}/{UserId}")]
        public async Task<ActionResult<bool>> DeleteStaffSalaryComponent(int Id, int UserId)
        {
            var dbResult = await new StaffSalaryComponent().DeleteStaffSalaryComponent(Id, UserId, _context);
            if (dbResult == true)
            {
                return Ok(new
                {
                    success = true,
                    Message = "Data Deleted Successfully"
                });
            }
            return NotFound("Data Not Found!");
        }

        [HttpGet("StaffSalaryComponentAlreadyExists/{Id}/{Title}")]
        public async Task<ActionResult<StaffSalaryComponent>> StaffSalaryComponentAlreadyExists(int Id, int StaffId, int ComponentId)
        {
            var dbResult = await new StaffSalaryComponent().AlreadyExists(Id, StaffId, ComponentId, _context);
            if (dbResult == true)
            {
                return Ok(new
                {
                    Success = true,
                    Message = "Record Already Exists"
                });
            }
            else
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Data Not Found!"
                });
            }
        }

        #endregion
       

        [HttpGet("GetLoanInfos{CompanyId}/{StaffId}")]
        public async Task<ActionResult<List<LoanApplication>>> GetLoanInfos(int CompanyId, int StaffId)
        {
            return await new LoanApplication().GetLoanInfo(CompanyId, StaffId, _context);
        }
        [HttpGet("GetLoanStatusInfos")]
        public async Task<ActionResult<List<LoanStatus>>> GetLoanStatusInfos()
        {
            return await new LoanApplication().GetLoanStatus(_context);
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
            var result = await new LoanApplication().DeleteLoanInfo(id, UserId, _context);
            if (id > 0)
                return Ok(new
                {
                    success = true,
                    Message = "Data Deleted Successfully"
                });
            return NotFound("Data Not Found!");
        }


    }
}
