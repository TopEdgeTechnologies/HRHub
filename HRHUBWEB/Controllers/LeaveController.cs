using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.ObjectModelRemoting;
using Newtonsoft.Json;
using NuGet.Common;
using HRHUBAPI.Models;
using HRHUBWEB.Extensions;
using HRHUBWEB.Filters;
using HRHUBWEB.Models;

using System.Net.Http.Headers;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Hosting;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Microsoft.AspNetCore.Http;

namespace HRHUBWEB.Controllers
{
    public class LeaveController : Controller
    {
        private readonly HttpClient _client;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly APIHelper _APIHelper;
        private readonly User _user;
        public LeaveController(IHttpClientFactory httpClient, IWebHostEnvironment webHostEnvironment, APIHelper APIHelper, IHttpContextAccessor httpContextAccessor)
        {
            _client = httpClient.CreateClient("APIClient");
            _webHostEnvironment = webHostEnvironment;
            _APIHelper = APIHelper;
            _httpContextAccessor = httpContextAccessor;
            _user = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
        }

        #region LeaveInfo
        [CustomAuthorization]
        public async Task<IActionResult> LeaveList(string data = "")
        {
            try
            {
                ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
                ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
                ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
                ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);

                ViewBag.Success = data;
                Leave ObjLeave = new Leave();
                var CompanyId = _user.CompanyId;
                var StaffId = _user.StaffId;

                ViewBag.StaffID = StaffId;
                ObjLeave.ListAllleaves = await _APIHelper.CallApiAsyncGet<IEnumerable<Leave>>($"api/Leave/GetLeaveInfos{CompanyId}/{StaffId}", HttpMethod.Get);
                ObjLeave.ListleaveStatus = await _APIHelper.CallApiAsyncGet<IEnumerable<LeaveStatus>>("api/Leave/GetLeaveStatusInfos", HttpMethod.Get);
                ObjLeave.ListleaveTypes = await _APIHelper.CallApiAsyncGet<IEnumerable<LeaveType>>($"api/Configuration/GetStaffWiseLeaveTypeInfos{StaffId}", HttpMethod.Get);

                return View(ObjLeave);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IActionResult> LeaveDetails(int id)
        {
            try
            {
                Leave ObjLeave = await GetLeavebyID(id);
                return View(ObjLeave);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task<Leave> GetLeavebyID(int id)
        {
            try
            {
                Leave ObjLeave = new Leave();
                ObjLeave = await _APIHelper.CallApiAsyncGet<Leave>($"api/Leave/GetLeaveInfoId{id}", HttpMethod.Get);
                ObjLeave.ListleaveTypes = await _APIHelper.CallApiAsyncGet<IEnumerable<LeaveType>>($"api/Configuration/GetStaffWiseLeaveTypeInfos{_user.StaffId}", HttpMethod.Get);

                return ObjLeave;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<IActionResult> LeaveCreateOrUpdate(int id)
        {
            try
            {
                var CompanyId = _user.CompanyId;
                var StaffId = _user.StaffId;
                Leave Info = new Leave();
                Info.ListleaveTypes = await _APIHelper.CallApiAsyncGet<IEnumerable<LeaveType>>($"api/Configuration/GetStaffWiseLeaveTypeInfos{StaffId}", HttpMethod.Get);

                if (id == 0)
                {
                    return View(Info);
                }
                Info = await GetLeavebyID(id);
                return View(Info);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> LeaveCreateOrUpdate(Leave ObjLeave)
        {
            try
            {
                ObjLeave.StaffId = _user.StaffId;
                ObjLeave.AppliedOn = DateTime.Now;
                ObjLeave.LeaveStatusId = 1; // New
                ObjLeave.CreatedBy = _user.UserId;
                ObjLeave.IsDeleted = false;

                var model = await _APIHelper.CallApiAsyncPost<Response>(ObjLeave, "api/Leave/LeaveAddOrCreate", HttpMethod.Post);

                if (model.Message.Contains("Insert"))
                {
                    return RedirectToAction("LeaveList", new { data = 1 });
                }
                else
                {
                    return RedirectToAction("LeaveList", new { data = 2 });
                }
            }
            catch (Exception)
            {

                return View();
            }
        }
        public async Task<IActionResult> LeaveDelete(int id)
        {
            try
            {
                var model = await _APIHelper.CallApiAsyncGet<Response>($"api/Leave/DeleteLeaveInfo{id}/{_user.UserId}", HttpMethod.Get);
                return RedirectToAction("LeaveList", new { data = 3 });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ActionResult<JsonObject>> CheckLeaveCountOnApply(int leavetypeid)
        {
            try
            {
                if (leavetypeid == 0) { return Json(null); }

                ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
                ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
                ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
                ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);

                var staffid = _user.StaffId;
                var result = await _APIHelper.CallApiAsyncGet<decimal>($"api/Leave/LeaveCheckData/{staffid}/{leavetypeid}", HttpMethod.Get);

                return Json(result);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IActionResult> GetLeaveChartData()
        {
            try
            {
                IEnumerable<LeaveType> leavetype;
                leavetype = await _APIHelper.CallApiAsyncGet<IEnumerable<LeaveType>>($"api/Configuration/GetStaffWiseLeaveTypeInfos{_user.StaffId}", HttpMethod.Get);
                return Json(leavetype);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<IActionResult> GetLeaveApprovalSettingData()
        {
            try
            {
                LeaveApprovalSetting leavetype = new LeaveApprovalSetting();
                leavetype = await _APIHelper.CallApiAsyncGet<LeaveApprovalSetting>($"api/Leave/GetLeaveApprovalSettingInfos{_user.CompanyId}", HttpMethod.Get);
                return Json(leavetype);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<IActionResult> ViewLeaveDetail(int id)
        {
            try
            {
                IEnumerable<Leave> leave;
                leave = await _APIHelper.CallApiAsyncGet<IEnumerable<Leave>>($"api/Leave/GetLeaveDetailInfoId{id}", HttpMethod.Get);
                return Json(leave);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        // Load filter vise data from database
        [HttpPost]
        public async Task<IActionResult> SearchList(int StaffId, int LeaveTypeId, int LeaveStatusId, DateTime Month, bool DateFilter)
        {
            var leaves = await _APIHelper.CallApiAsyncGet<IEnumerable<Leave>>($"api/Leave/SearchAllLeaves{_user.CompanyId}/{StaffId}/{LeaveTypeId}/{LeaveStatusId}/{Month}/{DateFilter}", HttpMethod.Get);

            return Json(new
            {
                success = true,
                ListLeaves = leaves,

            });

        }

        #endregion


        #region HRLeaveApplicationInfo

        public async Task<IActionResult> HRLeaveList(string data = "")
        {
            try
            {

                ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
                ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
                ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
                ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);

                ViewBag.Success = data;

                Leave ObjLeave = new Leave();
                ViewBag.Staffs = await _APIHelper.CallApiAsyncGet<IEnumerable<Staff>>($"api/Staffs/GetStaffByCompanyId{_user.CompanyId}", HttpMethod.Get);
                ObjLeave.ListAllleaves = await _APIHelper.CallApiAsyncGet<IEnumerable<Leave>>($"api/Leave/GetHRLeaveInfos{_user.CompanyId}/{_user.StaffId}", HttpMethod.Get);
                ObjLeave.ListleaveStatus = await _APIHelper.CallApiAsyncGet<IEnumerable<LeaveStatus>>("api/Leave/GetLeaveStatusInfos", HttpMethod.Get);
                // ObjLeave.ListleaveTypes = await _APIHelper.CallApiAsyncGet<IEnumerable<LeaveType>>($"api/Configuration/GetStaffWiseLeaveTypeInfos{StaffId}", HttpMethod.Get);
                ObjLeave.ListleaveTypes = await _APIHelper.CallApiAsyncGet<IEnumerable<LeaveType>>($"api/Configuration/GetLeaveTypeInfos{_user.CompanyId}", HttpMethod.Get);

                return View(ObjLeave);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IActionResult> HRRecentLeaves(string data = "")
        {
            try
            {
                ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
                ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
                ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
                ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);

                ViewBag.Success = data;

                Leave ObjLeave = new Leave();
                ViewBag.Staffs = await _APIHelper.CallApiAsyncGet<IEnumerable<Staff>>($"api/Staffs/GetStaffByCompanyId{_user.CompanyId}", HttpMethod.Get);
                ObjLeave.ListAllleaves = await _APIHelper.CallApiAsyncGet<IEnumerable<Leave>>($"api/Leave/GetHRLeaveInfos{_user.CompanyId}/{_user.StaffId}", HttpMethod.Get);

                return View(ObjLeave);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IActionResult> SaveLeaveApprovalData(int id, int staffid, int leavetypeid, int leavestatusid, string remarks, DateTime startdate, DateTime enddate, bool markasshortleave, bool markashalfleave)
        {
            try
            {
                LeaveApproval obj = new LeaveApproval();
                obj.LeaveId = id;
                obj.ApplicantStaffId = staffid;
                obj.LeaveTypeId = leavetypeid;
                obj.LeaveStatusId = leavestatusid;
                obj.Remarks = remarks;
                obj.StartDate = startdate;
                obj.EndDate = enddate;
                obj.MarkAsHalfLeave = markashalfleave;
                obj.MarkAsShortLeave = markasshortleave;
                obj.ApprovalByStaffId = _user.StaffId;
                obj.ApprovedByDesignationID = _user.DesignationID;
                obj.ApprovalDate = DateTime.Now;
                obj.CreatedBy = _user.UserId;

                var setting = await _APIHelper.CallApiAsyncGet<LeaveApprovalSetting>($"api/Leave/GetLeaveApprovalSettingInfos{_user.CompanyId}", HttpMethod.Get);
                // Extract the column name from the response
                obj.FinalApprovalDesignationID = setting.FinalApprovalByDesignationId;

                var response = await _APIHelper.CallApiAsyncPost<Response>(obj, "api/Leave/SaveLeaveApprovalDetail", HttpMethod.Post);
                return Json(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> ForwardLeaveToStaff(Leave ObjLeave)
        {
            try
            {
                ObjLeave.ForwardByStaffID = _user.StaffId;
                var model = await _APIHelper.CallApiAsyncPost<Response>(ObjLeave, "api/Leave/SaveForwardLeaveDetail", HttpMethod.Post);

                if (model.Message.Contains("Insert"))
                {
                    return RedirectToAction("HRLeaveList", new { data = 1 });
                }
                else
                {
                    return RedirectToAction("HRLeaveList", new { data = 2 });
                }
            }
            catch (Exception)
            {

                return View();
            }
        }

        public async Task<IActionResult> GetLeaveApprovalComments(int id, int approvalstatusid)
        {
            try
            {
                var loginstaffid = _user.StaffId;
                var obj = await _APIHelper.CallApiAsyncGet<LeaveApproval>($"api/Leave/GetStaffLeaveApprovalComments{id}/{approvalstatusid}/{loginstaffid}", HttpMethod.Get);
                return Json(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IActionResult> LeaveApproval(int id, int staffid)
        {
            try
            {
                var CompanyId = _user.CompanyId;
                var StaffId = staffid;

                ViewBag.ApprovalBtnVisibility = true;
                if (staffid == _user.StaffId)
                {
                    ViewBag.ApprovalBtnVisibility = false;
                }

                var setting = await _APIHelper.CallApiAsyncGet<LeaveApprovalSetting>($"api/Leave/GetLeaveApprovalSettingInfos{CompanyId}", HttpMethod.Get);
                // Extract the column name from the response
                var FinalApprovalDesignationID = setting.FinalApprovalByDesignationId;

                ViewBag.ForwardBtnVisibility = false;
                if (FinalApprovalDesignationID == _user.DesignationID)
                {
                    ViewBag.ForwardBtnVisibility = true;
                }

                ViewBag.Staffs = await _APIHelper.CallApiAsyncGet<IEnumerable<Staff>>($"api/Staffs/GetStaffByCompanyId{_user.CompanyId}", HttpMethod.Get);

                Leave ObjLeave = await GetLeavebyID(id);
                ObjLeave.ListLeaveApprovalData = await _APIHelper.CallApiAsyncGet<IEnumerable<LeaveApproval>>($"api/Leave/GetLeaveApprovalByLeaveId{id}", HttpMethod.Get);
                ObjLeave.ListleaveTypes = await _APIHelper.CallApiAsyncGet<IEnumerable<LeaveType>>($"api/Configuration/GetStaffWiseLeaveTypeInfos{StaffId}", HttpMethod.Get);

                return View(ObjLeave);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        // Code for save images into database

        private string uploadImage(string name, IFormFile file, string root)
        {

            try
            {
                string fileName = string.Empty;
                if (file != null)
                {
                    var fileExtension = Path.GetExtension(file.FileName);
                    fileName = name + "-" + DateTime.Now.Ticks + fileExtension;
                    var filepath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", root, fileName);

                    var OldpathImage = filepath;
                    if (System.IO.File.Exists(OldpathImage))
                    {
                        System.IO.File.Delete(OldpathImage);
                    }


                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return "/Images/" + root + "/" + fileName;    // Path.GetFullPath( filepath);// @"/Images/" + root + "/" + fileName;
                }
                else
                {

                    return "";
                }
            }
            catch (Exception ex)
            {

                throw;
            }


        }

    }
}
