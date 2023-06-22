using HRHUBAPI.Models;
using System.Net.Mail;
using System.Net;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace HRHUBAPI.BackGroundService
{

    public class EmailHelper 
    {

      
        public bool SendEmailBackGround(string toEmail, string subject, string body, string frommail, string password, string serverHost, int port)
        {
            try
            {
                
                bool issend = false;
                using (SmtpClient client = new SmtpClient(serverHost, Convert.ToInt32(port)))
                {
                    client.EnableSsl = true; // Enable SSL if required by your SMTP server
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(frommail, password);
                    client.Timeout = 10000;

                    using (MailMessage mailMessage = new MailMessage(frommail, toEmail, subject, body))
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

                return issend;



            }
            catch (Exception ex)
            {

                return false;

            }

        }


      
    }
}
