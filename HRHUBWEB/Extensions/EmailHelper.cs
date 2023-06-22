using HRHUBAPI.Models;
using HRHUBWEB.Models;
using System.Net.Mail;
using System.Net;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HRHUBWEB.Extensions
{


    public interface IEmailHelper
    {
        Task<Response> SendEmailAsync(string toEmail, string subject, string body);
        Task<bool> SendEmailToServer(string toEmail, string subject, string body);

    }


    public class EmailHelper : IEmailHelper
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly APIHelper _APIHelper;
        //  private readonly User _user;

        public EmailHelper(IHttpClientFactory httpClientFactory, IWebHostEnvironment webHostEnvironment, APIHelper APIHelper, IHttpContextAccessor httpContextAccessor)
        {

            _APIHelper = APIHelper;
            _httpContextAccessor = httpContextAccessor;
            // _user = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
        }


        public async Task<Response> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {

                var _user = _httpContextAccessor.HttpContext?.Session.GetObjectFromJson<User>("AuthenticatedUser");


                EmailLog obj = new EmailLog();
                obj.EmailTo = toEmail;
                obj.Subject = subject;
                obj.Body = body;
              
                obj.CreatedBy = _user.UserId;
                obj.CompanyId = _user.CompanyId;
                obj.CreatedOn = DateTime.Now;

                List<EmailLog> listobj = new List<EmailLog>();
                listobj.Add(obj);
                return await _APIHelper.CallApiAsyncPost<Response>(listobj, "api/Email/SendMail", HttpMethod.Post);
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        public Task<bool> SendEmailToServer(string toEmail, string subject, string body)
        {
            try
            {
                var _user = _httpContextAccessor.HttpContext?.Session.GetObjectFromJson<User>("AuthenticatedUser");

                bool issend = false;
                using (SmtpClient client = new SmtpClient(_user.EmailServerHost, Convert.ToInt32(_user.EmailSMTPPort)))
                {
                    client.EnableSsl = true; // Enable SSL if required by your SMTP server
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(_user.EmailSendFrom, _user.EmailPassword);
                    client.Timeout = 3000;

                    using (MailMessage mailMessage = new MailMessage(_user.EmailSendFrom, toEmail, subject, body))
                    {
                        try
                        {
                            client.Send(mailMessage);
                            issend = true;

                        }
                        catch (SmtpException ex)
                        {

                            issend = false;

                        }


                    }
                }

                return Task.FromResult(issend);



            }
            catch (Exception ex)
            {

                return Task.FromResult(false);

            }

        }

    
        public async Task<Response> BulksendEmail(List<EmailLog> listobj)
        {

            return await _APIHelper.CallApiAsyncPost<Response>(listobj, "api/Email/SendMail", HttpMethod.Post);

        }

      
    }
}
