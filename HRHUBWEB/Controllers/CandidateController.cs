using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.ObjectModelRemoting;
using Newtonsoft.Json;
using NuGet.Common;
using HRHUBAPI.Models;
using HRHUBWEB.Extensions;
using HRHUBWEB.Filters;
using HRHUBWEB.Models;

using System.Net.Http.Headers;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Hosting;
using System.ComponentModel.Design;
using System;
using HRHUBAPI.BackGroundService;

namespace HRHUBWEB.Controllers
{
    public class CandidateController : Controller
    {
        private readonly HttpClient _client;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailHelper _EmailHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly APIHelper _APIHelper;
        private readonly User _user;


        public CandidateController(IHttpClientFactory httpClient, IWebHostEnvironment webHostEnvironment, IEmailHelper EmailHelper , APIHelper aPIHelper,
            IHttpContextAccessor httpContextAccessor)
        {
            _client = httpClient.CreateClient("APIClient");
            _webHostEnvironment = webHostEnvironment;
            _EmailHelper = EmailHelper;
            _APIHelper = aPIHelper;
            _httpContextAccessor = httpContextAccessor;
            _user = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
        }

        #region CandidateInfo

        



        [CustomAuthorization]
        public async Task<IActionResult> CandidateList(string data = "",int id=0)
        {
            
            ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
            ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
            ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
            ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);
          
            

            ViewBag.Success = data;
            Candidate ObjCandidate = new Candidate();


           var list= await _APIHelper.CallApiAsyncGet<IEnumerable<Candidate>>($"api/Candidate/GetCandidateInfos{_user.CompanyId}", HttpMethod.Get);
            ObjCandidate.ListCandidate = list.Where(x => x.StatusId == id);


            ObjCandidate.UrlRequestSatausID = id;
            ObjCandidate.CompanyId = _user.CompanyId;

            ViewBag.CandidateStatus = await _APIHelper.CallApiAsyncGet<IEnumerable<StatusInfo>>($"api/Candidate/GetCandidateStatusInfos", HttpMethod.Get);
            ViewBag.CandidateDesignation = await _APIHelper.CallApiAsyncGet<IEnumerable<Designation>>($"api/Configuration/GetDesignationInfos{_user.CompanyId}", HttpMethod.Get);


            return View(ObjCandidate);


           
        }


        // Load filter vise data from database
        [HttpPost]
        public async Task<IActionResult> SearchList(string Name, int DesignationId, int ExperienceId,int CompanyId, int id = 0)
        {


            Candidate objCandidate = new Candidate();

            var list = await _APIHelper.CallApiAsyncGet<IEnumerable<Candidate>>($"api/Candidate/SearchAllCandidates{Name}/{DesignationId}/{ExperienceId}/{_user.CompanyId}", HttpMethod.Get);
            var ListCandidate = list.Where(x => x.StatusId == id);
            objCandidate.UrlRequestSatausID = id;

          
            if (ListCandidate !=null)
            {
                

                return Json(new
                {
                    success = true,
                    Listcandidate = ListCandidate,

                });

            }

            else
            {
                return Json(new
                {
                    success = false,
                    Listcandidate = "",

                });
            }




  

    }


        public async Task<IActionResult> CandidateDetails(int id)
        {
            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            ViewBag.CandidateId = id;
            var CandidateId = id;

            ViewBag.CandidateSkill = await _APIHelper.CallApiAsyncGet<IEnumerable<CandidateSkill>>($"api/Candidate/GetCandidateSkillInfos{CandidateId}", HttpMethod.Get);

            Candidate ObjCandidate = await GetCandidatebyID(id);

            return View(ObjCandidate);
           
        }
        private async Task<Candidate> GetCandidatebyID(int id)
        {
           
            Candidate ObjCandidate = new Candidate();
            ObjCandidate = await _APIHelper.CallApiAsyncGet<Candidate>($"api/Candidate/GetCandidateInfoId{id}", HttpMethod.Get);

            return ObjCandidate;
        }
        [HttpGet]
        public async Task<IActionResult> CandidateCreateOrUpdate(int id)
        {
            //var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            //_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            ////Get Company ID through Sessions
            //var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
            ViewBag.CompanyId = _user.CompanyId;
            var CompanyId = ViewBag.CompanyId;
            var CandidateId = id;



            ViewBag.CandidateDesignation = await _APIHelper.CallApiAsyncGet<IEnumerable<Designation>>($"api/Configuration/GetDesignationInfos{CompanyId}", HttpMethod.Get);
            ViewBag.CandidateSkill = await _APIHelper.CallApiAsyncGet<IEnumerable<CandidateSkill>>($"api/Candidate/GetCandidateSkillInfos{CandidateId}", HttpMethod.Get);


            if (id == 0)
            {
                Candidate Info = new Candidate();

                return View(Info);
            }
            Candidate Candidateinfo = await GetCandidatebyID(id);

            return View(Candidateinfo);
            
        }
        [HttpPost]
        public async Task<IActionResult> CandidateCreateOrUpdate(IFormCollection my,Candidate ObjCandidate)
        {
            try
            {
                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

               
                var CandidateResume = my.Files.GetFile("CandidateResume");
                var CandidatePicture = my.Files.GetFile("CandidatePicture");
                if (ObjCandidate.AttachmentPath == null && CandidateResume != null)
                {
                    ObjCandidate.AttachmentPath = uploadImage(ObjCandidate.Name, CandidateResume, "CandidateAttachment");

                }
                if (ObjCandidate.AttachmentPath != null && CandidateResume != null)
                {
                    ObjCandidate.AttachmentPath = uploadImage(ObjCandidate.Name, CandidateResume, "CandidateAttachment");

                }
                if (ObjCandidate.Picture == null && CandidatePicture != null)
                {

                    ObjCandidate.Picture = uploadImage(ObjCandidate.Name, CandidatePicture, "CandidateImages");
                }

                if (ObjCandidate.Picture != null && CandidatePicture != null)
                {

                    ObjCandidate.Picture = uploadImage(ObjCandidate.Name, CandidatePicture, "CandidateImages");
                }
                var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
                ObjCandidate.CompanyId = _user.CompanyId;
                ObjCandidate.CreatedBy = _user.UserId;
                HttpResponseMessage message = await _client.PostAsJsonAsync("api/Candidate/CandidateAddOrCreate", ObjCandidate);

                if (message.IsSuccessStatusCode)
                {

                    var body = message.Content.ReadAsStringAsync();


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

                      //var result = await _EmailHelper.SendEmailAsync(ObjCandidate.Email, "", "Test email hello hello");

                    }

                    return RedirectToAction("CandidateList", new { data = status,id = 1 });

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
        public async Task<IActionResult> CandidateDelete(int id)
        {
            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
            Candidate candidateObj= new Candidate();            
            candidateObj.CreatedBy = userObject.UserId;
            var UserId = userObject.UserId;
            HttpResponseMessage message = await _client.DeleteAsync($"api/Candidate/DeleteCandidateInfo{id}/{UserId}");
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

                return RedirectToAction("CandidateList", new { data = status });

            }

            else
            {
                return RedirectToAction("Loginpage", "User",  new {id=2 });
            }

        }

        public async Task<ActionResult<JsonObject>> CandidateCheckData(int id, string email)
        {

            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
            var CompanyId = userObject.CompanyId;

            HttpResponseMessage message = await _client.GetAsync($"api/Candidate/CandidateCheckDataInfo{id}/{email}/{CompanyId}");
            if (message.IsSuccessStatusCode)
            {
                var result = message.Content.ReadAsStringAsync().Result;
                return Json(result);

            }

            else
            {
                return Json(new 

                {
                    Success = false,
                    Message = "Error occur"

                }
                );
              
            }
        }

       

       
       







        // Update Candidate Status here
        [HttpPost]
        public async Task<ActionResult<JsonObject>> CandidateStatusEdit(IFormCollection my,CandidateScreening obj)
        {

            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
            obj.CompanyId = userObject.CompanyId;
            obj.CreatedBy = userObject.UserId;


            // Get key valuse form iformCollection

            var modelobj = JsonConvert.DeserializeObject<CandidateScreening>(my["obj"]);
            modelobj.CreatedBy = userObject.UserId;

            /////////////////////////
            ///

            // Save attachment in database
            var CandidateAttachment = my.Files.GetFile("file");
            modelobj.AttachmentPath=uploadImage(modelobj.Remarks, CandidateAttachment, "CandidateAttachmentOffer");

            HttpResponseMessage message = await _client.PostAsJsonAsync($"api/Candidate/CandidateStatusUpdate", modelobj);
            if (message.IsSuccessStatusCode)
            {
                var result = message.Content.ReadAsStringAsync().Result;
                var candidateObj = JsonConvert.DeserializeObject<CandidateScreening>(result);


                // send email on status update 
                SendToCandidateEmail(candidateObj.CandidateId, candidateObj.StatusId);



                // Update list of candidates
                HttpResponseMessage message1 = await _client.GetAsync($"api/Candidate/GetCandidateInfos{userObject.CompanyId}");
                if (message.IsSuccessStatusCode)
                {
                    var result1 = message1.Content.ReadAsStringAsync().Result;
                   
                   
                    var ListCandidate = JsonConvert.DeserializeObject<List<Candidate>>(result1);

                    return Json(new
                    {
                        success =true ,
                        Listcandidate = ListCandidate,
                   
                    });

                }

                else
                {
                    return Json(new
                    {
                        success = false,
                        Listcandidate = "",

                    });
                }
               



            }

            else
            {
                return Json(new
                {
                    Success = false,
                    Message = "Error occur"

                }
                );

            }
        }




        public async Task<ActionResult<JsonObject>> GetCandidateScreening(int id)
        {

            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            HttpResponseMessage message = await _client.GetAsync($"api/Candidate/GetCandidateStatusdata{id}");
            if (message.IsSuccessStatusCode)
            {
                var result = message.Content.ReadAsStringAsync().Result;

                var ListCandidate = JsonConvert.DeserializeObject<List<CandidateScreening>>(result);

                return Json(ListCandidate);

            }

            else
            {
                return Json(new CandidateScreening() );

            }
        }




        #endregion


        #region SendEmailNotification
        
        
        
        private async void SendToCandidateEmail(int? CandidateId, int? StatusId)
        {
            
            Candidate ObjCandidate = new Candidate();
            EmailTemplate objEmail = new EmailTemplate();
            EmailNotificationSetting objEmailSetting = new EmailNotificationSetting();

           int? statusid = StatusId;

            if (statusid == 9)// it means its rejected now get rejected email template 
            {
                //


                ObjCandidate = await _APIHelper.CallApiAsyncGet<Candidate>($"api/Candidate/GetCandidateInfoId{CandidateId}", HttpMethod.Get);
                objEmailSetting = await _APIHelper.CallApiAsyncGet<EmailNotificationSetting>($"api/Setting/GetEmailNotificationSettingById{_user.CompanyId}", HttpMethod.Get);
                var Id = objEmailSetting.OnRejectionTemplateId;

                objEmail = await _APIHelper.CallApiAsyncGet<EmailTemplate>($"api/Setting/EmailTemplateById{Id}", HttpMethod.Get);
                var Email = await _EmailHelper.SendEmailAsync(ObjCandidate.Email, objEmail.Subject, objEmail.Body);

                //HttpResponseMessage message = await _client.GetAsync($"api/Candidate/GetCandidateInfoId{CandidateId}");
                //if (message.IsSuccessStatusCode)
                //{
                //    var result = message.Content.ReadAsStringAsync().Result;
                //    ObjCandidate = JsonConvert.DeserializeObject<Candidate>(result);

                //}


                //HttpResponseMessage Message = await _client.GetAsync($"api/Setting/GetEmailNotificationSettingById{CompanyId}");
                //if (Message.IsSuccessStatusCode)
                //{
                //    var result = Message.Content.ReadAsStringAsync().Result;
                //    objEmailSetting = JsonConvert.DeserializeObject<EmailNotificationSetting>(result);

                //}

                //HttpResponseMessage message1 = await _client.GetAsync($"api/Setting/EmailTemplateById{Id}");
                //if (message1.IsSuccessStatusCode)
                //{
                //    var result = message1.Content.ReadAsStringAsync().Result;
                //    objEmail = JsonConvert.DeserializeObject<EmailTemplate>(result);
                //}
                //   var Email = await _EmailHelper.SendEmailAsync(ObjCandidate.Email, objEmail.Subject, objEmail.Body);




            }
            //else if (statusid == 8) // it means its Offer Accepted now get Offer Accepted email template 
            //{
            //    HttpResponseMessage message = await _client.GetAsync($"api/Candidate/GetCandidateInfoId{CandidateId}");
            //    if (message.IsSuccessStatusCode)
            //    {
            //        var result = message.Content.ReadAsStringAsync().Result;
            //        ObjCandidate = JsonConvert.DeserializeObject<Candidate>(result);

            //    }


            //    HttpResponseMessage Message = await _client.GetAsync($"api/Setting/GetEmailNotificationSettingById{CompanyId}");
            //    if (Message.IsSuccessStatusCode)
            //    {
            //        var result = Message.Content.ReadAsStringAsync().Result;
            //        objEmailSetting = JsonConvert.DeserializeObject<EmailNotificationSetting>(result);

            //    }
            //    var Id = objEmailSetting.OnStatusChangeTemplateId;

            //    HttpResponseMessage message1 = await _client.GetAsync($"api/Setting/EmailTemplateById{Id}");
            //    if (message1.IsSuccessStatusCode)
            //    {
            //        var result = message1.Content.ReadAsStringAsync().Result;
            //        objEmail = JsonConvert.DeserializeObject<EmailTemplate>(result);
            //    }
            //    var Email = await _EmailHelper.SendEmailAsync(ObjCandidate.Email, objEmail.Subject, objEmail.Body);

            //}

            //else if (statusid == 2) // it means its Short List now get Short List email template 
            //{
            //    HttpResponseMessage message = await _client.GetAsync($"api/Candidate/GetCandidateInfoId{CandidateId}");
            //    if (message.IsSuccessStatusCode)
            //    {
            //        var result = message.Content.ReadAsStringAsync().Result;
            //        ObjCandidate = JsonConvert.DeserializeObject<Candidate>(result);

            //    }


            //    HttpResponseMessage Message = await _client.GetAsync($"api/Setting/GetEmailNotificationSettingById{CompanyId}");
            //    if (Message.IsSuccessStatusCode)
            //    {
            //        var result = Message.Content.ReadAsStringAsync().Result;
            //        objEmailSetting = JsonConvert.DeserializeObject<EmailNotificationSetting>(result);

            //    }
            //    var Id = objEmailSetting.OnStatusChangeTemplateId;

            //    HttpResponseMessage message1 = await _client.GetAsync($"api/Setting/EmailTemplateById{Id}");
            //    if (message1.IsSuccessStatusCode)
            //    {
            //        var result = message1.Content.ReadAsStringAsync().Result;
            //        objEmail = JsonConvert.DeserializeObject<EmailTemplate>(result);
            //    }
            //    var Email = await _EmailHelper.SendEmailAsync(ObjCandidate.Email, objEmail.Subject, objEmail.Body);

            //}
            //else if (statusid == 3) // it means its Interview now get Interview  email template 
            //{ 
            
            //}








        }
        #endregion












        // Code for save images into database

        private string uploadImage(string name, IFormFile file, string root)
        {

            try
            {
                string fileName = string.Empty;
                if (file != null)
                {
                    var fileExtension = Path.GetExtension(file.FileName);
                    fileName = name + "-" + DateTime.Now.Ticks + fileExtension;
                    var filepath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", root, fileName);

                    var OldpathImage = filepath;
                    if (System.IO.File.Exists(OldpathImage))
                    {
                        System.IO.File.Delete(OldpathImage);
                    }


                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return "/Images/" + root + "/" + fileName;    // Path.GetFullPath( filepath);// @"/Images/" + root + "/" + fileName;
                }
                else
                {

                    return "";
                }
            }
            catch (Exception ex)
            {

                throw;
            }


        }




















        

    }
}
