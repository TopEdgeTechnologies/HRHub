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

namespace HRHUBWEB.Controllers
{
    public class LeaveController : Controller
    {
        private readonly HttpClient _client;
        private IWebHostEnvironment _webHostEnvironment;
        public LeaveController(IHttpClientFactory httpClient, IWebHostEnvironment webHostEnvironment)
        {
            _client = httpClient.CreateClient("APIClient");
            _webHostEnvironment = webHostEnvironment;
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
                List<Leave> ObjLeave = new List<Leave>();
                var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
                var CompanyId = userObject.CompanyId;
                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                if (Token != null)
                {

                    HttpResponseMessage message = await _client.GetAsync($"api/Leave/GetLeaveInfos{CompanyId}");
                    if (message.IsSuccessStatusCode)
                    {
                        var result = message.Content.ReadAsStringAsync().Result;
                        ObjLeave = JsonConvert.DeserializeObject<List<Leave>>(result);

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

                HttpResponseMessage message = await _client.GetAsync($"api/Leave/GetLeaveInfoId{id}"); //
                if (message.IsSuccessStatusCode)
                {
                    var result = message.Content.ReadAsStringAsync().Result;
                    ObjLeave = JsonConvert.DeserializeObject<Leave>(result);

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

        public async Task<ActionResult<JsonObject>> LeaveCheckData(int leavetypeid)
        {
            try
            {
                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
                var staffid = userObject.StaffId;

                HttpResponseMessage message = await _client.GetAsync($"api/Leave/LeaveCheckData{staffid}/{leavetypeid}");
                if (message.IsSuccessStatusCode)
                {
                    var result = message.Content.ReadAsStringAsync().Result;
                    return Json(result);

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

                if (Token != null)
                {
                    HttpResponseMessage message = await _client.GetAsync($"api/Staffs/GetStaffByCompanyId{CompanyId}"); // Staffs must be gotten from Staff api
                    if (message.IsSuccessStatusCode)
                    {
                        var result = message.Content.ReadAsStringAsync().Result;
                        ViewBag.Staffs = JsonConvert.DeserializeObject<List<Staff>>(result);

                    }

                    HttpResponseMessage message1 = await _client.GetAsync($"api/Leave/GetLeaveInfos{CompanyId}");
                    if (message1.IsSuccessStatusCode)
                    {
                        var result = message1.Content.ReadAsStringAsync().Result;
                        ObjLeave.ListAllleaves = JsonConvert.DeserializeObject<List<Leave>>(result);

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

                if (Token != null)
                {
                    HttpResponseMessage message = await _client.GetAsync($"api/Staffs/GetStaffByCompanyId{CompanyId}"); // Staffs must be gotten from Staff api
                    if (message.IsSuccessStatusCode)
                    {
                        var result = message.Content.ReadAsStringAsync().Result;
                        ViewBag.Staffs = JsonConvert.DeserializeObject<List<Staff>>(result);

                    }

                    HttpResponseMessage message1 = await _client.GetAsync($"api/Leave/GetLeaveInfos{CompanyId}");
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


        public async Task<IActionResult> SaveLeaveApprovalData(int id, int leavestatusid, string remarks)
        {
            try
            {
                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
                var CompanyId = userObject.CompanyId;

                LeaveApproval obj = new LeaveApproval();
                obj.LeaveId = id;
                obj.LeaveStatusId = leavestatusid;
                obj.Remarks = remarks;
                obj.ApprovalByStaffId = userObject.StaffId;
                obj.ApprovalDate = DateTime.Now;
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


        public async Task<IActionResult> LeaveApproval(int id , int staffid)
        {
            try
            {
                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                //Get Instutute ID through Sessions
                var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
                var CompanyId = userObject.CompanyId;
                var StaffId = staffid;

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
































        //        #region LeaveSubject
        //        [CustomAuthorization]
        //        public async Task<IActionResult> SubjectList(string data = "")
        //        {

        //            ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
        //            ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
        //            ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
        //            ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);


        //            ViewBag.Success = data;
        //            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
        //            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);


        //            List<ViewLeaveClassSubject> LeaveSubject = new List<ViewLeaveClassSubject>();

        //            HttpResponseMessage message = await _client.GetAsync("api/Leave/GetLeaveSubjects");
        //            if (message.IsSuccessStatusCode)
        //            {
        //                var result = message.Content.ReadAsStringAsync().Result;
        //                LeaveSubject = JsonConvert.DeserializeObject<List<ViewLeaveClassSubject>>(result);

        //            }
        //            else
        //            {
        //                return RedirectToAction("Loginpage", "User",  new {id=2 });
        //            }

        //            return View(LeaveSubject);
        //        }
        //        public async Task<IActionResult> SubjectDetails(int id)
        //        {
        //            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
        //            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);


        //            if (Token != null) { 
        //            LeaveClassSubject LeaveSubject = await GetSubjectbyID(id);

        //            return View(LeaveSubject);
        //             }
        //            else
        //            {
        //                return RedirectToAction("Loginpage", "User",  new {id=2 });

        //            }
        //}
        //        private async Task<LeaveClassSubject> GetSubjectbyID(int id)
        //        {
        //            LeaveClassSubject LeaveSubject = new LeaveClassSubject();
        //            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
        //            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        //            HttpResponseMessage message = await _client.GetAsync($"api/Leave/GetLeaveSubjectId{id}");
        //            if (message.IsSuccessStatusCode)
        //            {
        //                var result = message.Content.ReadAsStringAsync().Result;
        //                LeaveSubject = JsonConvert.DeserializeObject<LeaveClassSubject>(result);

        //            }

        //            return LeaveSubject;
        //        }
        //        [HttpGet]
        //        public async Task<IActionResult> LeaveSubject(int id)
        //        {

        //            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
        //            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

        //            //Get Instutute ID through Sessions
        //            var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
        //            var InstituteId = userObject.InstituteId;


        //            if (Token !=null) { 


        //            HttpResponseMessage message = await _client.GetAsync($"api/Configuration/GetClassInfos{InstituteId}");
        //            if (message.IsSuccessStatusCode)
        //            {
        //                var result = message.Content.ReadAsStringAsync().Result;
        //                ViewBag.Classlist = JsonConvert.DeserializeObject<List<ClassInfo>>(result);

        //            }

        //            HttpResponseMessage message1 = await _client.GetAsync($"api/Configuration/GetGroupInfos{InstituteId}");
        //            if (message1.IsSuccessStatusCode)
        //            {
        //                var result = message1.Content.ReadAsStringAsync().Result;
        //                ViewBag.Grouplist = JsonConvert.DeserializeObject<List<GroupInfo>>(result);

        //            }

        //            HttpResponseMessage message2 = await _client.GetAsync($"api/Configuration/GetSubjectInfos{InstituteId}");
        //            if (message2.IsSuccessStatusCode)
        //            {
        //                var result = message2.Content.ReadAsStringAsync().Result;
        //                ViewBag.Subjectlist = JsonConvert.DeserializeObject<List<SubjectInfo>>(result);

        //            }

        //            HttpResponseMessage message3 = await _client.GetAsync("api/Configuration/GetLanguage");
        //            if (message3.IsSuccessStatusCode)
        //            {
        //                var result = message3.Content.ReadAsStringAsync().Result;
        //                ViewBag.Languagelist = JsonConvert.DeserializeObject<List<LanguageType>>(result);

        //            }

        //            HttpResponseMessage message4 = await _client.GetAsync($"api/Configuration/GetClassLevels{InstituteId}");
        //            if (message4.IsSuccessStatusCode)
        //            {
        //                var result = message4.Content.ReadAsStringAsync().Result;
        //                ViewBag.LevelList = JsonConvert.DeserializeObject<List<ClassLevel>>(result);

        //            }



        //            if (id == 0)
        //            {
        //                LeaveClassSubject Info = new LeaveClassSubject();
        //                return View(Info);
        //            }
        //            LeaveClassSubject Leaveinfo = await GetSubjectbyID(id);

        //            return View(Leaveinfo);
        //            }
        //            else
        //            {
        //                return RedirectToAction("Loginpage", "User",  new {id=2 });
        //            }

        //        }
        //        [HttpPost]
        //        public async Task<IActionResult> LeaveSubject(List<LeaveClassSubject> rows)
        //        {
        //            try
        //            {
        //                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
        //                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);


        //                var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");

        //                foreach (var item in rows)
        //                {
        //                item.CreateBy = userObject.UserId;
        //                item.InstituteId = userObject.InstituteId;

        //                }

        //                HttpResponseMessage message = await _client.PostAsJsonAsync("api/Leave/LeaveSubjectAddOrCreate", rows);

        //                    if (message.IsSuccessStatusCode)
        //                    {

        //                        var body = message.Content.ReadAsStringAsync();


        //                        var model = JsonConvert.DeserializeObject<Response>(body.Result);


        //                        int status = 0;
        //                        if (model.Success)
        //                        {


        //                            if (model.Message.Contains("Insert"))
        //                            {
        //                                status = 1;
        //                            }
        //                            else if (model.Message.Contains("Update"))
        //                            {
        //                                status = 2;
        //                            }

        //                        TempData["OperationStatus"] = 1;
        //                        }

        //                        return RedirectToAction("SubjectList", new { data = status });

        //                    }
        //                else
        //                {
        //                    return RedirectToAction("Loginpage", "User",  new {id=2 });
        //                }


        //            }
        //            catch (Exception)
        //            {

        //                return View();
        //            }
        //        }
        //        public async Task<IActionResult> DeleteLeaveSubjectByClassId(int id)
        //        {
        //            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
        //            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

        //            HttpResponseMessage message = await _client.DeleteAsync($"api/Leave/DeleteLeaveClassSubjectByClassId{id}");
        //            if (message.IsSuccessStatusCode)
        //            {
        //                var body = message.Content.ReadAsStringAsync();

        //                var model = JsonConvert.DeserializeObject<Response>(body.Result);


        //                int status = 0;
        //                if (model.Success)
        //                {


        //                    if (model.Message.Contains("Delete"))
        //                    {
        //                        status = 3;
        //                    }



        //                }

        //                return RedirectToAction("SubjectList", new { data = status });

        //            }

        //            else
        //            {
        //                return RedirectToAction("Loginpage", "User",  new {id=2 });
        //            }
        //        }






        //        /// <summary>
        //        /// Bind ,Multiples dropdwons here
        //        /// </summary>





        //        //load subject data in dropdown by language ID
        //        public async Task<IActionResult> LoadClassByLanguageId(int id)
        //        {
        //            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
        //            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);


        //            List<SubjectInfo> cc = new List<SubjectInfo>();

        //            HttpResponseMessage message = await _client.GetAsync($"api/Leave/GetSubjectLanguageId{id}");
        //            if (message.IsSuccessStatusCode)
        //            {
        //                var result = message.Content.ReadAsStringAsync().Result;
        //                cc = JsonConvert.DeserializeObject<List<SubjectInfo>>(result);

        //            }
        //            else
        //            {
        //                return RedirectToAction("Loginpage", "User",  new {id=2 });
        //            }

        //            return Json(cc);

        //        }
        //        //load class data in dropdown by level ID
        //        public async Task<IActionResult> LoadClassByLevelId(int id)
        //        {
        //            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
        //            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

        //            List<ClassInfo> cc = new List<ClassInfo>();
        //            HttpResponseMessage message = await _client.GetAsync($"api/Leave/GetClassByLevelId{id}");
        //            if (message.IsSuccessStatusCode)
        //            {
        //                var result = message.Content.ReadAsStringAsync().Result;
        //                cc = JsonConvert.DeserializeObject<List<ClassInfo>>(result);

        //            }
        //            else
        //            {
        //                return RedirectToAction("Loginpage", "User",  new {id=2 });
        //            }

        //            return Json(cc);

        //        }
        //        //load subject data in dropdown by group Id
        //        public async Task<IActionResult> LoadClassByGroupId(int id, int groupId)
        //        {
        //            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
        //            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

        //            List<SubjectInfo> cc = new List<SubjectInfo>();
        //            HttpResponseMessage message = await _client.GetAsync($"api/Leave/GetSubjectByGroupId{id}/{groupId}");
        //            if (message.IsSuccessStatusCode)
        //            {
        //                var result = message.Content.ReadAsStringAsync().Result;
        //                cc = JsonConvert.DeserializeObject<List<SubjectInfo>>(result);

        //            }
        //            else
        //            {
        //                return RedirectToAction("Loginpage", "User",  new {id=2 });
        //            }

        //            return Json(cc);

        //        }

        //        //load Leavesubject data in dropdown by Class ID
        //        public async Task<IActionResult> LoadDataByClassId(int id)
        //        {
        //            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
        //            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

        //            List<LeaveClassSubject> cc = new List<LeaveClassSubject>();
        //            HttpResponseMessage message = await _client.GetAsync($"api/Leave/LoadDataByClassId{id}");
        //            if (message.IsSuccessStatusCode)
        //            {
        //                var result = message.Content.ReadAsStringAsync().Result;
        //                cc = JsonConvert.DeserializeObject<List<LeaveClassSubject>>(result);

        //            }
        //            else
        //            {
        //                return RedirectToAction("Loginpage", "User",  new {id=2 });
        //            }

        //            return Json(cc);

        //        }

        //        //Delete single row Data from table       
        //        public async Task<IActionResult> DeleteLeaveSubject(int id)
        //        {
        //            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
        //            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

        //            List<LeaveClassSubject> cc = new List<LeaveClassSubject>();
        //            HttpResponseMessage message = await _client.GetAsync($"api/Leave/DeleteLeaveSubject{id}");
        //            if (message.IsSuccessStatusCode)
        //            {
        //                var result = message.Content.ReadAsStringAsync().Result;
        //                cc = JsonConvert.DeserializeObject<List<LeaveClassSubject>>(result);

        //            }
        //            else
        //            {
        //                return RedirectToAction("Loginpage", "User",  new {id=2 });
        //            }

        //            return Json(cc);

        //        }


        //        #endregion

    }
}
