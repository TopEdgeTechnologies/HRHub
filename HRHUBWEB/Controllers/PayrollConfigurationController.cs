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
using System.Globalization;
using System.Threading;

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
        public async Task<IActionResult> StaffSalaryMaster()
        {
			return View();
		}

        public async Task<IActionResult> PostStaffSalaryMaster(int month = 0, int year = 0)
        {
            StaffSalary objStaffSalary = new StaffSalary();

            var dayMonth = "1";
            var strDateStart = $"{year}/{month}/{dayMonth}";
            //var strDateEnd = $"{year}/{month}/1";

            objStaffSalary.SalaryMonth = Convert.ToDateTime(strDateStart);
            objStaffSalary.CompanyId = _user.CompanyId;
            objStaffSalary.CreatedBy = _user.UserId;

            var result = await _APIHelper.CallApiAsyncPost<Response>(objStaffSalary, "api/PayrollConfiguration/PostStaffSalaryMaster", HttpMethod.Post);

            return Json(result);    
		}

		public async Task<IActionResult> PutStaffSalaryMaster(string salaryMonth)
		{
			StaffSalary objStaffSalary = new StaffSalary();
			objStaffSalary.SalaryMonth = Convert.ToDateTime(salaryMonth);
			objStaffSalary.CreatedBy = _user.UserId;
			var result = await _APIHelper.CallApiAsyncPost<Response>(objStaffSalary, "api/PayrollConfiguration/PutStaffSalaryMaster", HttpMethod.Post);
			return Json(result);
		}

        public async Task<IActionResult> AlreadyExistsMaster(int month = 0, int year = 0)
        {
            if (month > 0 && year > 0)
            {
                var result = await _APIHelper.CallApiAsyncGet<Response>($"api/PayrollConfiguration/AlreadyExistsMaster/{month}/{year}", HttpMethod.Get);
                return Json(result);
            }
            return Json(null);
        }

        public async Task<IActionResult> StaffSalaryList(String data = "", int month = 0, int year = 0)
        {
            ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
            ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
            ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
            ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);

            ViewBag.Success = data;

            StaffSalary objStaffSalary = new StaffSalary();
            objStaffSalary.MonthNumber = month;
            objStaffSalary.Year = year;
            
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

		public async Task<List<StaffSalary>> GetStaffSalaryHistory(DateTime dateFrom, DateTime dateTo, int staffId)
		{
			if (dateFrom != null && dateTo != null && staffId > 0)
			{
				var staffSalaryHistoryList = await _APIHelper.CallApiAsyncGet<List<StaffSalary>>($"api/PayrollConfiguration/GetStaffSalaryHistory/{_user.CompanyId}/{dateFrom.ToString("dd-MMM-yyyy")}/{dateTo.ToString("dd-MMM-yyyy")}/{staffId}", HttpMethod.Get);
                //StaffSalary objStaffSalary = new StaffSalary();
                //objStaffSalary.SalaryStatusTitle = staffSalaryHistoryList.FirstOrDefault()?.SalaryStatusTitle;
				return staffSalaryHistoryList;
			}
			return null;
		}

		public async Task<IActionResult> GetStaffSalaryCreateOrUpdate(int month, int year, int staffId, decimal GrossAmount)
        { 
            StaffSalary objStaffSalary = new StaffSalary();
			objStaffSalary.StaffSalaryEditList = await GetStaffSalaryById(month, year, staffId);

            objStaffSalary.StaffId = staffId;
			objStaffSalary.MonthNumber = month;
			objStaffSalary.Year = year;
			objStaffSalary.RegistrationNo = objStaffSalary.StaffSalaryEditList.FirstOrDefault()?.RegistrationNo;
			objStaffSalary.FirstName = objStaffSalary.StaffSalaryEditList.FirstOrDefault()?.FirstName;  
            objStaffSalary.LastName = objStaffSalary.StaffSalaryEditList?.FirstOrDefault()?.LastName;
            objStaffSalary.DepartmentTitle = objStaffSalary.StaffSalaryEditList?.FirstOrDefault()?.DepartmentTitle;    
			objStaffSalary.OV_PayableAmount = GrossAmount;

            return View(objStaffSalary);
        }

		public async Task<IActionResult> SingleStaffSalaryCreateOrUpdate(IFormCollection objForm, StaffSalary objStaffSalary)
		{
            if (objStaffSalary.MonthNumber > 0 && objStaffSalary.Year > 0 && objStaffSalary.StaffId > 0)
            {
                var strDate = $"{objStaffSalary.Year}/{objStaffSalary.MonthNumber}/01";
                DateTime getSalaryDate = Convert.ToDateTime(strDate).Date;

				objStaffSalary.SalaryMonth = getSalaryDate;
				//objStaffSalary.StaffId = staffId;
				//objStaffSalary.TotalEarnings = Convert.ToDecimal(ov_earnings);
                //objStaffSalary.TotalDeductions = Convert.ToDecimal(ov_deductions);
                objStaffSalary.CreatedBy = _user.UserId;
                objStaffSalary.UpdatedBy = _user.UserId;

                var result = await _APIHelper.CallApiAsyncPost<Response>(objStaffSalary, "api/PayrollConfiguration/PostSingleStaffSalary", HttpMethod.Post);

                if (result.Message.Contains("Insert"))
                {
                    return RedirectToAction("StaffSalaryList", new { data = 1, month = objStaffSalary.MonthNumber, year = objStaffSalary.Year });
                }
                else
                {
                    return RedirectToAction("StaffSalaryList", new { data = 2, month = objStaffSalary.MonthNumber, year = objStaffSalary.Year });
                }
            }
            return View(objStaffSalary);    
		}

		#endregion
	}
}
