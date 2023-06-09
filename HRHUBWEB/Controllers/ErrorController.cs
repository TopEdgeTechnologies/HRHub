using HRHUBAPI.Models;
using HRHUBWEB.Extensions;
using HRHUBWEB.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace HRHUBWEB.Controllers
{
    public class ErrorController : Controller
    {


		private readonly HttpClient _client;
	
		public ErrorController(IHttpClientFactory httpClient)
		{
			_client = httpClient.CreateClient("APIClient");
	
		}

		[Route("Error")]
        public async Task<IActionResult> Error()
        {

			var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
			if (userObject==null)
			{
                return RedirectToAction("Loginpage", "User", new { id = 2 });
            }
			var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var errorViewModel = new ExceptionLog
			{
                ErrorMessage = exceptionHandlerPathFeature?.Error.Message,
                StackTrace = exceptionHandlerPathFeature?.Error.StackTrace,
                InnerException = exceptionHandlerPathFeature?.Error.InnerException?.ToString(),
                UserId=userObject.UserId
            };



			// var DesignationResume = my.Files.GetFile("DesignationResume");

			//ObjDesignation.AttachmentPath = uploadImage(ObjDesignation.Name, DesignationResume, "DesignationAttachment");

		
			
			HttpResponseMessage message = await _client.PostAsJsonAsync("api/User/ExpectionLog", errorViewModel);

			if (message.IsSuccessStatusCode)
			{

				var body = message.Content.ReadAsStringAsync();
				var model = JsonConvert.DeserializeObject<Response>(body.Result);


			}


				return View(errorViewModel);
        }
    }
}
