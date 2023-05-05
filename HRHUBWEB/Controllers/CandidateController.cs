﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.ObjectModelRemoting;
using Newtonsoft.Json;
using NuGet.Common;
using HRHUBAPI.Models;
using HRHUBWEB.Extensions;
using HRHUBWEB.Filters;
using HRHUBWEB.Models;

using System.Net.Http.Headers;
using System.Text.Json.Nodes;

namespace HRHUBWEB.Controllers
{
    public class CandidateController : Controller
    {
        private readonly HttpClient _client;

        public CandidateController(IHttpClientFactory httpClient)
        {
            _client = httpClient.CreateClient("APIClient");
        }

        #region CandidateInfo
        [CustomAuthorization]
        public async Task<IActionResult> CandidateList(string data = "")
        {

            ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
            ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
            ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
            ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);


            ViewBag.Success = data;
            List<Candidate> ObjCandidate = new List<Candidate>();

            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);



            HttpResponseMessage message = await _client.GetAsync("api/Candidate/GetCandidateInfos");
            if (message.IsSuccessStatusCode)
            {
                var result = message.Content.ReadAsStringAsync().Result;
                ObjCandidate = JsonConvert.DeserializeObject<List<Candidate>>(result);

            }
            else
            {
                return RedirectToAction("Loginpage", "User",  new {id=2 });
            }


            return View(ObjCandidate);
        }
        public async Task<IActionResult> CandidateDetails(int id)
        {
            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
           
            if(Token != null) { 

            Candidate ObjCandidate = await GetCandidatebyID(id);

            return View(ObjCandidate);
            }
            else
            {
                return RedirectToAction("Loginpage", "User",  new {id=2 });

            }
        }
        private async Task<Candidate> GetCandidatebyID(int id)
        {
            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            Candidate ObjCandidate = new Candidate();
            



            HttpResponseMessage message = await _client.GetAsync($"api/Candidate/GetCandidateId{id}");
            if (message.IsSuccessStatusCode)
            {
                var result = message.Content.ReadAsStringAsync().Result;
                ObjCandidate = JsonConvert.DeserializeObject<Candidate>(result);

            }

            return ObjCandidate;
        }
        [HttpGet]
        public async Task<IActionResult> CandidateCreateOrUpdate(int id)
        {
            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            //Get Instutute ID through Sessions
            var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
            var InstituteId = userObject.InstituteId;

            if (Token != null) { 


            //HttpResponseMessage message = await _client.GetAsync($"api/Configuration/GetSessions{InstituteId}");
            //if (message.IsSuccessStatusCode)
            //{
            //    var result = message.Content.ReadAsStringAsync().Result;
            //    ViewBag.CandidateSession = JsonConvert.DeserializeObject<List<Session>>(result);

            //}
            

            //HttpResponseMessage message1 = await _client.GetAsync($"api/Configuration/GetGroupInfos{InstituteId}");
            //if (message1.IsSuccessStatusCode)
            //{
            //    var result = message1.Content.ReadAsStringAsync().Result;
            //    ViewBag.CandidateGroup = JsonConvert.DeserializeObject<List<GroupInfo>>(result);

            //}
           
            

            //HttpResponseMessage message3 = await _client.GetAsync($"api/Configuration/GetClassInfos{InstituteId}");
            //if (message1.IsSuccessStatusCode)
            //{
            //    var result = message3.Content.ReadAsStringAsync().Result;
            //    ViewBag.ClassList = JsonConvert.DeserializeObject<List<ClassInfo>>(result);

            //}
           

            if (id == 0)
            {
                Candidate Info = new Candidate();
                
                return View(Info);
            }
            Candidate Candidateinfo = await GetCandidatebyID(id);

            return View(Candidateinfo);
            }
            else
            {
                return RedirectToAction("Loginpage", "User",  new {id=2 });

            }
        }
        [HttpPost]
        public async Task<IActionResult> CandidateCreateOrUpdate(Candidate ObjCandidate)
        {
            try
            {
                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);



                var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
                ObjCandidate.CreatedBy = userObject.UserId;
                //ObjCandidate.InstituteId = userObject.InstituteId;
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


                    }

                    return RedirectToAction("CandidateList", new { data = status });

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

            HttpResponseMessage message = await _client.DeleteAsync($"api/Candidate/DeleteCandidateInfo{id}");
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

          
            HttpResponseMessage message = await _client.GetAsync($"api/Candidate/CandidateCheckData{id}/{email}");
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







        #endregion


//        #region CandidateSubject
//        [CustomAuthorization]
//        public async Task<IActionResult> SubjectList(string data = "")
//        {

//            ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
//            ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
//            ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
//            ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);


//            ViewBag.Success = data;
//            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
//            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);


//            List<ViewCandidateClassSubject> CandidateSubject = new List<ViewCandidateClassSubject>();

//            HttpResponseMessage message = await _client.GetAsync("api/Candidate/GetCandidateSubjects");
//            if (message.IsSuccessStatusCode)
//            {
//                var result = message.Content.ReadAsStringAsync().Result;
//                CandidateSubject = JsonConvert.DeserializeObject<List<ViewCandidateClassSubject>>(result);

//            }
//            else
//            {
//                return RedirectToAction("Loginpage", "User",  new {id=2 });
//            }

//            return View(CandidateSubject);
//        }
//        public async Task<IActionResult> SubjectDetails(int id)
//        {
//            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
//            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);


//            if (Token != null) { 
//            CandidateClassSubject CandidateSubject = await GetSubjectbyID(id);

//            return View(CandidateSubject);
//             }
//            else
//            {
//                return RedirectToAction("Loginpage", "User",  new {id=2 });

//            }
//}
//        private async Task<CandidateClassSubject> GetSubjectbyID(int id)
//        {
//            CandidateClassSubject CandidateSubject = new CandidateClassSubject();
//            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
//            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
//            HttpResponseMessage message = await _client.GetAsync($"api/Candidate/GetCandidateSubjectId{id}");
//            if (message.IsSuccessStatusCode)
//            {
//                var result = message.Content.ReadAsStringAsync().Result;
//                CandidateSubject = JsonConvert.DeserializeObject<CandidateClassSubject>(result);

//            }

//            return CandidateSubject;
//        }
//        [HttpGet]
//        public async Task<IActionResult> CandidateSubject(int id)
//        {

//            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
//            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

//            //Get Instutute ID through Sessions
//            var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
//            var InstituteId = userObject.InstituteId;


//            if (Token !=null) { 


//            HttpResponseMessage message = await _client.GetAsync($"api/Configuration/GetClassInfos{InstituteId}");
//            if (message.IsSuccessStatusCode)
//            {
//                var result = message.Content.ReadAsStringAsync().Result;
//                ViewBag.Classlist = JsonConvert.DeserializeObject<List<ClassInfo>>(result);

//            }
            
//            HttpResponseMessage message1 = await _client.GetAsync($"api/Configuration/GetGroupInfos{InstituteId}");
//            if (message1.IsSuccessStatusCode)
//            {
//                var result = message1.Content.ReadAsStringAsync().Result;
//                ViewBag.Grouplist = JsonConvert.DeserializeObject<List<GroupInfo>>(result);

//            }
            
//            HttpResponseMessage message2 = await _client.GetAsync($"api/Configuration/GetSubjectInfos{InstituteId}");
//            if (message2.IsSuccessStatusCode)
//            {
//                var result = message2.Content.ReadAsStringAsync().Result;
//                ViewBag.Subjectlist = JsonConvert.DeserializeObject<List<SubjectInfo>>(result);

//            }
            
//            HttpResponseMessage message3 = await _client.GetAsync("api/Configuration/GetLanguage");
//            if (message3.IsSuccessStatusCode)
//            {
//                var result = message3.Content.ReadAsStringAsync().Result;
//                ViewBag.Languagelist = JsonConvert.DeserializeObject<List<LanguageType>>(result);

//            }
           
//            HttpResponseMessage message4 = await _client.GetAsync($"api/Configuration/GetClassLevels{InstituteId}");
//            if (message4.IsSuccessStatusCode)
//            {
//                var result = message4.Content.ReadAsStringAsync().Result;
//                ViewBag.LevelList = JsonConvert.DeserializeObject<List<ClassLevel>>(result);

//            }
           


//            if (id == 0)
//            {
//                CandidateClassSubject Info = new CandidateClassSubject();
//                return View(Info);
//            }
//            CandidateClassSubject Candidateinfo = await GetSubjectbyID(id);

//            return View(Candidateinfo);
//            }
//            else
//            {
//                return RedirectToAction("Loginpage", "User",  new {id=2 });
//            }

//        }
//        [HttpPost]
//        public async Task<IActionResult> CandidateSubject(List<CandidateClassSubject> rows)
//        {
//            try
//            {
//                var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
//                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);


//                var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");

//                foreach (var item in rows)
//                {
//                item.CreateBy = userObject.UserId;
//                item.InstituteId = userObject.InstituteId;

//                }

//                HttpResponseMessage message = await _client.PostAsJsonAsync("api/Candidate/CandidateSubjectAddOrCreate", rows);

//                    if (message.IsSuccessStatusCode)
//                    {

//                        var body = message.Content.ReadAsStringAsync();


//                        var model = JsonConvert.DeserializeObject<Response>(body.Result);


//                        int status = 0;
//                        if (model.Success)
//                        {


//                            if (model.Message.Contains("Insert"))
//                            {
//                                status = 1;
//                            }
//                            else if (model.Message.Contains("Update"))
//                            {
//                                status = 2;
//                            }

//                        TempData["OperationStatus"] = 1;
//                        }

//                        return RedirectToAction("SubjectList", new { data = status });

//                    }
//                else
//                {
//                    return RedirectToAction("Loginpage", "User",  new {id=2 });
//                }


//            }
//            catch (Exception)
//            {

//                return View();
//            }
//        }
//        public async Task<IActionResult> DeleteCandidateSubjectByClassId(int id)
//        {
//            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
//            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

//            HttpResponseMessage message = await _client.DeleteAsync($"api/Candidate/DeleteCandidateClassSubjectByClassId{id}");
//            if (message.IsSuccessStatusCode)
//            {
//                var body = message.Content.ReadAsStringAsync();

//                var model = JsonConvert.DeserializeObject<Response>(body.Result);


//                int status = 0;
//                if (model.Success)
//                {


//                    if (model.Message.Contains("Delete"))
//                    {
//                        status = 3;
//                    }



//                }

//                return RedirectToAction("SubjectList", new { data = status });

//            }

//            else
//            {
//                return RedirectToAction("Loginpage", "User",  new {id=2 });
//            }
//        }






//        /// <summary>
//        /// Bind ,Multiples dropdwons here
//        /// </summary>





//        //load subject data in dropdown by language ID
//        public async Task<IActionResult> LoadClassByLanguageId(int id)
//        {
//            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
//            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);


//            List<SubjectInfo> cc = new List<SubjectInfo>();

//            HttpResponseMessage message = await _client.GetAsync($"api/Candidate/GetSubjectLanguageId{id}");
//            if (message.IsSuccessStatusCode)
//            {
//                var result = message.Content.ReadAsStringAsync().Result;
//                cc = JsonConvert.DeserializeObject<List<SubjectInfo>>(result);

//            }
//            else
//            {
//                return RedirectToAction("Loginpage", "User",  new {id=2 });
//            }

//            return Json(cc);

//        }
//        //load class data in dropdown by level ID
//        public async Task<IActionResult> LoadClassByLevelId(int id)
//        {
//            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
//            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

//            List<ClassInfo> cc = new List<ClassInfo>();
//            HttpResponseMessage message = await _client.GetAsync($"api/Candidate/GetClassByLevelId{id}");
//            if (message.IsSuccessStatusCode)
//            {
//                var result = message.Content.ReadAsStringAsync().Result;
//                cc = JsonConvert.DeserializeObject<List<ClassInfo>>(result);

//            }
//            else
//            {
//                return RedirectToAction("Loginpage", "User",  new {id=2 });
//            }

//            return Json(cc);

//        }
//        //load subject data in dropdown by group Id
//        public async Task<IActionResult> LoadClassByGroupId(int id, int groupId)
//        {
//            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
//            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

//            List<SubjectInfo> cc = new List<SubjectInfo>();
//            HttpResponseMessage message = await _client.GetAsync($"api/Candidate/GetSubjectByGroupId{id}/{groupId}");
//            if (message.IsSuccessStatusCode)
//            {
//                var result = message.Content.ReadAsStringAsync().Result;
//                cc = JsonConvert.DeserializeObject<List<SubjectInfo>>(result);

//            }
//            else
//            {
//                return RedirectToAction("Loginpage", "User",  new {id=2 });
//            }

//            return Json(cc);

//        }

//        //load Candidatesubject data in dropdown by Class ID
//        public async Task<IActionResult> LoadDataByClassId(int id)
//        {
//            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
//            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

//            List<CandidateClassSubject> cc = new List<CandidateClassSubject>();
//            HttpResponseMessage message = await _client.GetAsync($"api/Candidate/LoadDataByClassId{id}");
//            if (message.IsSuccessStatusCode)
//            {
//                var result = message.Content.ReadAsStringAsync().Result;
//                cc = JsonConvert.DeserializeObject<List<CandidateClassSubject>>(result);

//            }
//            else
//            {
//                return RedirectToAction("Loginpage", "User",  new {id=2 });
//            }

//            return Json(cc);

//        }

//        //Delete single row Data from table       
//        public async Task<IActionResult> DeleteCandidateSubject(int id)
//        {
//            var Token = HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");
//            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

//            List<CandidateClassSubject> cc = new List<CandidateClassSubject>();
//            HttpResponseMessage message = await _client.GetAsync($"api/Candidate/DeleteCandidateSubject{id}");
//            if (message.IsSuccessStatusCode)
//            {
//                var result = message.Content.ReadAsStringAsync().Result;
//                cc = JsonConvert.DeserializeObject<List<CandidateClassSubject>>(result);

//            }
//            else
//            {
//                return RedirectToAction("Loginpage", "User",  new {id=2 });
//            }

//            return Json(cc);

//        }


//        #endregion


    }
}
