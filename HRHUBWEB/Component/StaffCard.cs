using HRHUBAPI.BackGroundService;
using HRHUBAPI.Models;
using HRHUBWEB.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace HRHUBWEB.Component
{
    public class StaffCard : ViewComponent
    {

        private IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly APIHelper _APIHelper;
        private readonly User _user;
        public  StaffCard(IHttpClientFactory httpClient, IWebHostEnvironment webHostEnvironment, IEmailHelper EmailHelper, APIHelper aPIHelper,
            IHttpContextAccessor httpContextAccessor)
        {
            _webHostEnvironment = webHostEnvironment;
            _APIHelper = aPIHelper;
            _httpContextAccessor = httpContextAccessor;
            _user = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var list = await _APIHelper.CallApiAsyncGet<VInfoStaff>($"api/Staffs/GetStaffProfilebyId{id}", HttpMethod.Get);

            return View("_StaffCard", list);
        }



        }
}
