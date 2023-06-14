using HRHUBWEB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Diagnostics;

namespace HRHUBWEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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