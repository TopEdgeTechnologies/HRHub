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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;

namespace HRHUBWEB.Controllers
{
    public class StaffController : Controller
    {
        private IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly APIHelper _APIHelper;
        private readonly User _user;

		public StaffController(IHttpClientFactory httpClientFactory, IWebHostEnvironment webHostEnvironment, APIHelper APIHelper, IHttpContextAccessor httpContextAccessor) 
        {
            _webHostEnvironment = webHostEnvironment;
            _APIHelper = APIHelper;
            _httpContextAccessor = httpContextAccessor;
            _user = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
        }

		#region Staff

		    [CustomAuthorization]
            public async Task<IActionResult> StaffList(String data = "", int Id = 0)
            {
                ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
                ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
                ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
                ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);

                ViewBag.Success = data;

                Staff objStaff = new Staff();

                if (Id > 0)
                {
                    objStaff = await GetStaffById(Id);
                }

                // Fill Main Staff List
                objStaff.CompanyId = _user.CompanyId;
                objStaff.StaffList = await _APIHelper.CallApiAsyncGet<IEnumerable<Staff>>($"api/Staffs/GetStaffByCompanyId{objStaff.CompanyId}", HttpMethod.Get);

                // Fill BI StaffStatisticsList
                var StaffStatistics = await _APIHelper.CallApiAsyncGet<Staff>($"api/Staffs/GetStaffStatisticsByCompanyId{objStaff.CompanyId}", HttpMethod.Get);
            
                objStaff.TotalActiveStaff = StaffStatistics.TotalActiveStaff;
                objStaff.TotalMaleStaff = StaffStatistics.TotalMaleStaff;
                objStaff.TotalFemaleStaff = StaffStatistics.TotalFemaleStaff;
                objStaff.TotalProbationStaff = StaffStatistics.TotalProbationStaff;

                // End Fill BI StaffStatisticsList

                return View(objStaff);
            }

            public async Task<IActionResult> StaffDetails(int Id)
            {
                Staff staff = await GetStaffById(Id);
                return View(staff);
            }

            public async Task<Staff> GetStaffById(int Id)
            {
                Staff objStaff = new Staff();
                if (Id > 0)
                {          
                    objStaff = await _APIHelper.CallApiAsyncGet<Staff>($"api/Staffs/GetStaffById{Id}", HttpMethod.Get);
                    return objStaff;
                }
                return new Staff();
            }

            public async Task<IActionResult> GetStaffCreateOrUpdate(int Id) 
            {
                //ViewBag.MaterialStatus = GetMaterialStatusList();
                //ViewBag.BloodGroup = GetBloodGroup();
                Staff objStaff = new Staff();
            
                // Edit & Update Mode
                objStaff = await GetStaffById(Id);

                // Fill BI StaffAttendanceStatisticsList
                int month = DateTime.Now.Month;
                int year = DateTime.Now.Year;

                var staffAttendanceStatistics = await _APIHelper.CallApiAsyncGet<Staff>($"api/Staffs/GetStaffAttendanceStatistics{_user.CompanyId}/{month}/{year}/{Id}", HttpMethod.Get);

                objStaff.MTDPresentCount = staffAttendanceStatistics.MTDPresentCount;
                objStaff.YTDPaidLeaveCount = staffAttendanceStatistics.YTDPaidLeaveCount;
                objStaff.TotalAllowedLeaves = staffAttendanceStatistics.TotalAllowedLeaves;
       
                objStaff.MonthDaysCount = staffAttendanceStatistics.MonthDaysCount;
                objStaff.MonthName = staffAttendanceStatistics.MonthName;
                objStaff.MonthNumber = month;
                objStaff.Year = year;
                // End Fill BI StaffAttendanceStatisticsList

                objStaff.MaterialStatusList = GetMaterialStatusList();
                objStaff.BloodGroupList = GetBloodGroup();
                objStaff.DepartmentList = await _APIHelper.CallApiAsyncGet<IEnumerable<Department>>($"api/Configuration/GetDepartmentByCompanyID{_user.CompanyId}", HttpMethod.Get);
                objStaff.DesignationList = await _APIHelper.CallApiAsyncGet<IEnumerable<Designation>>($"api/Configuration/GetDesignationInfos{_user.CompanyId}", HttpMethod.Get);
                objStaff.SalaryMethodList = await _APIHelper.CallApiAsyncGet<IEnumerable<SalaryMethod>>("api/PayrollConfiguration/GetSalaryMethod", HttpMethod.Get);
                //ViewBag.ObjDepartmentList = await _APIHelper.CallApiAsyncGet<IEnumerable<Department>>($"api/Configuration/GetDepartmentByCompanyID{_user.CompanyId}", HttpMethod.Get);
                //ViewBag.ObjDesignationList = await _APIHelper.CallApiAsyncGet<IEnumerable<Designation>>($"api/Configuration/GetDesignationInfos{_user.CompanyId}", HttpMethod.Get);
                //ViewBag.ObjSalaryMethodList = await _APIHelper.CallApiAsyncGet<IEnumerable<SalaryMethod>>("api/PayrollConfiguration/GetSalaryMethod", HttpMethod.Get);

                objStaff.StaffLeaveAllocationslist = await _APIHelper.CallApiAsyncGet<IEnumerable<StaffLeaveAllocation>>($"api/Staffs/GetStaffLeaveAllocationsDetail{_user.CompanyId}/{Id}", HttpMethod.Get);

                objStaff.StaffSalaryComponentList = await _APIHelper.CallApiAsyncGet<IEnumerable<StaffSalaryComponent>>($"api/Staffs/GetStaffSalaryDetail{_user.CompanyId}/{Id}", HttpMethod.Get);

                if (Id == 0)
                {
                    return View(objStaff);
                }

                //Staff Document Details
                objStaff.StaffAttachmentList = await _APIHelper.CallApiAsyncGet<IEnumerable<StaffAttachment>>($"api/Staffs/GetStaffDocumentDetail{Id}", HttpMethod.Get);
            
                return View(objStaff);
		    }

            public List<SelectListItem> GetMaterialStatusList() 
            {
                List<SelectListItem> listobj = new List<SelectListItem>();
                listobj.Add(new SelectListItem { Text = "Single", Value = "1" });
                listobj.Add(new SelectListItem { Text = "Married", Value = "2" });

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

            private void SaveStaffAttachmentsDetail(Staff objStaff)
            {
                List<StaffAttachment> ListobjDoc = new List<StaffAttachment>();
                int i = 0;
                int j = 0;

                foreach (var item in objStaff.DocumentTitle)
                {
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        //DocFiles
                        StaffAttachment objDoc = new StaffAttachment();

                        if (objStaff.DocumentPath != null && objStaff.DocumentPath.Count() > 0 && i < objStaff.DocumentPath.Count())
                        {
                            objDoc.DocumentPath = objStaff.DocumentPath.ToArray()[i];
                            objDoc.DocumentTitle = objStaff.DocumentTitle.ToArray()[i];
                            objDoc.IsDeleted = false;
                            objDoc.CreatedOn = DateTime.Now;
                            objDoc.CreatedBy = objStaff.CreatedBy;
                            ListobjDoc.Add(objDoc);
                        }
                        else
                        {
                            objDoc.DocumentPath = UploadImage(objStaff.DocumentTitle.ToArray()[i] + '-' + DateTime.Now.Ticks, objStaff.DocFiles.ToArray()[j], "StaffImages");
                            objDoc.DocumentTitle = objStaff.DocumentTitle.ToArray()[i];
                            objDoc.IsDeleted = false;
                            objDoc.CreatedOn = DateTime.Now;
                            objDoc.CreatedBy = objStaff.CreatedBy;
                            ListobjDoc.Add(objDoc);
                            j++;
                        }
                    }
                    i++;
                }

                /*
                List<string> myCollection = new List<string>();

                if (objStaff.DocumentPath != null && objStaff.DocumentPath.Count() > 0)
                {
                    foreach (var item in objStaff.DocumentPath)
                    {
                        myCollection.Add(item.ToString());

                    }
                }

                i = 0;
                if (objStaff.DocFiles != null && objStaff.DocFiles.Count() > 0)
                {

                    foreach (var item in objStaff.DocFiles)
                    {

                        myCollection.Add(UploadImage("StaffDoc-" + DateTime.Now.Ticks.ToString(), item, "StaffImages"));

                    }
                }

                foreach (var item in myCollection)
                {
                    StaffAttachment objDoc = new StaffAttachment();
                    objDoc.DocumentPath = item;
                    objDoc.DocumentTitle = objStaff.DocumentTitle.ToArray()[i];
                    objDoc.IsDeleted = false;
                    objDoc.CreatedOn = DateTime.Now;
                    objDoc.CreatedBy = objStaff.CreatedBy;
                    ListobjDoc.Add(objDoc);
                    i++;
                }
                */

                objStaff.DocFiles = null;
                objStaff.StaffAttachmentList = ListobjDoc;
           
            }

            public async Task<IActionResult> StaffCreateOrUpdate(IFormCollection objForm, Staff objStaff)
            {
                objStaff.CompanyId = _user.CompanyId;
			    objStaff.CreatedBy = _user.UserId;
                objStaff.CreatedOn = DateTime.Now;

                var staffImage = objForm.Files.GetFile("StaffImageAttachmentFile");
                objStaff.SnapPath = UploadImage(objStaff.FirstName, staffImage, "StaffImages");

                SaveStaffAttachmentsDetail(objStaff);

                var result = await _APIHelper.CallApiAsyncPost<Response>(objStaff, "api/Staffs/PostStaff", HttpMethod.Post);
                if (result.Message.Contains("Insert"))
                {
                    return RedirectToAction("StaffList", new { data = 1 });
                }
                else
                {
                    return RedirectToAction("StaffList", new { data = 2 });
                }
            }

            public async Task<IActionResult> StaffAlreadyExists(int Id, string NationalIdnumber="0")
            {
                var result = await _APIHelper.CallApiAsyncGet<Response>($"api/Staffs/StaffAlreadyExists/{_user.CompanyId}/{Id}/{NationalIdnumber}", HttpMethod.Get);
                return Json(result); 
		    }

        #endregion

        #region Staff Academic

        public async Task<IActionResult> StaffAcademicList(int staffId = 0)
        {
            StaffAcademic objStaffAcademic = new StaffAcademic();
            objStaffAcademic.StaffAcademicList = await _APIHelper.CallApiAsyncGet<IEnumerable<StaffAcademic>>($"api/Staffs/GetStaffAcademicByStaffId/{staffId}", HttpMethod.Get);
            objStaffAcademic.StaffId = staffId;
            return View(objStaffAcademic);
        }

        private void loadAcademicType()
        {
            List<SelectListItem> items = new List<SelectListItem>
            {
                new SelectListItem { Text = "Study", Value = "Study" },
                new SelectListItem { Text = "Work", Value = "Work" },
            };

            //ViewBag.academicTypeViewBag = items;
            ViewData["academicTypeViewData"] = items;
        }
        
        public async Task<IActionResult> StaffAcademicCreateOrUpdate(IFormCollection MyAttachment, StaffAcademic objStaffAcademic)
        {
            objStaffAcademic.CreatedBy = _user.UserId;

            var result = await _APIHelper.CallApiAsyncPost<Response>(objStaffAcademic, "api/Staffs/PostStaffAcademic", HttpMethod.Post);

            if(result.Success)
            {
                return RedirectToAction("StaffList", new { data = "1"});
            }
            return RedirectToAction("StaffAcademicList");
        }

        public async Task<IActionResult> StaffAcademicAlreadyExists(int StaffId, string Title)
        {
            if (StaffId > 0)
            {
                var result = await _APIHelper.CallApiAsyncGet<Response>($"api/Staffs/StaffAcademicAlreadyExists/{StaffId}/{Title}", HttpMethod.Get);
                return Json(result);
            }
            return Json(null);
        }


        #endregion

        #region Staff Dependent

        public async Task<IActionResult> StaffDependentList(int staffId = 0)
        {
            StaffDependent objStaffDependent = new StaffDependent();

            objStaffDependent.StaffDependentList = await _APIHelper.CallApiAsyncGet<IEnumerable<StaffDependent>>($"api/Staffs/GetStaffDependentByStaffId/{staffId}", HttpMethod.Get);
            objStaffDependent.StaffId = staffId;
            return View(objStaffDependent);
        }    

        public async Task<IActionResult> StaffDependentCreateOrUpdate(IFormCollection MyAttachment, StaffDependent objStaffDependent)
        {
            objStaffDependent.CreatedBy = _user.UserId;

            var result = await _APIHelper.CallApiAsyncPost<Response>(objStaffDependent, "api/Staffs/PostStaffDependent", HttpMethod.Post);

            if (result.Success)
            {
                return RedirectToAction("StaffList", new { data = "1" });
            }
            return RedirectToAction("StaffDependentList");
        }

        public async Task<IActionResult> StaffDependentAlreadyExists(int StaffId, string Title)
        {
            if (StaffId > 0)
            {
                var result = await _APIHelper.CallApiAsyncGet<Response>($"api/Staffs/StaffDependentAlreadyExists/{StaffId}/{Title}", HttpMethod.Get);
                return Json(result);
            }
            return Json(null);
        }


        #endregion

        #region Staff Profile

        public async Task<IActionResult> StaffProfile(int staffId = 0)
        {
         	var result = await _APIHelper.CallApiAsyncGet<VInfoStaff>($"api/Staffs/GetStaffProfilebyId{(staffId > 0 ? staffId : _user.StaffId)}", HttpMethod.Get);
			return View(result);
		}

		//[HttpGet]
		//public async Task<IActionResult> Staff_BioData()
		//{
		//	var result = await _APIHelper.CallApiAsyncGet<VInfoStaff>($"api/Staffs/GetStaffProfilebyId{_user.UserId}", HttpMethod.Get);
        //  return Json(result);
		//}

		[HttpGet]
		public async Task<ActionResult<JsonObject>> GetAttendanceData(int staffId)
		{
			var currentDate = DateTime.Now;
			DateTime foMonth = new DateTime(currentDate.Year, 1, 1).Date;
			DateTime currentMonth = new DateTime(currentDate.Year, currentDate.Month, 1).Date;
			DateTime eoMonth = currentMonth.AddMonths(1).AddDays(-1).Date;

			var result = await _APIHelper.CallApiAsyncGet<List<AttendanceMaster>>($"api/Attendance/GetStaffAttendanceList{(staffId > 0 ? staffId : _user.StaffId)}/{foMonth.ToString("yyyy-MM-dd")}/{eoMonth.ToString("yyyy-MM-dd")}", HttpMethod.Get);

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
				});
			}
		}

		[HttpGet]
		public async Task<ActionResult<JsonObject>> GetLeaveData(int staffId)
		{
            var currentDate = DateTime.Now;
			DateTime foMonth = new DateTime(currentDate.Year, currentDate.Month, 1).Date;
			DateTime eoMonth = foMonth.AddMonths(1).AddDays(-1).Date; 

			var result = await _APIHelper.CallApiAsyncGet<List<Leave>>($"api/Leave/GetLeaveInfos{_user.CompanyId}/{(staffId > 0 ? staffId : _user.StaffId)}", HttpMethod.Get);

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
				});
			}
		}

		[HttpGet]
		public async Task<IActionResult> Staff_Loan_Statistics(int staffId)
		{
			string procrdure = "BI.GetStaff_Loan_Statistics";
			object[] parameters = new object[] { _user.CompanyId ?? 0, (staffId > 0 ? staffId : _user.StaffId) };

			var result = await _APIHelper.CallApiDynamic<dynamic>(parameters, $"api/Dashboard/GetDashboardData{_user.CompanyId}/{procrdure}", HttpMethod.Get);
			return Json(result);
		}

		public async Task<ActionResult<JsonObject>> GetStaffSalaryHistory(int staffId)
		{
			var currentDate = DateTime.Now;
			DateTime dateFrom = new DateTime(currentDate.Year, 1, 1).Date;
			DateTime dateTo = DateTime.Now; 

			if (dateFrom != null && dateTo != null && _user.UserId > 0)
			{
				var staffSalaryHistoryList = await _APIHelper.CallApiAsyncGet<List<StaffSalary>>($"api/PayrollConfiguration/GetStaffSalaryHistory/{_user.CompanyId}/{dateFrom.ToString("dd-MMM-yyyy")}/{dateTo.ToString("dd-MMM-yyyy")}/{(staffId > 0 ? staffId : _user.StaffId)}", HttpMethod.Get);
				return Json(staffSalaryHistoryList);
			}
			return null;
		}

		[HttpGet]
		public async Task<IActionResult> Staff_Activity(int staffId, DateTime currentDate)
		{
			string procrdure = "BI.GetStaff_Activity";
			object[] parameters = new object[] { _user.CompanyId ?? 0, (staffId > 0 ? staffId : _user.StaffId), "'" + currentDate.ToString("yyyy-MMM-dd") + "'" };

			var result = await _APIHelper.CallApiDynamic<dynamic>(parameters, $"api/Dashboard/GetDashboardData{_user.CompanyId}/{procrdure}", HttpMethod.Get);
			return Json(result);
		}

		[HttpGet]
		public async Task<IActionResult> Staff_AttendanceChart(int staffId, int currentYear)
		{
			string procrdure = "BI.GetStaff_Profile_Attendance";
			object[] parameters = new object[] { _user.CompanyId ?? 0, (staffId > 0 ? staffId : _user.StaffId), currentYear };

			var result = await _APIHelper.CallApiDynamic<dynamic>(parameters, $"api/Dashboard/GetDashboardData{_user.CompanyId}/{procrdure}", HttpMethod.Get);
			return Json(result);
		}

		[HttpGet]
		public async Task<IActionResult> Staff_PerformanceChart(int staffId, int currentYear)
		{
			string procrdure = "BI.GetStaff_Performance";
			object[] parameters = new object[] { _user.CompanyId ?? 0, (staffId > 0 ? staffId : _user.StaffId), currentYear };

			var result = await _APIHelper.CallApiDynamic<dynamic>(parameters, $"api/Dashboard/GetDashboardData{_user.CompanyId}/{procrdure}", HttpMethod.Get);
			return Json(result);
		}

		#endregion

		#region Staff Contract

		// staff Contract By Waheed
		[CustomAuthorization]
        public async Task<IActionResult> ContractList(String data = "")
		{


			ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
			ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
			ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
			ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);

			ViewBag.Success = data;

			ViewBag.ObjDesignationList = await _APIHelper.CallApiAsyncGet<IEnumerable<Designation>>($"api/Configuration/GetDesignationInfos{_user.CompanyId}", HttpMethod.Get);

			ViewBag.StaffList = await _APIHelper.CallApiAsyncGet<IEnumerable<Staff>>($"api/Staffs/GetStaffByCompanyId{_user.CompanyId}", HttpMethod.Get);

			ViewBag.ContractType = await _APIHelper.CallApiAsyncGet<IEnumerable<EmploymentType>>($"api/Staffs/GetEmploymentTypeInfos{_user.CompanyId}", HttpMethod.Get);

			ViewBag.ExpireContract = await _APIHelper.CallApiAsyncGet<IEnumerable<StaffContract>>($"api/Staffs/ListStaffExpiredContract{_user.CompanyId}", HttpMethod.Get);
			StaffContract ObjStaffContract = new StaffContract();


			ObjStaffContract.AllStaffContract = await _APIHelper.CallApiAsyncGet<IEnumerable<StaffContract>>($"api/Staffs/ListStaffAllContract{_user.CompanyId}", HttpMethod.Get);

			return View(ObjStaffContract);
		}

		public async Task<IActionResult> GetContractById(int id)
		{
			StaffContract ObjStaffContract = new StaffContract();
			ObjStaffContract = await _APIHelper.CallApiAsyncGet<StaffContract>($"api/Staffs/GetStaffContractById{id}", HttpMethod.Get);
			return Json(ObjStaffContract);
		}

		public async Task<IActionResult> StaffContractCreateOrUpdate(IFormCollection MyAttachment, StaffContract ObjStaffContract)
		{
			var Attachment = MyAttachment.Files.GetFile("ContractAttachment");

			if (ObjStaffContract.Attachment == null && Attachment != null)
			{
				ObjStaffContract.Attachment = UploadImage(ObjStaffContract.ContractDuration, Attachment, "ContractAttachment");


			}
			if (ObjStaffContract.Attachment != null && Attachment != null)
			{
				ObjStaffContract.Attachment = UploadImage(ObjStaffContract.ContractDuration, Attachment, "ContractAttachment");


			}


			ObjStaffContract.CreatedBy = _user.UserId;

			var result = await _APIHelper.CallApiAsyncPost<Response>(ObjStaffContract, "api/Staffs/PostStaffContract", HttpMethod.Post);

			if (result.Message.Contains("Insert"))
			{
				return RedirectToAction("ContractList", new { data = 1 });
			}
			else
			{
				return RedirectToAction("ContractList", new { data = 2 });
			}
		}

		public async Task<IActionResult> StaffContractDelete(int id)
		{
			var result = await _APIHelper.CallApiAsyncGet<Response>($"api/Staffs/DeleteStaffContract{id}/{_user.UserId}", HttpMethod.Get);
			return RedirectToAction("ContractList", new { data = 3 });
		}

		public async Task<IActionResult> StaffContractAlreadyExist(int id, DateTime EndDate, int StaffId)
		{
			var result = await _APIHelper.CallApiAsyncGet<Response>($"api/Staffs/StaffContractAlreadyExists{id}/{EndDate.ToString("dd-MMM-yyyy")}/{StaffId}", HttpMethod.Get);
			return Json(result);
		}

		#endregion

		private string UploadImage(string name, IFormFile file, string root)
        {
            try
            {
                string fileName = string.Empty;
                if (file != null)
                {
                    var fileExtension = Path.GetExtension(file.FileName);
                    fileName = name.Trim() + "-" + DateTime.Now.Ticks + fileExtension;
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
            catch { throw; }
        }

    }
}
