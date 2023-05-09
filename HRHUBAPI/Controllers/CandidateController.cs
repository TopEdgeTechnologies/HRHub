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
    public class CandidateController : ControllerBase
    {
        private readonly HrhubContext _context;

        public CandidateController(HrhubContext context)
        {
            _context = context;

        }

        #region CandidateInfo

        [HttpGet("GetCandidateInfos{CompanyId}")]
        public async Task<ActionResult<List<Candidate>>> GetCandidateInfos(int CompanyId)
        {

            return await new Candidate().GetCandidate(CompanyId,_context);
        }


        [HttpGet("GetCandidateInfoId{id}")]
        public async Task<ActionResult<Candidate>> GetCandidateInfoId(int id)
        {
            var result = await new Candidate().GetCandidateById(id, _context);
            if (result != null)
                return Ok(result);

            return NotFound();


        }

        [HttpPost("CandidateAddOrCreate")]
        public async Task<ActionResult<Candidate>> CandidateAddOrCreate(Candidate obj)
        {


            var result = await new Candidate().PostCandidate(obj, _context);
            if (result != null && result.CandidateId > 0)
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

        [HttpDelete("DeleteCandidateInfo{id}")]
        public async Task<ActionResult<bool>> DeleteCandidateInfo(int id)
        {
            var result = await new Candidate().DeleteCandidateInfo(id, _context);
            if (id > 0)
                return Ok(new
                {

                    Success = true,
                    Message = "Data Delete Successfully!"


                });

            return NotFound("Data Not Found!");
        }


        [HttpGet("CandidateCheckData{id}/{cnic}")]
        public async Task<ActionResult<JsonObject>> CandidateCheckData(int id,int CompanyId, string email)
        {
            if (await new Candidate().AlreadyExist(id, email, CompanyId, _context))
            {
                return Ok(new
                {

                    Success = true,
                    Message = "Candidate Already Exist!"


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


        ////Load Candidate data from database to Student form change dropdown selection
        //[HttpGet("GetCandidateIdVise{id}")]
        //public async Task<ActionResult<List<CandidateClassSubject>>> GetCandidateIdVise(int id)
        //{
        //    var result = await new CandidateInfo().GetCandidateIdVise(id, _context);
        //    if (result != null)
        //        return Ok(result);

        //    return NotFound();


        //}







        #endregion

        //#region CandidateSubject

        //[HttpGet("GetCandidateSubjects")]
        //public async Task<ActionResult<List<ViewCandidateClassSubject>>> GetCandidateSubjects()
        //{

        //    return await new CandidateClassSubject().GetCandidateClassSubject(_context);
        //}


       

        //[HttpGet("GetCandidateSubjectId{id}")]
        //public async Task<ActionResult<CandidateClassSubject>> GetCandidateSubjectId(int id)
        //{
        //    var result = await new CandidateClassSubject().GetCandidateClassSubjectById(id, _context);
        //    if (result != null)
        //        return Ok(result);

        //    return NotFound();


        //}

        //[HttpPost("CandidateSubjectAddOrCreate")]
        //public async Task<ActionResult> CandidateSubjectAddOrCreate(List<CandidateClassSubject> obj)
        //{

            
        //    var result = await new CandidateClassSubject().PostCandidateClassSubject(obj, _context);
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

        //[HttpDelete("DeleteCandidateClassSubjectByClassId{id}")]
        //public async Task<ActionResult<bool>> DeleteCandidateClassSubjectByClassId(int id)
        //{
        //    var result = await new CandidateClassSubject().DeleteCandidateSubjectByClassId(id, _context);
        //    if (id > 0)
        //        return Ok(new
        //        {

        //            Success = true,
        //            Message = "Data Delete Successfully!"


        //        });

        //    return NotFound("Data Not Found!");
        //}


        //[HttpDelete("DeleteCandidateSubject{id}")]
        //public async Task<ActionResult<bool>> DeleteCandidateSubject(int id)
        //{
        //    var result = await new CandidateClassSubject().DeleteCandidateClassSubject(id, _context);
        //    if (id > 0)
        //        return Ok(new
        //        {

        //            Success = true,
        //            Message = "Data Delete Successfully!"


        //        });

        //    return NotFound("Data Not Found!");
        //}


        //[HttpGet("CandidateSubjectCheckData{id}/{GroupId}/{title}/{language}")]
        //public async Task<ActionResult<JsonObject>> CandidateSubjectCheckData(int id, int GroupId, string title, string language)
        //{
        //    if (await new CandidateClassSubject().AlreadyExist(id, GroupId, title, language, _context))
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
        //public async Task<ActionResult<List<CandidateClassSubject>>> LoadDataByClassId(int id)
        //{
        //    var result = await new CandidateClassSubject().LoadDataByClassId(id, _context);
        //    if (result != null)
        //        return Ok(result);

        //    return NotFound();


        //}
        //#endregion





    }
}
