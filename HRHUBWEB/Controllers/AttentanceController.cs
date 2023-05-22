using HRHUBAPI.Models;
using HRHUBWEB.Extensions;
using HRHUBWEB.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.Design;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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



		//Get Attendance Data Company Vise 
		[HttpPost]
		public async Task<IActionResult> MarkAttendenceSubmit()
		{

			AttendanceMaster obj = new AttendanceMaster();

			var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

			var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
			obj.StaffId = userObject.StaffId;
			obj.CreatedBy = userObject.UserId;
			obj.CompanyID = userObject.CompanyId;
			
			obj.AttendanceDate= DateTime.Now;
			obj.FirstPunchIn = DateTime.Now.TimeOfDay;
			obj.LastPunchOut = DateTime.Now.TimeOfDay;

			
			if (Token != null)
			{

				HttpResponseMessage message = await _client.PostAsJsonAsync("api/Attendance/MarkStaffAttendance", obj);
				if (message.IsSuccessStatusCode)
				{
					var result = message.Content.ReadAsStringAsync().Result;

					if (result !=null)
					{
						//  Send "false"
						return Json(new { success = true, responseText = "Check In" });
					}
					




				}

				return Json(obj);
			}
			else
			{
				return RedirectToAction("Loginpage", "User", new { id = 2 });
			}

		}



		//Check Atendenece In Or Out
		public async Task<ActionResult<JsonObject>> StatusCheckData()
		{

			var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

			var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
			var CompanyId = userObject.CompanyId;

			HttpResponseMessage message = await _client.GetAsync($"api/Attendance/CheckData");
			if (message.IsSuccessStatusCode)
			{
				var result = message.Content.ReadAsStringAsync().Result;
				return Json(result);

			}

			else
			{
				return Json(new

				{
					Success = false,
					Message = "Error occur"

				}
				);

			}
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


		//Get Attendance Data Company Vise 
		public async Task<ActionResult<JsonObject>> GetAttendanceDataCompanyVise(int Staffid, string fromdate, string todate)
		{

			AttendanceMaster objAttendance = new AttendanceMaster();

			var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

			var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");

			if (Token != null)
			{

				HttpResponseMessage message = await _client.GetAsync($"api/Attendance/GetStaffAttendanceList{Staffid}/{fromdate}/{todate}");
				if (message.IsSuccessStatusCode)
				{
					var result = message.Content.ReadAsStringAsync().Result;
					return Json(JsonConvert.DeserializeObject<List<AttendanceMaster>>(result));

				}

				return Json(new AttendanceMaster());
			}
			else
			{
				return RedirectToAction("Loginpage", "User", new { id = 2 });
			}

		}



		//Get Attendance Data Company Vise 
		public async Task<ActionResult<JsonObject>> ListMarkAttendanceData(string todate)
		{

			AttendanceMaster objAttendance = new AttendanceMaster();

			var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

			var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");

			if (Token != null)
			{

				HttpResponseMessage message = await _client.GetAsync($"api/Attendance/MarkStaffAttendanceList{userObject.CompanyId}/{todate}");
				if (message.IsSuccessStatusCode)
				{
					var result = message.Content.ReadAsStringAsync().Result;
					return Json(JsonConvert.DeserializeObject<List<AttendanceMaster>>(result));

				}

				return Json(new AttendanceMaster());
			}
			else
			{
				return RedirectToAction("Loginpage", "User", new { id = 2 });
			}

		}








		public async Task<IActionResult> AttentanceOverView(string data = "")
		{
			return View();
		}


		public async Task<IActionResult> AttentanceByUser(string data = "")
		{


			var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

			var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
			if (Token != null)
			{

				var response = await _client.GetAsync($"api/Staffs/GetStaffByCompanyId{userObject.CompanyId}");
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					ViewBag.StaffList = JsonConvert.DeserializeObject<List<Staff>>(content);
				}

				return View();
			}
			else
			{
				return RedirectToAction("Loginpage", "User", new { id = 2 });
			}




			
		}



	    public async Task<IActionResult> AttendanceView(string data = "")
		{
			return View();
		}
		public async Task<IActionResult> OverviewCalendar(string data = "")
		{
			return View();
		}

		public async Task<IActionResult> MarkAttendance(string data = "")
		{
			return View();
		}




	}
}
