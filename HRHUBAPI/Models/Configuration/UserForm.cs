using HRHUBAPI.Models.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace HRHUBAPI.Models
{
    public partial class UserForm
    {
        
        #region NotMapped
        [NotMapped] public bool? type { get; set; }
        [NotMapped] public int Userid { get; set; }
        [NotMapped] public string? UserName { get; set; }
        [NotMapped] public string? PhotoPath { get; set; }
        [NotMapped] public string? UserPassword { get; set; }
        [NotMapped] public Nullable<int> GroupID { get; set; }
        [NotMapped] public bool Active { get; set; }
        [NotMapped] public string? GroupTitle { get; set; }
        [NotMapped] public string? Description { get; set; }
        [NotMapped] public bool? Inactive { get; set; }
        [NotMapped] public Nullable<bool> Assign { get; set; }
        [NotMapped] public Nullable<bool> IsEdit { get; set; }
        [NotMapped] public Nullable<bool> IsDelete { get; set; }
        [NotMapped] public Nullable<bool> IsPrint { get; set; }
        [NotMapped] public Nullable<bool> Isnew { get; set; }
        //[NotMapped] public int Formid { get; set; }
        //[NotMapped] public string? FormTitle { get; set; }
        [NotMapped] public string? controller { get; set; }
        [NotMapped] public string? action { get; set; }
        [NotMapped] public string? imageClass { get; set; }
        [NotMapped] public Nullable<bool> status { get; set; }
        [NotMapped] public Nullable<bool> isParent { get; set; }
        [NotMapped] public Nullable<int> parentId { get; set; }
        //[NotMapped] public Nullable<bool> IsMenu { get; set; }
        [NotMapped] public int? ReferenceID { get; set; }
        [NotMapped] public int? titlewithcounter { get; set; }

        #endregion

        public async Task<List<UserForm>> GetUserForm(HrhubContext _context)
        {
            try
            {
                return await _context.UserForms.ToListAsync();
            }
            catch (Exception ex) {throw;}
        }

        //public async Task<List<Staff>> GetStaffByCompanyId(int CompanyId)
        //{
        //    DbConnection _db = new DbConnection();
        //    try
        //    {
        //    //List<GetUserRightByUser> nav = new List<GetUserRightByUser>();
        //    //var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");

        //    string query = "EXEC sp_GetUserRightByUser " + userObject.UserId;
        //    DataTable dt = _db.ReturnDataTable(query);

        //    var objUserForm = dt.AsEnumerable()
        //        .Select(row => new UserForm
        //        {
        //            Userid = Convert.ToInt32(row["Userid"]),
        //            UserName = row["UserName"].ToString(),
        //            PhotoPath = row["PhotoPath"].ToString(),
        //            GroupID = Convert.ToInt32(row["GroupID"]),
        //            Assign = Convert.ToBoolean(row["Assign"]),
        //            IsEdit = Convert.ToBoolean(row["IsEdit"]),
        //            IsDelete = Convert.ToBoolean(row["IsDelete"]),
        //            IsPrint = Convert.ToBoolean(row["IsPrint"]),
        //            Isnew = Convert.ToBoolean(row["Isnew"]),
        //            Formid = Convert.ToInt32(row["Formid"]),
        //            FormTitle = row["FormTitle"].ToString(),
        //            controller = row["controller"].ToString(),
        //            action = row["action"].ToString(),
        //            imageClass = row["imageClass"].ToString(),
        //            status = Convert.ToBoolean(row["status"]),
        //            isParent = Convert.ToBoolean(row["isParent"]),
        //            parentId = Convert.ToInt32(row["parentId"]),
        //            IsMenu = Convert.ToBoolean(row["IsMenu"]),
        //            ReferenceID = string.IsNullOrWhiteSpace(row["ReferenceID"].ToString()) ? 0 : Convert.ToInt32(row["ReferenceID"]),
        //            titlewithcounter = string.IsNullOrWhiteSpace(row["titlewithcounter"].ToString()) ? 0 : Convert.ToInt32(row["titlewithcounter"])
        //        }).ToList();
            
        //        return objUserForm;
        //    }
        //    catch (Exception e) { throw; }
        //}

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
