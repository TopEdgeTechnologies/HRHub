﻿using HRHUBAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.Design;
using System.Net.Http.Headers;

namespace HRHUBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : ControllerBase
    {

        private readonly HrhubContext _context;

        public SettingController(HrhubContext context)
        {
            _context = context;

        }


        #region AttendanceSetting
        [HttpPost("PostAttendanceSetting")]
        public async Task<ActionResult<Policy>> PostAttendanceSetting(Company Obj)
        {

            var dbResult = await new Policy().PostAttendanceSetting(Obj, _context);
            if (dbResult != null)
            {
                return Ok(new
                {
                    Success = true,
                    Message = "Data Updated Successfully"
                }); ;
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
        #endregion


        #region LeaveSetting

        [HttpGet("GetLeaveSettingByCompanyId{CompanyId}")]
        public async Task<ActionResult<LeaveApprovalSetting>> GetLeaveSettingByCompanyId(int CompanyId)
        {
            var result = await new Policy().GetLeaveSettingByCompanyId(CompanyId, _context);
            if (result != null)
                return Ok(result);

            return NotFound();


        }
        [HttpGet("DeleteLeaveType{id}")]
        public async Task<ActionResult<bool>> DeleteLeaveType(int Id, int UserId)
        {
            if (Id > 0)
            {

                var dbResult = await new LeaveType().DeleteLeaveTypeInfo(Id, _context);
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

        [HttpPost("PostLeaveSetting")]
        public async Task<ActionResult<Policy>> PostLeaveSetting(LeaveApprovalSetting Obj)
        {

            var dbResult = await new Policy().PostLeaveSetting(Obj, _context);
            if (dbResult != null)
            {
                return Ok(new
                {
                    Success = true,
                    Message = "Data Updated Successfully"
                }); ;
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

        #endregion

        #region PayrollSetting
        [HttpPost("PostComponent")]
        public async Task<ActionResult<ComponentInfo>> PostComponent(ComponentInfo Obj)
        {

            var dbResult = await new ComponentInfo().PostComponent(Obj, _context);
            if (dbResult != null)
            {
                return Ok(new
                {
                    Success = true,
                    Message = "Data Updated Successfully"
                }); ;
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
        [HttpGet("GetComponentsById{id}")]
        public async Task<ActionResult<ComponentInfo>> GetComponentsById(int id)
        {
            var result = await new ComponentInfo().GetBenefitInfoById(id, _context);
            if (result != null)
                return Ok(result);

            return NotFound();

        }

        [HttpGet("DeleteTaxSlab{id}/{UserId}")]
        public async Task<ActionResult<bool>> DeleteTaxSlab(int Id, int UserId)
        {
            if (Id > 0)
            {

                var dbResult = await new Policy().DeleteTaxSlab(Id, UserId, _context);
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

        [HttpGet("GetTaxSlabSettingByCompanyId{CompanyId}")]
        public async Task<ActionResult<List<TaxSlabSetting>>> GetTaxSlabSettingByCompanyId(int CompanyId)
        {
            return await new Policy().GetTaxSlabSettingByCompanyId(CompanyId, _context);
        }

        [HttpGet("GetSalarySettingByCompanyId{CompanyId}")]
        public async Task<ActionResult<StaffSalarySetting>> GetSalarySettingByCompanyId(int CompanyId)
        {
            var result = await new Policy().GetSalarySettingByCompanyId(CompanyId, _context);
            if (result != null)
                return Ok(result);

            return NotFound();


        }
        [HttpPost("PostPayrollSetting")]
        public async Task<ActionResult<Policy>> PostPayrollSetting(StaffSalarySetting Obj)
        {

            var dbResult = await new Policy().PostPayrollSetting(Obj, _context);
            if (dbResult != null)
            {
                return Ok(new
                {
                    Success = true,
                    Message = "Data Updated Successfully"
                }); ;
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
        #endregion




        #region NotificationSetting



        [HttpGet("GetCandidateEmailNotificationList{CompanyId}")]
        public async Task<ActionResult<List<CandidateEmailNotificationSetting>>> GetCandidateEmailNotificationList(int CompanyId)
        {
            var result = await new EmailNotificationSetting().GetCandidateEmailNotification(CompanyId, _context);
            if (result != null)
                return Ok(result);

            return NotFound();


        }









        [HttpGet("GetEmailNotificationSettingById{CompanyId}")]
        public async Task<ActionResult<EmailNotificationSetting>> GetEmailNotificationSettingById(int CompanyId)
        {
            var dbResult = await new EmailNotificationSetting().GetEmailNotificationSettingById(CompanyId, _context);
            if (dbResult != null)
            {
                return Ok(dbResult);
            }
            return NotFound();
        }







        [HttpPost("PostEmailNotificationSetting")]
        public async Task<ActionResult<EmailNotificationSetting>> PostEmailNotificationSetting(EmailNotificationSetting Obj)
        {

            var dbResult = await new EmailNotificationSetting().PostEmailNotificationSetting(Obj, _context);
            if (dbResult != null && dbResult.Flag == 2)
            {
                return Ok(new
                {
                    Success = true,
                    Message = "Data Updated Successfully"
                }); ;
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



        // Post Email Template data here 





        [HttpGet("GetEmailTemplateByCompanyId{CompanyId}")]
        public async Task<ActionResult<List<EmailTemplate>>> GetEmailTemplateByCompanyId(int CompanyId)
        {
            var result = await new EmailNotificationSetting().GetEmailTemplate(CompanyId, _context);
            if (result != null)
                return Ok(result);

            return NotFound();


        }

        [HttpGet("EmailTemplateById{Id}")]
        public async Task<ActionResult<EmailTemplate>> EmailTemplateById(int Id)
        {
            var dbResult = await new EmailNotificationSetting().GetEmailTemplateById(Id, _context);
            if (dbResult != null)
            {
                return Ok(dbResult);
            }
            return NotFound();
        }



        [HttpPost("PostEmailTemplateData")]
        public async Task<ActionResult<EmailTemplate>> PostEmailTemplateData(EmailTemplate Obj)
        {

            var dbResult = await new EmailNotificationSetting().PostEmailTemplate(Obj, _context);
            if (dbResult != null)
            {
                return Ok(new
                {
                    Success = true,
                    Message = "Data Updated Successfully"
                }); ;
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




        [HttpGet("EmailTemplateDelete{id}/{UserId}")]
        public async Task<ActionResult<bool>> EmailTemplateDelete(int id, int UserId)
        {
            if (id > 0)
            {

                var dbResult = await new EmailNotificationSetting().DeleteEmailTemplate(id, UserId, _context);
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



        [HttpGet("EmailTemAlreadyExists{CompanyId}/{id}/{title}")]
        public async Task<ActionResult<bool>> EmailTemAlreadyExists(int CompanyId, int Id, string title)
        {
            var dbResult = await new EmailNotificationSetting().AlreadyExists(CompanyId, Id, title, _context);
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

        [HttpPost("UpdateStatusByEmailTemplateById")]
        public async Task<ActionResult<EmailTemplate>> UpdateStatusByEmailTemplateById(EmailTemplate obj)
        {
            var result = await new EmailNotificationSetting().UpdateStatusByEmailTempId(obj, _context);
            if (result != null)
                return Ok(new
                {
                    Success = true,
                    Message = "Data Update Successfully!"
                });
            else
                return NotFound();
        }






        // Update Candidate Notification Status

        [HttpPost("UpdateCandidateEmailNotification")]
        public async Task<ActionResult<CandidateEmailNotificationSetting>> UpdateCandidateEmailNotification(CandidateEmailNotificationSetting obj)
        {
            var result = await new EmailNotificationSetting().UpdateStatusEmailTemplate(obj, _context);
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

    }
}
