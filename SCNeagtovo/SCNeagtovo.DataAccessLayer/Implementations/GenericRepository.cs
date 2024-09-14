using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SCNeagtovo.DataAccessLayer.Interfaces;
using SCNeagtovo.DataModels;

namespace SCNeagtovo.DataAccessLayer.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T: class
    {
        protected readonly NeagtovoDbContext dbContext;

        public GenericRepository(NeagtovoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Add(T entity)
        {
           dbContext.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            dbContext?.Set<T>().Remove(entity);
        }

        public IList<T> Get(Expression<Func<T, bool>> expression, bool asNoTracking = false, params Expression<Func<T, object>>[] include)
        {
            IQueryable<T> query = dbContext.Set<T>();
            if(expression != null)
            {
                query = query.Where(expression);
            }
            if(include != null)
            {
                query = include.Aggregate(query, (curent, include) => curent.Include(include));
            }
            if(asNoTracking)
            {
                query = query.AsNoTracking();
            }
            return query.ToList();
        }

        public IList<T> GetAll()
        {
            return dbContext.Set<T>().ToList();
        }
        public IQueryable<T> GetAllAsQueryable()
        {
            return dbContext.Set<T>();
        }
        public T? GetById<IdType>(IdType id)
        {
            return dbContext.Set<T>().Find(id);
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }

        public void Update(T entity, IList<string> ignoreProperties)
        {
            dbContext.Set<T>().Update(entity);
        }

        public void UpdateProperty(string tableName, string entityId, string property, string propetyValue)
        {
            dbContext.Database.ExecuteSqlRaw($"Update [{tableName}] set {property} = {{0}} where Id = {{1}}", propetyValue, entityId);
            dbContext.SaveChanges();

        }
    }
}
