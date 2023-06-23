using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace HRHUBAPI.Models.Configuration
{
    public class DbConnection
    {

        // private readonly IConfiguration _config;
        string connectionString = ConnectionString.CName;
        //public DbConnection(IConfiguration config)
        //{
        //    _config = config;

        //}

        #region Global of Returns

        public bool IsNullOrWhiteSpace(string value)
        {     
            if (value != null)
            {
                for (int i = 0; i < value.Length; i++)
                {
                    if (!char.IsWhiteSpace(value[i]))
                    {
                        return false;  // Value NOT Empty aka Khali aka Nothing aka EmptyString aka ""
                    }
                }
            }
            return true;    // Value is Empty aka Khali aka Nothing aka EmptyString aka ""
        }

        public List<Dictionary<string, object>> GetTableRows(DataTable dtData)
        {
            List<Dictionary<string, object>>
            lstRows = new List<Dictionary<string, object>>();
            Dictionary<string, object> dictRow = null;

            foreach (DataRow dr in dtData.Rows)
            {
                dictRow = new Dictionary<string, object>();
                foreach (DataColumn col in dtData.Columns)
                {
                    dictRow.Add(col.ColumnName, dr[col]);
                }
                lstRows.Add(dictRow);
            }
            return lstRows;
        }

        public string ReturnColumn(string Query, string ColumnName)
        {
            string result = "";
            DataTable dt = new DataTable();
            dt = ReturnDataTable(Query);
            if (dt.Rows.Count > 0)
            {
                result = Convert.ToString(dt.Rows[0][ColumnName]);
            }
            return result;
        }

        public void ExecuteQuery(string Query)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand com = new SqlCommand(Query, con);
            com.CommandType = CommandType.Text;
            com.ExecuteNonQuery();
            con.Close();
        }
        public DataTable ReturnDataTable(string query)
        {
            DataTable dt = new DataTable();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand com = new SqlCommand(query, connection);
                com.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(dt);
                connection.Close();
                return dt;
            }
        }

        #endregion

        #region Encryption & Decryption
        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        #endregion
    }

    public static class ConnectionString
    {
        private static string cName = "Data Source=webserver; Initial Catalog=HRHUB;User ID=team;Password=dynamixsolpassword;TrustServerCertificate=True;";
        public static string CName
        {
            get => cName;
        }
    }
}
