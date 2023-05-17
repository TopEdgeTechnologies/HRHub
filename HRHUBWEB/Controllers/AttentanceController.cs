using HRHUBAPI.Models;
using HRHUBWEB.Extensions;
using HRHUBWEB.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;

namespace HRHUBWEB.Controllers
{
    public class AttentanceController : Controller
    {
        private readonly HttpClient _client;
        private IWebHostEnvironment _webHostEnvironment;
        public AttentanceController(IHttpClientFactory httpClient, IWebHostEnvironment webHostEnvironment)
        {
            _client = httpClient.CreateClient("APIClient");
            _webHostEnvironment = webHostEnvironment;
        }


        [CustomAuthorization]
        public async Task<IActionResult> AttentanceList(string data = "")
        {

            ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
            ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
            ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
            ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);

            return View(new  AttendanceMaster());
        }



		public async Task<ActionResult<JsonObject>> GetAttendanceData(string fromdate,string todate)
		{
			
			AttendanceMaster objAttendance = new AttendanceMaster();

			var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

			var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");

			if (Token != null)
			{

				HttpResponseMessage message = await _client.GetAsync($"api/Attendance/GetStaffAttendanceList{userObject.StaffId}/{fromdate}/{ todate}");
				if (message.IsSuccessStatusCode)
				{
					var result = message.Content.ReadAsStringAsync().Result;
					 return Json( JsonConvert.DeserializeObject<List<AttendanceMaster>>(result));

				}

				return Json(new AttendanceMaster());
			}
			else
			{
				return RedirectToAction("Loginpage", "User", new { id = 2 });
			}

		}





	}
}
