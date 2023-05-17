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

        [HttpGet("GetCompanyInfos")]
        public async Task<ActionResult<List<Company>>> GetCompanyInfos()
        {

            return await new Company().GetCompany(_context);
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


        [HttpGet("CompanyCheckData{id}/{companyName}/{email}")]
        public async Task<ActionResult<JsonObject>> CompanyCheckData(int id,string companyName, string email)
        {
            if (await new Company().AlreadyExist(id, companyName, email, _context))
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









        #endregion
    }
}
