using Microsoft.AspNetCore.Mvc;
using HRHUBAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace HRHUBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StaffsController : ControllerBase
    {
        private readonly HrhubContext _context;
        private IWebHostEnvironment _webHostEnvironment;

        public StaffsController(HrhubContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;   
        }

		#region Staff

		[HttpGet("GetStaffStatisticsByCompanyId{CompanyId}")]
		public async Task<ActionResult<Staff>> GetStaffStatisticsByCompanyId(int CompanyId)
		{
			return await new Staff().GetStaffStatisticsByCompanyId(CompanyId);
		}

        [HttpGet("GetStaffAttendanceStatistics{CompanyId}/{month}/{year}/{StaffId}")]
        public async Task<ActionResult<Staff>> GetStaffAttendanceStatistics(int CompanyId, int month, int year, int StaffId)
        {
            return await new Staff().GetStaffAttendanceStatistics(CompanyId, month, year, StaffId);
        }

        [HttpGet("GetStaffByCompanyId{CompanyId}")]
        public async Task<ActionResult<List<Staff>>> GetStaffByCompanyId(int CompanyId)
        {
			return await new Staff().GetStaffByCompanyId(CompanyId);
		}

        [HttpGet("GetStaffDocumentDetail{StaffId}")]
        public async Task<ActionResult<List<StaffAttachment>>> GetStaffDocumentDetail(int StaffId)
        {
            return await new Staff().GetStaffDocumentDetail(StaffId, _context);
        }

        [HttpGet("GetStaffLeaveAllocationsDetail{CompanyId}/{StaffId}")]
        public async Task<ActionResult<List<StaffLeaveAllocation>>> GetStaffLeaveAllocationsDetail(int CompanyId, int StaffId)
        {
            return await new Staff().GetStaffLeaveAllocationsDetail(CompanyId, StaffId, _context); 
        }

        [HttpGet("GetStaffSalaryDetail{CompanyId}/{StaffId}")]
        public async Task<ActionResult<List<StaffSalaryComponent>>> GetStaffSalaryDetail(int CompanyId, int StaffId)
        {
            return await new Staff().GetStaffSalaryDetail(CompanyId, StaffId, _context);
        }

        [HttpGet("GetStaffById{Id}")]
        public async Task<ActionResult<Staff>> GetStaffById(int Id)
        {
            var dbResult = await new Staff().GetStaffById(Id, _context);  
            if(dbResult != null)
            {
                return Ok(dbResult);
            }
            return NotFound();
        }

		[HttpPost("PostStaff")]
        public async Task<ActionResult<Staff>> PostStaff(Staff staff)
        {
            var dbResult = await new Staff().PostStaff(staff, _context);    
            if(dbResult != null && dbResult.TranFlag == 2)
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

        [HttpGet("StaffDelete{Id}")]
        public async Task<ActionResult<bool>> StaffDelete(int Id)
        {
            if (Id > 0)
            {
                var dbResult = await new Staff().StaffDelete(Id, _context);
                if (dbResult == true)
                {
                    return Ok(new
                    {
                        Success = true,
                        Message = "Record Deleted Successfully"
                    });
                }
            }
            return NotFound("Data not Found!");
        }

        [HttpGet("StaffAlreadyExists/{CompanyId}/{Id}/{nationalId}")]
        public async Task<ActionResult<bool>> StaffAlreadyExists(int CompanyId, int Id, string nationalId)
        {
            var dbResult = await new Staff().StaffAlreadyExists(CompanyId, Id, nationalId, _context);   
            if(dbResult == true)
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

		// Get single record of Staff by company ID

		[HttpGet("GetStaffCompanyVise{CompanyId}")]
		public async Task<ActionResult<Staff>> GetStaffCompanyVise(int CompanyId)
		{
			var dbResult = await new Staff().GetStaffCompanyId(CompanyId, _context);
			if (dbResult != null)
			{
				return Ok(dbResult);
			}
			return NotFound();
		}

        [HttpGet("GetStaffProfilebyId{StaffId}")]
        public async Task<ActionResult<VInfoStaff>> GetStaffProfilebyId( int StaffId)
        {
            return await new Staff().GetStaffProfilebyId( StaffId, _context);
        }

        #endregion

        #region Staff Academic

        [HttpGet("GetStaffAcademic")]
        public async Task<ActionResult<List<StaffAcademic>>> GetStaffAcademic()
        {
            return await new StaffAcademic().GetStaffAcademic(_context);
        }

        [HttpGet("GetStaffAcademicById/{Id}")]
        public async Task<ActionResult<StaffAcademic>> GetStaffAcademicById(int Id)
        {
            return await new StaffAcademic().GetStaffAcademicById(Id, _context);
        }

        [HttpGet("GetStaffAcademicByStaffId/{StaffId}")]
        public async Task<ActionResult<List<StaffAcademic>>> GetStaffAcademicByStaffId(int StaffId)
        {
            return await new StaffAcademic().GetStaffAcademicByStaffId(StaffId, _context);
        }

        [HttpPost("PostStaffAcademic")]
        public async Task<ActionResult<StaffAcademic>> PostStaffAcademic(StaffAcademic StaffAcademic)
        {
            var dbResult = await new StaffAcademic().PostStaffAcademic(StaffAcademic, _context);
            if (dbResult != null && dbResult.TranFlag == 1)
            {
                return Ok(new
                {
                    success = true,
                    Message = "Data Inserted Successfully"
                });
            }
            else
            {
                return Ok(new
                {
                    success = false,
                    Message = "Data Not Inserted"
                });
            }
        }

        [HttpGet("DeleteStaffAcademic/{Id}/{UserId}")]
        public async Task<ActionResult<bool>> DeleteStaffAcademic(int Id, int UserId)
        {
            var dbResult = await new StaffAcademic().DeleteStaffAcademic(Id, UserId, _context);
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

        [HttpGet("StaffAcademicAlreadyExists/{Id}/{StaffId}/{Title}")]
        public async Task<ActionResult<StaffAcademic>> StaffAcademicAlreadyExists(int Id, int StaffId, string Title)
        {
            var dbResult = await new StaffAcademic().AlreadyExists(Id, StaffId, Title, _context);
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

        #region Staff Dependent

        [HttpGet("GetStaffDependent")]
        public async Task<ActionResult<List<StaffDependent>>> GetStaffDependent()
        {
            return await new StaffDependent().GetStaffDependent(_context);
        }

        [HttpGet("GetStaffDependentById/{Id}")]
        public async Task<ActionResult<StaffDependent>> GetStaffDependentById(int Id)
        {
            return await new StaffDependent().GetStaffDependentById(Id, _context);
        }

        [HttpGet("GetStaffDependentByStaffId/{StaffId}")]
        public async Task<ActionResult<List<StaffDependent>>> GetStaffDependentByStaffId(int StaffId)
        {
            return await new StaffDependent().GetStaffDependentByStaffId(StaffId, _context);
        }

        [HttpPost("PostStaffDependent")]
        public async Task<ActionResult<StaffDependent>> PostStaffDependent(StaffDependent StaffDependent)
        {
            var dbResult = await new StaffDependent().PostStaffDependent(StaffDependent, _context);
            if (dbResult != null && dbResult.TranFlag == 1)
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

        [HttpGet("DeleteStaffDependent/{Id}/{UserId}")]
        public async Task<ActionResult<bool>> DeleteStaffDependent(int Id, int UserId)
        {
            var dbResult = await new StaffDependent().DeleteStaffDependent(Id, UserId, _context);
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

        [HttpGet("StaffDependentAlreadyExists/{Id}/{StaffId}/{FirstName}")]
        public async Task<ActionResult<StaffDependent>> StaffDependentAlreadyExists(int Id, int StaffId, string FirstName)
        {
            var dbResult = await new StaffDependent().AlreadyExists(Id, StaffId, FirstName, _context);
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

        #region Staff Skill

        [HttpGet("GetStaffSkill")]
        public async Task<ActionResult<List<StaffSkill>>> GetStaffSkill()
        {
            return await new StaffSkill().GetStaffSkill(_context);
        }

        [HttpGet("GetStaffSkillById/{Id}")]
        public async Task<ActionResult<StaffSkill>> GetStaffSkillById(int Id)
        {
            return await new StaffSkill().GetStaffSkillById(Id, _context);
        }

        [HttpGet("GetStaffSkillByStaffId/{StaffId}")]
        public async Task<ActionResult<StaffSkill>> GetStaffSkillByStaffId(int StaffId)
        {
            return await new StaffSkill().GetStaffSkillByStaffId(StaffId, _context);
        }

        [HttpPost("PostStaffSkill")]
        public async Task<ActionResult<StaffSkill>> PostStaffSkill(StaffSkill StaffSkill)
        {
            var dbResult = await new StaffSkill().PostStaffSkill(StaffSkill, _context);
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

        [HttpGet("DeleteStaffSkill/{Id}/{UserId}")]
        public async Task<ActionResult<bool>> DeleteStaffSkill(int Id, int UserId)
        {
            var dbResult = await new StaffSkill().DeleteStaffSkill(Id, UserId, _context);
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

        [HttpGet("StaffSkillAlreadyExists/{Id}/{StaffId}/{Title}")]
        public async Task<ActionResult<StaffSkill>> StaffSkillAlreadyExists(int Id, int StaffId, string Title)
        {
            var dbResult = await new StaffSkill().AlreadyExists(Id, StaffId, Title, _context);
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

        #region Staff Contract

        // Load all contract Staff List
        [HttpGet("ListStaffAllContract{CompanyId}")]
		public async Task<ActionResult<List<StaffContract>>> ListStaffAllContract(int CompanyId)
		{
			return await new StaffContract().GetStaffAllContract(CompanyId, _context);
		}

		// Load all contract Staff List
		[HttpGet("ListStaffExpiredContract{CompanyId}")]
		public async Task<ActionResult<List<StaffContract>>> ListStaffExpiredContract(int CompanyId)
		{
			return await new StaffContract().GetStaffExpiredContract(CompanyId, _context);
		}

		[HttpGet("GetStaffContractById{Id}")]
		public async Task<ActionResult<StaffContract>> GetStaffContractById(int Id)
		{
			var dbResult = await new StaffContract().StaffContractById(Id, _context);
			if (dbResult != null)
			{
				return Ok(dbResult);
			}
			return NotFound();
		}

		[HttpPost("PostStaffContract")]
		public async Task<ActionResult<StaffContract>> PostStaffContract(StaffContract ObjStaffContract)
		{

			var dbResult = await new StaffContract().PostStaffContract(ObjStaffContract, _context);
			if (dbResult != null && dbResult.Flag==2)
			{
				return Ok(new
				{
					Success = true,
					Message = "Data Updated Successfully"
				});
			}
			else
			{
				return Ok(new
				{
					Success = true,
					Message = "Data Inserted Successfully"
				});
			}
		}

		[HttpGet("DeleteStaffContract{Id}/{UserId}")]
		public async Task<ActionResult<bool>> DeleteStaffContract(int Id, int UserId)
		{
			if (Id > 0)
			{

				var dbResult = await new StaffContract().DeleteStaffContractInfo(Id, UserId, _context);
				if (dbResult == true)
				{
					return Ok(new
					{
						Success = true,
						Message = "Data Deleted Successfully"
					});
				}
			}
			return NotFound("Data Not Found!");
		}

		[HttpGet("StaffContractAlreadyExists{id}/{Enddate}/{StaffId}")]
		public async Task<ActionResult<bool>> StaffContractAlreadyExists(int id, DateTime Enddate, int StaffId)
		{
			var dbResult = await new StaffContract().AlreadyExists(id, Enddate, StaffId, _context);
			if (dbResult == true)
			{
				return Ok(new
				{
					Success = true,
					Message = "Contract Already Exists"
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

		//Load dropdown WeekendRule
		[HttpGet("GetEmploymentTypeInfos{CompnayId}")]
		public async Task<ActionResult<List<EmploymentType>>> GetEmploymentTypeInfos(int CompnayId)
		{

			return await new StaffContract().GetEmploymentType(CompnayId, _context);
		}

		#endregion

	}
}
