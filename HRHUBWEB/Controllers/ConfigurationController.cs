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

namespace HRHUBWEB.Controllers
{
    public class ConfigurationController : Controller
    {
        private readonly HttpClient _client;
        private IWebHostEnvironment _webHostEnvironment;
        private APIHelper _APIHelper;

		public ConfigurationController(IHttpClientFactory httpClient, IWebHostEnvironment webHostEnvironment, APIHelper APIHelper)
        {
            _client = httpClient.CreateClient("APIClient");
            _webHostEnvironment = webHostEnvironment;
			_APIHelper=APIHelper;

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

            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
            ObjDesignation.CompanyId = userObject.CompanyId;


			


			var adsdas = await _APIHelper.CallApiAsyncGet<IEnumerable<Designation>>( $"api/Configuration/GetDesignationInfos{ObjDesignation.CompanyId}", HttpMethod.Get, Token);

			//var result = await CallApiAsync<object>(model, apiUrl, HttpMethod.Post);

			if (Token != null)
            {


                HttpResponseMessage message = await _client.GetAsync($"api/Configuration/GetDesignationInfos{ObjDesignation.CompanyId}");
                if (message.IsSuccessStatusCode)
                {
                    var result = message.Content.ReadAsStringAsync().Result;
                    ObjDesignation.Listdesignation = JsonConvert.DeserializeObject<List<Designation>>(result);

                }
            }
            else
            {
                return RedirectToAction("Loginpage", "User", new { id = 2 });
            }


            return View(ObjDesignation);
        }
        public async Task<IActionResult> DesignationDetails(int id)
        {

            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            Designation designation = new Designation();
            var response = await _client.GetAsync($"api/Configuration/GetDesignationInfoId{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                designation = JsonConvert.DeserializeObject<Designation>(content);
                return Json(designation);
            }
            else
            {

                return Json(null);
            }

           
        }
       
        
        [HttpPost]
        public async Task<IActionResult> DesignationCreateOrUpdate(Designation ObjDesignation)
        {
            try
            {
                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

               // var DesignationResume = my.Files.GetFile("DesignationResume");

                //ObjDesignation.AttachmentPath = uploadImage(ObjDesignation.Name, DesignationResume, "DesignationAttachment");

                var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
                ObjDesignation.CompanyId = userObject.CompanyId;
                ObjDesignation.CreatedBy = userObject.UserId;
                HttpResponseMessage message = await _client.PostAsJsonAsync("api/Configuration/DesignationAddOrUpdate", ObjDesignation);

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

                    return RedirectToAction("DesignationList", new { data = status });

                }
                else
                {
                    return RedirectToAction("Loginpage", "User",  new {id=2 });
                }



            }
            catch (Exception)
            {

                return View();
            }
        }
        public async Task<IActionResult> DesignationDelete(int id)
        {
            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);


            var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");

            HttpResponseMessage message = await _client.DeleteAsync($"api/Configuration/DeleteDesignationInfo{id}/{userObject.UserId}");
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

                return RedirectToAction("DesignationList", new { data = status });

            }

            else
            {
                return RedirectToAction("Loginpage", "User",  new {id=2 });
            }

        }

        public async Task<ActionResult<JsonObject>> DesignationCheckData(int id, string title)
        {

            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);



            var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
            var CompanyId = userObject.CompanyId;



            HttpResponseMessage message = await _client.GetAsync($"api/Configuration/DesignationCheckData{id}/{title}/{CompanyId}");
            if (message.IsSuccessStatusCode)
            {
                var result = message.Content.ReadAsStringAsync().Result;
                return Json(result);

            }

            else
            {
                return RedirectToAction("Loginpage", "User",  new {id=2 });
            }
        }

        #endregion

        #region Department Info

        [CustomAuthorization]
        public async Task<IActionResult> DepartmentList(string data = "" )
        {
            ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
            ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
            ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
            ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);
            
            ViewBag.Success = data;

            Department departments = new Department();
            
            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
            departments.CompanyId = userObject.CompanyId;

            //if (id > 0)
            //{
            //    departments = await GetDepartmentById(id);
            //}
          

            if (Token != null)
            {
                HttpResponseMessage response = await _client.GetAsync($"api/Configuration/GetDepartmentByCompanyID{departments.CompanyId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    departments.Listdepartments = JsonConvert.DeserializeObject<List<Department>>(content);
                }
            }
            else
            {
                return RedirectToAction("Loginpage", "User", new { id = 2 });
            }
            return View(departments);
        }

        //public async Task<IActionResult> DepartmentDetails(int id)
        //{
        //    var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
        //    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        //    if (Token != null)
        //    {
        //        Department department = await GetDepartmentById(id);
        //        return View(department);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Loginpage", "User", new { id = 2 });
        //    }
        //}

        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            Department department = new Department();
            var response = await _client.GetAsync($"api/Configuration/GetDepartmentById{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                department = JsonConvert.DeserializeObject<Department>(content);
                return Json(department);
            }
            else {

                return Json(null);
            }

        
        }

        public async Task<IActionResult> GetDepartmentCreateOrUpdate(int id)
        {         
                return RedirectToAction("DepartmentList", new { id = id });  
        }

        public async Task<IActionResult> DepartmentCreateOrUpdate(IFormCollection MyAttachment, Department department)
        {
            //file add
            var DepartmentLogo = MyAttachment.Files.GetFile("LogoAttachmentFile");
            department.LogoAttachment = uploadImage(department.Title, DepartmentLogo, "DepartmentImages");
            ///

            //token get from session
            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            //

            //user get from session
            var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
            department.CompanyId = userObject.CompanyId;
            department.CreatedBy = userObject.UserId;

            HttpResponseMessage response = await _client.PostAsJsonAsync("api/Configuration/PostDepartment", department);
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
                return RedirectToAction("DepartmentList", new { data = status });
            }
            else
            {
                return RedirectToAction("Loginpage", "User", new { id = 2 });
            }
        }

        public async Task<IActionResult> DepartmentDelete(int id )
        {
            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");


            var response = await _client.GetAsync($"api/Configuration/DeleteDepartment{id}/{userObject.UserId}");
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
                return RedirectToAction("DepartmentList", new { data = status });
            }
            else
            {
                return RedirectToAction("Loginpage", "User", new { id = 2 });
            }
        }

        public async Task<IActionResult> DepartmentAlreadyExists(int id, string title)
        {
            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
            var CompanyId = userObject.CompanyId;
            var response = await _client.GetAsync($"api/Configuration/DepartmentAlreadyExists{CompanyId}/{id}/{title}");

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                return Json(content);
            }
            return RedirectToAction("Loginpage", "User", new { id = 2 });
        }

        #endregion


        #region LeaveTypeInfo
        //[CustomAuthorization]
        //public async Task<IActionResult> LeaveTypeList(string data = "")
        //{

        //    ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
        //    ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
        //    ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
        //    ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);


        //    ViewBag.Success = data;
        //    LeaveType leavetypes = new LeaveType();

        //    var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
        //    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

        //    var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
        //    var CompanyId = userObject.CompanyId;



        //    if (Token != null)
        //    {


        //        HttpResponseMessage message = await _client.GetAsync($"api/Configuration/GetLeaveTypeInfos{CompanyId}");
        //        if (message.IsSuccessStatusCode)
        //        {

        //            var content = await message.Content.ReadAsStringAsync();
        //            leavetypes.ListLeaveTypes = JsonConvert.DeserializeObject<List<LeaveType>>(content);

        //        }
        //    }
        //    else
        //    {
        //        return RedirectToAction("Loginpage", "User", new { id = 2 });
        //    }


        //    return View(leavetypes);
        //}


        [CustomAuthorization]
        public async Task<IActionResult> LeaveTypeList(string data = "")
        {
            ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
            ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
            ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
            ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);

            ViewBag.Success = data;

            LeaveType leavetypes = new LeaveType();

            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
            leavetypes.CompanyId = userObject.CompanyId;


            if (Token != null)
            {
                HttpResponseMessage response = await _client.GetAsync($"api/Configuration/GetLeaveTypeInfos{leavetypes.CompanyId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    leavetypes.ListLeaveTypes = JsonConvert.DeserializeObject<List<LeaveType>>(content);
                }
            }
            else
            {
                return RedirectToAction("Loginpage", "User", new { id = 2 });
            }
            return View(leavetypes);
        }


        //public async Task<IActionResult> LeaveTypeDetails(int id)
        //{
        //    var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
        //    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);



        //    if (Token != null)
        //    {

        //        LeaveType ObjLeaveType = await GetLeaveTypebyID(id);

        //        return View(ObjLeaveType);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Loginpage", "User", new { id = 2 });

        //    }
        //}
        //private async Task<LeaveType> GetLeaveTypebyID(int id)
        //{
        //    var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
        //    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);



        //    LeaveType ObjLeaveType = new LeaveType();




        //    HttpResponseMessage message = await _client.GetAsync($"api/Configuration/GetLeaveTypeInfoId{id}");
        //    if (message.IsSuccessStatusCode)
        //    {
        //        var result = message.Content.ReadAsStringAsync().Result;
        //        ObjLeaveType = JsonConvert.DeserializeObject<LeaveType>(result);

        //    }

        //    return ObjLeaveType;
        //}

        public async Task<IActionResult> GetLeaveTypeById(int id)
        {
            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            LeaveType leavetype = new LeaveType();
            var response = await _client.GetAsync($"api/Configuration/GetLeaveTypeInfoId{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                leavetype = JsonConvert.DeserializeObject<LeaveType>(content);
                return Json(leavetype);
            }
            else
            {

                return Json(null);
            }


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
        public async Task<IActionResult> LeaveTypeCreateOrUpdate(LeaveType ObjLeaveType)
        {
            try
            {
                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
                ObjLeaveType.CompanyId = userObject.CompanyId;
                ObjLeaveType.CreatedBy = userObject.UserId;
                HttpResponseMessage message = await _client.PostAsJsonAsync("api/Configuration/LeaveTypeAddOrUpdate", ObjLeaveType);

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

                    return RedirectToAction("LeaveTypeList", new { data = status });

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
        public async Task<IActionResult> LeaveTypeDelete(int id)
        {
            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            HttpResponseMessage message = await _client.DeleteAsync($"api/Configuration/DeleteLeaveTypeInfo{id}");
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

                return RedirectToAction("LeaveTypeList", new { data = status });

            }

            else
            {
                return RedirectToAction("Loginpage", "User", new { id = 2 });
            }

        }

        public async Task<ActionResult<JsonObject>> LeaveTypeAlreadyExists(int id, string title)
        {

            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);



            var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
            var CompanyId = userObject.CompanyId;



            HttpResponseMessage message = await _client.GetAsync($"api/Configuration/LeaveTypeAlreadyExists{id}/{title}/{CompanyId}");
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
                HttpResponseMessage response = await  _client.GetAsync($"api/Configuration/GetHolidaysByCompanyID{objHoliday.CompanyId}");
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
