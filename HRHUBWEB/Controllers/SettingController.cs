﻿using HRHUBAPI.Models;
using HRHUBWEB.Extensions;
using HRHUBWEB.Filters;
using HRHUBWEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.Design;
using System.Net.Http.Headers;

namespace HRHUBWEB.Controllers
{
    public class SettingController : Controller
    {
        private readonly HttpClient _client;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly APIHelper _APIHelper;
        private readonly User _user;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SettingController(IHttpClientFactory httpClient, IWebHostEnvironment webHostEnvironment, APIHelper APIHelper, IHttpContextAccessor httpContextAccessor)
        {
            _client = httpClient.CreateClient("APIClient");
            _webHostEnvironment = webHostEnvironment;
            _APIHelper = APIHelper;
            _httpContextAccessor = httpContextAccessor;
            _user = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
        }
        public async Task<IActionResult> GeneralSettings(string data = "", int Id = 0)
        {
            ViewBag.Success = data;

            ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
            ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
            ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
            ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);

            return View();
        }
        public async Task<IActionResult> GetPolicyDescription(int Id)
        {
            try
            {
                var obj = await _APIHelper.CallApiAsyncGet<Policy>($"api/Policy/GetsPolicyById{Id}", HttpMethod.Get);
                return Json(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public async Task<IActionResult> GetPoliciesByCompanyId(int PolicyCategoryId)
        {
            try
            {
                var CompanyId = _user.CompanyId;
                var list = await _APIHelper.CallApiAsyncGet<IEnumerable<Policy>>($"api/Policy/GetPoliciesByCompanyId{CompanyId}/{PolicyCategoryId}/", HttpMethod.Get);
                return Json(list);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public async Task<IActionResult> UpdatePolicyConfigurationStatus(int id, bool status)
        {

            PolicyConfiguration ObjPolicyConfiguration = new PolicyConfiguration();
            ObjPolicyConfiguration.PolicyConfigurationId = id;
            ObjPolicyConfiguration.Status = status;
            ObjPolicyConfiguration.UpdatedBy = _user.UserId;

            var result = await _APIHelper.CallApiAsyncPost<Response>(ObjPolicyConfiguration, "api/Policy/UpdateStatusByPolicyConfigurationId", HttpMethod.Post);

            return Json(result);

        }

        #region Attendance Settings
        public async Task<IActionResult> AttendanceSettings(string data = "")
        {
            ViewBag.Success = data;

            ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
            ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
            ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
            ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);

            var CompanyId = _user.CompanyId;
            var PolicyCategoryId = 2; // AttendancePolicyCategoryId

            Company Obj = new Company();
            Obj = await _APIHelper.CallApiAsyncGet<Company>($"api/Company/GetCompanyInfoId{CompanyId}", HttpMethod.Get);

            ViewBag.ListWeekEndRule = await _APIHelper.CallApiAsyncGet<IEnumerable<WeekendRule>>($"api/Configuration/GetWeekendRuleByCompanyID{CompanyId}", HttpMethod.Get);

            ViewBag.ListAttendancePolicy = await _APIHelper.CallApiAsyncGet<IEnumerable<Policy>>($"api/Policy/GetPoliciesByCompanyId{CompanyId}/{PolicyCategoryId}", HttpMethod.Get);

            //ViewBag.AttendancePolicy = await _APIHelper.CallApiAsyncGet<IEnumerable<Policy>>("api/Policy/GetAttendancePolicyInfos", HttpMethod.Get);
            ViewBag.AttendancePolicy = await _APIHelper.CallApiAsyncGet<IEnumerable<Policy>>($"api/Policy/GetPolicybyCategoryId{PolicyCategoryId}", HttpMethod.Get);

            return View(Obj);
        }
        public async Task<IActionResult> AttendancePolicyCreateOrUpdate(int id, string title, int policyId,  bool halfdayafterlateminutes, int lateminutes, bool allowgraceminutes, int graceminutes)   //bool halfleave, bool quarterleave,
        {

            var CompanyId = _user.CompanyId;
            var UserId = _user.UserId;

            var result = await _APIHelper.CallApiAsyncGet<Response>($"api/Policy/PostAttendancePolicyConfiguration{id}/{policyId}/{title}/{CompanyId}/{UserId}/{halfdayafterlateminutes}/{lateminutes}/{allowgraceminutes}/{graceminutes}", HttpMethod.Get);

            return Json(result);

        }
        public async Task<IActionResult> SaveAttendanceSetting(TimeSpan OfficeStartTime, TimeSpan OfficeEndTime, bool AttendanceByRosters, bool EmployeeWebCheckIn, int CompanyId, List<WeekendRule> WeekendList)
        {

            Company obj = new Company();
            obj.OfficeStartTime = OfficeStartTime;
            obj.OfficeEndTime = OfficeEndTime;
            obj.AttendanceByRosters = AttendanceByRosters;
            obj.EmployeeWebCheckIn = EmployeeWebCheckIn;
            obj.WeekendList = WeekendList;
            obj.CompanyId = Convert.ToInt32(_user.CompanyId);
            obj.UserId = _user.UserId.ToString();

            var result = await _APIHelper.CallApiAsyncPost<Response>(obj, "api/Setting/PostAttendanceSetting", HttpMethod.Post);
            return Json(result);

            //if (result.Message.Contains("Insert"))
            //{
            //    return RedirectToAction("AttendanceSettings", new { data = 1 });
            //}
            //else
            //{
            //    return RedirectToAction("AttendanceSettings", new { data = 2 });
            //}
        }
        public async Task<IActionResult> PolicyConfigurationAlreadyExists(int id, int policyId)
        {
            var result = await _APIHelper.CallApiAsyncGet<Response>($"api/Policy/PolicyConfigurationAlreadyExists{_user.CompanyId}/{id}/{policyId}", HttpMethod.Get);
            return Json(result);
        }
        public async Task<IActionResult> GetAttendancePolicyConfigurationById(int Id)
        {
            try
            {
                var obj = await _APIHelper.CallApiAsyncGet<Policy>($"api/Policy/GetsAttendancePolicyConfigurationById{Id}/{_user.CompanyId}", HttpMethod.Get);
                return Json(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IActionResult> GetWeekendRule()
        {
            try
            {
                var CompanyId = _user.CompanyId;
                var list = await _APIHelper.CallApiAsyncGet<IEnumerable<WeekendRule>>($"api/Configuration/GetWeekendRuleByCompanyID{CompanyId}", HttpMethod.Get);
                return Json(list);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<IActionResult> WeekendRuleDelete(int id)
        {
            var UserId = _user.UserId;
            var result = await _APIHelper.CallApiAsyncGet<Response>($"api/Configuration/DeleteWeekendRule{id}/{UserId}", HttpMethod.Get);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePolicyWeekendRuleStatus(int id, bool status)
        {

            WeekendRule ObjWeekendRule = new WeekendRule();
            ObjWeekendRule.WeekendRuleId = id;
            ObjWeekendRule.Status = status;
            ObjWeekendRule.UpdatedBy = _user.UserId;

            var result = await _APIHelper.CallApiAsyncPost<Response>(ObjWeekendRule, "api/Configuration/UpdateStatusByWeekendRuleId", HttpMethod.Post);

            return Json(result);

        }

        #endregion



        #region Leave Settings
        public async Task<IActionResult> LeaveSettings(string data = "")
        {
            ViewBag.Success = data;

            ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
            ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
            ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
            ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);

            var CompanyId = _user.CompanyId;
            var PolicyCategoryId = 1; // LeavePolicyCategoryId

            LeaveApprovalSetting Obj = new LeaveApprovalSetting();
            Obj = await _APIHelper.CallApiAsyncGet<LeaveApprovalSetting>($"api/Setting/GetLeaveSettingByCompanyId{CompanyId}", HttpMethod.Get);

            ViewBag.ListLeaveTypes = await _APIHelper.CallApiAsyncGet<IEnumerable<LeaveType>>($"api/Configuration/GetLeaveTypeInfos{_user.CompanyId}", HttpMethod.Get);

            ViewBag.ListDesignations = await _APIHelper.CallApiAsyncGet<IEnumerable<Designation>>($"api/Configuration/GetDesignationInfos{_user.CompanyId}", HttpMethod.Get);
            ViewBag.ListleaveStatus = await _APIHelper.CallApiAsyncGet<IEnumerable<LeaveStatus>>("api/Leave/GetLeaveStatusInfos", HttpMethod.Get);

            ViewBag.ListLeavePolicy = await _APIHelper.CallApiAsyncGet<IEnumerable<Policy>>($"api/Policy/GetPoliciesByCompanyId{CompanyId}/{PolicyCategoryId}", HttpMethod.Get);

            ViewBag.LeavePolicy = await _APIHelper.CallApiAsyncGet<IEnumerable<Policy>>($"api/Policy/GetPolicybyCategoryId{PolicyCategoryId}", HttpMethod.Get);

            return View(Obj);
        }

        public async Task<IActionResult> LeavePolicyCreateOrUpdate(int id, string title, int policyId, bool halfleave, bool quarterleave)
        {

            var CompanyId = _user.CompanyId;
            var UserId = _user.UserId;

            var result = await _APIHelper.CallApiAsyncGet<Response>($"api/Policy/PostLeavePolicyConfiguration{id}/{policyId}/{title}/{CompanyId}/{UserId}/{halfleave}/{quarterleave}", HttpMethod.Get);

            return Json(result);

        }
        public async Task<IActionResult> GetLeavePolicyConfigurationById(int Id)
        {
            try
            {
                var obj = await _APIHelper.CallApiAsyncGet<Policy>($"api/Policy/GetsLeavePolicyConfigurationById{Id}/{_user.CompanyId}", HttpMethod.Get);
                return Json(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<IActionResult> PostLeaveType(LeaveType obj)
        {

            obj.CompanyId = _user.CompanyId;
            obj.CreatedBy = _user.UserId;

            var result = await _APIHelper.CallApiAsyncPost<Response>(obj,"api/Configuration/LeaveTypeAddOrUpdate", HttpMethod.Post);

            return Json(result);

        }
        public async Task<IActionResult> GetLeaveTypeByCompanyId()
        {
            try
            {
                var CompanyId = _user.CompanyId;
                var list = await _APIHelper.CallApiAsyncGet<IEnumerable<LeaveType>>($"api/Configuration/GetLeaveTypeInfos{CompanyId}", HttpMethod.Get);
                return Json(list);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<IActionResult> LeaveTypeDelete(int id)
        {
            var result = await _APIHelper.CallApiAsyncGet<Response>($"api/Setting/DeleteLeaveType{id}", HttpMethod.Get);
            return Json(result);
        }

        public async Task<IActionResult> SaveLeaveSetting(LeaveApprovalSetting obj)
        {
            obj.CompanyId = Convert.ToInt32(_user.CompanyId);
            obj.CreatedBy = _user.UserId;

            var result = await _APIHelper.CallApiAsyncPost<Response>(obj, "api/Setting/PostLeaveSetting", HttpMethod.Post);
            return Json(result);
        }

        #endregion



        #region NotificationSetting

        public async Task<IActionResult> NotificationSettings(string data = "")
        {
            ViewBag.Success = data;

            ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
            ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
            ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
            ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);

           EmailNotificationSetting objEmail = new EmailNotificationSetting();
         
            ViewBag.ListEmailTemplate = await _APIHelper.CallApiAsyncGet<EmailTemplate>($"api/Setting/GetEmailTemplateByCompanyId{_user.CompanyId}", HttpMethod.Get);
            //LeaveApprovalSetting Obj = new LeaveApprovalSetting();
            //Obj = await _APIHelper.CallApiAsyncGet<LeaveApprovalSetting>($"api/Setting/GetLeaveSettingByCompanyId{CompanyId}", HttpMethod.Get);

            //ViewBag.ListLeaveTypes = await _APIHelper.CallApiAsyncGet<IEnumerable<LeaveType>>($"api/Configuration/GetLeaveTypeInfos{_user.CompanyId}", HttpMethod.Get);

            //ViewBag.ListDesignations = await _APIHelper.CallApiAsyncGet<IEnumerable<Designation>>($"api/Configuration/GetDesignationInfos{_user.CompanyId}", HttpMethod.Get);
            //ViewBag.ListleaveStatus = await _APIHelper.CallApiAsyncGet<IEnumerable<LeaveStatus>>("api/Leave/GetLeaveStatusInfos", HttpMethod.Get);

            //ViewBag.ListLeavePolicy = await _APIHelper.CallApiAsyncGet<IEnumerable<Policy>>($"api/Policy/GetPoliciesByCompanyId{CompanyId}/{PolicyCategoryId}", HttpMethod.Get);

            //ViewBag.LeavePolicy = await _APIHelper.CallApiAsyncGet<IEnumerable<Policy>>($"api/Policy/GetPolicybyCategoryId{PolicyCategoryId}", HttpMethod.Get);

            return View(objEmail);
        }




        #endregion







        public async Task<IActionResult> PayrollSettings(string data = "", int Id = 0)
        {
            ViewBag.Success = data;

            ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
            ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
            ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
            ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);

            ViewBag.ListPolicy = await _APIHelper.CallApiAsyncGet<IEnumerable<Policy>>("api/Policy/GetPolicyInfos", HttpMethod.Get);

            return View();
        }
    }
}
