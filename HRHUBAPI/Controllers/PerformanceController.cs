﻿using HRHUBAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace HRHUBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PerformanceController : ControllerBase
    {
        private readonly HrhubContext _context;

        public PerformanceController(HrhubContext context)
        {
            _context = context;

        }

        #region PerformanceAppraisal

        [HttpGet("GetPerformanceAppraisalInfos")]
        public async Task<ActionResult<List<StaffReviewFormProcessed>>> GetPerformanceAppraisalInfos()
        {

            return await new StaffReviewFormProcessed().GetPerformanceAppraisal( _context);
        }

        [HttpGet("GetPerformanceReviewSections{Id}/{staffid}")]
        public async Task<ActionResult<List<Section>>> GetPerformanceReviewSections(int Id, int staffid)
        {
            var result = await new StaffReviewFormProcessed().GetReviewSections(Id,staffid,  _context);
            if (result != null)
                return Ok(result);

            return NotFound();


        }
        #endregion

        #region Performance

        [HttpGet("GetPerformanceInfos{CompanyId}")]
        public async Task<ActionResult<List<PerformanceForm>>> GetPerformanceInfos(int CompanyId)
        {

            return await new PerformanceForm().GetPerformanceForm(CompanyId, _context);
        }


        [HttpGet("GetPerformanceInfoId{id}")]
        public async Task<ActionResult<PerformanceForm>> GetPerformanceInfoId(int id)
        {
            var result = await new PerformanceForm().GetPerformanceFormById(id, _context);
            if (result != null)
                return Ok(result);

            return NotFound();


        }

        [HttpPost("PerformanceAddOrUpdate")]
        public async Task<ActionResult<PerformanceForm>> PerformanceAddOrUpdate(PerformanceForm obj)
        {


            var result = await new PerformanceForm().PostPerformanceForm(obj, _context);
            if (result != null && result.Flag == 2)
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

        [HttpGet("DeletePerformanceInfo{id}/{UserId}")]
        public async Task<ActionResult<bool>> DeletePerformanceInfo(int id, int UserId)
        {
            var result = await new PerformanceForm().DeletePerformanceFormInfo(id, UserId, _context);
            if (id > 0)
                return Ok(new
                {

                    Success = true,
                    Message = "Data Delete Successfully!"


                });

            return NotFound("Data Not Found!");
        }


        [HttpGet("PerformanceCheckData{id}/{title}/{CompanyId}")]
        public async Task<ActionResult<PerformanceForm>> PerformanceCheckData(int id, string title, int CompanyId)
        {
            if (await new PerformanceForm().AlreadyExist(id, title, CompanyId, _context))
            {
                return Ok(new
                {

                    Success = true,
                    Message = "Title Already Exist!"


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

        #region Performance Sections

        [HttpGet("GetPerformanceSectionInfos")]
        public async Task<ActionResult<List<Section>>> GetPerformanceSectionInfos()
        {

            return await new Section().GetSection(_context);
        }


        [HttpGet("GetPerformanceSectionInfoId{id}")]
        public async Task<ActionResult<Section>> GetPerformanceSectionInfoId(int id)
        {
            var result = await new Section().SectionById(id, _context);
            if (result != null)
                return Ok(result);

            return NotFound();


        }

        [HttpPost("PerformanceSectionAddOrUpdate")]
        public async Task<ActionResult<Section>> PerformanceSectionAddOrUpdate(Section obj)
        {


            var result = await new Section().PostSection(obj, _context);
            if (result != null)
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

        [HttpGet("DeletePerformanceSectionInfo{id}/{UserId}")]
        public async Task<ActionResult<Section>> DeletePerformanceSectionInfo(int id, int UserId)
        {
            var result = await new Section().DeleteSectionInfo(id, UserId, _context);
            if (result !=null && id > 0)
                return Ok(result);

            return NotFound("Data Not Found!");
        }


        [HttpGet("PerformanceSectionCheckData{id}/{title}")]
        public async Task<ActionResult<Section>> PerformanceSectionCheckData(int id, string title)
        {
            if (await new Section().AlreadyExist(id, title, _context))
            {
                return Ok(new
                {

                    Success = true,
                    Message = "Section Already Exist!"


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
