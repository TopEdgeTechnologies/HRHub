using HRHUBAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace HRHUBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LeaveController : ControllerBase
    {
        private readonly HrhubContext _context;

        public LeaveController(HrhubContext context)
        {
            _context = context;

        }

        #region LeaveInfo

        [HttpGet("GetLeaveInfos{CompanyId}")]
        public async Task<ActionResult<List<Leave>>> GetLeaveInfos(int CompanyId)
        {

            return await new Leave().GetLeave(CompanyId, _context);
        }

        [HttpGet("GetNewOrPendingLeaveInfos{CompanyId}")]
        public async Task<ActionResult<List<Leave>>> GetNewOrPendingLeaveInfos(int CompanyId)
        {

            return await new Leave().GetNewOrPendingLeave(CompanyId, _context);
        }


        [HttpGet("GetLeaveDetailInfoId{id}")]
        public async Task<ActionResult<Leave>> GetLeaveDetailInfoId(int id)
        {
            var result = await new Leave().GetLeaveDetailById(id, _context);
            if (result != null)
                return Ok(result);

            return NotFound();


        }

        [HttpGet("GetLeaveInfoId{id}")]
        public async Task<ActionResult<Leave>> GetLeaveInfoId(int id)
        {
            var result = await new Leave().GetLeaveById(id, _context);
            if (result != null)
                return Ok(result);

            return NotFound();


        }

        [HttpPost("LeaveAddOrCreate")]
        public async Task<ActionResult<Leave>> LeaveAddOrCreate(Leave obj)
        {


            var result = await new Leave().PostLeave(obj, _context);
            if (result != null && result.LeaveId > 0)
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

        [HttpDelete("DeleteLeaveInfo{id}")]
        public async Task<ActionResult<bool>> DeleteLeaveInfo(int id)
        {
            var result = await new Leave().DeleteLeaveInfo(id, _context);
            if (id > 0)
                return Ok(new
                {

                    Success = true,
                    Message = "Data Delete Successfully!"


                });

            return NotFound("Data Not Found!");
        }


        [HttpGet("LeaveCheckData{id}/{cnic}")]
        public async Task<ActionResult<JsonObject>> LeaveCheckData(int id, string email)
        {
            if (await new Leave().AlreadyExist(id, email, _context))
            {
                return Ok(new
                {

                    Success = true,
                    Message = "Leave Already Exist!"


                });
            }
            else
            {

                return Ok(new
                {

                    Success = false,
                    Message = "Not found"


                });
            }

        }

        [HttpPost("SaveLeaveApprovalDetail")]  //{id}/{leavestatusid}/{remarks}/{staffid}
        public async Task<ActionResult<JsonObject>> SaveLeaveApprovalDetail(LeaveApproval obj)
        {

            var dbResult = await new Leave().SaveLeaveApprovalDetail(obj, _context);
            if (dbResult != null)
            {
                return Ok(new
                {
                    Success = true,
                    Message = obj.LeaveStatusId == 3 ? "Leave Approved Successfully" : "Leave Rejectd Successfully"
                });
            }
            else
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Not found"
                });
            }
        }


        [HttpPost("SaveForwardLeaveDetail")]
        public async Task<ActionResult<Leave>> SaveForwardLeaveDetail(Leave obj)
        {


            var result = await new Leave().PostLeaveApproval(obj, _context);
            if (result != null && result.LeaveId > 0)
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

        [HttpGet("GetLeaveApprovalByLeaveId{id}")]
        public async Task<ActionResult<List<LeaveApproval>>> GetLeaveApprovalByLeaveId(int id)
        {

            return await new LeaveApproval().GetLeaveAprrovalDetail(id, _context);
        }

        ////Load Leave data from database to Student form change dropdown selection
        //[HttpGet("GetLeaveIdVise{id}")]
        //public async Task<ActionResult<List<LeaveClassSubject>>> GetLeaveIdVise(int id)
        //{
        //    var result = await new LeaveInfo().GetLeaveIdVise(id, _context);
        //    if (result != null)
        //        return Ok(result);

        //    return NotFound();


        //}







        #endregion

        //#region LeaveSubject

        //[HttpGet("GetLeaveSubjects")]
        //public async Task<ActionResult<List<ViewLeaveClassSubject>>> GetLeaveSubjects()
        //{

        //    return await new LeaveClassSubject().GetLeaveClassSubject(_context);
        //}




        //[HttpGet("GetLeaveSubjectId{id}")]
        //public async Task<ActionResult<LeaveClassSubject>> GetLeaveSubjectId(int id)
        //{
        //    var result = await new LeaveClassSubject().GetLeaveClassSubjectById(id, _context);
        //    if (result != null)
        //        return Ok(result);

        //    return NotFound();


        //}

        //[HttpPost("LeaveSubjectAddOrCreate")]
        //public async Task<ActionResult> LeaveSubjectAddOrCreate(List<LeaveClassSubject> obj)
        //{


        //    var result = await new LeaveClassSubject().PostLeaveClassSubject(obj, _context);
        //    if (result > 0)
        //        return Ok(new
        //        {
        //            Success = true,
        //            Message = "Data Update Successfully!"
        //        });
        //    else
        //        return Ok(new
        //        {
        //            Success = true,
        //            Message = "Data Insert Successfully!"
        //        });

        //}

        //[HttpDelete("DeleteLeaveClassSubjectByClassId{id}")]
        //public async Task<ActionResult<bool>> DeleteLeaveClassSubjectByClassId(int id)
        //{
        //    var result = await new LeaveClassSubject().DeleteLeaveSubjectByClassId(id, _context);
        //    if (id > 0)
        //        return Ok(new
        //        {

        //            Success = true,
        //            Message = "Data Delete Successfully!"


        //        });

        //    return NotFound("Data Not Found!");
        //}


        //[HttpDelete("DeleteLeaveSubject{id}")]
        //public async Task<ActionResult<bool>> DeleteLeaveSubject(int id)
        //{
        //    var result = await new LeaveClassSubject().DeleteLeaveClassSubject(id, _context);
        //    if (id > 0)
        //        return Ok(new
        //        {

        //            Success = true,
        //            Message = "Data Delete Successfully!"


        //        });

        //    return NotFound("Data Not Found!");
        //}


        //[HttpGet("LeaveSubjectCheckData{id}/{GroupId}/{title}/{language}")]
        //public async Task<ActionResult<JsonObject>> LeaveSubjectCheckData(int id, int GroupId, string title, string language)
        //{
        //    if (await new LeaveClassSubject().AlreadyExist(id, GroupId, title, language, _context))
        //    {
        //        return Ok(new
        //        {

        //            Success = true,
        //            Message = "Title Already Exist!"


        //        });
        //    }
        //    else
        //    {

        //        return Ok(new
        //        {

        //            Success = false,
        //            Message = "Not found"


        //        });
        //    }

        //}



        ////load subject data in dropdown by language ID
        //[HttpGet("GetSubjectLanguageId{id}")]
        //public async Task<ActionResult<List<SubjectInfo>>> GetSubjectLanguageId(int id)
        //{
        //    var result = await new SubjectInfo().GetSubjectLangugeId(id, _context);
        //    if (result != null)
        //        return Ok(result);

        //    return NotFound();


        //}
        ////load class data in dropdown by level ID
        //[HttpGet("GetClassByLevelId{id}")]
        //public async Task<ActionResult<List<ClassInfo>>> GetClassByLevelId(int id)
        //{
        //    var result = await new ClassInfo().GetClassbyLevelId(id, _context);
        //    if (result != null)
        //        return Ok(result);

        //    return NotFound();


        //}
        ////load subject data in dropdown by Group ID
        //[HttpGet("GetSubjectByGroupId{id}/{groupId}")]
        //public async Task<ActionResult<List<SubjectInfo>>> GetSubjectByGroupId(int id, int groupId)
        //{
        //    var result = await new SubjectInfo().GetSubjectByGroupId(id, groupId, _context);
        //    if (result != null)
        //        return Ok(result);

        //    return NotFound();


        //}
        ////Load Class data from database to bootstrap table on class dropdown selection
        //[HttpGet("LoadDataByClassId{id}")]
        //public async Task<ActionResult<List<LeaveClassSubject>>> LoadDataByClassId(int id)
        //{
        //    var result = await new LeaveClassSubject().LoadDataByClassId(id, _context);
        //    if (result != null)
        //        return Ok(result);

        //    return NotFound();


        //}
        //#endregion





    }
}
