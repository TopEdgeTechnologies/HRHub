using HRHUBAPI.Models;
using HRHUBWEB.Extensions;
using HRHUBWEB.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HRHUBWEB.Controllers
{
    public class SettingController : Controller
    {
        private readonly HttpClient _client;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly APIHelper _APIHelper;
        private readonly User _user;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SettingController(IHttpClientFactory httpClient, IWebHostEnvironment webHostEnvironment, APIHelper APIHelper, IHttpContextAccessor httpContextAccessor)
        {
            _client = httpClient.CreateClient("APIClient");
            _webHostEnvironment = webHostEnvironment;
            _APIHelper = APIHelper;
            _httpContextAccessor = httpContextAccessor;
            _user = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
        }

       
        public async Task<IActionResult> PayrollSettings(string data = "", int Id = 0)
        {
            ViewBag.Success = data;
           
            ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
            ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
            ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
            ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);


			ViewBag.ListPolicy = await _APIHelper.CallApiAsyncGet<IEnumerable<Policy>>("api/Policy/GetPolicyInfos", HttpMethod.Get);

			return View();
        }
    }
}
