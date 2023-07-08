using HRHUBAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace HRHUBAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ResetsPasswordController : ControllerBase
	{

		private readonly HrhubContext _context;	

		public ResetsPasswordController(HrhubContext context)
		{
			_context = context;
			
		}

		#region Forget Password Via Email



		// Send Email on User Reset password

		[HttpPost("ForgetPasswords")]
		public async Task<ActionResult<PasswordResetLog>> ForgetPasswords(PasswordResetLog obj)
		{
			var result = await new PasswordResetLog().SendLinkPasswordReset(obj, _context);
			if (result != null)
				return Ok(new
				{
					Success = true,
					Message = "Email Send Successfully! Check your Email"
				});
			else
				return NotFound();
		}




		// Update or  Reset user password

		[HttpPost("ChangePassword")]
		public async Task<ActionResult<PasswordResetLog>> ChangePassword(PasswordResetLog obj)
		{
			var result = await new PasswordResetLog().PasswordReset(obj, _context);
			if (result)
			{
				return Ok(new
				{
					Success = true,
					Message = "Youre Password Change Successfully!"
				});
			}
			else
			{
				return Ok(new
				{
					Success = false,
					Message = "Fail!"
				});
			}
		}


		//Check Expire date time with the help of token
		[HttpGet("CheckExpiryDate{Token}")]
		public async Task<ActionResult<PasswordResetLog>> CheckExpiryDate(string Token)
		{

			var obj = await new PasswordResetLog().CheckEpiryDates(Token, _context);


			if (obj !=null)
			{
				return Ok(obj);
			}
			else
			{

				return Ok(new
				{

					Success = false,
					


				});
			}

		}



		// Check user Email exits or not 
		[HttpGet("UserCheckData{Email}")]
		public async Task<ActionResult<JsonObject>> UserEmailCheck(string Email)
		{
			if (await new User().CheckExistEmail(Email, _context))
			{
				return Ok(new
				{

					Success = true,
					Message = "Not found"


				});
			}
			else
			{

				return Ok(new
				{

					Success = false,
					Message = "User Account Not Exist! Register It"


				});
			}

		}


















		#endregion

	}
}
