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
    public class StaffOffBoardingController : Controller
    {
        private readonly HttpClient _client;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly APIHelper _APIHelper;
        private readonly User _user;
        public StaffOffBoardingController(IHttpClientFactory httpClient, IWebHostEnvironment webHostEnvironment, APIHelper APIHelper, IHttpContextAccessor httpContextAccessor)
        {
            _client = httpClient.CreateClient("APIClient");
            _webHostEnvironment = webHostEnvironment;
            _APIHelper = APIHelper;
            _httpContextAccessor = httpContextAccessor;
            _user = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");

        }

        #region StaffOffBoardingInfo
        [CustomAuthorization]
        public async Task<IActionResult> StaffOffBoardingList(string data = "")
        {
            try
            {
                ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
                ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
                ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
                ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);

                StaffOffBoarding ObjStaffOffBoarding = new StaffOffBoarding();
                var CompanyId = _user.CompanyId;
                var StaffId = _user.StaffId;

                ObjStaffOffBoarding.ListAllStaffOffBoardings = await _APIHelper.CallApiAsyncGet<IEnumerable<StaffOffBoarding>>($"api/StaffOffBoarding/GetStaffOffBoardingInfos{CompanyId}", HttpMethod.Get);
                ObjStaffOffBoarding.ListDepartments = await _APIHelper.CallApiAsyncGet<IEnumerable<Department>>($"api/Configuration/GetDepartmentByCompanyID{CompanyId}", HttpMethod.Get);
                ObjStaffOffBoarding.ListDesignations = await _APIHelper.CallApiAsyncGet<IEnumerable<Designation>>($"api/Configuration/GetDesignationInfos{CompanyId}", HttpMethod.Get);

                return View(ObjStaffOffBoarding);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IActionResult> OffBoardingCommentsScreen(int id)
        {
            try
            {
                ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
                ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
                ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
                ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);

                StaffOffBoarding ObjStaffOffBoarding = new StaffOffBoarding();
                var CompanyId = _user.CompanyId;
                var LoginStaffId = _user.StaffId;

                ObjStaffOffBoarding = await _APIHelper.CallApiAsyncGet<StaffOffBoarding>($"api/StaffOffBoarding/GetStaffOffBoardingInfoId{id}", HttpMethod.Get);
                ObjStaffOffBoarding.ListClearenceProcess = await _APIHelper.CallApiAsyncGet<IEnumerable<StaffOffBoarding>>($"api/StaffOffBoarding/GetStaffClearenceProcessData{id}", HttpMethod.Get);
                ObjStaffOffBoarding.ListClearenceDepartments = await _APIHelper.CallApiAsyncGet<IEnumerable<StaffOffBoarding>>($"api/StaffOffBoarding/GetClearenceDepartmentByCompanyID{CompanyId}/{id}/{LoginStaffId}", HttpMethod.Get);
                
                return View(ObjStaffOffBoarding);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public async Task<IActionResult> SaveStaffClearenceProcess(IFormCollection MyAttachment, StaffOffBoarding obj)
        {
            ClearenceProcess CP = new ClearenceProcess();

            var Attachmentfile = MyAttachment.Files.GetFile("LogoAttachmentFile");
            CP.Attachment = uploadImage(obj.StaffName, Attachmentfile, "ClearenceAttachment");

            CP.OffBoardingId = obj.OffBoardingId;
            CP.RemarksFromStaffId = _user.StaffId;
            CP.ProcessDate = DateTime.Now;
            CP.Remarks = obj.Remarks;

            var result = await _APIHelper.CallApiAsyncPost<Response>(CP, "api/StaffOffBoarding/SaveClearenceProcess", HttpMethod.Post);

            if (result.Message.Contains("Insert"))
            {
                return RedirectToAction("OffBoardingCommentsScreen", new { id = obj.OffBoardingId, data = 1 });
            }
            else
            {
                return RedirectToAction("OffBoardingCommentsScreen", new { id = obj.OffBoardingId, data = 2 });
            }
        }
        [HttpPost]
        public async Task<IActionResult> SaveExitInterviewDetail(StaffOffBoarding obj)
        {
            obj.InterviewDoneByStaffId = _user.StaffId;
            obj.UpdatedBy = _user.UserId;
            var result = await _APIHelper.CallApiAsyncPost<Response>(obj, "api/StaffOffBoarding/SaveInterviewDetail", HttpMethod.Post);

            if (result.Message.Contains("Insert"))
            {
                return RedirectToAction("OffBoardingCommentsScreen", new { id = obj.OffBoardingId,data = 1 });
            }
            else
            {
                return RedirectToAction("OffBoardingCommentsScreen", new { id = obj.OffBoardingId,data = 2 });
            }
        }
        public async Task<IActionResult> StaffOffBoardingDetails(int id)
        {
            try
            {
                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                if (Token != null)
                {

                    StaffOffBoarding ObjStaffOffBoarding = await GetStaffOffBoardingbyID(id);

                    return View(ObjStaffOffBoarding);
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
        private async Task<StaffOffBoarding> GetStaffOffBoardingbyID(int id)
        {
            try
            {

                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                StaffOffBoarding ObjStaffOffBoarding = new StaffOffBoarding();

                //Get Instutute ID through Sessions
                var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
                var CompanyId = userObject.CompanyId;
                var StaffId = userObject.StaffId;

                HttpResponseMessage message = await _client.GetAsync($"api/StaffOffBoarding/GetStaffOffBoardingInfoId{id}"); //
                if (message.IsSuccessStatusCode)
                {
                    var result = message.Content.ReadAsStringAsync().Result;
                    ObjStaffOffBoarding = JsonConvert.DeserializeObject<StaffOffBoarding>(result);

                }
                //HttpResponseMessage message1 = await _client.GetAsync($"api/Configuration/GetStaffWiseStaffOffBoardingTypeInfos{StaffId}");
                //if (message1.IsSuccessStatusCode)
                //{
                //    var result = message1.Content.ReadAsStringAsync().Result;
                //    ObjStaffOffBoarding.ListStaffOffBoardingTypes = JsonConvert.DeserializeObject<List<StaffOffBoardingType>>(result);

                //}


                return ObjStaffOffBoarding;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<IActionResult> StaffOffBoardingCreateOrUpdate(int id)
        {
            try
            {

                StaffOffBoarding objInfo = new StaffOffBoarding();
                var CompanyId = _user.CompanyId;
                var StaffId = _user.StaffId;

                objInfo.ListOffBoardingTypes = await _APIHelper.CallApiAsyncGet<IEnumerable<OffBoardingType>>("api/StaffOffBoarding/GetOffBoardingTypeInfos", HttpMethod.Get);
                
                objInfo.ListStaffs = await _APIHelper.CallApiAsyncGet<IEnumerable<Staff>>($"api/Staffs/GetStaffByCompanyId{CompanyId}", HttpMethod.Get);

                if (id == 0)
                {
                    return View(objInfo);
                }
               // objInfo = await GetStaffOffBoardingbyID(id);
                return View(objInfo);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> StaffOffBoardingCreateOrUpdate(StaffOffBoarding ObjStaffOffBoarding)
        {
            try
            {
                ObjStaffOffBoarding.ApplicationDate = DateTime.Now; ;
                ObjStaffOffBoarding.CreatedBy = _user.UserId;
                ObjStaffOffBoarding.IsDeleted = false;
                var result = await _APIHelper.CallApiAsyncPost<Response>(ObjStaffOffBoarding, "api/StaffOffBoarding/StaffOffBoardingAddOrCreate", HttpMethod.Post);

                if (result.Message.Contains("Insert"))
                {
                    return RedirectToAction("StaffOffBoardingCreateOrUpdate", new { data = 1 });
                }
                else
                {
                    return RedirectToAction("StaffOffBoardingCreateOrUpdate", new { data = 2 });

                }


            }
            catch (Exception)
            {

                return View();
            }
        }
      

        // Load filter vise data from database
        [HttpPost]
        public async Task<IActionResult> SearchList(int DepartmentId, int DesignationId, DateTime Date)
        {

            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
            var CompanyId = userObject.CompanyId;



            StaffOffBoarding obj = new StaffOffBoarding();

            HttpResponseMessage message = await _client.GetAsync($"api/StaffOffBoarding/SearchAllStaffOffBoardings{CompanyId}/{DepartmentId}/{DesignationId}/{Date}");
            if (message.IsSuccessStatusCode)
            {
                var result1 = message.Content.ReadAsStringAsync().Result;
                var StaffOffBoardings = JsonConvert.DeserializeObject<List<StaffOffBoarding>>(result1);
               
                return Json(new
                {
                    success = true,
                    ListStaffOffBoardings = StaffOffBoardings,

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
































        //        #region StaffOffBoardingSubject
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


        //            List<ViewStaffOffBoardingClassSubject> StaffOffBoardingSubject = new List<ViewStaffOffBoardingClassSubject>();

        //            HttpResponseMessage message = await _client.GetAsync("api/StaffOffBoarding/GetStaffOffBoardingSubjects");
        //            if (message.IsSuccessStatusCode)
        //            {
        //                var result = message.Content.ReadAsStringAsync().Result;
        //                StaffOffBoardingSubject = JsonConvert.DeserializeObject<List<ViewStaffOffBoardingClassSubject>>(result);

        //            }
        //            else
        //            {
        //                return RedirectToAction("Loginpage", "User",  new {id=2 });
        //            }

        //            return View(StaffOffBoardingSubject);
        //        }
        //        public async Task<IActionResult> SubjectDetails(int id)
        //        {
        //            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
        //            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);


        //            if (Token != null) { 
        //            StaffOffBoardingClassSubject StaffOffBoardingSubject = await GetSubjectbyID(id);

        //            return View(StaffOffBoardingSubject);
        //             }
        //            else
        //            {
        //                return RedirectToAction("Loginpage", "User",  new {id=2 });

        //            }
        //}
        //        private async Task<StaffOffBoardingClassSubject> GetSubjectbyID(int id)
        //        {
        //            StaffOffBoardingClassSubject StaffOffBoardingSubject = new StaffOffBoardingClassSubject();
        //            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
        //            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        //            HttpResponseMessage message = await _client.GetAsync($"api/StaffOffBoarding/GetStaffOffBoardingSubjectId{id}");
        //            if (message.IsSuccessStatusCode)
        //            {
        //                var result = message.Content.ReadAsStringAsync().Result;
        //                StaffOffBoardingSubject = JsonConvert.DeserializeObject<StaffOffBoardingClassSubject>(result);

        //            }

        //            return StaffOffBoardingSubject;
        //        }
        //        [HttpGet]
        //        public async Task<IActionResult> StaffOffBoardingSubject(int id)
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
        //                StaffOffBoardingClassSubject Info = new StaffOffBoardingClassSubject();
        //                return View(Info);
        //            }
        //            StaffOffBoardingClassSubject StaffOffBoardinginfo = await GetSubjectbyID(id);

        //            return View(StaffOffBoardinginfo);
        //            }
        //            else
        //            {
        //                return RedirectToAction("Loginpage", "User",  new {id=2 });
        //            }

        //        }
        //        [HttpPost]
        //        public async Task<IActionResult> StaffOffBoardingSubject(List<StaffOffBoardingClassSubject> rows)
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

        //                HttpResponseMessage message = await _client.PostAsJsonAsync("api/StaffOffBoarding/StaffOffBoardingSubjectAddOrCreate", rows);

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
        //        public async Task<IActionResult> DeleteStaffOffBoardingSubjectByClassId(int id)
        //        {
        //            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
        //            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

        //            HttpResponseMessage message = await _client.DeleteAsync($"api/StaffOffBoarding/DeleteStaffOffBoardingClassSubjectByClassId{id}");
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

        //            HttpResponseMessage message = await _client.GetAsync($"api/StaffOffBoarding/GetSubjectLanguageId{id}");
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
        //            HttpResponseMessage message = await _client.GetAsync($"api/StaffOffBoarding/GetClassByLevelId{id}");
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
        //            HttpResponseMessage message = await _client.GetAsync($"api/StaffOffBoarding/GetSubjectByGroupId{id}/{groupId}");
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

        //        //load StaffOffBoardingsubject data in dropdown by Class ID
        //        public async Task<IActionResult> LoadDataByClassId(int id)
        //        {
        //            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
        //            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

        //            List<StaffOffBoardingClassSubject> cc = new List<StaffOffBoardingClassSubject>();
        //            HttpResponseMessage message = await _client.GetAsync($"api/StaffOffBoarding/LoadDataByClassId{id}");
        //            if (message.IsSuccessStatusCode)
        //            {
        //                var result = message.Content.ReadAsStringAsync().Result;
        //                cc = JsonConvert.DeserializeObject<List<StaffOffBoardingClassSubject>>(result);

        //            }
        //            else
        //            {
        //                return RedirectToAction("Loginpage", "User",  new {id=2 });
        //            }

        //            return Json(cc);

        //        }

        //        //Delete single row Data from table       
        //        public async Task<IActionResult> DeleteStaffOffBoardingSubject(int id)
        //        {
        //            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
        //            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

        //            List<StaffOffBoardingClassSubject> cc = new List<StaffOffBoardingClassSubject>();
        //            HttpResponseMessage message = await _client.GetAsync($"api/StaffOffBoarding/DeleteStaffOffBoardingSubject{id}");
        //            if (message.IsSuccessStatusCode)
        //            {
        //                var result = message.Content.ReadAsStringAsync().Result;
        //                cc = JsonConvert.DeserializeObject<List<StaffOffBoardingClassSubject>>(result);

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
