using SaveMyHome.Infrastructure.Repository.Abstract;
using System;
using System.Linq;
using SaveMyHome.Models;
using System.Linq.Expressions;
using System.Data.Entity;
using SaveMyHome.DAL;

namespace SaveMyHome.Infrastructure.Repository.Concret
{
    public class UserRepository : IUserRepository
    {
        ApplicationDbContext db;

        public UserRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IQueryable<ApplicationUser> All => db.Users;

        public void Add(ApplicationUser entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ApplicationUser> AllIncluding(params Expression<Func<ApplicationUser, object>>[] includeProperties)
        {
            IQueryable<ApplicationUser> query = All;

            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);

            return query;
        }

        public void Delete(ApplicationUser entity)
        {
            throw new NotImplementedException();
        }

        public ApplicationUser GetOne(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(ApplicationUser entity)
        {
            throw new NotImplementedException();
        }
    }
}