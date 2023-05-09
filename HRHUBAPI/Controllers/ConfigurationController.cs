using HRHUBAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        #region DesignationInfo

        [HttpGet("GetDesignationInfos")]
        public async Task<ActionResult<List<Designation>>> GetDesignationInfos()
        {

            return await new Designation().GetDesignation(_context);
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


        [HttpGet("DesignationCheckData{id}/{title}")]
        public async Task<ActionResult<JsonObject>> DesignationCheckData(int id, string title)
        {
            if (await new Designation().AlreadyExist(id, title, _context))
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

      





    }
}
