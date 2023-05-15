﻿using Microsoft.AspNetCore.Mvc;
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
using Microsoft.AspNetCore.Mvc.Rendering;

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
            ViewBag.MaterialStatus = GetMaterialStatusList();
            ViewBag.BloodGroup = GetBloodGroup();

            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
			var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");

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

        public List<SelectListItem> GetMaterialStatusList() 
        {
            List<SelectListItem> listobj = new List<SelectListItem>();
            listobj.Add(new SelectListItem { Text = "Single", Value = "1" });
            listobj.Add(new SelectListItem { Text = "Married", Value = "2" });
            ViewBag.MaterialStatus = listobj;
            return listobj;
        }

        public List<SelectListItem> GetBloodGroup()
        {
            List<SelectListItem > listobj = new List<SelectListItem>();
            listobj.Add(new SelectListItem { Text = "A+", Value = "1" });
            listobj.Add(new SelectListItem { Text = "B+", Value = "2" });
            listobj.Add(new SelectListItem { Text = "O+", Value = "3" });
            listobj.Add(new SelectListItem { Text = "AB+", Value = "4" });
            listobj.Add(new SelectListItem { Text = "A-", Value = "5" });
            listobj.Add(new SelectListItem { Text = "B-", Value = "6" });
            listobj.Add(new SelectListItem { Text = "O-", Value = "7" });
            listobj.Add(new SelectListItem { Text = "AB-", Value = "8" });
            return listobj;
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
