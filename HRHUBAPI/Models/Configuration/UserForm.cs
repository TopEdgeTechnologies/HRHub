using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HRHUBAPI.Models
{
    public partial class UserForm
    {
      
        public async Task<List<UserForm>> GetUserForm(HrhubContext _context)
        {
            try
            {
                return await _context.UserForms.ToListAsync();
            }
            catch (Exception ex)
            {

                throw;

            }
        }








        //public async Task<UserForm> GetUserFormById(int id, HrhubContext _context)
        //{
        //    try
        //    {

        //        var result = await _context.UserForms.FirstOrDefaultAsync(x => x.GroupId == id && x.IsDeleted==false);
        //        if (result != null)
        //        {
        //            return result;
        //        }
        //        else
        //        {
        //            return null;

        //        }



        //    }
        //    catch (Exception ex)
        //    {

        //        throw;

        //    }
        //}


        //public async Task<UserForm> PostUserForm(UserForm UserForm, HrhubContext _context)
        //{
        //    try
        //    {
        //        string msg = "";
        //        var checkUserForm = await _context.UserForms.FirstOrDefaultAsync(x => x.GroupId == UserForm.GroupId && x.IsDeleted==false);
        //        if (checkUserForm != null && checkUserForm.GroupId > 0)
        //        {
        //            checkUserForm.GroupId=UserForm.GroupId;
        //            checkUserForm.Title = UserForm.Title;
        //            checkUserForm.DOrder = UserForm.DOrder;
        //            checkUserForm.Description = UserForm.Description;
        //            checkUserForm.ShortTitle = UserForm.ShortTitle; 
        //            checkUserForm.UpdatedOn = DateTime.Now;
        //            checkUserForm.IsActive = UserForm.IsActive;
        //            checkUserForm.UpdatedBy = UserForm.CreateBy;
        //            checkUserForm.InstituteId = UserForm.InstituteId;
        //            await _context.SaveChangesAsync();


        //        }
        //        else
        //        {
        //            UserForm.CreatedOn = DateTime.Now;
        //            _context.UserForms.Add(UserForm);
        //        }
        //        await _context.SaveChangesAsync();

        //        return checkUserForm;


        //    }
        //    catch (Exception ex)
        //    {

        //        throw;

        //    }
        //}




        //public async Task<bool> DeleteUserForm(int id, HrhubContext _context)
        //{
        //    try
        //    {
        //        bool check = false;
        //        var UserForm = await _context.UserForms.FirstOrDefaultAsync(x => x.GroupId == id && x.IsDeleted == false);

        //        if (UserForm != null)
        //        {
        //            UserForm.IsDeleted= true;   
        //            UserForm.UpdatedOn= DateTime.Now;
        //            check = true;

        //        }
        //        await _context.SaveChangesAsync();
        //        return check;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }


        //}


        //public async Task<bool> AlreadyExist(int UserFormId, string UserFormname, HrhubContext _context)
        //{
        //    try
        //    {
        //        if (UserFormId > 0)
        //        {
        //            var result = await _context.UserForms.FirstOrDefaultAsync(x => x.Title.ToLower() == UserFormname.ToLower() && x.GroupId != UserFormId && x.IsDeleted==false);
        //            if (result != null)
        //            {
        //                return true;
        //            }


        //        }
        //        else
        //        {
        //            var result = await _context.UserForms.FirstOrDefaultAsync(x => x.Title.ToLower() == UserFormname.ToLower() && x.IsDeleted == false);
        //            if (result != null)
        //            {
        //                return true;
        //            }

        //        }

        //        return false;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}





    }
}
