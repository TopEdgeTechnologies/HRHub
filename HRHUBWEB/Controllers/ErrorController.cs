using HRHUBAPI.Models;
using HRHUBWEB.Extensions;
using HRHUBWEB.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;


namespace HRHUBWEB.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error")]
        public IActionResult Error()
        {

			var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
			var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var errorViewModel = new ErrorViewModel
            {
                ErrorMessage = exceptionHandlerPathFeature?.Error.Message,
                StackTrace = exceptionHandlerPathFeature?.Error.StackTrace,
                InnerException = exceptionHandlerPathFeature?.Error.InnerException?.ToString(),
                UserID=userObject.UserId
            };


            return View(errorViewModel);
        }
    }
}
