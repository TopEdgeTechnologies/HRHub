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
    }
}
