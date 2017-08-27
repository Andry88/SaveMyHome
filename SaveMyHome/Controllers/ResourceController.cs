using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaveMyHome.Controllers
{
    public class ResourceController : Controller
    {
        public ActionResult Script() 
        {
            Response.ContentType = "text/javascript";
            return View();
        }
    }
}