using HRHUBAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;

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
        [HttpGet("GetsPolicyById{Id}")]
        public async Task<ActionResult<Policy>> GetsPolicyById(int Id)
        {
            return await new Policy().GetPolicyById(Id, _context);
        }
        [HttpGet("GetPoliciesByCompanyId{CompanyId}/{PolicyCategoryId}")]
        public async Task<ActionResult<List<Policy>>> GetPoliciesByCompanyId(int CompanyId , int PolicyCategoryId)
        {
            return await new Policy().GetPolicyByCompanyId(CompanyId, PolicyCategoryId, _context);
        }
        [HttpGet("GetAttendancePolicyInfos")]
        public async Task<ActionResult<List<Policy>>> GetAttendancePolicyInfos()
        {

            return await new Policy().GetAttendancePolicy(_context);
        }
        [HttpGet("GetPolicybyCategoryId{PolicyCategoryId}")]
        public async Task<ActionResult<List<Policy>>> GetPolicybyCategoryId(int PolicyCategoryId)
        {

            return await new Policy().GetPolicybyCategoryId(PolicyCategoryId,_context);
        }
        [HttpGet("PostAttendancePolicyConfiguration{id}/{policyId}/{title}/{CompanyId}/{UserId}/{halfdayafterlateminutes}/{lateminutes}/{allowgraceminutes}/{graceminutes}")]
        public async Task<ActionResult<Policy>> PostAttendancePolicyConfiguration(int id, int policyId, string title, int CompanyId,int UserId,bool halfdayafterlateminutes, int lateminutes, bool allowgraceminutes, int graceminutes)
        {

            var dbResult = await new Policy().PostAttendancePolicyConfiguration( id, policyId, title, CompanyId, UserId, halfdayafterlateminutes, lateminutes, allowgraceminutes, graceminutes, _context);
            if (dbResult != null)
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
        [HttpGet("GetsAttendancePolicyConfigurationById{Id}/{CompanyId}")]
        public async Task<ActionResult<Policy>> GetsAttendancePolicyConfigurationById(int Id, int CompanyId)
        {
            return await new Policy().GetAttendancePolicyConfigurationById(Id, CompanyId, _context);
        }
        [HttpGet("PolicyConfigurationAlreadyExists{CompanyId}/{id}/{policyId}")]
        public async Task<ActionResult<bool>> PolicyConfigurationAlreadyExists(int CompanyId, int Id, int policyId)
        {
            var dbResult = await new Policy().AlreadyExist(Id, policyId, CompanyId, _context);
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
        [HttpPost("UpdateStatusByPolicyConfigurationId")]
        public async Task<ActionResult<PolicyConfiguration>> UpdateStatusByPolicyConfigurationId(PolicyConfiguration obj)
        {
            var result = await new Policy().UpdateStatusByPolicyConfigurationId(obj, _context);
            if (result != null)
                return Ok(new
                {
                    Success = true,
                    Message = "Data Update Successfully!"
                });
            else
                return NotFound();
        }
        #endregion



        #region Leave Policy

        [HttpGet("PostLeavePolicyConfiguration{id}/{policyId}/{title}/{CompanyId}/{UserId}/{halfleave}/{quarterleave}/{calenderyearapproach}/{accrualapproach}/{monthno}")]
        public async Task<ActionResult<Policy>> PostLeavePolicyConfiguration(int id, int policyId, string title, int CompanyId, int UserId, bool halfleave, bool quarterleave, bool calenderyearapproach, bool accrualapproach, int monthno)
        {

            var dbResult = await new Policy().PostLeavePolicyConfiguration(id, policyId, title, CompanyId, UserId, halfleave, quarterleave, calenderyearapproach, accrualapproach, monthno, _context);
            if (dbResult != null)
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
        [HttpGet("GetsLeavePolicyConfigurationById{Id}/{CompanyId}")]
        public async Task<ActionResult<Policy>> GetsLeavePolicyConfigurationById(int Id, int CompanyId)
        {
            return await new Policy().GetLeavePolicyConfigurationById(Id, CompanyId, _context);
        }

        #endregion


        #region Payroll Policy

        [HttpPost("PostPayrollPolicyConfiguration")]
        public async Task<ActionResult<Policy>> PostPayrollPolicyConfiguration(Policy obj)
        {

            var dbResult = await new Policy().PostPayrollPolicyConfiguration(obj, _context);
            if (dbResult != null)
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
        [HttpGet("GetsPayrollPolicyConfigurationById{Id}/{CompanyId}")]
        public async Task<ActionResult<Policy>> GetsPayrollPolicyConfigurationById(int Id, int CompanyId)
        {
            return await new Policy().GetPayrollPolicyConfigurationById(Id, CompanyId, _context);
        }

        //[HttpGet("PostPayrollPolicyConfiguration{id}/{policyId}/{title}/{CompanyId}/{UserId}/{isincometaxapplicable}/{listTaxSlab}")]
        //public async Task<ActionResult<Policy>> PostPayrollPolicyConfiguration(int id, int policyId, string title, int CompanyId, int UserId,bool isincometaxapplicable, List<TaxSlabSetting> listTaxSlab)
        //{

        //    var dbResult = await new Policy().PostPayrollPolicyConfiguration(id, policyId, title, CompanyId, UserId, isincometaxapplicable, listTaxSlab, _context);
        //    if (dbResult != null)
        //    {
        //        return Ok(new
        //        {
        //            Success = true,
        //            Message = "Data Updated Successfully"
        //        });
        //    }
        //    else
        //    {
        //        return Ok(new
        //        {
        //            Success = true,
        //            Message = "Data Inserted Successfully"
        //        });
        //    }
        //}


        #endregion
    }
}
