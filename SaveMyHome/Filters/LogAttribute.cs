using SaveMyHome.Areas.Admin.Models;
using SaveMyHome.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaveMyHome.Filters
{
    public class LogAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            Visitor visitor = new Visitor()
            {
                Login = (request.IsAuthenticated) ? filterContext.HttpContext.User.Identity.Name : "null",
                Ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress,
                Url = request.RawUrl,
                Date = DateTime.UtcNow
            };

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Visitors.Add(visitor);
                db.SaveChanges();
            }

            base.OnActionExecuting(filterContext);
        }
    }
}