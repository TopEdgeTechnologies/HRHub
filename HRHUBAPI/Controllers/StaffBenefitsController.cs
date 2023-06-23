using HRHUBAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRHUBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffBenefitsController : ControllerBase
    {
        private readonly HrhubContext _context;

        public StaffBenefitsController(HrhubContext context)
        {
            _context = context;

        }


      

        #region Staff Benefits 

        [HttpGet("GetStaffBenefitInfos{CompanyId}")]
        public async Task<ActionResult<List<ComponentInfo>>> GetStaffBenefitInfos(int CompanyId)
        {

            return await new ComponentInfo().GetBenefitInfo(CompanyId, _context);
        }
        [HttpGet("GetComponentsInfos{CompanyId}")]
        public async Task<ActionResult<List<ComponentInfo>>> GetComponentsInfos(int CompanyId)
        {

            return await new ComponentInfo().GetComponentsInfo(CompanyId, _context);
        }
        [HttpGet("GetStaffBenefitById{Id}")]
        public async Task<ActionResult<ComponentInfo>> GetStaffBenefitById(int Id)
        {
            return await new ComponentInfo().GetBenefitInfoById(Id, _context);
        }

        [HttpPost("PostStaffBenefitInfo")]
        public async Task<ActionResult<ComponentInfo>> PostStaffBenefitInfo(ComponentInfo componentInfo)
        {
            var dbResult = await new ComponentInfo().PostBenefitInfo(componentInfo, _context);
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

        [HttpGet("DeleteStaffBenefitInfo/{Id}/{UserId}")]
        public async Task<ActionResult<bool>> DeleteStaffBenefitInfo(int Id, int UserId)
        {
            var dbResult = await new ComponentInfo().DeleteBenefitInfo(Id, UserId, _context);
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

        [HttpGet("StaffBenefitAlreadyExists/{Id}/{Title}/{CompanyId}")]
        public async Task<ActionResult<ComponentInfo>> StaffBenefitAlreadyExists(int Id, string Title, int CompanyId)
        {
            var dbResult = await new ComponentInfo().AlreadyExists(Id, Title,CompanyId, _context);
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

        [HttpGet("GetSalaryComponent/{CompanyId}/{ComponentId}")]
        public async Task<ActionResult<List<StaffSalaryComponent>>> GetSalaryComponent(int CompanyId, int ComponentId)
        {
            return await new StaffSalaryComponent().GetSalaryComponent(CompanyId, ComponentId, _context);
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

        [HttpGet("DeleteStaffSalaryComponent{Id}/{UserId}")]
        public async Task<ActionResult<StaffSalaryComponent>> DeleteStaffSalaryComponent(int Id,int UserId)
        {
            var dbResult = await new StaffSalaryComponent().DeleteStaffBenefitComponent(Id, UserId, _context);
            if (dbResult !=null)
            {
                return Ok(dbResult);
            }
            return NotFound("Data Not Found!");
        }

        [HttpGet("StaffSalaryComponentAlreadyExists/{Id}/{StaffId}/{ComponentId}")]
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



    }
}
