﻿using HRHUBAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using System.Text.Json.Nodes;

namespace HRHUBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConfigurationController : ControllerBase
    {
        private readonly HrhubContext _context;

        public ConfigurationController(HrhubContext context)
        {
            _context = context;

        }
        // test comment
        #region DesignationInfo

        [HttpGet("GetDesignationInfos{CompanyId}")]
        public async Task<ActionResult<List<Designation>>> GetDesignationInfos(int CompanyId)
        {

            return await new Designation().GetDesignation(CompanyId,_context);
        }


        [HttpGet("GetDesignationInfoId{id}")]
        public async Task<ActionResult<Designation>> GetDesignationInfoId(int id)
        {
            var result = await new Designation().GetDesignationById(id, _context);
            if (result != null)
                return Ok(result);

            return NotFound();


        }

        [HttpPost("DesignationAddOrUpdate")]
        public async Task<ActionResult<Designation>> DesignationAddOrUpdate(Designation obj)
        {


            var result = await new Designation().PostDesignation(obj, _context);
            if (result != null && result.DesignationId > 0)
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

        [HttpDelete("DeleteDesignationInfo{id}")]
        public async Task<ActionResult<bool>> DeleteDesignationInfo(int id)
        {
            var result = await new Designation().DeleteDesignationInfo(id, _context);
            if (id > 0)
                return Ok(new
                {

                    Success = true,
                    Message = "Data Delete Successfully!"


                });

            return NotFound("Data Not Found!");
        }


        [HttpGet("DesignationCheckData{id}/{title}/{CompanyId}")]
        public async Task<ActionResult<JsonObject>> DesignationCheckData(int id, string title,int CompanyId)
        {
            if (await new Designation().AlreadyExist(id, title,CompanyId, _context))
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

        #region LeaveTypeInfo

        [HttpGet("GetLeaveTypeInfos{CompanyId}")]
        public async Task<ActionResult<List<LeaveType>>> GetLeaveTypeInfos(int CompanyId)
        {

            return await new LeaveType().GetLeaveType(CompanyId, _context);
        }


        [HttpGet("GetLeaveTypeInfoId{id}")]
        public async Task<ActionResult<LeaveType>> GetLeaveTypeInfoId(int id)
        {
            var result = await new LeaveType().GetLeaveTypeById(id, _context);
            if (result != null)
                return Ok(result);

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

        [HttpDelete("DeleteLeaveTypeInfo{id}")]
        public async Task<ActionResult<bool>> DeleteLeaveTypeInfo(int id)
        {
            var result = await new LeaveType().DeleteLeaveTypeInfo(id, _context);
            if (id > 0)
                return Ok(new
                {

                    Success = true,
                    Message = "Data Delete Successfully!"


                });

            return NotFound("Data Not Found!");
        }


        [HttpGet("LeaveTypeCheckData{id}/{title}/{CompanyId}")]
        public async Task<ActionResult<JsonObject>> LeaveTypeCheckData(int id, string title, int CompanyId)
        {
            if (await new LeaveType().AlreadyExist(id, title, CompanyId, _context))
            {
                return Ok(new
                {

                    Success = true,
                    Message = "LeaveType Already Exist!"


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





    }
}
