using Microsoft.AspNet.Identity.EntityFramework;
using SaveMyHome.Areas.Admin.Models;
using SaveMyHome.Models;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace SaveMyHome.DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<ClientProfile> ClientProfiles { get; set; }
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Visitor> Visitors { get; set; }

        public ApplicationDbContext()
            : base("SaveMyHomeConnection", throwIfV1Schema: false)
        { }

        public static ApplicationDbContext Create() => new ApplicationDbContext();

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
    }
}