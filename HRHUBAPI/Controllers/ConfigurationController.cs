﻿using HRHUBAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using System.Text.Json.Nodes;
using Newtonsoft.Json;


namespace HRHUBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConfigurationController : ControllerBase
    {
        private readonly HrhubContext _context;
        private IWebHostEnvironment _webHostEnvironment;

        public ConfigurationController(HrhubContext context, IWebHostEnvironment WebHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = WebHostEnvironment;

        }

        #region DesignationInfo

        [HttpGet("GetDesignationInfos{CompanyId}")]
        public async Task<ActionResult<List<Designation>>> GetDesignationInfos(int CompanyId)
        {

            return await new Designation().GetDesignation(CompanyId, _context);
        }


        [HttpGet("GetDesignationInfoId{id}")]
        public async Task<ActionResult<Designation>> GetDesignationInfoId(int id)
        {
            var result = await new Designation().GetDesignationById(id, _context);
            if (result != null)
                return Ok(result);

            return NotFound();


        }
        [HttpPost("UpdateStatusByDesignationId")]
        public async Task<ActionResult<Designation>> UpdateStatusByDesignationId(Designation obj)
        {
            var result = await new Designation().UpdateStatusByDesignationId(obj, _context);
            if (result != null)
                return Ok(new
                {
                    Success = true,
                    Message = "Data Update Successfully!"
                });
            else
                return NotFound();
        }

        [HttpPost("DesignationAddOrUpdate")]
        public async Task<ActionResult<Designation>> DesignationAddOrUpdate(Designation obj)
        {
            var result = await new Designation().PostDesignation(obj, _context);
            if (result != null && result.Flag ==2)
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

        [HttpDelete("DeleteDesignationInfo{id}/{UserId}")]
        public async Task<ActionResult<bool>> DeleteDesignationInfo(int id, int UserId)
        {
            var result = await new Designation().DeleteDesignationInfo(id, UserId, _context);
            if (id > 0)
                return Ok(new
                {

                    Success = true,
                    Message = "Data Delete Successfully!"


                });

            return NotFound("Data Not Found!");
        }


        [HttpGet("DesignationCheckData{id}/{title}/{CompanyId}")]
        public async Task<ActionResult<Designation>> DesignationCheckData(int id, string title, int CompanyId)
        {
            if (await new Designation().AlreadyExist(id, title, CompanyId, _context))
            {
                return Ok(new
                {

                    Success = true,
                    Message = "Designation Already Exist!"


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









        #endregion

        #region Department Info

        [HttpGet("GetDepartmentByCompanyID{CompanyId}")]
        public async Task<ActionResult<List<Department>>> GetDepartment(int CompanyId)
        {
            return await new Department().GetDepartment(CompanyId, _context);
        }

        [HttpGet("GetDepartmentById{Id}")]
        public async Task<ActionResult<Department>> GetDepartmentById(int Id)
        {
            var dbResult = await new Department().GetDepartmentById(Id, _context);
            if (dbResult != null)
            {
                return Ok(dbResult);
            }
            return NotFound();
        }
        [HttpPost("UpdateStatusByDepartmentId")]
        public async Task<ActionResult<Department>> UpdateStatusByDepartmentId(Department obj)
        {
            var result = await new Department().UpdateStatusByDepartmentId(obj, _context);
            if (result != null)
                return Ok(new
                {
                    Success = true,
                    Message = "Data Update Successfully!"
                });
            else
                return NotFound();
        }
        [HttpPost("PostDepartment")]
        public async Task<ActionResult<Department>> PostDepartment(Department department)
        {

            var dbResult = await new Department().PostDepartment(department, _context);
            if (dbResult != null && dbResult.TransFlag == 2)
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

        [HttpGet("DeleteDepartment{Id}/{UserId}")]
        public async Task<ActionResult<bool>> DeleteDepartment(int Id, int UserId)
        {
            if (Id > 0)
            {

                var dbResult = await new Department().DeleteDepartment(Id, UserId, _context);
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

        [HttpGet("DepartmentAlreadyExists{CompanyId}/{id}/{title}")]
        public async Task<ActionResult<bool>> DepartmentAlreadyExists(int CompanyId, int Id, string title)
        {
            var dbResult = await new Department().AlreadyExists(CompanyId, Id, title, _context);
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

        #region LeaveTypeInfo

        [HttpGet("GetLeaveTypeInfos{CompanyId}")]
        public async Task<ActionResult<List<LeaveType>>> GetLeaveTypeInfos(int CompanyId)
        {

            return await new LeaveType().GetLeaveType(CompanyId, _context);
        }

        // using this api in leave apply form
        [HttpGet("GetStaffWiseLeaveTypeInfos{StaffId}")]
        public async Task<ActionResult<List<LeaveType>>> GetStaffWiseLeaveTypeInfos(int StaffId)
        {

            return await new LeaveType().GetStaffWiseLeaveType(StaffId, _context);
        }

        [HttpGet("GetLeaveTypeInfoId{id}")]
        public async Task<ActionResult<LeaveType>> GetLeaveTypeInfoId(int id)
        {
            var result = await new LeaveType().GetLeaveTypeById(id, _context);
            if (result != null)
                return Ok(result);

            return NotFound();

        }
        [HttpPost("UpdateStatusByLeaveTypeId")]
        public async Task<ActionResult<LeaveType>> UpdateStatusByLeaveTypeId(LeaveType obj)
        {
            var result = await new LeaveType().UpdateStatusByLeaveTypeId(obj, _context);
            if (result != null)
                return Ok(new
                {
                    Success = true,
                    Message = "Data Update Successfully!"
                });
            else
                return NotFound();
        }

        [HttpPost("LeaveTypeAddOrUpdate")]
        public async Task<ActionResult<LeaveType>> LeaveTypeAddOrUpdate(LeaveType obj)
        {


            var result = await new LeaveType().PostLeaveType(obj, _context);
            if (result != null && result.LeaveTypeId > 0)
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

        [HttpGet("DeleteLeaveTypeInfo{id}/{UserId}")]
        public async Task<ActionResult<bool>> DeleteLeaveTypeInfo(int id, int UserId)
        {
            if (id > 0)
            {
                var dbResult = await new LeaveType().DeleteLeaveTypeInfo(id, UserId, _context);
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


        [HttpGet("LeaveTypeAlreadyExists{id}/{title}/{CompanyId}")]
        public async Task<ActionResult<bool>> LeaveTypeAlreadyExists(int id, string title, int CompanyId)
        {
            var dbResult = await new LeaveType().AlreadyExist(id, title, CompanyId, _context);
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

        #region LoanTypeInfo

        [HttpGet("GetLoanTypeInfos{CompanyId}")]
        public async Task<ActionResult<List<LoanType>>> GetLoanTypeInfos(int CompanyId)
        {

            return await new LoanType().GetLoanType(CompanyId, _context);
        }


        [HttpGet("GetLoanTypeInfoId{id}")]
        public async Task<ActionResult<LoanType>> GetLoanTypeInfoId(int id)
        {
            var result = await new LoanType().GetLoanTypeById(id, _context);
            if (result != null)
                return Ok(result);

            return NotFound();


        }
        [HttpPost("UpdateStatusByLoanTypeId")]
        public async Task<ActionResult<LoanType>> UpdateStatusByLoanTypeId(LoanType obj)
        {
            var result = await new LoanType().UpdateStatusByLoanTypeId(obj, _context);
            if (result != null)
                return Ok(new
                {
                    Success = true,
                    Message = "Data Update Successfully!"
                });
            else
                return NotFound();
        }

        [HttpPost("LoanTypeAddOrUpdate")]
        public async Task<ActionResult<LoanType>> LoanTypeAddOrUpdate(LoanType obj)
        {


            var result = await new LoanType().PostLoanType(obj, _context);
            if (result != null && result.LoanTypeId > 0)
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

        [HttpGet("DeleteLoanTypeInfo{id}/{UserId}")]
        public async Task<ActionResult<bool>> DeleteLoanTypeInfo(int Id, int UserId)
        {
            if (Id > 0)
            {

                var dbResult = await new LoanType().DeleteLoanTypeInfo(Id, UserId, _context);
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


        [HttpGet("LoanTypeAlreadyExists{id}/{title}/{CompanyId}")]
        public async Task<ActionResult<bool>> LoanTypeAlreadyExists(int id, string title, int CompanyId)
        {
            var dbResult = await new LoanType().AlreadyExist(id, title, CompanyId, _context);
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

        #region Holiday
        [HttpGet("GetHolidaysByCompanyID{CompanyID}")]
        public async Task<ActionResult<List<Holiday>>> GetHolidays(int CompanyID)
        {
            return await new Holiday().GetHolidays(CompanyID, _context);
        }


        [HttpGet("GetHolidayByID{id}")]
        public async Task<ActionResult<List<Holiday>>> GetHolidayByID(int id)
        {
            var dbresult = await new Holiday().GetHOlidayByID(id, _context);
            if (dbresult != null)
            {
                return Ok(dbresult);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost("UpdateStatusByHolidayId")]
        public async Task<ActionResult<Holiday>> UpdateStatusByHolidayId(Holiday obj)
        {
            var result = await new Holiday().UpdateStatusByHolidayId(obj, _context);
            if (result != null)
                return Ok(new
                {
                    Success = true,
                    Message = "Data Update Successfully!"
                });
            else
                return NotFound();
        }
        [HttpPost("POstHoliday")]
        public async Task<ActionResult<Holiday>> PostHoliday(Holiday objHoliday)
        {

            Holiday obj = new Holiday();
            var dbResult = await obj.PostHoliday(objHoliday, _context);
            if (dbResult != null && dbResult.TransFlag == 2)
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


        [HttpGet("DeleteHoliday{id}/{UserID}")]
        public async Task<ActionResult<bool>> DeleteHoliday(int id, int UserID)
        {
            if (id > 0)
            {
                var dbResult = await new Holiday().DeleteHoliday(id, UserID, _context);
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



        [HttpGet("HolidayAlreadyExistsss{CompanyId}/{id}/{HolidayDate}")]
        public async Task<ActionResult<bool>> HolidayAlreadyExistsss(int CompanyId, int id, DateTime HolidayDate)
        {
            var dbResult = await new Holiday().AlredyExist(id, HolidayDate, CompanyId, _context);
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



        [HttpGet("FilterHolidayList{CompanyId}/{selectdate}/{Yeardate}")]
        public async Task<ActionResult<List<Holiday>>> FilterHolidayList(int CompanyId, string selectdate, int Yeardate)
        {

            return await new Holiday().GetFilterHolidayList(CompanyId, selectdate, Yeardate, _context);
        }


        [HttpGet("GetLoadCalanderEvent{CompanyID}/{month}/{year}")]
        public async Task<ActionResult<List<Holiday>>> GetLoadCalanderEvent(int CompanyID, int month, int year)
        {
            return await new Holiday().LoadCalanderEvent(CompanyID,month,year);
        }



        #endregion

        #region WeekendRule

        [HttpGet("GetWeekendRuleByCompanyID{CompanyID}")]
        public async Task<ActionResult<List<WeekendRule>>> GetWeekendRules(int CompanyID)
        {
            return await new WeekendRule().GetWeekendRules(CompanyID, _context);
        }


        [HttpGet("GetWeekendRuleByID{ID}")]
        public async Task<ActionResult<List<WeekendRule>>> GetWeekendRuleByID(int ID)
        {
            var dbResult = await new WeekendRule().GetWeekendRuleByID(ID, _context);
            if (dbResult != null)
            {
                return Ok(dbResult);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("DeleteWeekendRule{id}/{UserId}")]
        public async Task<ActionResult<bool>> DeleteWeekendRule(int Id, int UserId)
        {
            if (Id > 0)
            {

                var dbResult = await new WeekendRule().DeleteWeekendRuleInfo(Id, UserId, _context);
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
        [HttpPost("UpdateStatusByWeekendRuleId")]
        public async Task<ActionResult<WeekendRule>> UpdateStatusByWeekendRuleId(WeekendRule obj)
        {
            var result = await new WeekendRule().UpdateStatusByWeekendRuleId(obj, _context);
            if (result != null)
                return Ok(new
                {
                    Success = true,
                    Message = "Data Update Successfully!"
                });
            else
                return NotFound();
        }

        [HttpPost("PostWeekendRule")]
        public async Task<ActionResult<Holiday>> PostWeekendRule(WeekendRule objRule)
        {
            WeekendRule obj = new WeekendRule();
            var dbResult = await obj.PostWeekendRule(objRule, _context);
            if (dbResult != null && dbResult.TransFlag == 2)
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



		#endregion

		#region Announcement Info

		[HttpGet("GetAnnouncementByCompanyID{CompanyId}")]
		public async Task<ActionResult<List<Announcement>>> GetAnnouncement(int CompanyId)
		{
			return await new Announcement().GetAnnouncement(CompanyId, _context);
		}

		[HttpGet("GetAnnouncementById{Id}")]
		public async Task<ActionResult<Announcement>> GetAnnouncementById(int Id)
		{
			var dbResult = await new Announcement().GetAnnouncementById(Id, _context);
			if (dbResult != null)
			{
				return Ok(dbResult);
			}
			return NotFound();
		}
        [HttpPost("UpdateStatusByAnnouncementId")]
        public async Task<ActionResult<Announcement>> UpdateStatusByAnnouncementId(Announcement obj)
        {
            var result = await new Announcement().UpdateStatusByAnnouncementId(obj, _context);
            if (result != null)
                return Ok(new
                {
                    Success = true,
                    Message = "Data Update Successfully!"
                });
            else
                return NotFound();
        }
        [HttpPost("PostAnnouncement")]
		public async Task<ActionResult<Announcement>> PostAnnouncement(Announcement Announcement)
		{

			var dbResult = await new Announcement().PostAnnouncement(Announcement, _context);
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

		[HttpGet("DeleteAnnouncement{Id}/{UserId}")]
		public async Task<ActionResult<bool>> DeleteAnnouncement(int Id, int UserId)
		{
			if (Id > 0)
			{

				var dbResult = await new Announcement().DeleteAnnouncement(Id, UserId, _context);
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

		[HttpGet("AnnouncementAlreadyExists{CompanyId}/{id}/{title}")]
		public async Task<ActionResult<bool>> AnnouncementAlreadyExists(int CompanyId, int Id, string title)
		{
			var dbResult = await new Announcement().AlreadyExists(CompanyId, Id, title, _context);
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



		private string uploadImage(string name, IFormFile file, string root)
        {

            try
            {
                string fileName = string.Empty;
                if (file != null)
                {
                    var fileExtension = Path.GetExtension(file.FileName);
                    fileName = name + "-" + DateTime.Now.Ticks + fileExtension;
                    var filepath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", root, fileName);

                    var OldpathImage = filepath;
                    if (System.IO.File.Exists(OldpathImage))
                    {
                        System.IO.File.Delete(OldpathImage);
                    }


                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return "/Images/" + root + "/" + fileName;    // Path.GetFullPath( filepath);// @"/Images/" + root + "/" + fileName;
                }
                else
                {

                    return "";
                }
            }
            catch (Exception ex)
            {

                throw;
            }


        }





    }
}
