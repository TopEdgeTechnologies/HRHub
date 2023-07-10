﻿using Azure.Core;
using HRHUBAPI.Models;
using HRHUBWEB.Extensions;
using HRHUBWEB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.Net;
using System.Reflection.Metadata;
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


            // Send password reset email from Admin sides
			if(Obj.UserId > 0)
			{
                var GetUser = await _APIHelper.CallApiAsyncGet<User>($"api/User/GetUserViseId{_user.CompanyId}/{Obj.UserId}", HttpMethod.Get);
                if (GetUser != null)
                {
					Obj.Email = GetUser.UserName;
                }
            }
            

            /////





            var ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
			Obj.RequestFromIp = ipAddress;

			var baseUrl = $"{Request.Scheme}://{Request.Host.Host}:{Request.Host.Port}";
			Obj.Url = baseUrl;

			var result = await _APIHelper.CallApiAsyncPost<Response>(Obj, "api/ResetsPassword/ForgetPasswords", HttpMethod.Post);

			
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
		public async Task<IActionResult> NewChangePassword(string id)
		{
			try
			{
				string Token =id;
				/// check expiry validatity 
				var currentdate = DateTime.Now;
				var result = await _APIHelper.CallApiAsyncGet<PasswordResetLog>($"api/ResetsPassword/CheckExpiryDate{Token}", HttpMethod.Get);
				if (result != null && result.ExpiryTime < currentdate)
				{
					return NotFound();
					
				}
				else
				{
					ViewBag.Token = Token;
					return View();
				}

				
			}
			catch (Exception)
			{

				throw;
			}


			

		}
		[HttpPost]
		public async Task<IActionResult> NewChangePassword(PasswordResetLog Obj)
		{


			try
			{



				var ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
				Obj.UpdatedFromIp = ipAddress;
				var result = await _APIHelper.CallApiAsyncPost<Response>(Obj, "api/ResetsPassword/ChangePassword", HttpMethod.Post);

				if (result.Success)
				{
					//login
					return Json(result);
                   // return RedirectToAction("Loginpage", "User", new { id = 4 });
                }
				else
				{
					return NotFound();
				}
				

			}
			catch (Exception)
			{

				throw;
			}
		}



		









		#endregion
	}
}
