using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Uplift.DataAccess.Data.Repository
{
    public class Repository<T> : IRepository.IRepository<T> where T : class
    {
        protected readonly DbContext DbContext;
        internal DbSet<T> DbSet;

        public Repository(DbContext context)
        {
            DbContext = context;
            this.DbSet = context.Set<T>();
        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public T Get(int id)
        {
            return DbSet.Find(id);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {
            IQueryable<T> query = DbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                query = includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, property) => current.Include(property));
                //foreach (var property in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                //{
                //    query = query.Include(property);
                //}
            }

            return orderBy != null ? orderBy(query).ToList() : query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<T> query = DbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, property) => current.Include(property));
                //foreach (var property in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                //{
                //    query = query.Include(property);
                //}
            }

            return query.FirstOrDefault();
        }

        public void Remove(int id)
        {
            T entityToRemove = DbSet.Find(id);
            Remove(entityToRemove);
        }

        public void Remove(T entity)
        {
            DbSet.Remove(entity);
        }
    }
}
