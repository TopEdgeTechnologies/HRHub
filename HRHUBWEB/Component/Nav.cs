using HRHUBAPI.Models;
using HRHUBAPI.Models.Configuration;
using HRHUBWEB.Extensions;
using HRHUBWEB.Models;
using Microsoft.AspNetCore.Mvc;

using System.Data;

namespace HRHUBWEB.Component
{
    public class NavigationBarViewComponent : ViewComponent
    {
        private readonly DbConnection _db;

        public NavigationBarViewComponent(DbConnection db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Perform any necessary data access or processing here...

            // Return the view to be rendered

            List<GetUserRightByUser> nav = new List<GetUserRightByUser>();
            var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");


            string query = "exec sp_GetUserRightByUser " + userObject.UserId;
            DataTable dt = _db.ReturnDataTable(query);

            nav = dt.AsEnumerable()
            .Select(row => new GetUserRightByUser
            {
                Userid = Convert.ToInt32(row["Userid"]),
                UserName = row["UserName"].ToString(),
                PhotoPath = row["PhotoPath"].ToString(),
                GroupID = Convert.ToInt32(row["GroupID"]),
                Assign = Convert.ToBoolean(row["Assign"]),
                IsEdit = Convert.ToBoolean(row["IsEdit"]),
                IsDelete = Convert.ToBoolean(row["IsDelete"]),
                IsPrint = Convert.ToBoolean(row["IsPrint"]),
                Isnew = Convert.ToBoolean(row["Isnew"]),
                Formid = Convert.ToInt32(row["Formid"]),
                FormTitle = row["FormTitle"].ToString(),
                controller = row["controller"].ToString(),
                action = row["action"].ToString(),
                imageClass = row["imageClass"].ToString(),
                status = Convert.ToBoolean(row["status"]),
                isParent = Convert.ToBoolean(row["isParent"]),
                parentId = Convert.ToInt32(row["parentId"]),
                IsMenu = Convert.ToBoolean(row["IsMenu"]),
                ReferenceID = string.IsNullOrWhiteSpace(row["ReferenceID"].ToString()) ? 0 : Convert.ToInt32(row["ReferenceID"])



            })
            .ToList();

           return View("_Navigation", nav.Where(x=>x.IsMenu==true));
           // return PartialView("_Navigation", nav);
        }


    }
}
