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
using System.Net.Http;
using System.Collections.Specialized;

namespace HRHUBWEB.Controllers
{
    public class StaffController : Controller
    {
        private readonly HttpClient _client;
        private IWebHostEnvironment _webHostEnvironment;

        public StaffController(IHttpClientFactory httpClientFactory, IWebHostEnvironment webHostEnvironment) 
        {
            _client = httpClientFactory.CreateClient("APIClient");
            _webHostEnvironment = webHostEnvironment;
        }

        [CustomAuthorization]
        public async Task<IActionResult> StaffList(String data = "", int Id = 0)
        {
            ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
            ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
            ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
            ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);

            ViewBag.Success = data;

            Staff staff = new Staff();

            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
            staff.CompanyId = userObject.CompanyId;

            if( Id > 0)
            {
                staff = await GetStaffById(Id);
            }

            if(Token != null)
            {
                HttpResponseMessage response = await _client.GetAsync($"api/Staffs/GetStaffByCompanyId{staff.CompanyId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();   
                    staff.StaffList = JsonConvert.DeserializeObject<List<Staff>>(content);
                }
            }
            else
            {
                RedirectToAction("Loginpage", "User", new { id = 2 });
            }
            return View(staff);
        }

        public async Task<IActionResult> StaffDetails(int Id)
        {
            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            if(Token != null)
            {
                Staff staff = await GetStaffById(Id);
                return View(staff);
            }
            else
            {
                return RedirectToAction("Loginpage", "User", new { id = 2 });
            }
        }

        public async Task<Staff> GetStaffById(int Id)
        {
            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            Staff staff = new Staff();
            var response = await _client.GetAsync($"api/Staffs/GetStaffById{Id}");
            if(response.IsSuccessStatusCode) 
            {
                var content = await response.Content.ReadAsStringAsync();
                staff = JsonConvert.DeserializeObject<Staff>(content);
            }
            return staff;
        }

        public async Task<IActionResult> GetStaffCreateOrUpdate(int Id) 
        {
			var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
			var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
			//staff.CompanyId = userObject.CompanyId;
			//staff.CreatedBy = userObject.CreateBy;

			HttpResponseMessage response = await _client.GetAsync($"api/Staffs/GetStaffById{Id}");

			if (Token != null)
			{
				if (Id == 0)
				{
					Staff Info = new Staff();

					return View(Info);
				}
				Staff staff = await GetStaffById(Id);
				return View(staff);
			}
			else
			{
				return RedirectToAction("Loginpage", "User", new { id = 2 });
			}
		}

        public async Task<IActionResult> StaffCreateOrUpdate(Staff staff)
        {
            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
            staff.CompanyId = userObject.CompanyId;
            staff.CreatedBy = userObject.CreateBy;

            HttpResponseMessage response = await _client.PostAsJsonAsync("api/Staffs/PostStaff", staff);
            
            if(response.IsSuccessStatusCode) 
            {
                var content = response.Content.ReadAsStringAsync();
				var result = JsonConvert.DeserializeObject<Response>(content.Result);
                int status = 0;

                if(result.Success) 
                {
                    if (result.Message.Contains("Insert")) 
                        status = 1;
                    else if(result.Message.Contains("Update"))
                        status = 2; 
                }
                return RedirectToAction("StaffList", new {data = status});  
            }
            else
            {
                return RedirectToAction("Loginpage", "User", new { Id = 2 });
            }
        }



	}
}
