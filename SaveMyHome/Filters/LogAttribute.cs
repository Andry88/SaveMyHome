using SaveMyHome.Areas.Admin.Models;
using SaveMyHome.Infrastructure.Repository.Abstract;
using System;
using System.Web.Mvc;

namespace SaveMyHome.Filters
{
    public class LogAttribute : ActionFilterAttribute
    {
        IUnitOfWork Database;
        public LogAttribute(IUnitOfWork database) => Database = database;

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

            Database.Visitors.Add(visitor);
            Database.Save();
            base.OnActionExecuting(filterContext);
        }
    }
}