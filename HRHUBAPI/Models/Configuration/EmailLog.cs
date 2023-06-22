using HRHUBAPI.Models.Configuration;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Data;
using System.Net.NetworkInformation;

namespace HRHUBAPI.Models
{
    public partial class EmailLog
    {
        [NotMapped]
        public string? ServerName { get; set; }
        [NotMapped]
        public int? PortName { get; set; }
        [NotMapped]
        public string? FromMail { get; set; }
        [NotMapped]
        public string? Pasword { get; set; }

        public async Task<bool> PostEmail(List<EmailLog> Objemail, HrhubContext hrhubContext)
        {
         
           hrhubContext.EmailLogs.AddRange(Objemail);
           await hrhubContext.SaveChangesAsync();
            return true;         
           
        }

        public bool UpdateEmail(List<EmailLog> Objemail)
        {
            DbConnection dbConnection = new DbConnection();
            var ids =  string.Join(",", Objemail.Where(x => x.IsSent == true).Select(x => x.EmailId));
            string query  = " update EmailLog set IsSent=1 where EmailID in (" + ids + ")";
            DataTable dt = dbConnection.ReturnDataTable(query);

            return true;

        }

        public List<EmailLog> GetPendingEmail()
        {
            try
            {

                DbConnection dbConnection = new DbConnection();
                string query = @"select e.*, c.Email_SMTPPort, c.Email_SendFrom, c.Email_Password, c.Email_ServerHost from emaillog e
                                    inner join Company c on c.CompanyID = e.CompanyID    where IsSent is null";

                DataTable dt = dbConnection.ReturnDataTable(query);
                var resultRows = dt.AsEnumerable().Select(row => new EmailLog
                {

                    EmailId = Convert.ToInt32(row["EmailId"]),
                    EmailFrom = row["EmailFrom"].ToString(),
                    EmailTo = row["EmailTo"].ToString(),
                    CreatedBy = Convert.ToInt32(row["CreatedBy"]),
                    Body = row["Body"].ToString(),
                    Subject = row["Subject"].ToString(),
                    PortName = Convert.ToInt32(row["Email_SMTPPort"].ToString()),
                    FromMail = row["Email_SendFrom"].ToString(),
                    ServerName = row["Email_ServerHost"].ToString(),
                    Pasword = row["Email_Password"].ToString()
                }).ToList();



                return resultRows;

            }
            catch (Exception ex)
            {

                throw;

            }
        }


    }
}
