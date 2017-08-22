using SaveMyHome.DAL;
using SaveMyHome.Infrastructure.Repository.Abstract;
using SaveMyHome.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace SaveMyHome.Infrastructure.Repository.Concret
{
    public class ReactionRepository : IReactionRepository
    {
        ApplicationDbContext db;

        public ReactionRepository(ApplicationDbContext context)
        {
            db = context;
        }

        public IQueryable<Reaction> All => db.Reactions;
        public IQueryable<Reaction> AllIncluding(params Expression<Func<Reaction, object>>[] includeProperties)
        {
            IQueryable<Reaction> query = All;

            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);

            return query;
        }

        public void Add(Reaction entity)
        {
            db.Entry(entity).State = EntityState.Added;
        }

        public void Update(Reaction entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        public Reaction GetOne(int id)
        {
            return db.Reactions.FirstOrDefault(i => i.Id == id);
        }

        public void Delete(Reaction entity)
        {
            db.Entry(entity).State = EntityState.Deleted;
        }

        public void DeleteMany(IEnumerable<Reaction> entities)
        {
            foreach (var entity in entities)
                db.Entry(entity).State = EntityState.Deleted;
        }

        public int LastReactionId => db.Reactions.OrderBy(r => r.Id).ToList().LastOrDefault()?.EventId ?? 0;

        public Reaction CurrentReaction
        {
            get {
                string currentUserName = HttpContext.Current.User.Identity.Name;
                ApplicationUser currentUser = db.Users.Include(u => u.ClientProfile)
                    .FirstOrDefault(u => u.Email == currentUserName);
                return currentUser.ClientProfile.Apartment.Reactions.OrderByDescending(r => r.Id).FirstOrDefault(); 
            }
        }
    }
}
