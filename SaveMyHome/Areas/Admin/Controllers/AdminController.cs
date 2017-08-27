using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SaveMyHome.Infrastructure.Repository.Abstract;
using SaveMyHome.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SaveMyHome.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        IUnitOfWork Database;
        public AdminController(IUnitOfWork database)
        {
            Database = database;
        }

        //Выводит меню управления
        public ActionResult Index() => View();

        //Выводит список посещаемых пользователями страниц сайта 
        public ActionResult Log()=> View(Database.Visitors.All.ToList());

        //Выводит список зарегистрированных пользователей
        public ActionResult Accounts() => View(UserManager.Users
            .OrderBy(u => u.ClientProfile.ApartmentNumber).ToList());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);

            if (user != null)
            {
                IdentityResult result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    TempData["msg"] = $"Пользователь {user.Email} был успешно удален";
                    return RedirectToAction("Index");
                }
                else
                    return View("Error", result.Errors);
            }
            else
                return View("Error", new string[] { "Пользователь не найден" });
        }
       
        #region Helpers
        private ApplicationUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        private IAuthenticationManager AuthenticationManager=> HttpContext.GetOwinContext().Authentication;

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
                ModelState.AddModelError("", error);
        }
        #endregion
    }
}