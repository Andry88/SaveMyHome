using SaveMyHome.Models;
using System.Linq;
using System.Web.Mvc;
using SaveMyHome.ViewModels;
using SaveMyHome.Infrastructure.Repository.Abstract;

namespace SaveMyHome.Controllers
{
    public class HistoryController : Controller
    {
        IUnitOfWork Database;
        public HistoryController(IUnitOfWork unitOfWork)
        {
            this.Database = unitOfWork;
        }

        public ViewResult ProblemHistory()
        {
            var problemHistory =
                Database.Events.AllIncluding(p => p.Problem, r => r.Reactions)
                                          .Where(e => e.End != null)
                                          .Select(e => new ProblemHistoryViewModel
                                          {
                                              Name = e.Problem.Name,
                                              Start = e.Start,
                                              End = e.End,
                                              Culprit = e.Reactions.FirstOrDefault(c => c.ProblemStatus == ProblemStatus.Culprit)
                                                                   .ApartmentNumber,
                                              Victims = e.Reactions.Where(a => a.ProblemStatus == ProblemStatus.Victim)
                                                                   .OrderBy(a => a.ApartmentNumber)
                                                                   .Select(a => a.ApartmentNumber)
                                          });

            return View(problemHistory);
        }

        protected override void Dispose(bool disposing)
        {
            Database.Dispose();
            base.Dispose(disposing);
        }
    }
}