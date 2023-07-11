using HRHUBAPI.Models;
using HRHUBWEB.Extensions;
using HRHUBWEB.Filters;
using HRHUBWEB.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;

namespace HRHUBWEB.Controllers
{
    public class UserGroupController : Controller
    {
        private readonly HttpClient _client;

        public UserGroupController(IHttpClientFactory httpClient)
        {
            _client = httpClient.CreateClient("APIClient");
        }

        #region UserGroupController

        [CustomAuthorization]
        public async Task<IActionResult> UserGroupList(string data = "")
        {
            ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
            ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
            ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
            ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);

            var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");

            ViewBag.Success = data;
            List<GluserGroup> ObjUserGroup = new List<GluserGroup>();

            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            if (Token != null)
            {
                HttpResponseMessage message = await _client.GetAsync($"api/UserGroup/ListUserGroup{userObject.CompanyId}");
                if (message.IsSuccessStatusCode)
                {
                    var result = message.Content.ReadAsStringAsync().Result;
                    ObjUserGroup = JsonConvert.DeserializeObject<List<GluserGroup>>(result);
                }
                return View(ObjUserGroup);
            }
            else
            {
                return RedirectToAction("Loginpage", "User", new { id = 2 });
            }
        }

        public async Task<IActionResult> UserGroupDetails(int id)
        {
            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            if(Token != null) { 

            GluserGroup ObjUserGroup = await GetUserGroupID(id);

            return View(ObjUserGroup);
            }
            else
            {
                return RedirectToAction("Loginpage", "User",  new {id=2 });
            }
        }

        private async Task<GluserGroup> GetUserGroupID(int id)
        {
            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            GluserGroup ObjUserGroup = new GluserGroup();

            HttpResponseMessage message = await _client.GetAsync($"api/UserGroup/GetUserGroupId{id}");
            if (message.IsSuccessStatusCode)
            {
                var result = message.Content.ReadAsStringAsync().Result;
                ObjUserGroup = JsonConvert.DeserializeObject<GluserGroup>(result);
            }
            return ObjUserGroup;
        }

        [HttpGet]
        public async Task<IActionResult> UserGroupCreateOrUpdate(int id)
        {
            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
			GluserGroup ObjUserGroup = new GluserGroup();
			ObjUserGroup = await GetUserGroupID(id);
			if (Token !=null) {

                HttpResponseMessage message = await _client.GetAsync($"api/UserGroup/GetUserGroupDetails{id}");

                if (message.IsSuccessStatusCode)
                {
                    var result = message.Content.ReadAsStringAsync().Result;
					ObjUserGroup.ListGluserGroupDetail = JsonConvert.DeserializeObject<List<GluserGroupDetail>>(result);					
				}

                if (id == 0)
                {
                    return View(ObjUserGroup);
                }		
                return View(ObjUserGroup);
            }
            else
            {
                return RedirectToAction("Loginpage", "User",  new {id=2 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UserGroupCreateOrUpdate(GluserGroup ObjUserGroup , IFormCollection data)
        {
            try
            {
                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
                ObjUserGroup.CreateBy = userObject.UserId;
                _client.Timeout = Timeout.InfiniteTimeSpan;
                HttpResponseMessage message = await _client.PostAsJsonAsync("api/UserGroup/UserGroupAddOrCreate", ObjUserGroup);

                if (message.IsSuccessStatusCode)
                {
                    var body = message.Content.ReadAsStringAsync();

                    //var data=  JsonConvert.DeserializeObject(body.Result);
                    var model = JsonConvert.DeserializeObject<Response>(body.Result);
                    int status = 0;
                    if (model.Success)
                    {


                        if (model.Message.Contains("Insert"))
                        {
                            status = 1;
                        }
                        else if (model.Message.Contains("Update"))
                        {
                            status = 2;
                        }


                    }

                    return RedirectToAction("UserGroupList", new { data = status });

                }
                else
                {
                    return RedirectToAction("Loginpage", "User",  new {id=2 });
                }


            }
            catch (Exception)
            {

                return View();
            }
        }

        public async Task<IActionResult> DeleteStaffType(int id)
        {
            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);


            HttpResponseMessage message = await _client.DeleteAsync($"api/UserGroup/UserGroupDelete{id}");
            if (message.IsSuccessStatusCode)
            {
                var body = message.Content.ReadAsStringAsync();

                var model = JsonConvert.DeserializeObject<Response>(body.Result);


                int status = 0;
                if (model.Success)
                {


                    if (model.Message.Contains("Delete"))
                    {
                        status = 3;
                    }


                }

                return RedirectToAction("StaffTypeList", new { data = status });


            }

            else
            {
                return RedirectToAction("Loginpage", "User",  new {id=2 });
            }
        }

        public async Task<ActionResult<JsonObject>> CheckDublicateUserGroup(int id, string title)
        {

            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);



            HttpResponseMessage message = await _client.GetAsync($"api/UserGroup/UserGroupCheckData{id}/{title}");
            if (message.IsSuccessStatusCode)
            {
                var result = message.Content.ReadAsStringAsync().Result;
                return Json(result);

            }
            else
            {
                return RedirectToAction("Loginpage", "User",  new {id=2 });
            }


        }

        [HttpGet]
        public async Task<ActionResult<JsonObject>> UpdateUserGroupStatus(int id, bool status)
        {

            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");



           
            HttpResponseMessage message = await _client.GetAsync($"api/UserGroup/UpdateGroupUsers{id}/{status}/{userObject.UserId}");

            if (message.IsSuccessStatusCode)
            {
                var result = message.Content.ReadAsStringAsync().Result;
                return Json(result);

            }
            else
            {
                return RedirectToAction("Loginpage", "User", new { id = 2 });
            }


        }

        #endregion

    }
}
