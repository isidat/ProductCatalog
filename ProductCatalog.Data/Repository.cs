using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using ProductCatalog.Data.Context;

namespace ProductCatalog.Data
{
    public interface IRepository<T> where T : class
    {
        Database Database { get; }
        IList<T> Get(Expression<Func<T, bool>> predicate = null);
        bool Any(Expression<Func<T, bool>> predicate);
        int Count(Expression<Func<T, bool>> predicate);
        T First(Expression<Func<T, bool>> predicate);
        IList<T> ToList();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Commit();
        void Dispose();
    }

    public class Repository<T> : IRepository<T>, IDisposable where T : class
    {
        protected readonly DatabaseContext Context;

        private DbSet<T> DbSet
        {
            get
            {
                return Context.Set<T>();
            }
        }

        public Database Database
        {
            get
            {
                return Context.Database;
            }
        }

        public Repository(IDatabaseContext context)
        {
            Context = (DatabaseContext)context;
        }

        public IList<T> Get(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate).ToList();
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Any(predicate);
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate).Count();
        }

        public T First(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate).FirstOrDefault();
        }

        public IList<T> ToList()
        {
            return DbSet.ToList();
        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public void Update(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
