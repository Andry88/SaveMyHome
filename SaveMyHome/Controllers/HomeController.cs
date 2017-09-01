using AutoMapper;
using SaveMyHome.Models;
using SaveMyHome.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PagedList;
using SaveMyHome.Infrastructure.Repository.Abstract;
using System.Web;

namespace SaveMyHome.Controllers
{
    public class HomeController : Controller
    {
        IUnitOfWork Database;
        public HomeController(IUnitOfWork database) => Database = database;

        //Выводит главную страницу, а также. если пользователь аутентифицирован,
        //передает в представление статус посетителя для последующего принятия решения в представлении 
        //о необходимости отобразить кнопку "Внимание" 
        [AllowAnonymous]
        public ViewResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                List<Reaction> reactions = null;
                try
                {
                    reactions = Database.ClientProfiles.CurrentClientProfile.Apartment.Reactions.ToList();
                }
                catch (NullReferenceException)
                {
                    reactions = new List<Reaction>();
                }
                
                if (reactions.Count > 0)
                    ViewBag.ProblemStatus = reactions.LastOrDefault().ProblemStatus;
            }
            return View();
        }

        //Выводит список жильцов дома,
        //осуществяет сортировку списка по имени и номеру квартиры по возрастанию и убыванию,
        //поиск по имени и пагинацию
        public ViewResult GetPeople(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.ApartSortParm = sortOrder == "Apart" ? "apart_desc" : "Apart";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var users = Database.ClientProfiles.AllIncluding(u => u.ApplicationUser);

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

            Mapper.Initialize(cfg => cfg.CreateMap<ClientProfile, UserViewModel>()
            .ForMember(vm => vm.Email, conf => conf.MapFrom(cp => cp.ApplicationUser.Email))
            .ForMember(vm => vm.PhoneNumber, conf => conf.MapFrom(cp => cp.ApplicationUser.PhoneNumber)));
            
            var models = Mapper.Map<IQueryable<ClientProfile>, IEnumerable<UserViewModel>>(users);

            int pageSize = 12;
            int pageNumber = (page ?? 1);

            return View(models.ToPagedList(pageNumber, pageSize));
        }

        //Детальный просмотр конкретного жильца дома
        public ActionResult Details(string Id)
        {
            if (Id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var user = Database.ClientProfiles.GetUserWithProfile(Id);

            if(user == null)
                return HttpNotFound();
            
            Mapper.Initialize(cfg => cfg.CreateMap<ClientProfile, UserViewModel>()
            .ForMember(vm => vm.Email, conf => conf.MapFrom(cp => cp.ApplicationUser.Email))
            .ForMember(vm => vm.PhoneNumber, conf => conf.MapFrom(cp => cp.ApplicationUser.PhoneNumber)));
            UserViewModel model = Mapper.Map<ClientProfile, UserViewModel>(user);

            return View(model);
        }

        public RedirectResult ChangeCulture(string lang)
        {
            string returnUrl = Request.UrlReferrer.AbsolutePath;
            // Список культур
            List<string> cultures = new List<string>() { "ru", "en", "de" };
            if (!cultures.Contains(lang))
                lang = "ru";
            
            // Сохраняем выбранную культуру в куки
            HttpCookie cookie = Request.Cookies["lang"];
            if (cookie != null)
                cookie.Value = lang; // если куки уже установлено, то обновляем значение
            else
            {
                cookie = new HttpCookie("lang")
                {
                    HttpOnly = false,
                    Value = lang,
                    Expires = DateTime.Now.AddYears(1)
                };
            }
            Response.Cookies.Add(cookie);
            return Redirect(returnUrl);
        }

        protected override void Dispose(bool disposing)
        {
            Database.Dispose();
            base.Dispose(disposing);
        }
    }
}