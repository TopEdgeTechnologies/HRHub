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

		[HttpPost("ForgetPassword")]
		public async Task<ActionResult<PasswordResetLog>> ForgetPassword(PasswordResetLog obj)
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

		[HttpPost("ForgetPassword")]
		public async Task<ActionResult<PasswordResetLog>> ChangePassword(PasswordResetLog obj)
		{
			var result = await new PasswordResetLog().PasswordReset(obj, _context);
			if (result != null)
				return Ok(new
				{
					Success = true,
					Message = "Youre Password Change Successfully!"
				});
			else
				return NotFound();
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
