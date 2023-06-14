using HRHUBAPI.Models;
using HRHUBWEB.Extensions;
using HRHUBWEB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Diagnostics;

namespace HRHUBWEB.Controllers
{
    public class HomeController : Controller
    {


        private readonly HttpClient _client;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly APIHelper _APIHelper;
        private readonly User _user;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HomeController(IHttpClientFactory httpClient, IWebHostEnvironment webHostEnvironment, APIHelper APIHelper, IHttpContextAccessor httpContextAccessor)
        {
            _client = httpClient.CreateClient("APIClient");
            _webHostEnvironment = webHostEnvironment;
            _APIHelper = APIHelper;
            _httpContextAccessor = httpContextAccessor;
            _user = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
        }

        public IActionResult Index()
        {

      
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        
        public IActionResult Test()
        {
            return View();
        }

        public async Task<IActionResult> ResignationStatus()
        {
            return View();
        }

        public async Task<IActionResult> StaffPerformance()
        {
            return View();
        }

        public async Task<IActionResult> StaffPerformanceEvaluation()
        {
            return View();
        }

        public async Task<IActionResult> GenerealSettings()
        {
            return View();
        }
        public async Task<IActionResult> AttendanceSettings()
        {
            return View();
        }

        public async Task<IActionResult> LeaveSettings()
        {
            return View();
        }
		[ResponseCache(NoStore = true)]
		public IActionResult PayrollSettings()
        {

            return View();
        }

        public async Task<IActionResult> SideModal()
        {
            return View();
        }

        public IActionResult PerformanceReview()
        {
            return View();
        }

    }
}