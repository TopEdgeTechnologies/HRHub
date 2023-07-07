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

		[HttpGet("ForgetPassword{Email}")]
		public async Task<ActionResult<bool>> ForgetPassword(string Email)
		{
			var result = await new User().SendLinkPasswordReset(Email, _context);
			if (result != null)
				return Ok(new
				{
					Success = true,
					Message = "Email Send Successfully!"
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
					Message = "User Email Not Exist!"


				});
			}

		}


















		#endregion

	}
}
