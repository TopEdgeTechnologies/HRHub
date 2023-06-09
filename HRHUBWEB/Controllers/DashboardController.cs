using HRHUBAPI.Models;
using HRHUBWEB.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace HRHUBWEB.Controllers
{
    public class DashboardController : Controller
    {
        private IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly APIHelper _APIHelper;
        private readonly User _user;

        public DashboardController(IHttpClientFactory httpClientFactory, IWebHostEnvironment webHostEnvironment, APIHelper APIHelper, IHttpContextAccessor httpContextAccessor)
        {
            _webHostEnvironment = webHostEnvironment;
            _APIHelper = APIHelper;
            _httpContextAccessor = httpContextAccessor;
            _user = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
        }
       
        [HttpGet]
		public async Task<IActionResult> DashboardTotals()
		{
			string procrdure = "BI.sp_DashboardTotals";
			object[] parameters = new object[] { _user.CompanyId ?? 0 };
			var result = await _APIHelper.CallApiDynamic<dynamic>(parameters, $"api/Dashboard/GetDashboardData{_user.CompanyId}/{procrdure}", HttpMethod.Get);
			return Json(result);
		}

		[HttpGet]
		public async Task<IActionResult> Announmnetdata()
		{
			string procrdure = "BI.sp_Announmnetdata";
			object[] parameters = new object[] { _user.CompanyId ?? 0 };
			var result = await _APIHelper.CallApiDynamic<dynamic>(parameters, $"api/Dashboard/GetDashboardData{_user.CompanyId}/{procrdure}", HttpMethod.Get);
			return Json(result);
		}


        [HttpGet]
        public async Task<IActionResult> UpcommingEvent( int month)
        {
            string procrdure = "BI.sp_UpcommingEvent";
            object[] parameters = new object[] { month, _user.CompanyId ?? 0 };
            var result = await _APIHelper.CallApiDynamic<dynamic>(parameters, $"api/Dashboard/GetDashboardData{_user.CompanyId}/{procrdure}", HttpMethod.Get);
            return Json(result);
        }



        [HttpGet]
        public async Task<IActionResult> EmployeeGenderWiseCount()
        {
            string procrdure = "bi.sp_employeeGenderWiseCount";
            object[] parameters = new object[] { _user.CompanyId ?? 0 };
            var result = await _APIHelper.CallApiDynamic<dynamic>(parameters, $"api/Dashboard/GetDashboardData{_user.CompanyId}/{procrdure}", HttpMethod.Get);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> EmployeeSalarySummery(int year)
        {
            string procrdure = "BI.sp_salaryYearWiseSumary";
            object[] parameters = new object[] { year, _user.CompanyId??0 };
            var result = await _APIHelper.CallApiDynamic<dynamic>(parameters, $"api/Dashboard/GetDashboardData{_user.CompanyId}/{procrdure}", HttpMethod.Get);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> StaffDailyAttendance(DateTime date)
        {
            string procrdure = "BI.sp_staffDailyAttendance";
            object[] parameters = new object[] { "'"+date.ToString("dd-MMM-yyyy")+"'", _user.CompanyId ?? 0 };
         
            var result = await _APIHelper.CallApiDynamic<dynamic>(parameters, $"api/Dashboard/GetDashboardData{_user.CompanyId}/{procrdure}", HttpMethod.Get);
            return Json(result);
        }


        public async Task<IActionResult> HR()
        {

			
			return View();
        }


    }
}
