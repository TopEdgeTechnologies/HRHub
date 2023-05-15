﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HRHUBAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Reflection.Metadata.Ecma335;

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

        [HttpGet("GetStaffByCompanyId{CompanyId}")]
        public async Task<ActionResult<List<Staff>>> GetStaffByCompanyId(int CompanyId)
        {
			//return await new Staff().GetStaffByCompanyId(CompanyId, _context);
			//return await new Staff().GetStaffByCompanyId(CompanyId);
			return await new Staff().GetEmployee(CompanyId);
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

        [HttpGet("PostStaff")]
        public async Task<ActionResult<Staff>> PostStaff(Staff staff)
        {
            var dbResult = await new Staff().PostStaff(staff, _context);    
            if(dbResult != null && dbResult.TranFlag == 2)
            {
                return Ok(new
                {
                    success = true,
                    Message = "Data Updated Successfully"
                }); ;
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

        [HttpGet("StaffAlreadyExists{CompanyId}/{Id}/{title}")]
        public async Task<ActionResult<bool>> StaffAlreadyExists(int CompanyId, int Id, string title)
        {
            var dbResult = await new Staff().StaffAlreadyExists(CompanyId, Id, title, _context);   
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

        #endregion

    }
}
