using HRHUBAPI.Models;
using HRHUBAPI.Models.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Text.Json.Nodes;

namespace SchoolManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserGroupController : ControllerBase
    {
        private readonly HrhubContext _context;
        private readonly DbConnection _db;

        public UserGroupController(HrhubContext context, DbConnection db)
        {
            _context = context;
            _db = db;
        }

        #region UserGroup

        [HttpGet("ListUserGroup{CompanyId}")]
        public async Task<ActionResult<List<GluserGroup>>> ListUserGroup(int CompanyId)
        {

            return await new GluserGroup().GetGluserGroup(CompanyId,_context);
        }


        [HttpGet("GetUserGroupId{id}")]
        public async Task<ActionResult<GluserGroup>> GetUserGroupId(int id)
        {
            var result = await new GluserGroup().GetGluserGroupById(id, _context);
            if (result != null)
                return Ok(result);

            return NotFound();


        }

        [HttpPost("UserGroupAddOrCreate")]
        public async Task<ActionResult<GluserGroup>> UserGroupAddOrCreate(GluserGroup obj)
        {


            var result = await new GluserGroup().PostGluserGroup(obj, _context);
            if (result != null && result.GroupId > 0)
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

        [HttpDelete("UserGroupDelete{id}")]
        public async Task<ActionResult<bool>> UserGroupDelete(int id)
        {
            var result = await new GluserGroup().DeleteGluserGroup(id, _context);
            if (id > 0)
                return Ok(new
                {

                    Success = true,
                    Message = "Data Delete Successfully!"


                });

            return NotFound("Data Not Found!");
        }


        [HttpGet("UserGroupCheckData{id}/{title}/{CompnayId}")]
        public async Task<ActionResult<JsonObject>> UserGroupCheckData(int id, string title,int CompnayId)
        {
            if (await new GluserGroup().AlreadyExist(id, title.Trim(), CompnayId, _context))
            {
                return Ok(new
                {

                    Success = true,
                    Message = "Users Group Already Exist!"


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






       // Load GluserDetails in edit mode permission

       [HttpGet("GetUserGroupDetails{id}")]
        public async Task<ActionResult<GluserGroupDetail>> GetUserGroupDetails(int id)
        {
            var result = await new GluserGroup().GetGlUserByGroupId(id, _db);
            if (result != null)
                return Ok(result);

            return NotFound();


        }




        // Update Active and inactive Status User

        [HttpGet("UpdateGroupUsers{id}/{status}/{UserId}")]
        public async Task<ActionResult<bool>> UpdateGroupUsers(int id, bool status, int UserId)
        {
            var result = await new GluserGroup().UpdateGroupStatusUser(id, status, UserId, _context);
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
