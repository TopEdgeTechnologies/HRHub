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
    public class UserFormController : ControllerBase
    {
        private readonly HrhubContext _context;
        public UserFormController(HrhubContext context)
        {
            _context = context;

        }

        #region UserForm

        [HttpGet("ListUserForm")]
        public async Task<ActionResult<List<UserForm>>> ListUserForm()
        {

            return await new UserForm().GetUserForm(_context);
        }


        // Load userFormsData in edit mode permission

        //[HttpGet("GetUserFormId{id}")]
        //public async Task<ActionResult<UserForm>> GetUserFormId(int id)
        //{
        //    var result = await new get().GetStaffTypeById(id, _context);
        //    if (result != null)
        //        return Ok(result);

        //    return NotFound();


        //}









        //[HttpGet("GetUserFormId{id}")]
        //public async Task<ActionResult<UserForm>> GetUserFormId(int id)
        //{
        //    var result = await new get().GetStaffTypeById(id, _context);
        //    if (result != null)
        //        return Ok(result);

        //    return NotFound();


        //}

        //[HttpPost("StaffTypeAddOrCreate")]
        //public async Task<ActionResult<StaffTypeInfo>> StaffTypeAddOrCreate(StaffTypeInfo obj)
        //{


        //    var result = await new StaffTypeInfo().PostStaffType(obj, _context);
        //    if (result != null && result.StaffTypeId > 0)
        //        return Ok(new
        //        {
        //            Success = true,
        //            Message = "Data Update Successfully!"
        //        });
        //    else
        //        return Ok(new
        //        {
        //            Success = true,
        //            Message = "Data Insert Successfully!"
        //        });





        //}

        //[HttpDelete("StaffTypeDelete{id}")]
        //public async Task<ActionResult<bool>> StaffTypeDelete(int id)
        //{
        //    var result = await new StaffTypeInfo().DeleteStaffType(id, _context);
        //    if (id > 0)
        //        return Ok(new
        //        {

        //            Success = true,
        //            Message = "Data Delete Successfully!"


        //        });

        //    return NotFound("Data Not Found!");
        //}


        //[HttpGet("StaffTypeCheckData{id}/{title}")]
        //public async Task<ActionResult<JsonObject>> StaffTypeCheckData(int id, string title)
        //{
        //    if (await new StaffTypeInfo().AlreadyExist(id, title.Trim(), _context))
        //    {
        //        return Ok(new
        //        {

        //            Success = true,
        //            Message = "Staff Already Exist!"


        //        });
        //    }
        //    else
        //    {

        //        return Ok(new
        //        {

        //            Success = false,
        //            Message = "Not found"


        //        });
        //    }

        //}
        #endregion






    }
}
