using HRHUBAPI.Models;
using HRHUBWEB.Extensions;
using HRHUBWEB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using System.Text.Json.Nodes;

namespace HRONBOARDING.Controllers
{
    public class CompanyController : Controller
    {
        private readonly HttpClient _client;       
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly APIHelper _APIHelper;
        private readonly User _user;

        public CompanyController(IHttpClientFactory httpClient,  APIHelper APIHelper, IHttpContextAccessor httpContextAccessor)
        {
            _client = httpClient.CreateClient("APIClient");           
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
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> OnBoard(IFormCollection my)
        {
            try
            {
                var Obj = new Company
                {
                    ContactPerson = my["ContactPerson"],
                    CompanyName = my["CompanyName"],
                    WebUrl = my["WebUrl"],
                    StaffDesignation = my["StaffDesignation"],
                    StaffDepartment = my["StaffDepartment"],
                    StaffEmail = my["StaffEmail"],
                    Username = my["Username"],
                    UserPassword = my["UserPassword"],
                    LogoAttachment = "~/Images/CompanyLogo.png"

                };


                var result = await _APIHelper.CallApiAsyncPost<Response>(Obj, $"api/Company/CompanyAddOrUpdate", HttpMethod.Post);

                return Redirect("https://localhost:7211/User/Loginpage");
            }
            catch (Exception)
            {

                throw;
            } 
          
           
        }



        // check duplicate company name
        [HttpGet]
        public async Task<ActionResult<JsonObject>> CompanyNameCheckData(int id, string companyName)
        {
            try
            {
                var result = await _APIHelper.CallApiAsyncGet<Response>($"api/Company/CompanyNameCheck{id}/{companyName}", HttpMethod.Get);
                return Json(result);
            }
            catch (Exception)
            {

                throw;
            }


        }





        // check duplicate company name
        [HttpGet]
        public async Task<ActionResult<JsonObject>> UserCheckData(int id, string username)
        {

            try
            {
                var result = await _APIHelper.CallApiAsyncGet<Response>($"api/User/UserCheckData{id}/{username}", HttpMethod.Get);
                return Json(result);
            }
            catch (Exception)
            {

                throw;
            }

        }








    }
}
