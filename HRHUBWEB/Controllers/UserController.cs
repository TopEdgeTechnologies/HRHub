using HRHUBAPI.Models;
using HRHUBWEB.Extensions;
using HRHUBWEB.Filters;
using HRHUBWEB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;

namespace HRHUBWEB.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _client;
        //   private readonly CacheExtensions _cacheExtensions;
        public UserController(IHttpClientFactory httpClient )
        {
            _client = httpClient.CreateClient("APIClient");
           // _cacheExtensions = cacheExtensions; 
        }

        #region Login      

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Loginpage(int id=0)
        {
            User obj = new User();
            obj.status = id;
           // _cacheExtensions.SetObject("UserDataKey", obj);

            return PartialView("_Login" , obj); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Loginpage(User user)
        {
            try
            {
               // Destroy();
                user.status = 1;
                HttpResponseMessage message = await _client.PostAsJsonAsync("api/User/Login", user);

                if (message.IsSuccessStatusCode)
                {
                    var body = message.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<Response>(body.Result);
					if (model.Success)
                    {
                        if (model.Message.Contains("Successfully"))
                        {
                            HttpContext.Session.SetObjectAsJson("AuthenticatedUser", model.Data);
                            HttpContext.Session.SetObjectAsJson("AuthenticatedToken", model.Token);
                            //  _cacheExtensions.SetObject("UserDataKey", model.Data);
                            return RedirectToAction("StaffDashboard", "Dashboard");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Loginpage", "User" , new {id=1 });
                    }
                }
                return RedirectToAction("Loginpage", "User", new { id = 1 });
            }
            catch (Exception)
            {
                return RedirectToAction("Loginpage", "User", new { id = 1 });
            }
        }

		[HttpPost]
		public async Task<IActionResult> passwordchange(string Password ,string OldPasword) 
		{
			try
			{


				var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
				var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
				_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);


				if (string.IsNullOrWhiteSpace( OldPasword ))
				{
					return Json(new
					{

						Message = "Please Enter Current Password !",
						Success = false
					});
				}


				if (string.IsNullOrWhiteSpace(Password))
				{
					return Json(new
					{

						Message = "Please Enter New Password !",
						Success = false
					});
				}


				if (OldPasword != userObject.Password)
				{
					return Json(new
					{

						Message = "Fail due old password incorrect",
						Success = false
					});
				}

				userObject.Password = Password;
				userObject.CreateBy = userObject.UserId;
                if (Token != null)
                {
                    HttpResponseMessage message = await _client.PostAsJsonAsync("api/User/UserChangePassword", userObject);

                    if (message.IsSuccessStatusCode)
                    {
                        var body = message.Content.ReadAsStringAsync();

                        var model = JsonConvert.DeserializeObject<Response>(body.Result);

                        return Json(model);


                    }else
                    {

						return Json( new { 
                        
                        Message="Fail ",
					    Success=false
						}  );

					}
                }
                else
                {
					return RedirectToAction("Loginpage", "User", new { id = 2 });
				} 
				
			}
			catch (Exception)
			{

				return Json(new
				{

					Message = "Fail",
					Success = false


				});
			}
		}
		#endregion

		#region Register

		[HttpGet]
        public async Task<IActionResult> Register()
        {
           
           User user =  new User();

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            try
            {

                HttpResponseMessage message = await _client.PostAsJsonAsync("api/User/Register", user);

                if (message.IsSuccessStatusCode)
                {
                    var body = message.Content.ReadAsStringAsync();

                    //var data=  JsonConvert.DeserializeObject(body.Result);
                    var model = JsonConvert.DeserializeObject<Response>(body.Result);
                    int status = 0;
                    if (model.Success)
                    {


                        if (model.Message.Contains("Register"))
                        {
                            status = 1;
                        }
                       


                    }

                    return RedirectToAction("HouseList", new { data = status });

                }
                return View();


            }
            catch (Exception)
            {

                return View();
            }
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            try
            {
				var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");

				HttpResponseMessage message = await _client.PostAsJsonAsync("api/User/UserLogOutHistory", userObject);

                if (message.IsSuccessStatusCode)
                {

                 }

				foreach (var cookie in Request.Cookies.Keys)
                {
                    Response.Cookies.Delete(cookie);
                }
                HttpContext.Session.Clear();
                return RedirectToAction("Loginpage", "User");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        } 
        //destroy session befor login user

        private void Destroy()
        {
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            HttpContext.Session.Clear();
        }





		#region System User

		[CustomAuthorization]
		public async Task<IActionResult> UserList(string data = "")
		{

			ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
			ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
			ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
			ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);


			ViewBag.Success = data;
			List<User> ObjUser = new List<User>();


			var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
			var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");

			if (Token != null)
			{

				HttpResponseMessage message = await _client.GetAsync($"api/User/ListUser{userObject.CompanyId}");
				if (message.IsSuccessStatusCode)
				{
					var result = message.Content.ReadAsStringAsync().Result;
					ObjUser = JsonConvert.DeserializeObject<List<User>>(result);

				}

				return View(ObjUser);
			}
			else
			{
				return RedirectToAction("Loginpage", "User", new { id = 2 });
			}
		}





        [HttpGet]
		public async Task<ActionResult<JsonObject>> UpdateUserStatus(int id, bool status)
		{

			var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
			var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");


			User Obj = new User();
			Obj.UserId = id;
			Obj.IsActive = status;
			Obj.UpdatedBy = userObject.UserId;




            //HttpResponseMessage message = await _client.PostAsJsonAsync("api/User/UpdateUsers", Obj);
            HttpResponseMessage message = await _client.GetAsync($"api/User/UpdateUsers{id}/{status}/{userObject.UserId}");

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
