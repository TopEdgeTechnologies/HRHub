﻿using HRHUBAPI.Models;
using HRHUBAPI.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Text.Json.Nodes;

namespace SchoolManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly HrhubContext _context;
        private readonly jwtTokenGenrator _jwtToken;
        private readonly IConfiguration _configuration;

    

        public UserController(HrhubContext context, jwtTokenGenrator jwtToken  , IConfiguration configuration)
        {
            _context = context;
            _jwtToken= jwtToken;
            _configuration = configuration;
        }

        #region Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login(User Obj)
        {
            if (Obj == null)
                return BadRequest();

            var user = await Obj.Login(Obj, _context);


            if (user != null)
            {
                var token =  _jwtToken.GenerateJwtToken(user.UserId.ToString(), user.UserName, _configuration.GetSection("Jwt").GetValue<string>("Key"), _configuration["Jwt:Issuer"], _configuration["Jwt:ValidAudience"]);

                return Ok(
                   new
                   {
                       Success = true,
                       Message = "Login Successfully!",
                       Data= user,
                       Token= token,
					
                   }


                    );

            }

            return Ok(new
            {

                Success = false,
                Message = "Invalid User name and Password"
          


            });






        }
        [HttpPost("Register")]
        #endregion

        #region Register
        public async Task<IActionResult> Register(User Obj)
        {
            try
            {
                var result = await new User().RegisterUser(Obj, _context);
                if (result != null)
                    return Ok(new
                    {
                        Success = true,
                        Message = "User Register Successfully!"
                    });
                return NotFound();

            }
            catch (Exception)
            {

                throw;
            }



        }
        #endregion

        [HttpGet("UserCheckData{id}/{username}")]
        public async Task<ActionResult<JsonObject>> UserCheckData(int id, string username)
        {
            if (await new User().AlreadyExist(id, username, _context))
            {
                return Ok(new
                {

                    Success = true,
                    Message = "User Already Exist!"


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


		[HttpPost("UserLogOutHistory")]
		public async Task<ActionResult<JsonObject>> UserLogOutHistory(User Obj)
		{
			if (await new User().UserLogOutHistory(Obj.UserId, _context))
			{
				return Ok(new
				{

					Success = true,
					Message = "User logout!"


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



		
		[Authorize]

		[HttpPost("UserChangePassword")]
		public async Task<ActionResult<JsonObject>> UserChangePassword(User obj  )
		{


            var result = await obj.ChangePassword(obj, _context);
			if( result!=null)
			{
				return Ok(new
				{

					Success = true,
					Message = "Password Updated Successfully"


				});
			}
			else
			{

				return Ok(new
				{

					Success = false,
					Message = "Fail"


				});
			}

		}



		[HttpPost("ExpectionLog")]
		public async Task<ActionResult<JsonObject>> ExpectionLog(ExceptionLog obj)
		{


			var result = await obj.PostExecptiontion(obj, _context);
			if (result)
			{
				return Ok(new
				{

					Success = true,
					Message = "Insert Execptiontion"


				});
			}
			else
			{

				return Ok(new
				{

					Success = false,
					Message = "Fail"


				});
			}

		}





		// Get single record of User by company ID

		[HttpGet("GetUserCompanyViseId{CompanyId}")]
		public async Task<ActionResult<User>> GetUserCompanyViseId(int CompanyId)
		{
			var dbResult = await new User().GetUserCompanyVise(CompanyId, _context);
			if (dbResult != null)
			{
				return Ok(dbResult);
			}
			return NotFound();
		}

        // Get single record of User by company ID and User Id 

        [HttpGet("GetUserViseId{CompanyId}/{UserId}")]
        public async Task<ActionResult<User>> GetUserViseId(int CompanyId, int UserId)
        {
            var dbResult = await new User().GetUserIdVise(CompanyId,UserId, _context);
            if (dbResult != null)
            {
                return Ok(dbResult);
            }
            return NotFound();
        }






        #region System Users

        [HttpGet("ListUser{CompanyId}")]
        public async Task<ActionResult<List<User>>> ListUser(int CompanyId)
        {

            return await new User().GetUser(CompanyId, _context);
        }






        // Update Active and inactive Status User

        [HttpGet("UpdateUsers{id}/{status}/{UserId}")]
        public async Task<ActionResult<bool>> UpdateUsers(int id,bool status,int UserId)
        {
            var result = await new User().UpdateStatusUser(id, status, UserId, _context);
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
