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


namespace HRHUBWEB.Controllers
{
    public class PayrollConfigurationController : Controller
    {
        private readonly HttpClient _client;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _token;
        private readonly User _user;

        public PayrollConfigurationController(IHttpClientFactory httpClientFactory, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
            _client = httpClientFactory.CreateClient("APIClient");

            _user = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
            _token = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);    
        }

        #region Salary Method

        [CustomAuthorization]
        public async Task<IActionResult> SalaryMethodList(string data = "", int Id = 0)
        {
            ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
            ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
            ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
            ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);

            ViewBag.Success = data;

            SalaryMethod objSalaryMethod = new SalaryMethod();
            if (Id > 0)
            {
                objSalaryMethod = await GetSalaryMethodById(Id);
            }

            if (_token != null)
            {
                HttpResponseMessage response = await _client.GetAsync("api/PayrollConfiguration/GetSalaryMethod");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    objSalaryMethod.SalaryMethodList = JsonConvert.DeserializeObject<List<SalaryMethod>>(content);
                }
            }
            else
            {
                return RedirectToAction("Loginpage", "User", new { Id = 2 });
            }
            return View(objSalaryMethod);
        }

        public async Task<SalaryMethod> GetSalaryMethodById(int Id)
        {
            SalaryMethod objSalaryMethod = new SalaryMethod();
            HttpResponseMessage response = await _client.GetAsync($"api/PayrollConfiguration/GetSalaryMethodById/{Id}");
            if(response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                objSalaryMethod = JsonConvert.DeserializeObject<SalaryMethod>(content); 
            }
            return objSalaryMethod;
        }

        public async Task<IActionResult> SalaryMethodCreateOrUpdate(SalaryMethod objSalaryMethod)
        {
            //user get from session
            
            objSalaryMethod.CreatedBy = _user.UserId;

            HttpResponseMessage response = await _client.PostAsJsonAsync("api/PayrollConfiguration/PostSalaryMethod", objSalaryMethod);
            if(response.IsSuccessStatusCode) 
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
                return RedirectToAction("SalaryMethodList", new { data = status});   
            }
            else
            {
                return RedirectToAction("Loginpage", "User", new { id = 2 });
            }
        }

        public async Task<IActionResult> DeleteSalaryMethod(int Id, int UserId)
        {
            SalaryMethod objSalaryMethod = new SalaryMethod();
            HttpResponseMessage response = await _client.GetAsync($"api/PayrollConfiguration/DeleteSalaryMethod/{Id}/{UserId}");
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
                return RedirectToAction("SalaryMethodList", new { data = status });
            }
            else
            {
                return RedirectToAction("Loginpage", "User", new { id = 2 });
            }
        }

        public async Task<IActionResult> SalaryMethodAlreadyExists(int Id, string Title)
        {
            HttpResponseMessage response = await _client.GetAsync($"api/PayrollConfiguration/SalaryMethodAlreadyExists/{Id}/{Title}");
            if(response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync();
                return Json(content);
            }
            return RedirectToAction("Loginpage", "User", new { Id = 2 });
        }

        #endregion
    }
}
