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
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

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

        #region Staff Payroll

        [CustomAuthorization]
        public async Task<IActionResult> StaffSalaryMaster()
        {
            StaffSalary objStaffSalary = new StaffSalary();
            loadMonthAndYear();
            return View(objStaffSalary);
		}

        private void loadMonthAndYear()
        {
            List<SelectListItem> itemsYear = new List<SelectListItem>
            {
                new SelectListItem { Text = "2023", Value = "2023" },
                new SelectListItem { Text = "2024", Value = "2024" },
                new SelectListItem { Text = "2025", Value = "2025" },
                new SelectListItem { Text = "2026", Value = "2026" },
            };

            // Assign the list to ViewBag
            ViewBag.yearViewBag = itemsYear;

            List<SelectListItem> items = new List<SelectListItem>
            {
                new SelectListItem { Text = "January", Value = "1"},
                new SelectListItem { Text = "February", Value = "2"},
                new SelectListItem { Text = "March", Value = "3"},
                new SelectListItem { Text = "April", Value = "4"},
                new SelectListItem { Text = "May", Value = "5"},
                new SelectListItem { Text = "June", Value = "6"},
                new SelectListItem { Text = "July", Value = "7"},
                new SelectListItem { Text = "August", Value = "8"},
                new SelectListItem { Text = "September", Value = "9"},
                new SelectListItem { Text = "October", Value = "10"},
                new SelectListItem { Text = "November", Value = "11"},
                new SelectListItem { Text = "December", Value = "12"},
            };

            // Create the SelectList using the items and return it
            ViewBag.monthViewBag = items;
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

		public async Task<IActionResult> PutStaffSalaryMaster(string salaryMonth, int salaryStatusId)
		{
            if(salaryStatusId > 0)
            {
			    StaffSalary objStaffSalary = new StaffSalary();
			    objStaffSalary.SalaryMonth = Convert.ToDateTime(salaryMonth);
                objStaffSalary.SalaryStatusId = Convert.ToInt32(salaryStatusId);    
			    objStaffSalary.UpdatedBy = _user.UserId;
			    var result = await _APIHelper.CallApiAsyncPost<Response>(objStaffSalary, "api/PayrollConfiguration/PutStaffSalaryMaster", HttpMethod.Post);
			    return Json(result);
            }
            return Json(null);
		}

		public async Task<IActionResult> PutStaffSalaryPaid_Hold(string salaryMonth, int staffId, int salaryStatusId)
		{
			if (salaryStatusId > 0)
			{
				StaffSalary objStaffSalary = new StaffSalary();
				objStaffSalary.SalaryMonth = Convert.ToDateTime(salaryMonth);
				objStaffSalary.StaffId = Convert.ToInt32(staffId);
				objStaffSalary.SalaryStatusId = Convert.ToInt32(salaryStatusId);
				objStaffSalary.UpdatedBy = _user.UserId;
				var result = await _APIHelper.CallApiAsyncPost<Response>(objStaffSalary, "api/PayrollConfiguration/PutStaffSalaryPaid_Hold", HttpMethod.Post);
				return Json(result);
			}
			return Json(null);
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
            if(_user == null)
            {
                return RedirectToAction("Loginpage", "User", new { id = 0 });
			}

            ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
            ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
            ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
            ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);

            ViewBag.Success = data;

            StaffSalary objStaffSalary = new StaffSalary();
            objStaffSalary.MonthNumber = month;
            objStaffSalary.Year = year;
                        
            objStaffSalary.StaffSalaryList = await _APIHelper.CallApiAsyncGet<IEnumerable<StaffSalary>>($"api/PayrollConfiguration/GetStaffSalaryByCompanyId/{_user.CompanyId}/{month}/{year}", HttpMethod.Get);
            objStaffSalary.SalaryMasterInserted = objStaffSalary.StaffSalaryList.FirstOrDefault().SalaryMasterInserted;
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

		public async Task<IActionResult> GetStaffSalaryCreateOrUpdate(int month, int year, int staffId)
        { 
            StaffSalary objStaffSalary = new StaffSalary();
			objStaffSalary.StaffSalaryEditList = await GetStaffSalaryById(month, year, staffId);

            objStaffSalary.StaffId = staffId;
			objStaffSalary.MonthNumber = month;
			objStaffSalary.Year = year;
			objStaffSalary.RegistrationNo = objStaffSalary.StaffSalaryEditList.FirstOrDefault()?.RegistrationNo;
			objStaffSalary.FirstName = objStaffSalary.StaffSalaryEditList.FirstOrDefault()?.FirstName;  
            objStaffSalary.LastName = objStaffSalary.StaffSalaryEditList.FirstOrDefault()?.LastName;
            objStaffSalary.DepartmentTitle = objStaffSalary.StaffSalaryEditList.FirstOrDefault()?.DepartmentTitle;
            objStaffSalary.GrossSalary = objStaffSalary.StaffSalaryEditList.FirstOrDefault()?.GrossSalary;

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
                else if (result.Message.Contains("Update"))
				{
                    return RedirectToAction("StaffSalaryList", new { data = 2, month = objStaffSalary.MonthNumber, year = objStaffSalary.Year });
                }
            }
			return RedirectToAction("StaffSalaryList", new { data = 0, month = objStaffSalary.MonthNumber, year = objStaffSalary.Year });
		}

        #endregion

        #region Staff Payroll History

        [CustomAuthorization]
        public async Task<IActionResult> PayrollHsitory(int staffId)
        {
            var currentDate = DateTime.Now;
            DateTime dateFrom = new DateTime(currentDate.Year, 1, 1).Date;
            DateTime dateTo = DateTime.Now;

            if (dateFrom != null && dateTo != null && _user.UserId > 0)
            {
                StaffSalary objStaffSalary = new StaffSalary();
                objStaffSalary.StaffSalaryList = await _APIHelper.CallApiAsyncGet<List<StaffSalary>>($"api/PayrollConfiguration/GetStaffSalaryHistory/{_user.CompanyId}/{dateFrom.ToString("dd-MMM-yyyy")}/{dateTo.ToString("dd-MMM-yyyy")}/{(staffId > 0 ? staffId : _user.StaffId)}", HttpMethod.Get);
                return View(objStaffSalary);
            }
            return null;
        }

        #endregion

    }
}
