using HRHUBAPI.Models;
using HRHUBWEB.Extensions;
using HRHUBWEB.Filters;
using HRHUBWEB.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.ComponentModel.Design;

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

        #region HR Dashboard

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
        
            [HttpGet]
            public async Task<IActionResult> StaffLeaves(DateTime leavedate)
            {
                string procrdure = "sp_DashBoardStaffLeaves";
                object[] parameters = new object[] { "'" + leavedate.ToString("dd-MMM-yyyy") + "'", _user.CompanyId ?? 0, _user.StaffId ?? 0 };

                var result = await _APIHelper.CallApiDynamic<dynamic>(parameters, $"api/Dashboard/GetDashboardData{_user.CompanyId}/{procrdure}", HttpMethod.Get);
                return Json(result);
            }

            [CustomAuthorization]
            public IActionResult  HR()
            {
			
			    return View();
            }

		#endregion

		#region Staff Dashboard

		[HttpGet]
		public async Task<IActionResult> StaffMonthlyAttendance(DateTime currentDate)
		{
			string procrdure = "BI.GetStaff_MonthlyAttendance_BetweenDate";
			object[] parameters = new object[] { _user.CompanyId ?? 0, "'" + currentDate.ToString("dd-MMM-yyyy") + "'", _user.UserId };

			var result = await _APIHelper.CallApiDynamic<dynamic>(parameters, $"api/Dashboard/GetDashboardData{_user.CompanyId}/{procrdure}", HttpMethod.Get);
			return Json(result);
		}

		[HttpGet]
		public async Task<IActionResult> Staff_YTDAttendance(DateTime currentDate)
		{
			string procrdure = "BI.GetStaff_YTDAttendance";
			object[] parameters = new object[] { _user.CompanyId ?? 0, "'" + currentDate.ToString("dd-MMM-yyyy") + "'", _user.UserId };

			var result = await _APIHelper.CallApiDynamic<dynamic>(parameters, $"api/Dashboard/GetDashboardData{_user.CompanyId}/{procrdure}", HttpMethod.Get);
			return Json(result);
		}
        		
		[HttpGet]
		public async Task<IActionResult> Staff_UpComingHolidays(DateTime currentDate)
		{
			string procrdure = "BI.GetStaff_UpComingHolidays";
			object[] parameters = new object[] { _user.CompanyId ?? 0, "'" + currentDate.ToString("dd-MMM-yyyy") + "'" };

			var result = await _APIHelper.CallApiDynamic<dynamic>(parameters, $"api/Dashboard/GetDashboardData{_user.CompanyId}/{procrdure}", HttpMethod.Get);
			return Json(result);
		}

        public async Task<Leave> GetleaveTypeList()
        {
            Leave ObjLeave = new Leave();
            if (_user.CompanyId > 0)
            {
                ObjLeave.ListleaveTypes = await _APIHelper.CallApiAsyncGet<IEnumerable<LeaveType>>($"api/Configuration/GetLeaveTypeInfos{_user.CompanyId}", HttpMethod.Get);
                return ObjLeave;
            }
            return new Leave();
        }
		
        public async Task<IActionResult> LeaveCreateOrUpdate(int leaveTypeId, DateTime startDate, DateTime endDate, string leaveSubject, bool markAsHalfLeave, bool markAsShortLeave)
		{
            Leave ObjLeave = new Leave();
            ObjLeave.AppliedOn = DateTime.Now;
            ObjLeave.StaffId = _user.UserId;

            ObjLeave.LeaveStatusId = leaveTypeId;
            ObjLeave.StartDate = startDate; 
            ObjLeave.EndDate = endDate;
			ObjLeave.ApplicationHtml = "<p>" + leaveSubject + "</p>";
			ObjLeave.LeaveSubject = leaveSubject;
            ObjLeave.MarkAsHalfLeave = markAsHalfLeave;
            ObjLeave.MarkAsShortLeave = markAsShortLeave;
			ObjLeave.IsDeleted = false;
			ObjLeave.CreatedBy = _user.UserId;

			var result = await _APIHelper.CallApiAsyncPost<Response>(ObjLeave, "api/Leave/LeaveAddOrCreate", HttpMethod.Post);

            //if (result.Message.Contains("Insert"))
            //{
            //    return new RedirectToAction("StaffDashboard", new { data = 1 });
            //}
			//else
			//{
			//	return RedirectToAction("SalaryMethodList", new { data = 2 });
			//}
            //return new RedirectToActionResult("Dashboard", "StaffDashboard", result);    
			return Json(result);
		}

		//[HttpGet]
		//public async Task<IActionResult> StaffMonthlyAttendance(DateTime dateFrom, DateTime dateTo, int staffId)
		//{
		//	string functionName = "Payroll.GetStaffSalaryCardList_BetweenDates";
		//	object[] parameters = new object[] { _user.CompanyId ?? 0, "'" + dateFrom.ToString("dd-MMM-yyyy") + "'", "'" + dateTo.ToString("dd-MMM-yyyy") + "'", staffId };

		//	var result = await _APIHelper.CallApiDynamic<dynamic>(parameters, $"api/Dashboard/GetDashboardDataFunction/{_user.CompanyId}/{functionName}", HttpMethod.Get);
		//	return Json(result);
		//}

		[CustomAuthorization]
        public IActionResult StaffDashboard()
        {

            return View();
        }

        #endregion

    }
}
