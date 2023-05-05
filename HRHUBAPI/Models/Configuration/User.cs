using HRHUBAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRHUBAPI.Models
{
    public partial class User
    {
        [NotMapped]
        public int? status { get; set; }
        public async Task<User> Login(User Obj, HrhubContext _context)
        {


            try
            {
                //Obj.Password = PasswordHasher.Encrypt(Obj.Password, true);
                Obj.Password = Obj.Password;
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == Obj.UserName && x.Password == Obj.Password && x.IsActive == true);


                if (user != null)
                {

                   
                return user;

                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<User> RegisterUser(User Obj, HrhubContext _context)
        {

            try
            {
                //Obj.Password = PasswordHasher.Encrypt(Obj.Password, true);
                //Obj.Password = Obj.Password;
                //Obj.Token = "";
                Obj.CreatedOn = DateTime.Now;
                Obj.IsActive = true;
                await _context.Users.AddAsync(Obj);
                await _context.SaveChangesAsync();
                return Obj;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> AlreadyExist(int userid, string username, HrhubContext _context)
        {
            try
            {
                if (userid > 0)
                {
                    var result = await _context.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == username.ToLower() && x.UserId != userid);
                    if (result != null)
                    {
                        return true;
                    }


                }
                else
                {
                    var result = _context.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == username.ToLower());
                    if (result != null)
                    {
                        return true;
                    }

                }

                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
