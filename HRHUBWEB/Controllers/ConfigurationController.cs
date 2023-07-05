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
using System.ComponentModel.Design;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Net.WebSockets;

namespace HRHUBWEB.Controllers
{
    public class ConfigurationController : Controller
    {
        private readonly HttpClient _client;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly APIHelper _APIHelper;
        private readonly User _user;

        public ConfigurationController(IHttpClientFactory httpClient, IWebHostEnvironment webHostEnvironment, APIHelper APIHelper, IHttpContextAccessor httpContextAccessor)
        {
            _client = httpClient.CreateClient("APIClient");
            _webHostEnvironment = webHostEnvironment;
            _APIHelper = APIHelper;
            _httpContextAccessor = httpContextAccessor;
            _user = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
        }

        #region DesignationInfo
        [CustomAuthorization]
        public async Task<IActionResult> DesignationList(string data = "")
        {

            ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
            ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
            ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
            ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);


            ViewBag.Success = data;
            Designation ObjDesignation = new Designation();
            ObjDesignation.CompanyId = _user.CompanyId;
            ObjDesignation.Status = true;

            ObjDesignation.Listdesignation = await _APIHelper.CallApiAsyncGet<IEnumerable<Designation>>($"api/Configuration/GetDesignationInfos{ObjDesignation.CompanyId}", HttpMethod.Get);

            return View(ObjDesignation);
        }
        public async Task<IActionResult> DesignationDetails(int id)
        {
            var result = await _APIHelper.CallApiAsyncGet<Designation>($"api/Configuration/GetDesignationInfoId{id}", HttpMethod.Get);
            return Json(result);


        }
        [HttpPost]
        public async Task<IActionResult> UpdateDesignationStatus(int id, bool status)
        {

            Designation ObjDesignation = new Designation();
            ObjDesignation.DesignationId = id;
            ObjDesignation.Status = status;
            ObjDesignation.UpdatedBy = _user.UserId;

            var result = await _APIHelper.CallApiAsyncPost<Response>(ObjDesignation, "api/Configuration/UpdateStatusByDesignationId", HttpMethod.Post);

            return Json(result);

        }

        [HttpPost]
        public async Task<IActionResult> DesignationCreateOrUpdate(Designation ObjDesignation)
        {
            try
            {

                ObjDesignation.CompanyId = _user.CompanyId;
                ObjDesignation.CreatedBy = _user.UserId;
                var result = await _APIHelper.CallApiAsyncPost<Response>(ObjDesignation, "api/Configuration/DesignationAddOrUpdate", HttpMethod.Post);

                if (result.Message.Contains("Insert"))
                {
                    return RedirectToAction("DesignationList", new { data = 1 });
                }
                else
                {
                    return RedirectToAction("DesignationList", new { data = 2 });

                }


            }
            catch (Exception)
            {

                return View();
            }
        }
        public async Task<IActionResult> DesignationDelete(int id)
        {

            var result = await _APIHelper.CallApiAsyncGet<Response>($"api/Configuration/DeleteDesignationInfo{id}/{_user.UserId}", HttpMethod.Delete);
            return RedirectToAction("DesignationList", new { data = 3 });

        }

        public async Task<ActionResult<JsonObject>> DesignationCheckData(int id, string title)
        {
            var result = await _APIHelper.CallApiAsyncGet<Response>($"api/Configuration/DesignationCheckData{id}/{title}/{_user.CompanyId}", HttpMethod.Get);
            return Json(result);
        }

        #endregion

        #region Department Info

        [CustomAuthorization]
        public async Task<IActionResult> DepartmentList(string data = "")
        {
            ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
            ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
            ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
            ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);

            ViewBag.Success = data;

            Department departments = new Department();
            departments.CompanyId = _user.CompanyId;
            departments.Listdepartments = await _APIHelper.CallApiAsyncGet<IEnumerable<Department>>($"api/Configuration/GetDepartmentByCompanyID{departments.CompanyId}", HttpMethod.Get);

            return View(departments);
        }

        public async Task<IActionResult> GetDepartmentById(int id)
        {
            Department department = new Department();
            department = await _APIHelper.CallApiAsyncGet<Department>($"api/Configuration/GetDepartmentById{id}", HttpMethod.Get);
            return Json(department);
        }

        //public async Task<IActionResult> GetDepartmentCreateOrUpdate(int id)
        //{         
        //    return RedirectToAction("DepartmentList", new { id = id });  
        //}

        public async Task<IActionResult> DepartmentCreateOrUpdate(IFormCollection MyAttachment, Department department)
        {
            var DepartmentLogo = MyAttachment.Files.GetFile("LogoAttachmentFile");
            department.LogoAttachment = uploadImage(department.Title, DepartmentLogo, "DepartmentImages");

            department.CompanyId = _user.CompanyId;
            department.CreatedBy = _user.UserId;

            var result = await _APIHelper.CallApiAsyncPost<Response>(department, "api/Configuration/PostDepartment", HttpMethod.Post);

            if (result.Message.Contains("Insert"))
            {
                return RedirectToAction("DepartmentList", new { data = 1 });
            }
            else
            {
                return RedirectToAction("DepartmentList", new { data = 2 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDepartmentStatus(int id, bool status)
        {

            Department ObjDepartment = new Department();
            ObjDepartment.DepartmentId = id;
            ObjDepartment.Status = status;
            ObjDepartment.UpdatedBy = _user.UserId;

            var result = await _APIHelper.CallApiAsyncPost<Response>(ObjDepartment, "api/Configuration/UpdateStatusByDepartmentId", HttpMethod.Post);

            return Json(result);

        }

        public async Task<IActionResult> DepartmentDelete(int id)
        {
            var result = await _APIHelper.CallApiAsyncGet<Response>($"api/Configuration/DeleteDepartment{id}/{_user.UserId}", HttpMethod.Get);
            return RedirectToAction("DepartmentList", new { data = 3 });
        }

        public async Task<IActionResult> DepartmentAlreadyExists(int id, string title)
        {
            var result = await _APIHelper.CallApiAsyncGet<Response>($"api/Configuration/DepartmentAlreadyExists{_user.CompanyId}/{id}/{title}", HttpMethod.Get);
            return Json(result);
        }

        #endregion

        #region LeaveTypeInfo

        [CustomAuthorization]
        public async Task<IActionResult> LeaveTypeList(string data = "")
        {
            ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
            ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
            ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
            ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);

            ViewBag.Success = data;

            LeaveType leavetypes = new LeaveType();
            leavetypes.ListLeaveTypes = await _APIHelper.CallApiAsyncGet<IEnumerable<LeaveType>>($"api/Configuration/GetLeaveTypeInfos{_user.CompanyId}", HttpMethod.Get);

            return View(leavetypes);
        }
        public async Task<IActionResult> GetLeaveTypeById(int id)
        {
            LeaveType leavetype = new LeaveType();
            leavetype = await _APIHelper.CallApiAsyncGet<LeaveType>($"api/Configuration/GetLeaveTypeInfoId{id}", HttpMethod.Get);
            return Json(leavetype);
        }

        //[HttpGet]
        //public async Task<IActionResult> LeaveTypeCreateOrUpdate(int id)
        //{
        //    var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
        //    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

        //    //Get Instutute ID through Sessions
        //    var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
        //    ViewBag.CompanyId = userObject.CompanyId;

        //    if (Token != null)
        //    {





        //        if (id == 0)
        //        {
        //            LeaveType Info = new LeaveType();

        //            return View(Info);
        //        }
        //        LeaveType LeaveTypeinfo = await GetLeaveTypebyID(id);

        //        return View(LeaveTypeinfo);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Loginpage", "User", new { id = 2 });

        //    }
        //}
        [HttpPost]
        public async Task<IActionResult> UpdateLeaveTypeStatus(int id, bool status, bool nonpaid, int noofleaves)
        {
            LeaveType ObjLeaveType = new LeaveType();
            ObjLeaveType.LeaveTypeId = id;
            ObjLeaveType.NoOfLeaves = noofleaves;
            ObjLeaveType.Status = status;
            ObjLeaveType.IsNonPaid = nonpaid;
            ObjLeaveType.UpdatedBy = _user.UserId;

            var result = await _APIHelper.CallApiAsyncPost<Response>(ObjLeaveType, "api/Configuration/UpdateStatusByLeaveTypeId", HttpMethod.Post);

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> LeaveTypeCreateOrUpdate(LeaveType ObjLeaveType)
        {
            try
            {
                ObjLeaveType.CompanyId = _user.CompanyId;
                ObjLeaveType.CreatedBy = _user.UserId;

                var result = await _APIHelper.CallApiAsyncPost<Response>(ObjLeaveType, "api/Configuration/LeaveTypeAddOrUpdate", HttpMethod.Post);

                if (result.Message.Contains("Insert"))
                {
                    return RedirectToAction("LeaveTypeList", new { data = 1 });
                }
                else
                {
                    return RedirectToAction("LeaveTypeList", new { data = 2 });
                }
            }
            catch (Exception)
            {

                return View();
            }
        }
        public async Task<IActionResult> LeaveTypeDelete(int id)
        {
            var result = await _APIHelper.CallApiAsyncGet<Response>($"api/Configuration/DeleteLeaveTypeInfo{id}/{_user.UserId}", HttpMethod.Get);
            return RedirectToAction("LeaveTypeList", new { data = 3 });
        }
        public async Task<ActionResult<JsonObject>> LeaveTypeAlreadyExists(int id, string title)
        {
            var result = await _APIHelper.CallApiAsyncGet<Response>($"api/Configuration/LeaveTypeAlreadyExists{id}/{title}/{_user.CompanyId}", HttpMethod.Get);
            return Json(result);

        }

        #endregion

        #region LoanTypeInfo

        [CustomAuthorization]
        public async Task<IActionResult> LoanTypeList(string data = "")
        {
            ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
            ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
            ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
            ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);

            ViewBag.Success = data;

            LoanType LoanTypes = new LoanType();
            LoanTypes.ListLoanTypes = await _APIHelper.CallApiAsyncGet<IEnumerable<LoanType>>($"api/Configuration/GetLoanTypeInfos{_user.CompanyId}", HttpMethod.Get);

            return View(LoanTypes);
        }

        public async Task<IActionResult> GetLoanTypeById(int id)
        {
            LoanType LoanType = new LoanType();
            LoanType = await _APIHelper.CallApiAsyncGet<LoanType>($"api/Configuration/GetLoanTypeInfoId{id}", HttpMethod.Get);
            return Json(LoanType);


        }

        //[HttpGet]
        //public async Task<IActionResult> LoanTypeCreateOrUpdate(int id)
        //{
        //    var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
        //    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

        //    //Get Instutute ID through Sessions
        //    var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
        //    ViewBag.CompanyId = userObject.CompanyId;

        //    if (Token != null)
        //    {





        //        if (id == 0)
        //        {
        //            LoanType Info = new LoanType();

        //            return View(Info);
        //        }
        //        LoanType LoanTypeinfo = await GetLoanTypebyID(id);

        //        return View(LoanTypeinfo);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Loginpage", "User", new { id = 2 });

        //    }
        //}
        [HttpPost]
        public async Task<IActionResult> UpdateLoanTypeStatus(int id, bool status, bool needapproval)
        {
            LoanType ObjLeaveType = new LoanType();
            ObjLeaveType.LoanTypeId = id;
            ObjLeaveType.Status = status;
            ObjLeaveType.IsNeedApproval = needapproval;
            ObjLeaveType.UpdatedBy = _user.UserId;

            var result = await _APIHelper.CallApiAsyncPost<Response>(ObjLeaveType, "api/Configuration/UpdateStatusByLoanTypeId", HttpMethod.Post);

            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> LoanTypeCreateOrUpdate(LoanType ObjLoanType)
        {
            try
            {
                ObjLoanType.CompanyId = _user.CompanyId;
                ObjLoanType.CreatedBy = _user.UserId;

                var result = await _APIHelper.CallApiAsyncPost<Response>(ObjLoanType, "api/Configuration/LoanTypeAddOrUpdate", HttpMethod.Post);

                if (result.Message.Contains("Insert"))
                {
                    return RedirectToAction("LoanTypeList", new { data = 1 });
                }
                else
                {
                    return RedirectToAction("LoanTypeList", new { data = 2 });
                }
            }
            catch (Exception)
            {

                return View();
            }
        }
        public async Task<IActionResult> LoanTypeDelete(int id)
        {
            var result = await _APIHelper.CallApiAsyncGet<Response>($"api/Configuration/DeleteLoanTypeInfo{id}/{_user.UserId}", HttpMethod.Get);
            return RedirectToAction("LoanTypeList", new { data = 3 });
        }

        public async Task<ActionResult<JsonObject>> LoanTypeAlreadyExists(int id, string title)
        {
            var result = await _APIHelper.CallApiAsyncGet<Response>($"api/Configuration/LoanTypeAlreadyExists{id}/{title}/{_user.CompanyId}", HttpMethod.Get);
            return Json(result);
        }

        #endregion

        #region HOliday

        [CustomAuthorization]
        public async Task<IActionResult> HolidayList(string data = "")
        {
            ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
            ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
            ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
            ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);

            ViewBag.Success = data;


            Holiday objHoliday = new Holiday();

            #region Token Authentication & User Data
            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
            #endregion

            objHoliday.CompanyId = userObject.CompanyId;


            if (Token != null)
            {
                HttpResponseMessage response = await _client.GetAsync($"api/Configuration/GetHolidaysByCompanyID{objHoliday.CompanyId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    objHoliday.ListOfHolidays = JsonConvert.DeserializeObject<List<Holiday>>(content);
                }
            }
            else
            {
                return RedirectToAction("Loginpage", "User", new { id = 2 });
            }
            return View(objHoliday);

        }

        public async Task<IActionResult> GetHolidayByID(int id)
        {
            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            Holiday objholiday = new Holiday();

            var response = await _client.GetAsync($"api/Configuration/GetHolidayById{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                objholiday = JsonConvert.DeserializeObject<Holiday>(content);
                return Json(objholiday);
            }
            else
            {

                return Json(null);
            }


        }
        [HttpPost]
        public async Task<IActionResult> UpdateHolidayStatus(int id, bool status)
        {

            Holiday ObjHoliday = new Holiday();
            ObjHoliday.HolidayId = id;
            ObjHoliday.Status = status;
            ObjHoliday.UpdatedBy = _user.UserId;

            var result = await _APIHelper.CallApiAsyncPost<Response>(ObjHoliday, "api/Configuration/UpdateStatusByHolidayId", HttpMethod.Post);

            return Json(result);

        }

        public async Task<IActionResult> HolidayCreateOrUpdate(Holiday objHoliday)
        {
            //token get from session
            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            //

            //user get from session
            var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
            objHoliday.CompanyId = userObject.CompanyId;
            objHoliday.CreatedBy = userObject.UserId;

            HttpResponseMessage response = await _client.PostAsJsonAsync("api/Configuration/PostHoliday", objHoliday);
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Response>(content.Result);


                int status = 0;


                if (result.Success)
                {
                    if (result.Message.Contains("Insert"))
                    {
                        status = 1;
                    }
                    else if (result.Message.Contains("Update"))
                    {
                        status = 2;
                    }
                }
                return RedirectToAction("HOlidayList", new { data = status });

            }
            else
            {
                return RedirectToAction("Loginpage", "User", new { id = 2 });
            }

        }

        public async Task<IActionResult> DeleteHoliday(int id)
        {
            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");

            var response = await _client.GetAsync($"api/Configuration/DeleteHoliday{id}/{userObject.UserId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Response>(content);
                int status = 0;

                if (result.Success)
                {
                    if (result.Message.Contains("Delete"))
                    {
                        status = 3;
                    }
                }
                return RedirectToAction("HOlidayList", new { data = status });
            }
            else
            {
                return RedirectToAction("Loginpage", "User", new { id = 2 });
            }

        }

        public async Task<IActionResult> HolidayAlreadyExists(int id, DateTime HolidayDate)
        {
            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
            var CompanyId = userObject.CompanyId;




            var response = await _client.GetAsync($"api/Configuration/HolidayAlreadyExistsss{CompanyId}/{id}/{HolidayDate.ToString("dd-MMM-yyyy")}");

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                return Json(content);
            }
            return RedirectToAction("Loginpage", "User", new { id = 2 });
        }


        // Filter data through year

        public async Task<ActionResult<JsonObject>> FilterHolidayData(int Yeardate, string selectdate)
        {



            var result = await _APIHelper.CallApiAsyncGet<IEnumerable<Holiday>>($"api/Configuration/FilterHolidayList{_user.CompanyId}/{selectdate}/{Yeardate}", HttpMethod.Get);

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



        // Load calander Event
        [HttpGet]
        public async Task<ActionResult<JsonObject>> LoadCalander(int month, int year)
        {



            var result = await _APIHelper.CallApiAsyncGet<IEnumerable<Holiday>>($"api/Configuration/GetLoadCalanderEvent{_user.CompanyId}/{month}/{year}", HttpMethod.Get);

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





        #endregion

        #region Announcement Info

        [CustomAuthorization]
        public async Task<IActionResult> AnnouncementList(string data = "")
        {
            ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
            ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
            ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
            ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);

            ViewBag.Success = data;

            Announcement Announcements = new Announcement();
            Announcements.CompanyId = _user.CompanyId;
            Announcements.ListAnnouncements = await _APIHelper.CallApiAsyncGet<IEnumerable<Announcement>>($"api/Configuration/GetAnnouncementByCompanyID{Announcements.CompanyId}", HttpMethod.Get);

            return View(Announcements);
        }

        public async Task<IActionResult> GetAnnouncementById(int id)
        {
            Announcement Announcement = new Announcement();
            Announcement = await _APIHelper.CallApiAsyncGet<Announcement>($"api/Configuration/GetAnnouncementById{id}", HttpMethod.Get);
            return Json(Announcement);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAnnouncementStatus(int id, bool status)
        {

            Announcement ObjAnnouncement = new Announcement();
            ObjAnnouncement.AnnouncementId = id;
            ObjAnnouncement.Status = status;
            ObjAnnouncement.UpdatedBy = _user.UserId;

            var result = await _APIHelper.CallApiAsyncPost<Response>(ObjAnnouncement, "api/Configuration/UpdateStatusByAnnouncementId", HttpMethod.Post);

            return Json(result);

        }
        public async Task<IActionResult> AnnouncementCreateOrUpdate(Announcement Announcement)
        {



            Announcement.CompanyId = _user.CompanyId;
            Announcement.CreatedBy = _user.UserId;

            var result = await _APIHelper.CallApiAsyncPost<Response>(Announcement, "api/Configuration/PostAnnouncement", HttpMethod.Post);

            if (result.Message.Contains("Insert"))
            {
                return RedirectToAction("AnnouncementList", new { data = 1 });
            }
            else
            {
                return RedirectToAction("AnnouncementList", new { data = 2 });
            }
        }

        public async Task<IActionResult> AnnouncementDelete(int id)
        {
            var result = await _APIHelper.CallApiAsyncGet<Response>($"api/Configuration/DeleteAnnouncement{id}/{_user.UserId}", HttpMethod.Get);
            return RedirectToAction("AnnouncementList", new { data = 3 });
        }

        public async Task<IActionResult> AnnouncementAlreadyExists(int id, string title)
        {
            var result = await _APIHelper.CallApiAsyncGet<Response>($"api/Configuration/AnnouncementAlreadyExists{_user.CompanyId}/{id}/{title}", HttpMethod.Get);
            return Json(result);
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
