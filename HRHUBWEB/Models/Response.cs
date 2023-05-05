using  HRHUBAPI.Models;

namespace HRHUBWEB.Models
{
    public class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
       public User Data { get; set; }
        public string Token { get; set; }


    }






}
