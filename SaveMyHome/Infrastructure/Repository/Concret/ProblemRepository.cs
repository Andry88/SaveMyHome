using SaveMyHome.Infrastructure.Repository.Abstract;
using System;
using System.Linq;
using SaveMyHome.Models;
using System.Linq.Expressions;
using System.Data.Entity;
using SaveMyHome.DAL;

namespace SaveMyHome.Infrastructure.Repository.Concret
{
    public class ProblemRepository : IProblemRepository
    {
        ApplicationDbContext db;

        public ProblemRepository(ApplicationDbContext context)
        {
            db = context;
        }

        public IQueryable<Problem> All => db.Problems;

        public IQueryable<Problem> AllIncluding(params Expression<Func<Problem, object>>[] includeProperties)
        {
            IQueryable<Problem> query = All;

            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);

            return query;
        }

        public int GetProblemIdByName(string name)
        {
            return db.Problems.SingleOrDefault(p => p.Name == name).Id;
        }

        public void Add(Problem entity)
        {
            db.Entry(entity).State = EntityState.Added;
        }

        public void Update(Problem entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        public Problem GetOne(int id)
        {
            return db.Problems.FirstOrDefault(i => i.Id == id);
        }

        public void Delete(Problem entity)
        {
            throw new NotImplementedException();
        }
    }
}