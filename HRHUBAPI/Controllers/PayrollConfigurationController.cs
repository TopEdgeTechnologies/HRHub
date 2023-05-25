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

        #region Salary Component

            [HttpGet("GetSalaryComponentByCompanyId")]
            public async Task<ActionResult<List<SalaryComponent>>> GetSalaryComponentByCompanyId(int CompanyId)
            {
                return await new SalaryComponent().GetSalaryComponentByCompanyId(CompanyId, _context);
            }

            [HttpGet("GetSalaryComponentById/{Id}")]
            public async Task<ActionResult<SalaryComponent>> GetSalaryComponentById(int Id)
            {
                return await new SalaryComponent().GetSalaryComponentById(Id, _context);
            }

            [HttpPost("PostSalaryComponent")]
            public async Task<ActionResult<SalaryComponent>> PostSalaryComponent(SalaryComponent SalaryComponent)
            {
                var dbResult = await new SalaryComponent().PostSalaryComponent(SalaryComponent, _context);
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

            [HttpGet("DeleteSalaryComponent/{Id}/{UserId}")]
            public async Task<ActionResult<bool>> DeleteSalaryComponent(int Id, int UserId)
            {
                var dbResult = await new SalaryComponent().DeleteSalaryComponent(Id, UserId, _context);
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

            [HttpGet("SalaryComponentAlreadyExists/{Id}/{Title}")]
            public async Task<ActionResult<SalaryComponent>> SalaryComponentAlreadyExists(int Id, string Title)
            {
                var dbResult = await new SalaryComponent().AlreadyExists(Id, Title, _context);
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

        #region Fixed Component

            [HttpGet("GetFixedComponent")]
            public async Task<ActionResult<List<FixedComponent>>> GetFixedComponent()
            {
                return await new FixedComponent().GetFixedComponent(_context);
            }

            [HttpGet("GetFixedComponentById/{Id}")]
            public async Task<ActionResult<FixedComponent>> GetFixedComponentById(int Id)
            {
                return await new FixedComponent().GetFixedComponentById(Id, _context);
            }

            [HttpPost("PostFixedComponent")]
            public async Task<ActionResult<FixedComponent>> PostFixedComponent(FixedComponent FixedComponent)
            {
                var dbResult = await new FixedComponent().PostFixedComponent(FixedComponent, _context);
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

            [HttpGet("DeleteFixedComponent/{Id}/{UserId}")]
            public async Task<ActionResult<bool>> DeleteFixedComponent(int Id, int UserId)
            {
                var dbResult = await new FixedComponent().DeleteFixedComponent(Id, UserId, _context);
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

            [HttpGet("FixedComponentAlreadyExists/{Id}/{Title}")]
            public async Task<ActionResult<FixedComponent>> FixedComponentAlreadyExists(int Id, string Title)
            {
                var dbResult = await new FixedComponent().AlreadyExists(Id, Title, _context);
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

        #region Staff Salary

        [HttpGet("GetStaffSalary")]
            public async Task<ActionResult<List<StaffSalary>>> GetStaffSalary()
            {
                return await new StaffSalary().GetStaffSalary(_context);
            }

            [HttpGet("GetStaffSalaryById/{Id}")]
            public async Task<ActionResult<StaffSalary>> GetStaffSalaryById(int Id)
            {
                return await new StaffSalary().GetStaffSalaryById(Id, _context);
            }

            [HttpPost("PostStaffSalary")]
            public async Task<ActionResult<StaffSalary>> PostStaffSalary(StaffSalary StaffSalary)
            {
                var dbResult = await new StaffSalary().PostStaffSalary(StaffSalary, _context);
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

            [HttpGet("DeleteStaffSalary/{Id}/{UserId}")]
            public async Task<ActionResult<bool>> DeleteStaffSalary(int Id, int UserId)
            {
                var dbResult = await new StaffSalary().DeleteStaffSalary(Id, UserId, _context);
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

            [HttpGet("StaffSalaryAlreadyExists/{Id}/{Title}")]
            public async Task<ActionResult<StaffSalary>> StaffSalaryAlreadyExists(int Id, int StaffId)
            {
                var dbResult = await new StaffSalary().AlreadyExists(Id, StaffId, _context);
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
    }
}
