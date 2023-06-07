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
                
                return View(obj);
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
             StaffContract ObjStaffContract = new StaffContract();


            ObjStaffContract.AllStaffContract = await _APIHelper.CallApiAsyncGet<IEnumerable<StaffContract>>($"api/Staffs/ListStaffAllContract{_user.CompanyId}", HttpMethod.Get);

            return View(ObjStaffContract);
        }


        public async Task<IActionResult> GetPerformanceById(int id)
        {
            StaffContract ObjStaffContract = new StaffContract();
            ObjStaffContract = await _APIHelper.CallApiAsyncGet<StaffContract>($"api/Staffs/GetStaffContractById{id}", HttpMethod.Get);
            return Json(ObjStaffContract);
        }

        public async Task<IActionResult> PerformanceCreateOrUpdate(IFormCollection MyAttachment, StaffContract ObjStaffContract)
        {
            var Attachment = MyAttachment.Files.GetFile("ContractAttachment");
            ObjStaffContract.CreatedBy = _user.UserId;

            var result = await _APIHelper.CallApiAsyncPost<Response>(ObjStaffContract, "api/Staffs/PostStaffContract", HttpMethod.Post);

            if (result.Message.Contains("Insert"))
            {
                return RedirectToAction("ContractList", new { data = 1 });
            }
            else
            {
                return RedirectToAction("ContractList", new { data = 2 });
            }
        }

        public async Task<IActionResult> PerformanceDelete(int id)
        {
            var result = await _APIHelper.CallApiAsyncGet<Response>($"api/Staffs/DeleteStaffContract{id}/{_user.UserId}", HttpMethod.Get);
            return RedirectToAction("ContractList", new { data = 3 });
        }

        public async Task<IActionResult> PerformanceAlreadyExist(int id, DateTime EndDate, int StaffId)
        {
            var result = await _APIHelper.CallApiAsyncGet<Response>($"api/Staffs/StaffContractAlreadyExists{id}/{EndDate.ToString("dd-MMM-yyyy")}/{StaffId}", HttpMethod.Get);
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