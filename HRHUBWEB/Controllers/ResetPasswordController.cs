using HRHUBAPI.Models;
using HRHUBWEB.Extensions;
using HRHUBWEB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
		public async Task<IActionResult> ForgetPassword(string Email)
		{


			var result = await _APIHelper.CallApiAsyncGet<Response>($"api/ResetsPassowrd/ForgetPassword{Email}", HttpMethod.Get);
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














		#endregion
	}
}
