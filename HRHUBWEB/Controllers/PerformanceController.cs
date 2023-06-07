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