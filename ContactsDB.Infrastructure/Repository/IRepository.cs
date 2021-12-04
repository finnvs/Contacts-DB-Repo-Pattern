using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ContactsDB.Infrastructure.Repository
{
    public interface IRepository<T>
    {
        T Delete(T entity);
        T Update(T entity);
        T Insert(T entity);
        IEnumerable<T> Select(Expression<Func<T, bool>> predicate);
        void SaveChanges();

    }
}
