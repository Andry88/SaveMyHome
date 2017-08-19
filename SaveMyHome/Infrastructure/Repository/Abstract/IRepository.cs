using System;
using System.Linq;
using System.Linq.Expressions;

namespace SaveMyHome.Infrastructure.Repository.Abstract
{
    public interface IRepository<T> where T : class, new()
    {
        IQueryable<T> All { get; }
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        T GetOne(int id);
    }
}
