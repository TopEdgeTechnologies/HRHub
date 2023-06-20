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
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Microsoft.AspNetCore.Http;

namespace HRHUBWEB.Controllers
{
    public class PerformanceController : Controller
    {
        private readonly HttpClient _client;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly APIHelper _APIHelper;
        private readonly User _user;
        public PerformanceController(IHttpClientFactory httpClient, IWebHostEnvironment webHostEnvironment, APIHelper APIHelper, IHttpContextAccessor httpContextAccessor)
        {
            _client = httpClient.CreateClient("APIClient");
            _webHostEnvironment = webHostEnvironment;
            _APIHelper = APIHelper;
            _httpContextAccessor = httpContextAccessor;
            _user = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
        }

        #region PerformanceAppraisal
        [CustomAuthorization]
        public async Task<IActionResult> PerformanceAppraisalList(string data = "")
        {
            try
            {
                ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
                ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
                ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
                ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);

                StaffReviewFormProcessed obj = new StaffReviewFormProcessed();

                obj.ListStaffAppraisal = await _APIHelper.CallApiAsyncGet<IEnumerable<StaffReviewFormProcessed>>("api/Performance/GetPerformanceAppraisalInfos", HttpMethod.Get);
                ViewBag.UserId = _user.UserId;

                return View(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IActionResult> ViewPerformanceIndicatorDetail(int id, int staffid)
        {
            try
            {
                var list = await _APIHelper.CallApiAsyncGet<IEnumerable<Section>>($"api/Performance/GetPerformanceReviewSections{id}/{staffid}", HttpMethod.Get);
                return Json(list);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IActionResult> SaveStaffAppraisalDetail(List<Appraisal> list)
        {

            var result = await _APIHelper.CallApiAsyncPost<Response>(list, "api/Performance/PostStaffAppraisal", HttpMethod.Post);

            return Json(new
            {
                success = true,
                message = result.Message
            });


        }
        public async Task<IActionResult> EditStaffAppraisalDetail(Appraisal obj)
        {
            decimal AppraisalAmount = Convert.ToDecimal((obj.AppraisalPercentage * obj.CurrentMonthlySalary) / 100);
            obj.NewMonthlySalary = obj.CurrentMonthlySalary + AppraisalAmount;
            obj.CreatedBy = _user.UserId;
            var result = await _APIHelper.CallApiAsyncPost<Response>(obj, "api/Performance/EditStaffAppraisal", HttpMethod.Post);

            return Json(new
            {
                success = true,
                message = result.Message
            });

            //return RedirectToAction("PerformanceAppraisalList", new { data = 1 });

        }

        #endregion

        #region Performance
        [CustomAuthorization]
        public async Task<IActionResult> PerformanceList(String data = "")
        {


            ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
            ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
            ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
            ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);

            ViewBag.Success = data;
             PerformanceForm ObjPerformanceForm = new PerformanceForm();


            ObjPerformanceForm.ListPerformanceForm = await _APIHelper.CallApiAsyncGet<IEnumerable<PerformanceForm>>($"api/Performance/GetPerformanceInfos{_user.CompanyId}", HttpMethod.Get);

            return View(ObjPerformanceForm);
        }


        public async Task<IActionResult> GetPerformanceById(int id)
        {
            PerformanceForm ObjPerformanceForm = new PerformanceForm();

            ObjPerformanceForm = await _APIHelper.CallApiAsyncGet<PerformanceForm>($"api/Performance/GetPerformanceInfoId{id}", HttpMethod.Get);
            return Json(ObjPerformanceForm);
        }

        public async Task<IActionResult> PerformanceCreateOrUpdate(PerformanceForm ObjPerformanceForm)
        {
			ObjPerformanceForm.CompanyId = _user.CompanyId;
			ObjPerformanceForm.CreatedBy = _user.UserId;

            var result = await _APIHelper.CallApiAsyncPost<Response>(ObjPerformanceForm, "api/Performance/PerformanceAddOrUpdate", HttpMethod.Post);

            if (result.Message.Contains("Insert"))
            {
                return RedirectToAction("PerformanceList", new { data = 1 });
            }
            else
            {
                return RedirectToAction("PerformanceList", new { data = 2 });
            }
        }

        public async Task<IActionResult> PerformanceDelete(int id)
        {
            var result = await _APIHelper.CallApiAsyncGet<Response>($"api/Performance/DeletePerformanceInfo{id}/{_user.UserId}", HttpMethod.Get);
            return RedirectToAction("PerformanceList", new { data = 3 });
        }

        public async Task<ActionResult<JsonObject>> PerformanceCheckData(int id, string title)
        {
            var result = await _APIHelper.CallApiAsyncGet<Response>($"api/Performance/PerformanceCheckData{id}/{title}/{_user.CompanyId}", HttpMethod.Get);
            return Json(result);
        }

		#endregion


		#region Performance Sections
		[CustomAuthorization]
		public async Task<IActionResult> PerformanceSectionList(string data = "", int id = 0)
		{
			ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
			ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
			ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
			ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);

			ViewBag.Success = data;
            PerformanceForm ObjPerformanceForm = new PerformanceForm();
			ObjPerformanceForm = await _APIHelper.CallApiAsyncGet<PerformanceForm>($"api/Performance/GetPerformanceInfoId{id}", HttpMethod.Get);
            ViewBag.Name = ObjPerformanceForm.Title;


			Section ObjSection = new Section();
        


            ObjSection.ReviewFormId= id;
            ObjSection.AllSection = await _APIHelper.CallApiAsyncGet<IEnumerable<Section>>($"api/Performance/GetPerformanceSectionInfos{ObjSection.ReviewFormId}", HttpMethod.Get);

			return View(ObjSection);
		}


		private async Task<Section> GetPerformanceSectionById(int id)
		{
			Section ObjPerformanceForm = new Section();

			ObjPerformanceForm = await _APIHelper.CallApiAsyncGet<Section>($"api/Performance/GetPerformanceSectionInfoId{id}", HttpMethod.Get);
			return ObjPerformanceForm;
		}


        [HttpGet]
        public async Task<IActionResult> PerformanceSectionCreateOrUpdate(string data = "",int id =0  ,int ReviewFormId=0) 
        {
			ViewBag.Success = data;
			ViewBag.QuestionList = await _APIHelper.CallApiAsyncGet<IEnumerable<Question>>($"api/Performance/GetQuestionInfos{_user.CompanyId}", HttpMethod.Get);

           
            if (id == 0)
                {
                    Section objInfo = new Section();
                    objInfo.ReviewFormId = ReviewFormId;
                    return View(objInfo);
                }
                Section obj =  await GetPerformanceSectionById(id);
                ViewBag.QuestionSectionList = await _APIHelper.CallApiAsyncGet<IEnumerable<SectionQuestion>>($"api/Performance/GetQuestionSectionInfos{id}", HttpMethod.Get);

            return View(obj);

        }
        [HttpPost]
        public async Task<IActionResult> PerformanceSectionCreateOrUpdate(Section ObjSection)
		{
           
            ObjSection.CreatedBy = _user.UserId;

			var result = await _APIHelper.CallApiAsyncPost<Response>(ObjSection, "api/Performance/PerformanceSectionAddOrUpdate", HttpMethod.Post);

			if (result.Message.Contains("Insert"))
			{
				return RedirectToAction("PerformanceSectionList", new { data = 1, id = ObjSection.ReviewFormId });
			}
			else
			{
				return RedirectToAction("PerformanceSectionList", new { data = 2, id = ObjSection.ReviewFormId });
			}
		}

		public async Task<IActionResult> PerformanceSectionDelete(int id)
		{
			var result = await _APIHelper.CallApiAsyncGet<Section>($"api/Performance/DeletePerformanceSectionInfo{id}/{_user.UserId}", HttpMethod.Get);
			return RedirectToAction("PerformanceSectionList", new { data = 3,id = result.ReviewFormId });
		}

		public async Task<ActionResult<JsonObject>> PerformanceSectionCheckData(int id, string title)
		{
			var result = await _APIHelper.CallApiAsyncGet<Response>($"api/Performance/PerformanceSectionCheckData{id}/{title}", HttpMethod.Get);
			return Json(result);
		}

		#endregion

		
        #region Question 
		[HttpPost]
		public async Task<IActionResult> PerformanceQuestionCreateOrUpdate(Question ObjQuestion)
		{

			ObjQuestion.CompanyId = _user.CompanyId;
			ObjQuestion.CreatedBy = _user.UserId;

			var result = await _APIHelper.CallApiAsyncPost<Response>(ObjQuestion, "api/Performance/QuestionAddOrUpdate", HttpMethod.Post);
            return Json(result);

			//if (result.Message.Contains("Insert"))
			//{
			//	return RedirectToAction("PerformanceSectionCreateOrUpdate", new { data=1,id = 0 });
			//}
			//else
			//{
			//	return RedirectToAction("PerformanceSectionCreateOrUpdate", new { data=2,id = 0 });
			//}

		}



		public async Task<ActionResult<JsonObject>> PerformanceQuestionCheckData(int id, string title)
		{
			var result = await _APIHelper.CallApiAsyncGet<Response>($"api/Performance/QuestionCheckData{id}/{title}/{_user.CompanyId}", HttpMethod.Get);
			return Json(result);
		}



        #endregion


        #region Staff Performance Evaluation
        [CustomAuthorization]
        public async Task<IActionResult> StaffPerformanceList(String data = "")
        {


            ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
            ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
            ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
            ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);

            ViewBag.Success = data;
            PerformanceForm ObjPerformanceForm = new PerformanceForm();


            ObjPerformanceForm.ListPerformanceForm = await _APIHelper.CallApiAsyncGet<IEnumerable<PerformanceForm>>($"api/Performance/GetPerformanceInfos{_user.CompanyId}", HttpMethod.Get);

            return View(ObjPerformanceForm);


        }


        [CustomAuthorization]
        public async Task<IActionResult> StaffPerformanceDetail(String data = "", int ReviewFormId = 0)
        {

            ViewBag.ListAnswerSatff = await _APIHelper.CallApiAsyncGet<IEnumerable<StaffReviewFormProcessed>>($"api/Performance/GetStaffSectionAnswerList{ReviewFormId}/{_user.CompanyId}", HttpMethod.Get);

            ViewBag.IsNew = Convert.ToBoolean(TempData["IsNew"]);
            ViewBag.IsEdit = Convert.ToBoolean(TempData["IsEdit"]);
            ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
            ViewBag.IsPrint = Convert.ToBoolean(TempData["IsPrint"]);

            ViewBag.Success = data;
            StaffReviewFormProcessed ObjStaffReviewFormProcessed = new StaffReviewFormProcessed();
            return View(ObjStaffReviewFormProcessed);
        }

        public async Task<IActionResult> GetStaffPerformanceById(int id)
        {
            PerformanceForm ObjPerformanceForm = new PerformanceForm();

            ObjPerformanceForm = await _APIHelper.CallApiAsyncGet<PerformanceForm>($"api/Performance/GetPerformanceInfoId{id}", HttpMethod.Get);
            return Json(ObjPerformanceForm);
        }

        [HttpGet]
        public async Task<IActionResult> StaffPerformanceCreateOrUpdate(string data = "", int ReviewedStaffId = 0, int ReviewFormId = 0)
        {
            ViewBag.Success = data;
            SectionAnswer objInfo = new SectionAnswer();           
            ViewBag.UserStaffID = _user.StaffId;
            objInfo.StaffList = await _APIHelper.CallApiAsyncGet<Staff>($"api/Performance/GetStaffProfileId{ReviewedStaffId}", HttpMethod.Get);

            if (ReviewedStaffId == _user.StaffId) // it means staff itself opening his own performance fowm now load only self scoring sections
            {
                objInfo.ListSectionQuestion = await _APIHelper.CallApiAsyncGet<IEnumerable<SectionQuestion>>($"api/Performance/SectionQuestionSelfScoring{ReviewFormId}/{_user.CompanyId}", HttpMethod.Get);

            }
            else
            {
                objInfo.ListSectionQuestion = await _APIHelper.CallApiAsyncGet<IEnumerable<SectionQuestion>>($"api/Performance/GetSectionQuestionList{ReviewFormId}/{_user.CompanyId}", HttpMethod.Get);

            }



            objInfo.ReviewedStaffId = ReviewedStaffId;
               
                return View(objInfo);
         



        }

        [HttpGet]
        public async Task<IActionResult> StaffPerformanceUpdate(string data = "", int ReviewedStaffId = 0, int ReviewFormId = 0)
        
        {
            ViewBag.Success = data;
            SectionAnswer objInfo = new SectionAnswer();
            ViewBag.ReviewedId= ViewBag.RevieverId = _user.StaffId;

            ViewBag.UserStaffID = _user.StaffId;
           
            objInfo.StaffList = await _APIHelper.CallApiAsyncGet<Staff>($"api/Performance/GetStaffProfileId{ReviewedStaffId}", HttpMethod.Get);

            // objInfo.ListSectionQuestion = await _APIHelper.CallApiAsyncGet<IEnumerable<SectionQuestion>>($"api/Performance/GetSectionQuestionList{ReviewFormId}/{_user.CompanyId}", HttpMethod.Get);



            if (ReviewedStaffId == _user.StaffId) // it means staff itself opening his own performance fowm now load only self scoring sections
            {
                objInfo.ListSectionQuestion = await _APIHelper.CallApiAsyncGet<IEnumerable<SectionQuestion>>($"api/Performance/SectionQuestionSelfScoring{ReviewFormId}/{_user.CompanyId}", HttpMethod.Get);

            }
            else
            {
                objInfo.ListSectionQuestion = await _APIHelper.CallApiAsyncGet<IEnumerable<SectionQuestion>>($"api/Performance/GetSectionQuestionList{ReviewFormId}/{_user.CompanyId}", HttpMethod.Get);

            }


            objInfo.ListSectionAnswer = await _APIHelper.CallApiAsyncGet<IEnumerable<SectionAnswer>>($"api/Performance/StaffSectionAnswerList{_user.CompanyId}/{ReviewFormId}/{ReviewedStaffId}/{_user.StaffId}", HttpMethod.Get);


           
            // SelfScoringAvailableINDB purpose to do this is to check if self scoring answers already exist or not

            if (objInfo.ListSectionAnswer.Count() > 0 && objInfo.ListSectionAnswer.Count(x => x.SelfReviewExistance > 0) > 0)
            {
                objInfo.ReviewerStaffId = objInfo.ListSectionAnswer.ToArray()[0].ReviewerStaffId == 0 ? _user.StaffId : objInfo.ListSectionAnswer.ToArray()[0].ReviewerStaffId; // sir yha issue ha aik yha id nhi mily gi ek minute ok
            }
            else if (objInfo.ListSectionAnswer.Count() > 0 && objInfo.ListSectionAnswer.Count(x => x.AnswerId > 0) > 0)
            {
                objInfo.ReviewerStaffId = objInfo.ListSectionAnswer.ToArray()[0].ReviewerStaffId == 0 ? _user.StaffId : objInfo.ListSectionAnswer.ToArray()[0].ReviewerStaffId; // sir yha issue ha aik yha id nhi mily gi ek minute ok
            }
            
            objInfo.ReviewedStaffId = ReviewedStaffId;
           
            return View("StaffPerformanceCreateOrUpdate", objInfo);

        }


        [HttpPost]
        public async Task<IActionResult> StaffPerformanceCreateOrUpdate(SectionAnswer obj)
        {

            
              obj.CreatedBy = _user.UserId;
              obj.UpdatedBy = _user.UserId;
              obj.ReviewerStaffId = _user.StaffId;
              obj.ReviewerDesignationId = _user.DesignationID;
              obj.CompanyId = _user.CompanyId;
              

           var result = await _APIHelper.CallApiAsyncPost<Response>(obj, "api/Performance/PostStaffPerformance", HttpMethod.Post);


            if (result.Message.Contains("Insert"))
            {
                return RedirectToAction("StaffPerformanceList", new { data = 1 });
            }
            else
            {
                return RedirectToAction("StaffPerformanceList", new { data = 2 });
            }
        }

        
        public async Task<IActionResult> StaffPerformanceDelete(int id)
        {
            var result = await _APIHelper.CallApiAsyncGet<Response>($"api/Performance/DeletePerformanceInfo{id}/{_user.UserId}", HttpMethod.Get);
            return RedirectToAction("PerformanceList", new { data = 3 });
        }

        public async Task<ActionResult<JsonObject>> StaffPerformanceCheckData(int id, string title)
        {
            var result = await _APIHelper.CallApiAsyncGet<Response>($"api/Performance/PerformanceCheckData{id}/{title}/{_user.CompanyId}", HttpMethod.Get);
            return Json(result);
        }

        private async Task<SectionAnswer> GetPerformanceStaffById(int id)
        {
            SectionAnswer ObjSectionAnswer = new SectionAnswer();

            ObjSectionAnswer = await _APIHelper.CallApiAsyncGet<SectionAnswer>($"api/Performance/GetStaffPerformanceInfoId{id}", HttpMethod.Get);
            return ObjSectionAnswer;
        }

        // Show Individual Staff Performance All Sections
       
        public async Task<IActionResult> StaffPerformance(String data = "", int ReviewedStaffId = 0,int ReviewFormId=0)
        {

            SectionAnswer objInfo = new SectionAnswer();
            objInfo.ListSectionAnswer = await _APIHelper.CallApiAsyncGet<IEnumerable<SectionAnswer>>($"api/Performance/GetViewStaffPerformanceList{ReviewedStaffId}/{ReviewFormId}", HttpMethod.Get);
           
            objInfo.StaffList = await _APIHelper.CallApiAsyncGet<Staff>($"api/Performance/GetStaffProfileId{ReviewedStaffId}", HttpMethod.Get);

            return View(objInfo);
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