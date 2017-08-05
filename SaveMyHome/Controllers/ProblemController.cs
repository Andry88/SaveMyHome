using Microsoft.AspNet.Identity.Owin;
using SaveMyHome.Models;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SaveMyHome.ViewModels;
using SaveMyHome.Filters;

namespace SaveMyHome.Controllers
{
    [ForUsers]
    public class ProblemController : Controller
    {
        //Выводит историю проблем
        public ActionResult ProblemHistory()
        {
            var problemHistory =
                db.Events.Include(p => p.Problem).Include(r => r.Reactions)
                                          .Where(e => e.End != null)
                                          .Select(e => new ProblemHistoryVM
                                          {
                                              Name = e.Problem.Name,
                                              Start = e.Start,
                                              End = e.End,
                                              Culprit = e.Reactions.FirstOrDefault(c => c.ProblemStatus == ProblemStatus.Culprit).ApartmentNumber,
                                              Victims = e.Reactions.Where(a => a.ProblemStatus == ProblemStatus.Victim).OrderBy(a => a.ApartmentNumber)
                                                                   .Select(a => a.ApartmentNumber)
                                          });

            return View(problemHistory);
        }
        private ApplicationDbContext db => HttpContext.GetOwinContext().Get<ApplicationDbContext>();
    }
}