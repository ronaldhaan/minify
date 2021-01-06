using System;
using System.Linq;
using System.Linq.Expressions;

namespace Minify.DAL.Repositories
{
    public interface IRepository<T> where T : class
    {
        T Find<TKey>(TKey id);

        T FindOneBy(Expression<Func<T, bool>> predicate);

        bool Any(Expression<Func<T, bool>> predicate);

        IQueryable<T> GetAll();

        void Add(T entity);

        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);

        int SaveChanges();

        void Update(T entity);

        void Remove(T entity);

        void AddRange(params T[] entities);

        void RemoveRange(params T[] entities);
    }
}