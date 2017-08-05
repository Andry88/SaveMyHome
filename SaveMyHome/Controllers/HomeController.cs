using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SaveMyHome.Filters;
using SaveMyHome.Models;
using SaveMyHome.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

namespace SaveMyHome.Controllers
{
    [ForUsers]
    public class HomeController : Controller
    {
        //Выводит главную страницу, а также. если пользователь аутентифицирован,
        //передает в представление статус посетителя для последующего принятия решения в представлении 
        //о необходимости отобразить кнопку "Внимание" 
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var reactions = CurrUser.Apartment.Reactions;
                if (reactions.Count > 0)
                    ViewBag.ProblemStatus = reactions.Last().ProblemStatus;
            }
            return View();
        }

        //Выводит список жильцов дома,
        //осуществяет сортировку списка по имени и номеру квартиры по возрастанию и убыванию,
        //поиск по имени и пагинацию
        public ActionResult GetPeople(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.ApartSortParm = sortOrder == "Apart" ? "apart_desc" : "Apart";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var users = UserManager.Users;

            if (!String.IsNullOrEmpty(searchString))
                users = users.Where(s => s.LastName.Contains(searchString)
                                      || s.FirstName.Contains(searchString));

            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(u => u.LastName);
                    break;
                case "Apart":
                    users = users.OrderBy(u => u.ApartmentNumber);
                    break;
                case "apart_desc":
                    users = users.OrderByDescending(u => u.ApartmentNumber);
                    break;
                default:
                    users = users.OrderBy(u => u.LastName);
                    break;
            }

            Mapper.Initialize(cfg => cfg.CreateMap<ApplicationUser, UserVM>());
            IEnumerable<UserVM> models = Mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<UserVM>>(users);

            int pageSize = 12;
            int pageNumber = (page ?? 1);

            return View(models.ToPagedList(pageNumber, pageSize));
        }

        //Детальный просмотр конкретного жильца дома
        public ActionResult Details(string Id)
        {
            if (Id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = UserManager.Users.First(u => u.Id == Id);
            if (user == null)
                return HttpNotFound();

            Mapper.Initialize(cfg => cfg.CreateMap<ApplicationUser, UserVM>());
            UserVM model = Mapper.Map<ApplicationUser, UserVM>(user);

            return View(model);
        }

        #region Helpers
        private ApplicationUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        private ApplicationUser CurrUser => UserManager.FindByEmail(User.Identity.Name);
        #endregion
    }
}