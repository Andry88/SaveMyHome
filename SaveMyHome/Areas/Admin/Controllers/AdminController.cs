using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SaveMyHome.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SaveMyHome.Areas.Admin.Controllers
{
    //[Authorize(Roles = nameof(AppRoles.Admin))]
    public class AdminController : Controller
    {
        //Выводит меню управления
        public ActionResult Index() => View();

        //Выводит список посещаемых пользователями страниц сайта 
        public ActionResult Log()=> View(db.Visitors.ToList());

        //Выводит список зарегистрированных пользователей
        public ActionResult Accounts() => View(UserManager.Users.OrderBy(u => u.ApartmentNumber));

        
        public ActionResult Create()=> View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                Mapper.Initialize(cfg => cfg.CreateMap<RegisterViewModel, ApplicationUser>());
                ApplicationUser user = Mapper.Map<RegisterViewModel, ApplicationUser>(model);

                IdentityResult result =
                    await UserManager.CreateAsync(user);

                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    AddErrorsFromResult(result);
            }
            return View(model);
        }

        public async Task<ActionResult> Edit(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                Mapper.Initialize(cfg => cfg.CreateMap<ApplicationUser, EditAccountVM>());
                EditAccountVM model = Mapper.Map<ApplicationUser, EditAccountVM>(user);

                return View(model);
            }
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditAccountVM model, HttpPostedFileBase Image = null)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                if (Image != null)
                {
                    model.ImageMimeType = Image.ContentType;
                    model.ImageData = new byte[Image.ContentLength];
                    Image.InputStream.Read(model.ImageData, 0, Image.ContentLength);
                }

                Mapper.Initialize(cfg => cfg.CreateMap<EditAccountVM, ApplicationUser>());
                Mapper.Map(model, user);

                IdentityResult validEmail = await UserManager.UserValidator.ValidateAsync(user);

                if (!validEmail.Succeeded)
                    AddErrorsFromResult(validEmail);

                IdentityResult validPass = null;
                if (model.Password != string.Empty)
                {
                    validPass = await UserManager.PasswordValidator.ValidateAsync(model.Password);

                    if (validPass.Succeeded)
                        user.PasswordHash = UserManager.PasswordHasher.HashPassword(model.Password);
                    else
                        AddErrorsFromResult(validPass);
                }

                if ((validEmail.Succeeded && validPass == null) ||
                        (validEmail.Succeeded && model.Password != string.Empty && validPass.Succeeded))
                {
                    IdentityResult result = await UserManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                        AddErrorsFromResult(result);
                }
            }
            else
                ModelState.AddModelError("", "Пользователь не найден");
            return View(model);
        }

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
                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    TempData["msg"] = $"{user.FirstName} был удален";
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
        private ApplicationDbContext db => HttpContext.GetOwinContext().Get<ApplicationDbContext>();
        private IAuthenticationManager AuthenticationManager=> HttpContext.GetOwinContext().Authentication;
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
                ModelState.AddModelError("", error);
        }
        #endregion
    }
}