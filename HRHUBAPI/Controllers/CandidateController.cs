using HRHUBAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpDelete("DeleteCandidateInfo{id}/{UserId}")]
        public async Task<ActionResult<bool>> DeleteCandidateInfo(int id, int UserId)
        {
            var result = await new Candidate().DeleteCandidate(id,UserId, _context);
            if (id > 0)
                return Ok(new
                {

                    Success = true,
                    Message = "Data Delete Successfully!"


                });

            return NotFound("Data Not Found!");
        }


        [HttpGet("CandidateCheckDataInfo{id}/{email}/{CompanyId}")]
        public async Task<ActionResult<JsonObject>> CandidateCheckDataInfo(int id, string email,int CompanyId)
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


        ////Load Candidate Skill data from database to 
        [HttpGet("GetCandidateSkillInfos{CandidateId}")]
        public async Task<ActionResult<List<CandidateSkill>>> GetCandidateSkillInfos(int CandidateId)
        {

            return await new Candidate().GetCandidateSkill(CandidateId,_context);
        }





        ////Load Satatus data candidate list form 
        [HttpGet("GetCandidateStatusInfos")]
        public async Task<ActionResult<List<StatusInfo>>> GetCandidateStatusInfos()
        {

            return await new Candidate().GetCandidateStatus( _context);
        }






        // update candidate status

        [HttpPost("CandidateStatusUpdate")]
        public async Task<ActionResult<CandidateScreening>> CandidateStatusUpdate(CandidateScreening obj)
        {


            var result = await new Candidate().PostScreening(obj, _context);
            if (result != null && result.ScreeningId > 0)
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




        // update candidate status

        [HttpGet("GetCandidateStatusdata{id}")]
        public async Task<ActionResult<List< CandidateScreening>>> GetCandidateStatusdata(int id)
        {


            return await new Candidate().GetCandidateStatus(id, _context);
           


        }



        // search data Candidate by Name , Designation and ExperienceId

        [HttpGet("SearchAllCandidates{Name}/{DesignationId}/{ExperienceId}/{CompanyId}")]
        public async Task<ActionResult<List<Candidate>>> SearchAllCandidates(string Name, int DesignationId, int ExperienceId, int CompanyId)
        {
            var result = await new Candidate().SearchCandidates(Name, DesignationId, ExperienceId, CompanyId, _context);
            if (result != null)
                return Ok(result);

            return NotFound();


        }



        #endregion






    }
}
