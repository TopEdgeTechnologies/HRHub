using HRHUBAPI.Models;
using HRHUBAPI.Models.Configuration;
using HRHUBWEB.Extensions;
using HRHUBWEB.Filters;
using HRHUBWEB.Models;
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
		private readonly APIHelper _APIHelper;
		private readonly User _user;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public AttentanceController(IHttpClientFactory httpClient, IWebHostEnvironment webHostEnvironment, APIHelper APIHelper, IHttpContextAccessor httpContextAccessor)
		{
			_client = httpClient.CreateClient("APIClient");
			_webHostEnvironment = webHostEnvironment;
			_APIHelper = APIHelper;
			_httpContextAccessor = httpContextAccessor;
			_user = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
		}


		[CustomAuthorization]
		public async Task<IActionResult> AttentanceList(string data = "")
		{

			ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
			ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
			ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
			ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);

			var staffId = _user.StaffId;
			ViewBag.ListStatusCount = await _APIHelper.CallApiAsyncGet<IEnumerable<AttendanceMaster>>($"api/Attendance/SetStaffAttendanceStatus{staffId}", HttpMethod.Get);

			return View(new AttendanceMaster());

		}



		//Submit Attendance Data Company Vise 
		[HttpPost]
		public async Task<IActionResult> MarkAttendenceSubmit()
		{

			AttendanceMaster obj = new AttendanceMaster();
			obj.StaffId = _user.StaffId;
			obj.CreatedBy = _user.UserId;
			obj.CompanyID = _user.CompanyId;

			obj.AttendanceDate = DateTime.Now;
			obj.FirstPunchIn = DateTime.Now.TimeOfDay;
			obj.LastPunchOut = DateTime.Now.TimeOfDay;

			var result = await _APIHelper.CallApiAsyncPost<AttendanceMaster>(obj, "api/Attendance/MarkStaffAttendance", HttpMethod.Post);

			if (result != null)
			{
				return Json(result);

			}

			return Json(new
			{
				Success = false,
				Message = "Error occur"

			});
		}

	



		//Check Atendenece In Or Out
		public async Task<ActionResult> StatusCheckData()
		{
			
			var StaffId = _user.StaffId;
			var result = await _APIHelper.CallApiAsyncGet<AttendanceDetail>($"api/Attendance/CheckData{StaffId}", HttpMethod.Get);

			if (result != null)
			{				
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


			
			var result = await _APIHelper.CallApiAsyncGet<List<AttendanceMaster>>($"api/Attendance/GetStaffAttendanceList{_user.StaffId}/{fromdate}/{todate}", HttpMethod.Get);

			if (result != null)
			{
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


		//Get Attendance Data Company Vise 
		public async Task<ActionResult<JsonObject>> GetAttendanceDataCompanyVise(int Staffid, string fromdate, string todate)
		{

			var result = await _APIHelper.CallApiAsyncGet<IEnumerable<AttendanceMaster>>($"api/Attendance/GetStaffAttendanceList{Staffid}/{fromdate}/{todate}", HttpMethod.Get);

			if (result != null)
			{
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

		//Get Mark table Attendance Data Company Vise 
		public async Task<ActionResult<JsonObject>> ListMarkAttendanceData(int DepartmentId,string todate)
		{



			var result = await _APIHelper.CallApiAsyncGet<IEnumerable<AttendanceMaster>>($"api/Attendance/MarkStaffAttendanceList{_user.CompanyId}/{DepartmentId}/{todate}", HttpMethod.Get);

			if (result != null)
			{
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


		//Get Attendance Data Status Counts Staff Vise
		public async Task<ActionResult<JsonObject>> GetAttendanceCount(int staffId)
		{


			var result = await _APIHelper.CallApiAsyncGet<IEnumerable<AttendanceMaster>>($"api/Attendance/SetStaffAttendanceStatus{staffId}", HttpMethod.Get);

			if (result != null)
			{
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


		public async Task<IActionResult> AttentanceOverView(string data = "")
		{

			ViewBag.StaffList = await _APIHelper.CallApiAsyncGet<IEnumerable<Staff>>($"api/Staffs/GetStaffByCompanyId{_user.CompanyId}", HttpMethod.Get);
			ViewBag.ListDepartment = await _APIHelper.CallApiAsyncGet<IEnumerable<Department>>($"api/Configuration/GetDepartmentByCompanyID{_user.CompanyId}", HttpMethod.Get);

			return View();
		}


		//Attendance Over View List Company Wise filter 
		public async Task<ActionResult<JsonObject>> AttendanceOverViewListCompanyWise(int StaffId, int DepartmentId, int monthId, int yearId)
		{

			var result = await _APIHelper.CallApiAsyncGet<IEnumerable<dynamic>>($"api/Attendance/StaffAttendanceOverViewList{StaffId}/{DepartmentId}/{monthId}/{yearId}", HttpMethod.Get);

			if (result != null)
			{
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


        //Attendance Leave Wise List filter
        public async Task<ActionResult<JsonObject>> AttendanceLeaveWiseList(int StaffId, int DepartmentId, string datefrom, string dateTo)
        {

            var result = await _APIHelper.CallApiAsyncGet<IEnumerable<dynamic>>($"api/Attendance/StaffAttendanceLeaveWiseList{StaffId}/{DepartmentId}/{datefrom}/{dateTo}", HttpMethod.Get);

            if (result != null)
            {
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


        public async Task<IActionResult> AttentanceByUser(string data = "")
		{


			ViewBag.StaffList = await _APIHelper.CallApiAsyncGet<IEnumerable<Staff>>($"api/Staffs/GetStaffByCompanyId{_user.CompanyId}", HttpMethod.Get);


			return View();



			
		}



	    public async Task<IActionResult> AttendanceView(string data = "")
		{
			ViewBag.StaffList = await _APIHelper.CallApiAsyncGet<IEnumerable<Staff>>($"api/Staffs/GetStaffByCompanyId{_user.CompanyId}", HttpMethod.Get);
			ViewBag.ListDepartment = await _APIHelper.CallApiAsyncGet<IEnumerable<Department>>($"api/Configuration/GetDepartmentByCompanyID{_user.CompanyId}", HttpMethod.Get);

			return View();
		}
		public async Task<IActionResult> OverviewCalendar(string data = "")
		{
			return View();
		}

		public async Task<IActionResult> MarkAttendance(string data = "")
		{
			ViewBag.ListDepartment = await _APIHelper.CallApiAsyncGet<IEnumerable<Department>>($"api/Configuration/GetDepartmentByCompanyID{_user.CompanyId}", HttpMethod.Get);
			return View();
		}




	}
}
