using NewsPortal.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace NewsPortal.Concrete
{
    public class OpertionClass<T> : IOperation<T> where T:class
    {
        private NewsPortalEntities _context = null;
        private DbSet<T> _table = null;
        public OpertionClass()
        {
            this._context = new NewsPortalEntities();
            this._table = _context.Set<T>();
        }
        public OpertionClass(NewsPortalEntities context)
        {
            this._context = context;
            this._table = _context.Set<T>();
        }
        public void Delete(Expression<Func<T, bool>> predicate, params string[] navigationProperties)
        {
            T list;
            var query = _table.AsQueryable();
            foreach (string navigationProperty in navigationProperties)
            {
                query = query.Include(navigationProperty);//got to reaffect it.
            }
            list = query.Where(predicate).FirstOrDefault();

            T existing = list;
            _table.Remove(existing);
        }
        public void Edit(T entity)
        {
            _table.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate, params string[] navigationProperties)
        {
                List<T> list;
                var query = _table.AsQueryable();
                foreach (string navigationProperty in navigationProperties)
                {
                 query = query.Include(navigationProperty);//got to reaffect it.
                }
                list = query.Where(predicate).ToList<T>();
            return list.AsQueryable();
        }
        public T GetByID(Expression<Func<T, bool>> predicate, params string[] navigationProperties)
        {
            T list;
            var query = _table.AsQueryable();
            foreach (string navigationProperty in navigationProperties)
            {
                query = query.Include(navigationProperty);//got to reaffect it.
            }
            list = query.Where(predicate).FirstOrDefault();
            return list;
        }
        public void BulkDelete(List<T> existing)
        {
            _table.RemoveRange(existing);
        }
        public void Insert(T entity)
        {
            _table.Add(entity);
        }
        public int Save()
        {
            return _context.SaveChanges();
        }
        public IQueryable<T> SearchFor(T enity)
        {
            throw new NotImplementedException();
        }
    }
}