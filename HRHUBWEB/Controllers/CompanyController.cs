using HRHUBAPI.Models;
using HRHUBWEB.Extensions;
using HRHUBWEB.Filters;
using HRHUBWEB.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Packaging.Signing;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;

namespace HRHUBWEB.Controllers
{
    public class CompanyController : Controller
    {
        private readonly HttpClient _client;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly APIHelper _APIHelper;
        private readonly User _user;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CompanyController(IHttpClientFactory httpClient, IWebHostEnvironment webHostEnvironment, APIHelper APIHelper, IHttpContextAccessor httpContextAccessor)
        {
            _client = httpClient.CreateClient("APIClient");
            _webHostEnvironment = webHostEnvironment;
            _APIHelper = APIHelper;
            _httpContextAccessor = httpContextAccessor;
            _user = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
        }
        #region CompanyInfo
        [CustomAuthorization]
        public async Task<IActionResult> CompanyList(string data = "")
        {

            ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
            ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
            ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
            ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);


            ViewBag.Success = data;
            List<Company> ObjCompany = new List<Company>();

            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");

            var CompanyId = userObject.CompanyId;
            if (Token != null)
            {


                HttpResponseMessage message = await _client.GetAsync($"api/Company/GetCompanyInfos{CompanyId}");
                if (message.IsSuccessStatusCode)
                {
                    var result = message.Content.ReadAsStringAsync().Result;
                    ObjCompany = JsonConvert.DeserializeObject<List<Company>>(result);

                }
            }
            else
            {
                return RedirectToAction("Loginpage", "User", new { id = 2 });
            }


            return View(ObjCompany);
        }
        public async Task<IActionResult> CompanyDetails(int id)
        {
            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);



            if (Token != null)
            {

                Company ObjCompany = await GetCompanybyID(id);

                return View(ObjCompany);
            }
            else
            {
                return RedirectToAction("Loginpage", "User", new { id = 2 });

            }
        }
        private async Task<Company> GetCompanybyID(int? id)
        {
            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

			var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
            

			Company ObjCompany = new Company();
            var CompanyId = userObject.CompanyId;
            var userId = userObject.UserId;

			HttpResponseMessage message = await _client.GetAsync($"api/Company/GetCompanyInfoId{id}");
            if (message.IsSuccessStatusCode)
            {
                var result = message.Content.ReadAsStringAsync().Result;
                ObjCompany = JsonConvert.DeserializeObject<Company>(result);		


			}


			var response = await _client.GetAsync($"api/Staffs/GetStaffCompanyVise{CompanyId}");
			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				ObjCompany.SettingStaff = JsonConvert.DeserializeObject<Staff>(content);
			}

			var response1 = await _client.GetAsync($"api/User/GetUserCompanyViseId{userId}");
			if (response1.IsSuccessStatusCode)
			{
				var content = await response1.Content.ReadAsStringAsync();
				ObjCompany.SettingUser = JsonConvert.DeserializeObject<User>(content);
			}


			return ObjCompany;
        }
        [HttpGet]
        public async Task<IActionResult> CompanyCreateOrUpdate(int id)
        {
            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            //Get Instutute ID through Sessions
            var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
            ViewBag.CompanyId = userObject.CompanyId;

            if (Token != null)
            {


                if (id == 0)
                {
                    Company Info = new Company();
                    Info.LogoAttachment = "~/Images/CompanyLogo.png";

					return View(Info);
                }
                Company Companyinfo = await GetCompanybyID(id);

                return View(Companyinfo);
            }
            else
            {
                return RedirectToAction("Loginpage", "User", new { id = 2 });

            }
        }
        [HttpPost]
        public async Task<IActionResult> CompanyCreateOrUpdate(IFormCollection my,Company ObjCompany)
        {
            try
            {
                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

				var CompanyLogo = my.Files.GetFile("CompanyLogo");

				ObjCompany.LogoAttachment = uploadImage(ObjCompany.CompanyName, CompanyLogo, "CompanyAttachment");
                


				var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");               
                ObjCompany.CreatedBy = userObject.UserId;
                HttpResponseMessage message = await _client.PostAsJsonAsync("api/Company/CompanyAddOrUpdate", ObjCompany);

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

                    // return RedirectToAction("CompanyList", new { data = status });
                    return RedirectToAction("CompanySetting", new { tab = "CompanyProfile" });
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

        [HttpPost]
        public async Task<IActionResult> PostCompanydata(IFormCollection my, Company ObjCompany)
        {
            try
            {
                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                var CompanyLogo = my.Files.GetFile("CompanyLogo");

                ObjCompany.LogoAttachment = uploadImage(ObjCompany.CompanyName, CompanyLogo, "CompanyAttachment");



                var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
                ObjCompany.CreatedBy = userObject.UserId;
                HttpResponseMessage message = await _client.PostAsJsonAsync("api/Company/PostCompanyProfile", ObjCompany);

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

                    // return RedirectToAction("CompanyList", new { data = status });
                    return RedirectToAction("CompanySetting", new { tab = "CompanyProfile" });
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


        public async Task<IActionResult> CompanyDelete(int id)
        {
            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            HttpResponseMessage message = await _client.DeleteAsync($"api/Company/DeleteCompanyInfo{id}");
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

                return RedirectToAction("CompanyList", new { data = status });

            }

            else
            {
                return RedirectToAction("Loginpage", "User", new { id = 2 });
            }

        }



		// Company Settings 
		[HttpGet]
		public async Task<IActionResult> CompanySetting()
		{
			var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

			//Get Instutute ID through Sessions
			var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
			var CompanyId = userObject.CompanyId;// ary database se data tp compny ki base per hi is id ki base per or wo opr hy id aa ri hy wo? bass

			if (Token != null)
			{

                HttpResponseMessage message2 = await _client.GetAsync($"api/Leave/GetLeaveStatusInfos");
                if (message2.IsSuccessStatusCode)
                {
                    var result = message2.Content.ReadAsStringAsync().Result;
                    ViewBag.LeaveStatusList = JsonConvert.DeserializeObject<List<LeaveStatus>>(result);

                }


                HttpResponseMessage message3 = await _client.GetAsync($"api/Leave/GetWeekendRuleInfos");
                if (message3.IsSuccessStatusCode)
                {
                    var result = message3.Content.ReadAsStringAsync().Result;
                    ViewBag.LeaveWeekendRule = JsonConvert.DeserializeObject<List<WeekendRule>>(result);

                }

    //            if (id == 0) 
				//{
				//	Company Info = new Company();
				//	Info.LogoAttachment = "~/Images/CompanyLogo.png";

				//	return View(Info);
				//}

				//HttpResponseMessage message1 = await _client.GetAsync($"api/Staffs/GetStaffByCompanyId{CompanyId}");
				//if (message1.IsSuccessStatusCode)
				//{
				//	var result = message1.Content.ReadAsStringAsync().Result;
				//	ViewBag.Stafflist = JsonConvert.DeserializeObject<List<Staff>>(result);

				//}



				Company Companyinfo = await GetCompanybyID(CompanyId);

				return View(Companyinfo);
			}
			else
			{
				return RedirectToAction("Loginpage", "User", new { id = 2 });

			}
		}



















		// check duplicate company email 
		public async Task<ActionResult<JsonObject>> CompanyEmailCheckData(int id, string email)
        {

            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);



            var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");          



            HttpResponseMessage message = await _client.GetAsync($"api/Company/CompanyEmailCheck{id}/{email}");
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


		// check duplicate company name
		public async Task<ActionResult<JsonObject>> CompanyNameCheckData(int id, string companyName)
		{

			var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);



			var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");



			HttpResponseMessage message = await _client.GetAsync($"api/Company/CompanyNameCheck{id}/{companyName}");
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



		// check duplicate company name
	public async Task<ActionResult<JsonObject>> UserCheckData(int id, string username)
    {

			var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);



			var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");



			HttpResponseMessage message = await _client.GetAsync($"api/User/UserCheckData{id}/{username}");
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
