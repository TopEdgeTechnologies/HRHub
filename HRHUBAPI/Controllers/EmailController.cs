using HRHUBAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRHUBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmailController : ControllerBase
    {

        private readonly HrhubContext _context;

        public EmailController(HrhubContext context)
        {
            _context = context;

        }


        [HttpPost("SendMail")]
        public async Task<ActionResult> SendMail(List<EmailLog> obj)
        {
            var result =await new EmailLog().PostEmail(obj, _context);
            if (result)
                return Ok(new
                {
                    Success = true,
                    Message = "Data Update Successfully!"
                });
            else
                return NotFound();
        }


    

    
    }
}
