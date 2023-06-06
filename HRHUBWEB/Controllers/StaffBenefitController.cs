using HRHUBAPI.Models;
using HRHUBWEB.Extensions;
using HRHUBWEB.Filters;
using HRHUBWEB.Models;
using Microsoft.AspNetCore.Mvc;

namespace HRHUBWEB.Controllers
{
	public class StaffBenefitController : Controller
	{
		private readonly HttpClient _client;
		private IWebHostEnvironment _webHostEnvironment;
		private readonly APIHelper _APIHelper;
		private readonly User _user;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public StaffBenefitController(IHttpClientFactory httpClient, IWebHostEnvironment webHostEnvironment, APIHelper APIHelper, IHttpContextAccessor httpContextAccessor)
		{
			_client = httpClient.CreateClient("APIClient");
			_webHostEnvironment = webHostEnvironment;
			_APIHelper = APIHelper;
			_httpContextAccessor = httpContextAccessor;
			_user = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
		}

        #region Staff Benefits       

        [CustomAuthorization]
		public async Task<IActionResult> BenefitList(string data = "",int Id=0)
		{
            ViewBag.Success = data;
            ComponentInfo objComponentInfo = new ComponentInfo();
			ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
			ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
			ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
			ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);

			ViewBag.ComponentList = await _APIHelper.CallApiAsyncGet<IEnumerable<ComponentInfo>>($"api/StaffBenefits/GetStaffBenefitInfos{_user.CompanyId}", HttpMethod.Get);

			return View(new ComponentInfo());
		}

        public async Task<IActionResult> BenefitCreateOrUpdate(ComponentInfo objComponentInfo)
        {
            objComponentInfo.CompanyId = _user.CompanyId;
            objComponentInfo.CreatedBy = _user.UserId;
            var result = await _APIHelper.CallApiAsyncPost<Response>(objComponentInfo, "api/StaffBenefits/PostStaffBenefitInfo", HttpMethod.Post);

            if (result.Message.Contains("Insert"))
            {
                return RedirectToAction("BenefitList", new { data = 1 });
            }
            else
            {
                return RedirectToAction("BenefitList", new { data = 2 });
            }
        }

        [CustomAuthorization]
        public async Task<IActionResult> BenefitDetails(string data = "", int Id = 0)
        {
            ViewBag.Success = data;
            ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
            ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
            ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
            ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);


            StaffSalaryComponent ObjComponentInfo = new StaffSalaryComponent();
            ObjComponentInfo.ComponentId= Id;
           // ObjComponentInfo = await _APIHelper.CallApiAsyncGet<ComponentInfo>($"api/StaffBenefits/GetStaffBenefitById/{Id}", HttpMethod.Get);
			ViewBag.StaffList = await _APIHelper.CallApiAsyncGet<IEnumerable<Staff>>($"api/Staffs/GetStaffByCompanyId{_user.CompanyId}", HttpMethod.Get);

            ViewBag.StaffSalaryList = await _APIHelper.CallApiAsyncGet<IEnumerable<StaffSalaryComponent>>($"api/StaffBenefits/GetSalaryComponent/{_user.CompanyId}/{ObjComponentInfo.ComponentId}", HttpMethod.Get);


            return View(ObjComponentInfo);
        }



        public async Task<IActionResult> BenefitInfoDelete(int Id)
        {
            var result = await _APIHelper.CallApiAsyncGet<Response>($"api/StaffBenefits/DeleteStaffBenefitInfo/{Id}/{_user.UserId}", HttpMethod.Get);
            return RedirectToAction("BenefitList", new { data = 3 });
        }

        public async Task<IActionResult> BenefitInfoAlreadyExists(int Id, string Title)
        {
            ComponentInfo objComponentInfo = new ComponentInfo();
            var result = await _APIHelper.CallApiAsyncGet<Response>($"api/StaffBenefits/StaffBenefitAlreadyExists/{Id}/{Title}/{_user.CompanyId}", HttpMethod.Get);
            return Json(result);
        }

        #endregion

        #region Staff Salary Component       

       

        public async Task<IActionResult> StaffSalaryCreateOrUpdate(StaffSalaryComponent objStaffSalaryComponent)
        {

            objStaffSalaryComponent.CreatedBy = _user.CreateBy;
            var result = await _APIHelper.CallApiAsyncPost<Response>(objStaffSalaryComponent, "api/StaffBenefits/PostStaffSalaryComponent", HttpMethod.Post);

            if (result.Message.Contains("Insert"))
            {
                return RedirectToAction("BenefitDetails", new { data = 1,Id= objStaffSalaryComponent.ComponentId });
            }
            else
            {
                return RedirectToAction("BenefitDetails", new { data = 2, Id = objStaffSalaryComponent.ComponentId });
            }
        }


        public async Task<IActionResult> StaffSalaryDetails(int Id)
        {
            StaffSalaryComponent ObjStaffSalaryComponent = new StaffSalaryComponent();
            ObjStaffSalaryComponent = await _APIHelper.CallApiAsyncGet<StaffSalaryComponent>($"api/StaffBenefits/GetStaffSalaryComponentById/{Id}", HttpMethod.Get);
            
			ViewBag.ComponentList = await _APIHelper.CallApiAsyncGet<IEnumerable<ComponentInfo>>($"api/StaffBenefits/GetStaffBenefitInfos{_user.CompanyId}", HttpMethod.Get);


            return Json(ObjStaffSalaryComponent);
        }



        public async Task<IActionResult> StaffSalaryInfoDelete(int Id)
        {
            var result = await _APIHelper.CallApiAsyncGet<StaffSalaryComponent>($"api/StaffBenefits/DeleteStaffSalaryComponent{Id}/{_user.UserId}", HttpMethod.Get);

            return RedirectToAction("BenefitDetails", new { data = 3 ,Id= result.ComponentId });

           
            
            
        }

        public async Task<IActionResult> StaffSalaryInfoAlreadyExists(int Id,int StaffId, int Componentid)
        {
           
            var result = await _APIHelper.CallApiAsyncGet<Response>($"api/StaffBenefits/StaffSalaryComponentAlreadyExists/{Id}/{StaffId}/{Componentid}", HttpMethod.Get);
            return Json(result);
        }

        #endregion




    }
}
