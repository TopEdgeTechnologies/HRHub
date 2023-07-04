using HRHUBAPI.Models;
using HRHUBWEB.Extensions;
using HRHUBWEB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;

namespace HRONBOARDING.Controllers
{
    public class CompanyController : Controller
    {
        private readonly HttpClient _client;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly APIHelper _APIHelper;
        private readonly User _user;

        public CompanyController(IHttpClientFactory httpClient, IWebHostEnvironment webHostEnvironment, APIHelper APIHelper, IHttpContextAccessor httpContextAccessor)
        {
            _client = httpClient.CreateClient("APIClient");
            _webHostEnvironment = webHostEnvironment;
            _APIHelper = APIHelper;
            _httpContextAccessor = httpContextAccessor;
            _user = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
        }

        [HttpGet]
        public async Task<IActionResult> OnBoard()
        {
            return View();
        }

         [HttpPost]
        public async Task<IActionResult> OnBoard(Company ObjCompany)
        {

           
            var result = await _APIHelper.CallApiAsyncPost<Response>(ObjCompany, "api/Company/CompanyAddOrUpdate", HttpMethod.Post);
            ObjCompany.LogoAttachment = "~/Images/CompanyLogo.png";

            return View();
        }









        // check duplicate company name
        public async Task<ActionResult<JsonObject>> CompanyNameCheckData(int id, string companyName)
        {
            var result = await _APIHelper.CallApiAsyncGet<Response>($"api/Company/CompanyNameCheck{id}/{companyName}", HttpMethod.Get);
            return Json(result);


        }






    }
}
