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
                var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
                var CompanyId = userObject.CompanyId;
                var StaffId = userObject.StaffId;
                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                if (Token != null)
                {

                    HttpResponseMessage message = await _client.GetAsync($"api/Leave/GetLeaveInfos{CompanyId}/{StaffId}");
                    if (message.IsSuccessStatusCode)
                    {
                        var result = message.Content.ReadAsStringAsync().Result;
                        ObjLeave.ListAllleaves = JsonConvert.DeserializeObject<List<Leave>>(result);
                        ViewBag.StaffID = StaffId;
                    }

                    HttpResponseMessage message1 = await _client.GetAsync($"api/Leave/GetLeaveStatusInfos");
                    if (message1.IsSuccessStatusCode)
                    {
                        var result = message1.Content.ReadAsStringAsync().Result;
                        ObjLeave.ListleaveStatus = JsonConvert.DeserializeObject<List<LeaveStatus>>(result);

                    }

                    HttpResponseMessage message2 = await _client.GetAsync($"api/Configuration/GetStaffWiseLeaveTypeInfos{StaffId}");
                    if (message2.IsSuccessStatusCode)
                    {
                        var result = message2.Content.ReadAsStringAsync().Result;
                        ObjLeave.ListleaveTypes = JsonConvert.DeserializeObject<List<LeaveType>>(result);

                    }


                }
                else
                {
                    return RedirectToAction("Loginpage", "User", new { id = 2 });
                }
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
                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                if (Token != null)
                {

                    Leave ObjLeave = await GetLeavebyID(id);

                    return View(ObjLeave);
                }
                else
                {
                    return RedirectToAction("Loginpage", "User", new { id = 2 });

                }
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

                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                Leave ObjLeave = new Leave();

                //Get Instutute ID through Sessions
                var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
                var CompanyId = userObject.CompanyId;
                var StaffId = userObject.StaffId;

                HttpResponseMessage message = await _client.GetAsync($"api/Leave/GetLeaveInfoId{id}"); //
                if (message.IsSuccessStatusCode)
                {
                    var result = message.Content.ReadAsStringAsync().Result;
                    ObjLeave = JsonConvert.DeserializeObject<Leave>(result);

                }
                HttpResponseMessage message1 = await _client.GetAsync($"api/Configuration/GetStaffWiseLeaveTypeInfos{StaffId}");
                if (message1.IsSuccessStatusCode)
                {
                    var result = message1.Content.ReadAsStringAsync().Result;
                    ObjLeave.ListleaveTypes = JsonConvert.DeserializeObject<List<LeaveType>>(result);

                }


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
                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                //Get Instutute ID through Sessions
                var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
                var CompanyId = userObject.CompanyId;
                var StaffId = userObject.StaffId;

                if (Token != null)
                {
                    Leave Info = new Leave();
                    HttpResponseMessage message = await _client.GetAsync($"api/Configuration/GetStaffWiseLeaveTypeInfos{StaffId}");
                    if (message.IsSuccessStatusCode)
                    {
                        var result = message.Content.ReadAsStringAsync().Result;
                        Info.ListleaveTypes = JsonConvert.DeserializeObject<List<LeaveType>>(result);

                    }


                    if (id == 0)
                    {
                        return View(Info);
                    }
                    Info = await GetLeavebyID(id);
                    return View(Info);
                }
                else
                {
                    return RedirectToAction("Loginpage", "User", new { id = 2 });

                }
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
                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
                ObjLeave.StaffId = userObject.StaffId;
                ObjLeave.AppliedOn = DateTime.Now;
                ObjLeave.LeaveStatusId = 1; // New
                ObjLeave.CreatedBy = userObject.UserId;
                ObjLeave.IsDeleted = false;
                //ObjLeave.InstituteId = userObject.InstituteId;
                HttpResponseMessage message = await _client.PostAsJsonAsync("api/Leave/LeaveAddOrCreate", ObjLeave);

                if (message.IsSuccessStatusCode)
                {

                    var body = message.Content.ReadAsStringAsync();


                    var model = JsonConvert.DeserializeObject<Response>(body.Result);


                    int status = 0;
                    if (model.Success)
                    {


                        if (model.Message.Contains("Insert"))
                        {
                            status = 1;
                        }
                        else if (model.Message.Contains("Update"))
                        {
                            status = 2;
                        }


                    }

                    return RedirectToAction("LeaveList", new { data = status });

                }
                else
                {
                    return RedirectToAction("Loginpage", "User", new { id = 2 });
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
                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                HttpResponseMessage message = await _client.DeleteAsync($"api/Leave/DeleteLeaveInfo{id}");
                if (message.IsSuccessStatusCode)
                {
                    var body = message.Content.ReadAsStringAsync();

                    var model = JsonConvert.DeserializeObject<Response>(body.Result);


                    int status = 0;
                    if (model.Success)
                    {


                        if (model.Message.Contains("Delete"))
                        {
                            status = 3;
                        }



                    }

                    return RedirectToAction("LeaveList", new { data = status });

                }

                else
                {
                    return RedirectToAction("Loginpage", "User", new { id = 2 });
                }
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

                ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
                ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
                ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
                ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);

                var staffid = _user.StaffId;
                var result = await _APIHelper.CallApiAsyncGet<decimal>($"api/Leave/LeaveCheckData{staffid}/{leavetypeid}", HttpMethod.Get);

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
                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                //Get Instutute ID through Sessions
                var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
                var StaffId = userObject.StaffId;


                List<LeaveType> leavetype = new List<LeaveType>();
                var response = await _client.GetAsync($"api/Configuration/GetStaffWiseLeaveTypeInfos{StaffId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    leavetype = JsonConvert.DeserializeObject<List<LeaveType>>(content);
                    return Json(leavetype);
                }
                else
                {

                    return Json(null);
                }
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
                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                //Get Instutute ID through Sessions
                var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
                var CompanyId = userObject.CompanyId;


                LeaveApprovalSetting leavetype = new LeaveApprovalSetting();
                var response = await _client.GetAsync($"api/Leave/GetLeaveApprovalSettingInfos{CompanyId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    leavetype = JsonConvert.DeserializeObject<LeaveApprovalSetting>(content);
                    return Json(leavetype);
                }
                else
                {

                    return Json(null);
                }
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
                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                List<Leave> leave = new List<Leave>();
                var response = await _client.GetAsync($"api/Leave/GetLeaveDetailInfoId{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    leave = JsonConvert.DeserializeObject<List<Leave>>(content);
                    return Json(leave);
                }
                else
                {

                    return Json(null);
                }
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

            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
            var CompanyId = userObject.CompanyId;
            Leave obj = new Leave();
            HttpResponseMessage message = await _client.GetAsync($"api/Leave/SearchAllLeaves{CompanyId}/{StaffId}/{LeaveTypeId}/{LeaveStatusId}/{Month}/{DateFilter}");
            if (message.IsSuccessStatusCode)
            {
                var result1 = message.Content.ReadAsStringAsync().Result;
                var leaves = JsonConvert.DeserializeObject<List<Leave>>(result1);
                //objCandidate.UrlRequestSatausID = id;

                return Json(new
                {
                    success = true,
                    ListLeaves = leaves,

                });

            }

            else
            {
                return Json(new
                {
                    success = false,
                    Listcandidate = "",

                });
            }






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

                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
                var CompanyId = userObject.CompanyId;
                var StaffId = userObject.StaffId;

                if (Token != null)
                {
                    HttpResponseMessage message = await _client.GetAsync($"api/Staffs/GetStaffByCompanyId{CompanyId}");
                    if (message.IsSuccessStatusCode)
                    {
                        var result = message.Content.ReadAsStringAsync().Result;
                        ViewBag.Staffs = JsonConvert.DeserializeObject<List<Staff>>(result);

                    }

                    HttpResponseMessage message1 = await _client.GetAsync($"api/Leave/GetHRLeaveInfos{CompanyId}/{StaffId}");
                    if (message1.IsSuccessStatusCode)
                    {
                        var result = message1.Content.ReadAsStringAsync().Result;
                        ObjLeave.ListAllleaves = JsonConvert.DeserializeObject<List<Leave>>(result);

                    }

                    HttpResponseMessage message3 = await _client.GetAsync($"api/Leave/GetLeaveStatusInfos");
                    if (message3.IsSuccessStatusCode)
                    {
                        var result = message3.Content.ReadAsStringAsync().Result;
                        ObjLeave.ListleaveStatus = JsonConvert.DeserializeObject<List<LeaveStatus>>(result);

                    }

                    HttpResponseMessage message2 = await _client.GetAsync($"api/Configuration/GetStaffWiseLeaveTypeInfos{StaffId}");
                    if (message2.IsSuccessStatusCode)
                    {
                        var result = message2.Content.ReadAsStringAsync().Result;
                        ObjLeave.ListleaveTypes = JsonConvert.DeserializeObject<List<LeaveType>>(result);

                    }



                    //HttpResponseMessage message2 = await _client.GetAsync($"api/Leave/GetNewOrPendingLeaveInfos{CompanyId}");
                    //if (message2.IsSuccessStatusCode)
                    //{
                    //    var result = message2.Content.ReadAsStringAsync().Result;
                    //    ObjLeave.ListNewOrPendingleaves = JsonConvert.DeserializeObject<List<Leave>>(result);

                    //}
                }
                else
                {
                    return RedirectToAction("Loginpage", "User", new { id = 2 });
                }

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

                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
                var CompanyId = userObject.CompanyId;
                var StaffId = userObject.StaffId;

                if (Token != null)
                {
                    HttpResponseMessage message = await _client.GetAsync($"api/Staffs/GetStaffByCompanyId{CompanyId}"); // Staffs must be gotten from Staff api
                    if (message.IsSuccessStatusCode)
                    {
                        var result = message.Content.ReadAsStringAsync().Result;
                        ViewBag.Staffs = JsonConvert.DeserializeObject<List<Staff>>(result);

                    }

                    HttpResponseMessage message1 = await _client.GetAsync($"api/Leave/GetHRLeaveInfos{CompanyId}/{StaffId}");
                    if (message1.IsSuccessStatusCode)
                    {
                        var result = message1.Content.ReadAsStringAsync().Result;
                        ObjLeave.ListAllleaves = JsonConvert.DeserializeObject<List<Leave>>(result);

                    }

                    //HttpResponseMessage message1 = await _client.GetAsync($"api/Leave/GetNewOrPendingLeaveInfos{CompanyId}");
                    //if (message1.IsSuccessStatusCode)
                    //{
                    //    var result = message1.Content.ReadAsStringAsync().Result;
                    //    ObjLeave.ListNewOrPendingleaves = JsonConvert.DeserializeObject<List<Leave>>(result);

                    //}
                }
                else
                {
                    return RedirectToAction("Loginpage", "User", new { id = 2 });
                }


                return View(ObjLeave);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IActionResult> SaveLeaveApprovalData(int id,int staffid,int leavetypeid, int leavestatusid, string remarks , DateTime startdate, DateTime enddate, bool markasshortleave, bool markashalfleave)
        {
            try
            {
                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
                var CompanyId = userObject.CompanyId;

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
                obj.ApprovalByStaffId = userObject.StaffId;
                obj.ApprovedByDesignationID = userObject.DesignationID;
                obj.ApprovalDate = DateTime.Now;
                obj.CreatedBy = userObject.UserId;

                var setting= await _APIHelper.CallApiAsyncGet<LeaveApprovalSetting>($"api/Leave/GetLeaveApprovalSettingInfos{CompanyId}", HttpMethod.Get);
                // Extract the column name from the response
                obj.FinalApprovalDesignationID = setting.FinalApprovalByDesignationId;

                var response = await _client.PostAsJsonAsync("api/Leave/SaveLeaveApprovalDetail", obj);

                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    return Json(content);
                }
                return RedirectToAction("Loginpage", "User", new { id = 2 });
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
                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
                ObjLeave.ForwardByStaffID = userObject.StaffId;

                HttpResponseMessage message = await _client.PostAsJsonAsync("api/Leave/SaveForwardLeaveDetail", ObjLeave);

                if (message.IsSuccessStatusCode)
                {

                    var body = message.Content.ReadAsStringAsync();


                    var model = JsonConvert.DeserializeObject<Response>(body.Result);


                    int status = 0;
                    if (model.Success)
                    {


                        if (model.Message.Contains("Insert"))
                        {
                            status = 1;
                        }
                        else if (model.Message.Contains("Update"))
                        {
                            status = 2;
                        }


                    }

                    return RedirectToAction("HRLeaveList", new { data = status });

                }
                else
                {
                    return RedirectToAction("Loginpage", "User", new { id = 2 });
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
                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
                var CompanyId = userObject.CompanyId;
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

                if (Token != null)
                {
                    HttpResponseMessage message1 = await _client.GetAsync($"api/Staffs/GetStaffByCompanyId{CompanyId}"); // Staffs must be gotten from Staff api
                    if (message1.IsSuccessStatusCode)
                    {
                        var result = message1.Content.ReadAsStringAsync().Result;
                        ViewBag.Staffs = JsonConvert.DeserializeObject<List<Staff>>(result);

                    }

                    Leave ObjLeave = await GetLeavebyID(id);

                    HttpResponseMessage message2 = await _client.GetAsync($"api/Leave/GetLeaveApprovalByLeaveId{id}"); //
                    if (message2.IsSuccessStatusCode)
                    {
                        var result = message2.Content.ReadAsStringAsync().Result;
                        ObjLeave.ListLeaveApprovalData = JsonConvert.DeserializeObject<List<LeaveApproval>>(result);

                    }
                    HttpResponseMessage message = await _client.GetAsync($"api/Configuration/GetStaffWiseLeaveTypeInfos{StaffId}");
                    if (message.IsSuccessStatusCode)
                    {
                        var result = message.Content.ReadAsStringAsync().Result;
                        //ViewBag.LeaveTypes = JsonConvert.DeserializeObject<List<LeaveType>>(result);
                        ObjLeave.ListleaveTypes = JsonConvert.DeserializeObject<List<LeaveType>>(result);
                    }

                    //HttpResponseMessage message = await _client.GetAsync($"api/Configuration/GetLeaveTypeInfos{CompanyId}");
                    //if (message.IsSuccessStatusCode)
                    //{
                    //    var result = message.Content.ReadAsStringAsync().Result;
                    //    //ViewBag.LeaveTypes = JsonConvert.DeserializeObject<List<LeaveType>>(result);
                    //    ObjLeave.ListleaveTypes = JsonConvert.DeserializeObject<List<LeaveType>>(result);
                    //}

                    return View(ObjLeave);
                }
                else
                {
                    return RedirectToAction("Loginpage", "User", new { id = 2 });

                }
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
