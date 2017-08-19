using SaveMyHome.Infrastructure.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SaveMyHome.Models;
using System.Linq.Expressions;
using System.Data.Entity;
using SaveMyHome.DAL;

namespace SaveMyHome.Infrastructure.Repository.Concret
{
    public class ClientProfileRepository : IClientProfileRepository
    {
        ApplicationDbContext db;

        public ClientProfileRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public ClientProfile CurrentUserProfile => db.ClientProfiles.Include(p => p.ApplicationUser)
            .FirstOrDefault(u => u.ApplicationUser.Email == HttpContext.Current.User.Identity.Name);

        public IQueryable<ClientProfile> All => db.ClientProfiles;

        public void Add(ClientProfile entity)
        {
            db.ClientProfiles.Add(entity);
        }

        public IQueryable<ClientProfile> AllIncluding(params Expression<Func<ClientProfile, object>>[] includeProperties)
        {
            IQueryable<ClientProfile> query = All;

            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);

            return query;
        }

        public void Delete(ClientProfile entity)
        {
            throw new NotImplementedException();
        }

        public ClientProfile GetOne(int id)
        {
            throw new NotImplementedException();
        }

        public ClientProfile GetUserWithProfile(string id)
        {
           return db.ClientProfiles.Include(p => p.ApplicationUser).FirstOrDefault(p => p.Id == id);
        }

        public void Update(ClientProfile entity)
        {
            throw new NotImplementedException();
        }
    }
}