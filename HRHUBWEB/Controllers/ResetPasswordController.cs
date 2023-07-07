using HRHUBAPI.Models;
using HRHUBWEB.Extensions;
using HRHUBWEB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json.Nodes;

namespace HRHUBWEB.Controllers
{
   
    public class ResetPasswordController : Controller
	{
		private IWebHostEnvironment _webHostEnvironment;
		private readonly IEmailHelper _EmailHelper;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly APIHelper _APIHelper;
		private readonly User _user;

       
        public ResetPasswordController(IHttpClientFactory httpClient, IWebHostEnvironment webHostEnvironment, IEmailHelper EmailHelper, APIHelper aPIHelper,
			IHttpContextAccessor httpContextAccessor)
		{

			_webHostEnvironment = webHostEnvironment;
			_EmailHelper = EmailHelper;
			_APIHelper = aPIHelper;
			_httpContextAccessor = httpContextAccessor;
			_user = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
		}



		#region Reset Password Via Email
		[AllowAnonymous]
		[IgnoreAntiforgeryToken]
        public async Task<IActionResult> ForgetPassword()
		{
			return View();
		
		}
		[HttpPost]
		public async Task<IActionResult> ForgetPassword(PasswordResetLog Obj)
		{
			var ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
			Obj.RequestFromIp = ipAddress;

			var baseUrl = $"{Request.Scheme}://{Request.Host.Host}:{Request.Host.Port}";
			Obj.Url = baseUrl;

			var result = await _APIHelper.CallApiAsyncPost<Response>(Obj, "api/ResetsPassword/ForgetPassword", HttpMethod.Post);

			
			return Json(result);


		}




		// check Email Exit or not In User Table
		[HttpGet]
	
		public async Task<ActionResult<JsonObject>> CheckEmail(string Email)
		{
			try
			{
				var result = await _APIHelper.CallApiAsyncGet<Response>($"api/ResetsPassword/UserCheckData{Email}", HttpMethod.Get);
				return Json(result);
			}
			catch (Exception)
			{

				throw;
			}


		}

		[HttpGet]
		[AllowAnonymous]
		[IgnoreAntiforgeryToken]
		public async Task<IActionResult> NewChangePassword()
		{

			return View();

		}
		[HttpPost]
		public async Task<IActionResult> NewChangePassword(PasswordResetLog Obj)
		{

			var ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
			Obj.UpdatedFromIp = ipAddress;			

			var result = await _APIHelper.CallApiAsyncPost<Response>(Obj, "api/ResetsPassword/ChangePassword", HttpMethod.Post);


			return Json(result);

		}



		









		#endregion
	}
}
