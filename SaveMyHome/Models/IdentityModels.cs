using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Collections.Generic;
using SaveMyHome.Areas.Admin.Models;

namespace SaveMyHome.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Имя"), Required(ErrorMessage = "Укажите Ваше имя")]
        [StringLength(15, ErrorMessage = "{0} должно содержать от {2} до {1} символов", MinimumLength = 2)]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия"), Required(ErrorMessage = "Укажите Вашу фамилию")]
        [StringLength(15, ErrorMessage = "{0} должна содержать от {2} до {1} символов", MinimumLength = 2)]
        public string LastName { get; set; }
        public string FullName => FirstName + " " + LastName;

        [Display(Name = "Возраст"), Required(ErrorMessage = "Укажите Ваш возраст")]
        [Range(8, 100, ErrorMessage = "{0} должен быть не меьше {1} и не более {2} лет")]
        public int Age { get; set; }

        [Display(Name = "Дополнительный телефон"), DataType(DataType.PhoneNumber)]
        //[StringLength(20, ErrorMessage = "{0} должен содержать от {2} до {1} символов", MinimumLength = 11)]
        public string SecondPhoneNumber { get; set; }

        [Display(Name = "Навыки и умения"), DataType(DataType.MultilineText)]
        public string Skills { get; set; }

        [Display(Name = "Увлечения"), DataType(DataType.MultilineText)]
        public string Hobbies { get; set; }

        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }

        public int ApartmentNumber { get; set; }
        [ForeignKey("ApartmentNumber")]
        public virtual Apartment Apartment { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Visitor> Visitors { get; set; }

        public ApplicationDbContext()
            : base("SaveMyHomeConnection", throwIfV1Schema: false)
        {}

        public static ApplicationDbContext Create()=> new ApplicationDbContext();

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessages = string.Join("; ", ex.EntityValidationErrors
                                                           .SelectMany(x => x.ValidationErrors)
                                                           .Select(x => x.ErrorMessage));
                throw new DbEntityValidationException(errorMessages);
            }
        }
        //public System.Data.Entity.DbSet<SaveMyHome.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}