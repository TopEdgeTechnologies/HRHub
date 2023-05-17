using HRHUBAPI.Models;
using HRHUBWEB.Extensions;
using HRHUBWEB.Filters;
using HRHUBWEB.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;

namespace HRHUBWEB.Controllers
{
    public class CompanyController : Controller
    {
        private readonly HttpClient _client;
		private IWebHostEnvironment _webHostEnvironment;
		public CompanyController(IHttpClientFactory httpClient, IWebHostEnvironment webHostEnvironment)
        {
            _client = httpClient.CreateClient("APIClient");
			_webHostEnvironment = webHostEnvironment;
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

            if (Token != null)
            {


                HttpResponseMessage message = await _client.GetAsync($"api/Company/GetCompanyInfos");
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
        private async Task<Company> GetCompanybyID(int id)
        {
            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);



            Company ObjCompany = new Company();




            HttpResponseMessage message = await _client.GetAsync($"api/Company/GetCompanyInfoId{id}");
            if (message.IsSuccessStatusCode)
            {
                var result = message.Content.ReadAsStringAsync().Result;
                ObjCompany = JsonConvert.DeserializeObject<Company>(result);

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

                    return RedirectToAction("CompanyList", new { data = status });

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

        public async Task<ActionResult<JsonObject>> CompanyCheckData(int id, string companyName, string email)
        {

            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);



            var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");          



            HttpResponseMessage message = await _client.GetAsync($"api/Company/CompanyCheckData{id}/{companyName}/{email}");
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
