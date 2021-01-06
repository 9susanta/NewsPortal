using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Concrete
{
   public interface IOperation<T> where T : class
    {
        void Insert(T entity);
        void Delete(Expression<Func<T, bool>> predicate, params string[] navigationProperties);
        void Edit(T enity);
        IQueryable<T> SearchFor(T enity);
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate, params string[] navigationProperties);
        T GetByID(Expression<Func<T, bool>> predicate, params string[] navigationProperties);
        int Save();
        void BulkDelete(List<T> existing);
    }
}
