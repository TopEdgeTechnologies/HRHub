using HRHUBAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Nodes;

namespace HRHUBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly HrhubContext _context;       

        public CompanyController(HrhubContext context)
        {
            _context = context;           

        }


        #region CompanyInfo

        [HttpGet("GetCompanyInfos{CompanyId}")]
        public async Task<ActionResult<List<Company>>> GetCompanyInfos(int CompanyId)
        {

            return await new Company().GetCompany(CompanyId,_context);
        }


        [HttpGet("GetCompanyInfoId{id}")]
        public async Task<ActionResult<Company>> GetCompanyInfoId(int id)
        {
            var result = await new Company().GetCompanyById(id, _context);
            if (result != null)
                return Ok(result);

            return NotFound();


        }

        [HttpPost("CompanyAddOrUpdate")]
        public async Task<ActionResult<Company>> CompanyAddOrUpdate(Company obj)
        {


            var result = await new Company().PostCompany(obj, _context);
            if (result != null && result.CompanyId > 0)
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

        [HttpDelete("DeleteCompanyInfo{id}/{UserId}")]
        public async Task<ActionResult<bool>> DeleteCompanyInfo(int id, int UserId)
        {
            var result = await new Company().DeleteCompanyInfo(id, UserId, _context);
            if (id > 0)
                return Ok(new
                {

                    Success = true,
                    Message = "Data Delete Successfully!"


                });

            return NotFound("Data Not Found!");
        }


        [HttpGet("CompanyEmailCheck{id}/{email}")]
        public async Task<ActionResult<JsonObject>> CompanyEmailCheck(int id, string email)
        {
            if (await new Company().AlreadyCompanyEmailExist(id,email, _context))
            {
                return Ok(new
                {

                    Success = true,
                    Message = "Email Already Exist!"


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




		[HttpGet("CompanyNameCheck{id}/{companyName}")]
		public async Task<ActionResult<JsonObject>> CompanyNameCheck(int id, string companyName)
		{
			if (await new Company().AlreadyCompanyNameExist(id, companyName, _context))
			{
				return Ok(new
				{

					Success = true,
					Message = "Company Already Exist!"


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


        // Get user Details by Company Id
		[HttpGet("GetUserbyId{CompanyId}")]
		public async Task<ActionResult<Company>> GetUserbyId(int CompanyId)
		{
			var result = await new Company().GetUserByCompanyId(CompanyId, _context);
			if (result != null)
				return Ok(result);

			return NotFound();


		}






		#endregion
	}
}
