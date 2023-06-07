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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRHUBWEB.Controllers
{
    public class PayrollConfigurationController : Controller
    {
        private IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly APIHelper _APIHelper;
        private readonly User _user;

        public PayrollConfigurationController(IHttpClientFactory httpClientFactory, IWebHostEnvironment webHostEnvironment, APIHelper APIHelper, IHttpContextAccessor httpContextAccessor)
        {
            _webHostEnvironment = webHostEnvironment;
            _APIHelper = APIHelper;
            _httpContextAccessor = httpContextAccessor;
            _user = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
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

            objSalaryMethod.SalaryMethodList = await _APIHelper.CallApiAsyncGet<IEnumerable<SalaryMethod>>("api/PayrollConfiguration/GetSalaryMethod", HttpMethod.Get);
            return View(objSalaryMethod);
        }

        public async Task<SalaryMethod> GetSalaryMethodById(int Id)
        {
            SalaryMethod objSalaryMethod = new SalaryMethod();
            objSalaryMethod = await _APIHelper.CallApiAsyncGet<SalaryMethod>($"api/PayrollConfiguration/GetSalaryMethodById/{Id}", HttpMethod.Get);
            return objSalaryMethod;
        }

        public async Task<IActionResult> SalaryMethodCreateOrUpdate(SalaryMethod objSalaryMethod)
        {
            objSalaryMethod.CreatedBy = _user.UserId;
            var result = await _APIHelper.CallApiAsyncPost<Response>(objSalaryMethod, "api/PayrollConfiguration/PostSalaryMethod", HttpMethod.Post);

            if (result.Message.Contains("Insert"))
            {
                return RedirectToAction("SalaryMethodList", new { data = 1 });
            }
            else
            {
                return RedirectToAction("SalaryMethodList", new { data = 2 });
            }
        }

        public async Task<IActionResult> SalaryMethodDelete(int Id)
        {
            var result = await _APIHelper.CallApiAsyncGet<Response>($"api/PayrollConfiguration/DeleteSalaryMethod/{Id}/{_user.UserId}", HttpMethod.Get);
            return RedirectToAction("SalaryMethodList", new { data = 3 });
        }

        public async Task<IActionResult> SalaryMethodAlreadyExists(int Id, string Title)
        {
            SalaryMethod objSalaryMethod = new SalaryMethod();
            var result = await _APIHelper.CallApiAsyncGet<Response>($"api/PayrollConfiguration/SalaryMethodAlreadyExists/{Id}/{Title}", HttpMethod.Get);
            return Json(result);
        }

        #endregion

        #region Component Info

        [CustomAuthorization]
        public async Task<IActionResult> ComponentInfoList(string data = "", int Id = 0)
        {
            ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
            ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
            ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
            ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);
            
            ViewBag.Success = data;

            ComponentInfo objComponentInfo = new ComponentInfo();
            if (Id > 0)
            {
                objComponentInfo = await GetComponentInfoById(Id);
            }

            ViewBag.CalculationMethod = GetCalculationMethodList();
            ViewBag.Category = GetCategoryList();
            ViewBag.Type = GetTypeList();
            ViewBag.ObjComponentGroupList = await _APIHelper.CallApiAsyncGet<IEnumerable<ComponentGroup>>("api/PayrollConfiguration/GetComponentGroup", HttpMethod.Get);

            objComponentInfo.ComponentInfoList = await _APIHelper.CallApiAsyncGet<IEnumerable<ComponentInfo>>("api/PayrollConfiguration/GetComponentInfo", HttpMethod.Get);
            return View(objComponentInfo);
        }

        public async Task<ComponentInfo> GetComponentInfoById(int Id)
        {
            ComponentInfo objComponentInfo = new ComponentInfo();
            objComponentInfo = await _APIHelper.CallApiAsyncGet<ComponentInfo>($"api/PayrollConfiguration/GetComponentInfoById/{Id}", HttpMethod.Get);
            return objComponentInfo;
        }

        public List<SelectListItem> GetCalculationMethodList()
        {
            List<SelectListItem> listobj = new List<SelectListItem>();
            listobj.Add(new SelectListItem { Text = "Percentage", Value = "1" });
            listobj.Add(new SelectListItem { Text = "Amount", Value = "2" });
            return listobj;
        }

        public List<SelectListItem> GetCategoryList()
        {
            List<SelectListItem> listobj = new List<SelectListItem>();
            listobj.Add(new SelectListItem { Text = "Earning", Value = "1" });
            listobj.Add(new SelectListItem { Text = "Deduction", Value = "2" });
            return listobj;
        }

        public List<SelectListItem> GetTypeList()
        {
            List<SelectListItem> listobj = new List<SelectListItem>();
            listobj.Add(new SelectListItem { Text = "Fixed", Value = "1" });
            listobj.Add(new SelectListItem { Text = "Variable", Value = "2" });
            return listobj;
        }

        public async Task<IActionResult> ComponentInfoCreateOrUpdate(ComponentInfo objComponentInfo)
        {
            objComponentInfo.CompanyId = _user.CompanyId;
            objComponentInfo.CreatedBy = _user.UserId;
            var result = await _APIHelper.CallApiAsyncPost<Response>(objComponentInfo, "api/PayrollConfiguration/PostComponentInfo", HttpMethod.Post);

            if (result.Message.Contains("Insert"))
            {
                return RedirectToAction("ComponentInfoList", new { data = 1 });
            }
            else
            {
                return RedirectToAction("ComponentInfoList", new { data = 2 });
            }
        }

        public async Task<IActionResult> ComponentInfoDelete(int Id)
        {
            var result = await _APIHelper.CallApiAsyncGet<Response>($"api/PayrollConfiguration/DeleteComponentInfo/{Id}/{_user.UserId}", HttpMethod.Get);
            return RedirectToAction("ComponentInfoList", new { data = 3 });
        }

        public async Task<IActionResult> ComponentInfoAlreadyExists(int Id, string Title)
        {
            ComponentInfo objComponentInfo = new ComponentInfo();
            var result = await _APIHelper.CallApiAsyncGet<Response>($"api/PayrollConfiguration/ComponentInfoAlreadyExists/{Id}/{Title}", HttpMethod.Get);
            return Json(result);
        }

        #endregion

        #region Staff Salary

        [CustomAuthorization]
        public async Task<IActionResult> StaffSalaryList(string data ="", int Id = 0)
        {
            ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
            ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
            ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
            ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);

            ViewBag.Success = data;

            StaffSalary objStaffSalary = new StaffSalary();
            var month = 5;
            var year = 2023;
            objStaffSalary.StaffSalaryList = await _APIHelper.CallApiAsyncGet<IEnumerable<StaffSalary>>($"api/PayrollConfiguration/GetStaffSalaryByCompanyId/{_user.CompanyId}/{month}/{year}", HttpMethod.Get);

            return View(objStaffSalary);
        }

        public async Task<List<StaffSalary>> GetStaffSalaryById(int month, int year, int staffId)
        {
            if(month > 0 && year > 0 && staffId > 0)
            {
                var staffSalaryCardList = await _APIHelper.CallApiAsyncGet<List<StaffSalary>>($"api/PayrollConfiguration/GetStaffSalaryById/{_user.CompanyId}/{month}/{year}/{staffId}", HttpMethod.Get);
				return staffSalaryCardList;
			}
            return null;
        }

        public async Task<IActionResult> GetStaffSalaryCreateOrUpdate(int month, int year, int staffId, decimal GrossAmount)
        { 
            StaffSalary objStaffSalary = new StaffSalary();
			objStaffSalary.StaffSalaryEditList = await GetStaffSalaryById(month, year, staffId);
            ViewBag.SalaryMonth = month;
            ViewBag.SalaryYear = year;  
            objStaffSalary.OV_PayableAmount = GrossAmount;

            //StaffSalary objStaffSalary = new StaffSalary();
            //         objStaffSalary = await _APIHelper.CallApiAsyncGet<StaffSalary>($"api/PayrollConfiguration/GetStaffSalaryById/{_user.CompanyId}/{month}/{year}/{StaffId}", HttpMethod.Get);
            return View(objStaffSalary);
        }

		#endregion
	}
}
