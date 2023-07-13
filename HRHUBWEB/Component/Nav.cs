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
		private readonly APIHelper _APIHelper;

		public NavigationBarViewComponent(DbConnection db, APIHelper APIHelper)
        {
            _db = db;
            _APIHelper = APIHelper;

		}
        public async Task<IViewComponentResult> InvokeAsync()
        {
			// Perform any necessary data access or processing here...

			// Return the view to be rendered


           var userObject = HttpContext.Session.GetObjectFromJson<User>("AuthenticatedUser");
            string query = "exec sp_GetUserRightByUser " + userObject.UserId;
            DataTable dt = _db.ReturnDataTable(query);
            List<GetUserRightByUser> nav = _db.ConvertDataTableToList<GetUserRightByUser>(dt);
           // List<dynamic> dynamicList = _db.ConvertDataTableToList(dt);
			

           return View("_Navigation", nav.Where(x=>x.IsMenu==true));
           // return PartialView("_Navigation", nav);
        }


    }
}
