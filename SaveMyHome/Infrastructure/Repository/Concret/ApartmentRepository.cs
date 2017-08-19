using SaveMyHome.DAL;
using SaveMyHome.Infrastructure.Repository.Abstract;
using SaveMyHome.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace SaveMyHome.Infrastructure.Repository.Concret
{
    public class ApartmentRepository : IApartmentRepository
    {
        ApplicationDbContext db;

        public ApartmentRepository(ApplicationDbContext context)
        {
            db = context;
        }

        public IQueryable<Apartment> All => db.Apartments;

        public void Add(Apartment entity)
        {
            db.Entry(entity).State = EntityState.Added;
        }

        public IQueryable<Apartment> AllIncluding(params Expression<Func<Apartment, object>>[] includeProperties)
        {
            IQueryable<Apartment> query = All;

            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);

            return query;
        }

        public void Delete(Apartment entity)
        {
            throw new NotImplementedException();
        }

        public Apartment GetOne(int id)
        {
            return db.Apartments.FirstOrDefault(i => i.Number == id);
        }

        public void Update(Apartment entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }
    }
}