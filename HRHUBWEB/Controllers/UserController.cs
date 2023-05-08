using HRHUBAPI.Models;
using HRHUBWEB.Extensions;
using HRHUBWEB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace HRHUBWEB.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _client;
    



        public UserController(IHttpClientFactory httpClient)
        {
            _client = httpClient.CreateClient("APIClient");
         
        }

    
        #region Login      


        [HttpGet]
       [AllowAnonymous]
        public async Task<IActionResult> Loginpage(int id=0)
        {
            User obj = new User();
            obj.status = id;
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

                            return RedirectToAction("Index", "Home");
                            

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
        public IActionResult Logout()
        {
            try
            {
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
            finally
            {
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


    
    
    }
}
