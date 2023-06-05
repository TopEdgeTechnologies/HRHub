using HRHUBAPI.Models;
using HRHUBWEB.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace HRHUBWEB.Controllers
{
    public class DashboardController : Controller
    {
        private IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly APIHelper _APIHelper;
        private readonly User _user;

        public DashboardController(IHttpClientFactory httpClientFactory, IWebHostEnvironment webHostEnvironment, APIHelper APIHelper, IHttpContextAccessor httpContextAccessor)
        {
            _webHostEnvironment = webHostEnvironment;
            _APIHelper = APIHelper;
            _httpContextAccessor = httpContextAccessor;
            _user = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
        }
        // // Dashboad/GetData
        public async Task<IActionResult> GetData()
        {
            object[] parameters = new object[] { 1 };  
            string procrdure = "dbo.sp_GetUserRightByUser";
            var result = await _APIHelper.CallApiDynamic<dynamic>(parameters,  $"api/Dashboard/GetDashboardData{_user.CompanyId}/{procrdure}", HttpMethod.Get);
            return Json(result);
        }
    }
}
