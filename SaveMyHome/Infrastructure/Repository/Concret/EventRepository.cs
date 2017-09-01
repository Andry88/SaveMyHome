using SaveMyHome.Infrastructure.Repository.Abstract;
using SaveMyHome.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using SaveMyHome.DAL;

namespace SaveMyHome.Infrastructure.Repository.Concret
{
    public class EventRepository : IEventRepository
    {
        ApplicationDbContext db;

        public EventRepository(ApplicationDbContext db) => this.db = db;

        public IQueryable<Event> All => db.Events;

        public Event GetOne(int id)
        {
           return db.Events.FirstOrDefault(i => i.Id == id);
        }

        public IQueryable<Event> AllIncluding(params Expression<Func<Event, object>>[] includeProperties)
        {
            IQueryable<Event> query = All;

            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);

            return query;
        }
        public void Add(Event entity)
        {
            db.Entry(entity).State = EntityState.Added;
        }

        public void Update(Event entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(Event entity)
        {
            throw new NotImplementedException();
        }
    }
}