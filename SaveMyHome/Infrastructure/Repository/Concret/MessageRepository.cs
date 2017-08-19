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
    public class MessageRepository : IMessageRepository
    {
        ApplicationDbContext db;

        public MessageRepository(ApplicationDbContext context)
        {
            db = context;
        }

        public IQueryable<Message> All => db.Messages;

        public void Add(Message entity)
        {
            db.Entry(entity).State = EntityState.Added;
        }

        public IQueryable<Message> AllIncluding(params Expression<Func<Message, object>>[] includeProperties)
        {
            IQueryable<Message> query = All;

            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);

            return query;
        }

        public void Delete(Message entity)
        {
            throw new NotImplementedException();
        }

        public Message GetOne(int id)
        {
            return db.Messages.FirstOrDefault(i => i.Id == id);
        }

        public void Update(Message entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }
    }
}