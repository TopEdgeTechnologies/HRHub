
using HRHUBAPI.Models;
using HRHUBAPI.Models.Configuration;
using HRHUBWEB.Extensions;
using HRHUBWEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Data;


namespace  HRHUBWEB.Filters
{
    public class CustomAuthorization : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            

            var userObject = context.HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
            // Check if user is authenticated
            if (userObject == null)
            {
                

                context.Result = new RedirectResult("~/User/Loginpage", true);
                //   context.Result = new UnauthorizedResult("");
                return;
            }

            DbConnection _db = new DbConnection();
            string query = "exec sp_GetUserRightByUser " + userObject.UserId;
            DataTable dt = _db.ReturnDataTable(query);
            if (dt.Rows.Count > 0)
            {
                var result = dt.AsEnumerable()
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
                   }).ToList();

                string actionName = context.ActionDescriptor.RouteValues["action"].ToString();
                string controllerName = context.ActionDescriptor.RouteValues["controller"].ToString();
                var ResultRole=result.FirstOrDefault(x => x.action == actionName && x.controller == controllerName);
                if (ResultRole != null)
                {
                    var controller = context.Controller as Controller;
                    controller.TempData["IsNew"] = ResultRole.Isnew;
                    controller.TempData["IsEdit"] = ResultRole.IsEdit;
                    controller.TempData["IsDelete"] = ResultRole.IsDelete;
                    controller.TempData["IsPrint"] = ResultRole.IsPrint;
                    controller.TempData["ReferenceID"] = ResultRole.ReferenceID;
					controller.TempData["actionName"] = actionName;

					return;

                }
                else
                {

                    context.Result = new RedirectResult("~/User/Loginpage", true);
                 
                    return;
                }
            

            }

            base.OnActionExecuting(context);
        }
    }
}
