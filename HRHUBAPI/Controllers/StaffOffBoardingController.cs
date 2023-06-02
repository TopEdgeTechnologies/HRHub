using HRHUBAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using System.Text.Json.Nodes;

namespace HRHUBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StaffOffBoardingController : ControllerBase
    {
        private readonly HrhubContext _context;

        public StaffOffBoardingController(HrhubContext context)
        {
            _context = context;

        }

        #region StaffOffBoardingInfo

        [HttpGet("GetOffBoardingTypeInfos")]
        public async Task<ActionResult<List<OffBoardingType>>> GetOffBoardingTypeInfos()
        {

            return await new StaffOffBoarding().GetOffBoardingType(_context);
        }

        //[HttpGet("GetStaffOffBoardingActionInfo{CompanyId}")]
        //public async Task<ActionResult<List<StaffOffBoarding>>> GetStaffOffBoardingActionInfo(int CompanyId)
        //{

        //    return await new StaffOffBoarding().GetStaffOffBoardingAction(CompanyId, _context);
        //}

        [HttpGet("GetStaffOffBoardingInfos{CompanyId}")]
        public async Task<ActionResult<List<StaffOffBoarding>>> GetStaffOffBoardingInfos(int CompanyId)
        {
            return await new StaffOffBoarding().GetStaffOffBoarding(CompanyId, _context);
        }

        //[HttpGet("GetNewOrPendingStaffOffBoardingInfos{CompanyId}")]
        //public async Task<ActionResult<List<StaffOffBoarding>>> GetNewOrPendingStaffOffBoardingInfos(int CompanyId)
        //{

        //    return await new StaffOffBoarding().GetNewOrPendingStaffOffBoarding(CompanyId, _context);
        //}


        //[HttpGet("GetStaffOffBoardingDetailInfoId{id}")]
        //public async Task<ActionResult<StaffOffBoarding>> GetStaffOffBoardingDetailInfoId(int id)
        //{
        //    var result = await new StaffOffBoarding().GetStaffOffBoardingDetailById(id, _context);
        //    if (result != null)
        //        return Ok(result);

        //    return NotFound();


        //}

        //      [HttpGet("GetStaffOffBoardingInfoId{id}")]
        //      public async Task<ActionResult<StaffOffBoarding>> GetStaffOffBoardingInfoId(int id)
        //      {
        //          var result = await new StaffOffBoarding().GetStaffOffBoardingById(id, _context);
        //          if (result != null)
        //              return Ok(result);

        //          return NotFound();


        //      }

        [HttpPost("StaffOffBoardingAddOrCreate")]
        public async Task<ActionResult<StaffOffBoarding>> StaffOffBoardingAddOrCreate(StaffOffBoarding obj)
        {

            var result = await new StaffOffBoarding().PostStaffOffBoarding(obj, _context);
            if (result != null && result.OffBoardingId > 0)
                return Ok(new
                {
                    Success = true,
                    Message = "Data Update Successfully!"
                });
            else
                return Ok(new
                {
                    Success = true,
                    Message = "Data Insert Successfully!"
                });

        }

        //      [HttpDelete("DeleteStaffOffBoardingInfo{id}")]
        //      public async Task<ActionResult<bool>> DeleteStaffOffBoardingInfo(int id)
        //      {
        //          var result = await new StaffOffBoarding().DeleteStaffOffBoardingInfo(id, _context);
        //          if (id > 0)
        //              return Ok(new
        //              {

        //                  Success = true,
        //                  Message = "Data Delete Successfully!"


        //              });

        //          return NotFound("Data Not Found!");
        //      }


        //      [HttpGet("StaffOffBoardingCheckData{staffid}/{StaffOffBoardingtypeid}")]
        //      public async Task<ActionResult<JsonObject>> StaffOffBoardingCheckData(int staffid,int StaffOffBoardingtypeid)
        //      {
        //          if (await new StaffOffBoarding().CheckStaffOffBoarding(staffid,StaffOffBoardingtypeid,  _context))
        //          {
        //              return Ok(new
        //              {

        //                  Success = true,
        //                  Message = "You Have Consumed All StaffOffBoarding!"


        //              });
        //          }
        //          else
        //          {

        //              return Ok(new
        //              {

        //                  Success = false,
        //                  Message = "Not found"


        //              });
        //          }


        //      }


        //      //[HttpGet("StaffOffBoardingCheckData{id}/{cnic}")]
        //      //public async Task<ActionResult<JsonObject>> StaffOffBoardingCheckData(int id, string email)
        //      //{
        //      //    if (await new StaffOffBoarding().AlreadyExist(id, email, _context))
        //      //    {
        //      //        return Ok(new
        //      //        {

        //      //            Success = true,
        //      //            Message = "StaffOffBoarding Already Exist!"


        //      //        });
        //      //    }
        //      //    else
        //      //    {

        //      //        return Ok(new
        //      //        {

        //      //            Success = false,
        //      //            Message = "Not found"


        //      //        });
        //      //    }

        //      //}

        //      [HttpPost("SaveStaffOffBoardingApprovalDetail")]  //{id}/{StaffOffBoardingstatusid}/{remarks}/{staffid}
        //      public async Task<ActionResult<JsonObject>> SaveStaffOffBoardingApprovalDetail(StaffOffBoardingApproval obj)
        //      {

        //          var dbResult = await new StaffOffBoarding().SaveStaffOffBoardingApprovalDetail(obj, _context);
        //          if (dbResult != null)
        //          {
        //              return Ok(new
        //              {
        //                  Success = true,
        //                  Message = obj.StaffOffBoardingStatusId == 3 ? "StaffOffBoarding Approved Successfully" : "StaffOffBoarding Rejectd Successfully"
        //              });
        //          }
        //          else
        //          {
        //              return Ok(new
        //              {
        //                  Success = false,
        //                  Message = "Not found"
        //              });
        //          }
        //      }


        //      [HttpPost("SaveForwardStaffOffBoardingDetail")]
        //      public async Task<ActionResult<StaffOffBoarding>> SaveForwardStaffOffBoardingDetail(StaffOffBoarding obj)
        //      {


        //          var result = await new StaffOffBoarding().PostStaffOffBoardingApproval(obj, _context);
        //          if (result != null && result.StaffOffBoardingId > 0)
        //              return Ok(new
        //              {
        //                  Success = true,
        //                  Message = "Data Update Successfully!"
        //              });
        //          else
        //              return Ok(new
        //              {
        //                  Success = true,
        //                  Message = "Data Insert Successfully!"
        //              });

        //      }

        //      [HttpGet("GetStaffOffBoardingApprovalByStaffOffBoardingId{id}")]
        //      public async Task<ActionResult<List<StaffOffBoardingApproval>>> GetStaffOffBoardingApprovalByStaffOffBoardingId(int id)
        //      {

        //          return await new StaffOffBoardingApproval().GetStaffOffBoardingAprrovalDetail(id, _context);
        //      }


        ////Load dropdown StaffOffBoardingStatus
        //[HttpGet("GetStaffOffBoardingStatusInfos")]
        //public async Task<ActionResult<List<StaffOffBoardingStatus>>> GetStaffOffBoardingStatusInfos()
        //{

        //	return await new StaffOffBoarding().GetStaffOffBoardingStatus(_context);
        //}





        ////Load dropdown WeekendRule
        //[HttpGet("GetWeekendRuleInfos")]
        //public async Task<ActionResult<List<WeekendRule>>> GetWeekendRuleInfos()
        //{

        //	return await new StaffOffBoarding().GetWeekendRule(_context);
        //}



        //      [HttpGet("GetStaffOffBoardingApprovalSettingInfos{CompanyId}")]
        //      public async Task<ActionResult<StaffOffBoardingApprovalSetting>> GetStaffOffBoardingApprovalSettingInfos(int CompanyId)
        //      {

        //          return await new StaffOffBoarding().GetStaffOffBoardingApprovalSetting(CompanyId, _context);
        //      }

        //      // search StaffOffBoarding by StaffOffBoardingType , StaffOffBoardingStatus and Date

        [HttpGet("SearchAllStaffOffBoardings{CompanyId}/{DepartmentId}/{DesignationId}/{Date}")]
        public async Task<ActionResult<List<StaffOffBoarding>>> SearchAllStaffOffBoardings(int CompanyId, int DepartmentId, int DesignationId, DateTime Date)
        {
            var result = await new StaffOffBoarding().SearchStaffOffBoardings(CompanyId, DepartmentId, DesignationId, Date,_context);
            if (result != null)
                return Ok(result);

            return NotFound();


        }




        //      ////Load StaffOffBoarding data from database to Student form change dropdown selection
        //      //[HttpGet("GetStaffOffBoardingIdVise{id}")]
        //      //public async Task<ActionResult<List<StaffOffBoardingClassSubject>>> GetStaffOffBoardingIdVise(int id)
        //      //{
        //      //    var result = await new StaffOffBoardingInfo().GetStaffOffBoardingIdVise(id, _context);
        //      //    if (result != null)
        //      //        return Ok(result);

        //      //    return NotFound();


        //      //}







        #endregion



    }
}
