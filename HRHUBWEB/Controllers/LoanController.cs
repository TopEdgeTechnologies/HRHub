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
    public class LoanController : Controller
    {
        private readonly HttpClient _client;
        private IWebHostEnvironment _webHostEnvironment;
        public LoanController(IHttpClientFactory httpClient, IWebHostEnvironment webHostEnvironment)
        {
            _client = httpClient.CreateClient("APIClient");
            _webHostEnvironment = webHostEnvironment;
        }

        #region LoanInfo
        [CustomAuthorization]
        public async Task<IActionResult> LoanList(string data = "")
        {
            try
            {
                ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
                ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
                ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
                ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);


                ViewBag.Success = data;
                LoanApplication Obj = new LoanApplication();
                var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
                var CompanyId = userObject.CompanyId;
                var StaffId = userObject.StaffId;
                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                if (Token != null)
                {

                    HttpResponseMessage message = await _client.GetAsync($"api/PayrollConfiguration/GetLoanInfos{CompanyId}/{StaffId}");
                    if (message.IsSuccessStatusCode)
                    {
                        var result = message.Content.ReadAsStringAsync().Result;
                        Obj.ListLoanData = JsonConvert.DeserializeObject<List<LoanApplication>>(result);
                        ViewBag.StaffID = StaffId;
                    }

                    HttpResponseMessage message1 = await _client.GetAsync($"api/PayrollConfiguration/GetLoanStatusInfos");
                    if (message1.IsSuccessStatusCode)
                    {
                        var result = message1.Content.ReadAsStringAsync().Result;
                        Obj.ListLoanStatus = JsonConvert.DeserializeObject<List<LoanStatus>>(result);

                    }

                    HttpResponseMessage message2 = await _client.GetAsync($"api/Configuration/GetLoanTypeInfos{CompanyId}");
                    if (message2.IsSuccessStatusCode)
                    {
                        var result = message2.Content.ReadAsStringAsync().Result;
                        Obj.ListLoanTypes = JsonConvert.DeserializeObject<List<LoanType>>(result);

                    }


                }
                else
                {
                    return RedirectToAction("Loginpage", "User", new { id = 2 });
                }
                return View(Obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public async Task<IActionResult> LoanDetails(int id)
        //{
        //    try
        //    {
        //        var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
        //        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

        //        if (Token != null)
        //        {

        //            LoanApplication ObjLeave = await GetLoanById(id);

        //            return View(ObjLeave);
        //        }
        //        else
        //        {
        //            return RedirectToAction("Loginpage", "User", new { id = 2 });

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        private async Task<LoanApplication> GetLoanDetailById(int id)
        {
            try
            {

                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                LoanApplication Obj = new LoanApplication();

                //Get Instutute ID through Sessions
                var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
                var CompanyId = userObject.CompanyId;
                var StaffId = userObject.StaffId;

                HttpResponseMessage message = await _client.GetAsync($"api/PayrollConfiguration/GetLoanInfoId{id}");
                if (message.IsSuccessStatusCode)
                {
                    var result = message.Content.ReadAsStringAsync().Result;
                    Obj = JsonConvert.DeserializeObject<LoanApplication>(result);

                }
                HttpResponseMessage message1 = await _client.GetAsync($"api/Configuration/GetLoanTypeInfos{CompanyId}");
                if (message1.IsSuccessStatusCode)
                {
                    var result = message1.Content.ReadAsStringAsync().Result;
                    Obj.ListLoanTypes = JsonConvert.DeserializeObject<List<LoanType>>(result);

                }


                return Obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> GetLoanById(int id)
        {
            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            LoanApplication Loan= new LoanApplication();
            var response = await _client.GetAsync($"api/PayrollConfiguration/GetLoanInfoId{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Loan = JsonConvert.DeserializeObject<LoanApplication>(content);
                return Json(Loan);
            }
            else
            {

                return Json(null);
            }


        }

        //[HttpGet]
        //public async Task<IActionResult> LoanCreateOrUpdate(int id)
        //{
        //    try
        //    {
        //        var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
        //        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

        //        //Get Instutute ID through Sessions
        //        var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
        //        var CompanyId = userObject.CompanyId;
        //        var StaffId = userObject.StaffId;

        //        if (Token != null)
        //        {
        //            LoanApplication Info = new LoanApplication();
        //            HttpResponseMessage message = await _client.GetAsync($"api/Configuration/GetLoanTypeInfos{CompanyId}");
        //            if (message.IsSuccessStatusCode)
        //            {
        //                var result = message.Content.ReadAsStringAsync().Result;
        //                Info.ListLoanTypes = JsonConvert.DeserializeObject<List<LoanType>>(result);

        //            }
                   

        //            if (id == 0)
        //            {
        //                return View(Info);
        //            }
        //            Info = await GetLoanById(id);
        //            return View(Info);
        //        }
        //        else
        //        {
        //            return RedirectToAction("Loginpage", "User", new { id = 2 });

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        [HttpPost]
        public async Task<IActionResult> LoanCreateOrUpdate(LoanApplication ObjLeave)
        {
            try
            {
                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
                ObjLeave.StaffId = userObject.StaffId;
                ObjLeave.ApplicationDate = DateTime.Now;
               // ObjLeave.LoanStatusId = 1; // New
                ObjLeave.CreatedBy = userObject.UserId;
                ObjLeave.IsDeleted = false;
                HttpResponseMessage message = await _client.PostAsJsonAsync("api/PayrollConfiguration/LoanAddOrCreate", ObjLeave);

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

                    return RedirectToAction("LoanList", new { data = status });

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
        public async Task<IActionResult> LoanDelete(int id)
        {
            try
            {
                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");

                HttpResponseMessage message = await _client.DeleteAsync($"api/PayrollConfiguration/DeleteLoanInfo{id}/{userObject.UserId}");
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

                    return RedirectToAction("LoanList", new { data = status });

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
        public async Task<IActionResult> GetLoanChartData()
        {
            try
            {
                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                //Get Instutute ID through Sessions
                var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
                var StaffId = userObject.StaffId;


                List<LoanType> loantype = new List<LoanType>();
                var response = await _client.GetAsync($"api/PayrollConfiguration/GetStaffLoanRemainingAmount{StaffId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    loantype = JsonConvert.DeserializeObject<List<LoanType>>(content);
                    return Json(loantype);
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


        // Load filter wise data from database
        [HttpPost]
        public async Task<IActionResult> SearchList(int StaffId, int LoanTypeId, int LoanStatusId, DateTime ApplicationDateFrom, DateTime ApplicationDateTo)
        {

            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
            var CompanyId = userObject.CompanyId;

            HttpResponseMessage message = await _client.GetAsync($"api/PayrollConfiguration/SearchAllLoanData{CompanyId}/{StaffId}/{LoanTypeId}/{LoanStatusId}/{ApplicationDateFrom}/{ApplicationDateTo}");
            if (message.IsSuccessStatusCode)
            {
                var result1 = message.Content.ReadAsStringAsync().Result;
                var loan = JsonConvert.DeserializeObject<List<LoanApplication>>(result1);

                return Json(new
                {
                    success = true,
                    ListLoans = loan,

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


        #region HRLoanInfo

        public async Task<IActionResult> HRLoanList(string data = "")
        {
            try
            {


                ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
                ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
                ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
                ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);


                ViewBag.Success = data;
                LoanApplication Obj = new LoanApplication();
                var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
                var CompanyId = userObject.CompanyId;
                var StaffId = userObject.StaffId;
                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                if (Token != null)
                {
                    HttpResponseMessage message = await _client.GetAsync($"api/Staffs/GetStaffByCompanyId{CompanyId}");
                    if (message.IsSuccessStatusCode)
                    {
                        var result = message.Content.ReadAsStringAsync().Result;
                        ViewBag.Staffs = JsonConvert.DeserializeObject<List<Staff>>(result);

                    }

                    HttpResponseMessage message1 = await _client.GetAsync($"api/PayrollConfiguration/GetLoanInfos{CompanyId}/{StaffId}");
                    if (message1.IsSuccessStatusCode)
                    {
                        var result = message1.Content.ReadAsStringAsync().Result;
                        Obj.ListLoanData = JsonConvert.DeserializeObject<List<LoanApplication>>(result);

                    }
                    HttpResponseMessage message2 = await _client.GetAsync($"api/PayrollConfiguration/GetLoanStatusInfos");
                    if (message2.IsSuccessStatusCode)
                    {
                        var result = message2.Content.ReadAsStringAsync().Result;
                        Obj.ListLoanStatus = JsonConvert.DeserializeObject<List<LoanStatus>>(result);

                    }

                    HttpResponseMessage message3 = await _client.GetAsync($"api/Configuration/GetLoanTypeInfos{CompanyId}");
                    if (message3.IsSuccessStatusCode)
                    {
                        var result = message3.Content.ReadAsStringAsync().Result;
                        Obj.ListLoanTypes = JsonConvert.DeserializeObject<List<LoanType>>(result);

                    }

                }
                else
                {
                    return RedirectToAction("Loginpage", "User", new { id = 2 });
                }

                return View(Obj);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IActionResult> SaveLoanApprovalData(int id, int loanstatusid, string remarks, DateTime PaymentDate)
        {
            try
            {
                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
                var CompanyId = userObject.CompanyId;

                LoanApplicationProcess obj = new LoanApplicationProcess();
                obj.LoanApplicationId = id;
                obj.LoanStatusId = loanstatusid;
                if (loanstatusid == 2)
                {
                    obj.PaymentDate = null;
                }
                else
                {
                    obj.PaymentDate = PaymentDate;
                }
                obj.Remarks = remarks;
                obj.ApprovedByStaffId = userObject.StaffId;
                obj.IsDelete = false;
                obj.ProcessDate = DateTime.Now;
                var response = await _client.PostAsJsonAsync("api/PayrollConfiguration/SaveLoanApprovalDetail", obj);

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
        public async Task<IActionResult> ForwardLoanToStaff(LoanApplication Obj)
        {
            try
            {
                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");

                HttpResponseMessage message = await _client.PostAsJsonAsync("api/PayrollConfiguration/SaveForwardLoanDetail", Obj);

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

                    return RedirectToAction("HRLoanList", new { data = status });

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


        public async Task<IActionResult> LoanApproval(int id, int staffid)
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

                    LoanApplication ObjLeave = await GetLoanDetailById(id);

                    HttpResponseMessage message2 = await _client.GetAsync($"api/PayrollConfiguration/GetLoanApprovalByLoanId{id}"); //
                    if (message2.IsSuccessStatusCode)
                    {
                        var result = message2.Content.ReadAsStringAsync().Result;
                        ObjLeave.ListLoanApprovalData = JsonConvert.DeserializeObject<List<LoanApplicationProcess>>(result);

                    }

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
