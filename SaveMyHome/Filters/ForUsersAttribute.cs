using System;
using System.Web;
using System.Web.Mvc;

namespace SaveMyHome.Filters
{
    public class ForUsersAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContenxt");

            if (!httpContext.User.Identity.IsAuthenticated)
                return false;

            bool authorize = httpContext.User.IsInRole(AppRoles.User);
            return authorize;
        }
    }

}