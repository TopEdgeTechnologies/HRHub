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
                string query = @"exec sp_sendEMail  ";

                DataTable dt = dbConnection.ReturnDataTable(query);
                var resultRows = dt.AsEnumerable().Select(row => new EmailLog
                {

                    EmailId = Convert.ToInt32(row["EmailId"]),
                    EmailFrom = string.IsNullOrWhiteSpace(row["EmailFrom"].ToString()) ? "" : row["EmailFrom"].ToString(),
                    EmailTo = string.IsNullOrWhiteSpace(row["EmailTo"].ToString()) ?"":  row["EmailTo"].ToString(),
                    CreatedBy =  string.IsNullOrWhiteSpace(row["CreatedBy"].ToString())?0:Convert.ToInt32(row["CreatedBy"]),
                    Body = string.IsNullOrWhiteSpace(row["Body"].ToString()) ? "" : row["Body"].ToString(),
                    Subject = string.IsNullOrWhiteSpace(row["Subject"].ToString()) ? "" : row["Subject"].ToString(),
                    PortName = string.IsNullOrWhiteSpace(row["Email_SMTPPort"].ToString()) ? 0 : Convert.ToInt32(row["Email_SMTPPort"].ToString()),
                    FromMail = string.IsNullOrWhiteSpace(row["Email_SendFrom"].ToString()) ? "" : row["Email_SendFrom"].ToString(),
                    ServerName = string.IsNullOrWhiteSpace(row["Email_ServerHost"].ToString()) ? "" : row["Email_ServerHost"].ToString(),
                    Pasword = string.IsNullOrWhiteSpace(row["Email_Password"].ToString()) ? "" : row["Email_Password"].ToString()
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
